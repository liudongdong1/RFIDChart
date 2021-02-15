using RealtimeChart.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
namespace RealtimeChart
{

    /// <summary>
    /// RFID Tag  中包含的数据，  EPC，TID ，RSS，phase，多普勒值， GPS 坐标详细见Tag
    /// </summary>
    public class RFIDData
    {
        private string id;
        private List<double> phaseAngleInRadians;
        private List<long> timestampList;
        private string gesturename;
        private string refreshResult="Status";
        private int status = 0;   // rfid 数据状态  静止0-->运动1--> 静止2--> 服务器出来结果4；
        private List<double> staticJudge;
        Socket clientSocket = null;   //Socket 句柄
        public RFIDData()
        {
            id = "None";
            gesturename = "None";
            status = 0;
            phaseAngleInRadians = new List<double>();
            timestampList = new List<long>();
            staticJudge = new List<double>();
            clientSocket = SocketUtil.connectToCNNServer();
            System.Diagnostics.Debug.WriteLine("RFIDData：SocketUtil.connectToCNNServer" + "成功\t");
        }
        public RFIDData(string rfidid)
        {
            id = rfidid;
            gesturename = "None";
            status = 0;
            phaseAngleInRadians = new List<double>();
            timestampList = new List<long>();
            staticJudge = new List<double>();
            clientSocket = SocketUtil.connectToCNNServer();
            System.Diagnostics.Debug.WriteLine("RFIDData：SocketUtil.connectToCNNServer" + "成功\t");
        }
        /// <summary>
        /// 添加 phase 和 timestamps 数据，如果数据长度大于500，则删除最开始的80个数据，维持数组的大小
        /// </summary>
        /// <param name="phase"> 相位信息</param>
        /// <param name="time">时间戳信息</param>
        public void addPhaseTime(double phase,long time)
        {
            if (phaseAngleInRadians.Count > ItemString.datawindowlength && timestampList.Count > ItemString.datawindowlength)
            {
                phaseAngleInRadians.RemoveRange(0, 80);
                timestampList.RemoveRange(0, 80);
            }
           
            phaseAngleInRadians.Add(phase);
            timestampList.Add(time);
        }

        public void stateMachineChanging(double phase, long time)
        {
            switch (status)
            {
                case 0:
                    if (addStaticJudge(phase))                 // 这里变化有些快    
                    {
                        refreshResult = "准备绘制";
                        status = 1;
                        System.Diagnostics.Debug.WriteLine("stateMachineChanging： 准备绘制");
                    }
                    break;
                case 1:
                    addStaticJudge(phase);
                    if (staticJudgeStatic())
                    {
                        refreshResult = "开始绘制";
                        status = 2;
                        System.Diagnostics.Debug.WriteLine("stateMachineChanging： 开始绘制");
                    }
                    break;
                case 2:
                    addPhaseTime(phase, time);
                    addStaticJudge(phase);
                    if (!staticJudgeStatic())
                    {
                        refreshResult = "绘制中";
                        status = 3;
                        System.Diagnostics.Debug.WriteLine("stateMachineChanging： 绘制中");
                    }
                    break;
                case 3:
                    addPhaseTime(phase, time);
                    addStaticJudge(phase);
                    if (staticJudgeStatic())
                    {
                        refreshResult = "结束绘制";
                        status = 4;
                        System.Diagnostics.Debug.WriteLine("stateMachineChanging： 结束绘制");
                    }
                    break;
                case 4:
                    if (phaseAngleInRadians.Count < 50)
                    {
                        refreshResult = "数据无效，重新绘制" ;
                        System.Diagnostics.Debug.WriteLine("stateMachineChanging： 数据无效，重新绘制");
                        status = 5;
                    }
                    //  发送至服务器，并获取结果
                    //calculateGestureName();
                    gesturename = "2";
                    if (gesturename != "None")
                    {
                        refreshResult = "绘制手势\t"+gesturename;
                        System.Diagnostics.Debug.WriteLine("stateMachineChanging："+ "绘制手势\t" + gesturename);
                        status = 5;
                    }
                    break;
                case 5:
                    //refresh result and clear data
                    clear();
                    status = 0;
                    break;
                default:
                    //default 
                    break;
            }
        }
        public string getResult()
        {
            return refreshResult;
        }
        public void calculateGestureName()
        {
            //unwrap data
            List<double> unwrapdata = RfidUnwrap.MatUnwrap(phaseAngleInRadians);
            string sendStr = "";
            int length = Math.Min(phaseAngleInRadians.Count, timestampList.Count);
            for (int i = 2; i < length; ++i)   //  上传数据的时候将前15个数据减去，刚开始静止，不知道有什么影响
                sendStr += Convert.ToString(phaseAngleInRadians[i]) + " " + Convert.ToString(timestampList[i])+ "\n";
            sendStr += "end";
            System.Diagnostics.Debug.WriteLine("calculateGestureName :" + sendStr);
            try
            {
                byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
                _ = clientSocket.Send(sendBytes);
                byte[] recBytes = new byte[4096];
                int bytes = clientSocket.Receive(recBytes);
                System.Diagnostics.Debug.WriteLine("calculateGestureName :" + Encoding.ASCII.GetString(recBytes, 0, bytes));
                gesturename = Encoding.ASCII.GetString(recBytes, 0, bytes);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("calculateGestureName :" + e.StackTrace);
            }
        }
        private bool addStaticJudge(double phase)
        {
            bool flag = false;       // 如果 数据长度小于 阈值，则只添加数据，不进行跳转
            if (staticJudge.Count > ItemString.judgewindowlength)
            {
                staticJudge.RemoveRange(0, 1);
                flag = true;
            }
            staticJudge.Add(phase);
            return flag;
        }

        /// <summary>
        /// 返回Unwrap后的相位数据
        /// </summary>
        /// <returns></returns>
        public List<double> getUnwarpPhase()
        {
            return RfidUnwrap.MatUnwrap(phaseAngleInRadians);
        }
        public List<double> getPhase()
        {
            return phaseAngleInRadians;
        }
        public List<long> getTimestamps()
        {
            return timestampList;
        }
        public string getID()
        {
            return id;
        }

        /// <summary>
        /// 判断最后的40个标签是否静止
        /// </summary>
        /// <returns>方差小于系统设定的阈值</returns>
        public bool staticJudgeStatic()
        {
            System.Diagnostics.Debug.WriteLine("staticJudgeStatic： 方差" + MathUtil.varianceSequence(staticJudge));
            return MathUtil.varianceSequence(staticJudge)<ItemString.phasethreshold;
        }
        /// <summary>
        /// 存储rfid 相位和时间戳信息存储到 description+id.txt 文件中
        /// </summary>
        /// <param name="description"></param>
        public void saveFile(string description)
        {
            FileUtil.writeTwoVector(phaseAngleInRadians, timestampList, description + id);
        }
        /// <summary>
        /// 清空rfid数据
        /// </summary>
        public void clear()
        {
            timestampList.Clear();
            phaseAngleInRadians.Clear();
            staticJudge.Clear();
        }
        /// <summary>
        /// 深拷贝,将数据拷贝到rfid 中，通过引用传值方式
        /// </summary>
        /// <returns></returns>
        //public void DeepClone(RFIDData rfid)
        //{
        //    rfid.Clear();
        //    rfid.ID = ID;
        //    for(int i = 0; i < PhaseAngleInRadians.Length&&i<timestampList.Length; i++)
        //    {
        //        rfid.AddPhaseNoFilter(timestampList[i], PhaseAngleInRadians[i]);
        //    }
        //}
        ///// <summary>
        ///// 深拷贝  RFData
        ///// </summary>
        ///// <returns></returns>
        //public RFIDData DeepClone()
        //{
        //    RFIDData rfid = new RFIDData
        //    {
        //        ID = ID
        //    };
        //    for (int i = 0; i < PhaseAngleInRadians.Length && i < timestampList.Length; i++)
        //    {
        //        rfid.AddPhaseNoFilter(timestampList[i], PhaseAngleInRadians[i]);
        //    }
        //    return rfid; 
        //}
       
        ///// <summary>
        /////不加过滤，添加RFID 相位和时间戳
        ///// </summary>
        ///// <param name="relativeTime"></param>
        ///// <param name="phase"></param>
        //public void AddPhaseNoFilter(long relativeTime, double phase)
        //{

        //    timestampList.Append(relativeTime);
        //    PhaseAngleInRadians.Append(phase);
        //}
        ///// <summary>
        ///// 相对系统程序启动的时间  s*100
        ///// </summary>
        ///// <param name="relativeTime"></param>
        ///// <param name="phase"></param>
        //public void AddPhase(long relativeTime, double phase)
        //{
      
        //    timestampList.Append(relativeTime);
        //    PhaseAngleInRadians.AppendFilter(phase);
        //}
        //public LongVector getTimestampList()
        //{
        //    return timestampList;
        //}

        //public DoubleVector getPhaseAngleInRadians()
        //{
        //    return PhaseAngleInRadians;
        //}

        //public void setPhaseAngleInRadians(DoubleVector vector)
        //{
        //    this.PhaseAngleInRadians = vector;
        //}

        //public void setTimestampList(LongVector timestamps)
        //{
        //    this.timestampList = timestamps;
        //}
        //public void Clear()
        //{
        //    this.timestampList.Clear();
        //    this.PhaseAngleInRadians.Clear();
        //}
        //public void removeFirst1000()
        //{
        //    timestampList.val.RemoveRange(0, 1000);
        //    PhaseAngleInRadians.val.RemoveRange(0, 1000);
        //}
        //public RfidDTWTempData getSection(long time1,long time2,long time3)
        //{
        //    RfidDTWTempData RFIDData = new RfidDTWTempData(ID);
        //    int temp1 = MathUtil.findTimestampIndex(time1,timestampList);
        //    int temp2 = MathUtil.findTimestampIndex(time2,timestampList);
        //    int temp3 = MathUtil.findTimestampIndex(time3,timestampList);
        //    System.Diagnostics.Debug.WriteLine("getSection: index1="+temp1+" index2="+temp2+" index3="+temp3);
        //    System.Diagnostics.Debug.WriteLine("getSection: timestampList.size=" + timestampList.Length + " PhaseAngleInRadians.size=" + PhaseAngleInRadians.Length);
        //    if (temp1 == -1 || temp2 == -1 || temp3 == -1||temp2-temp1<4||temp3-temp2<50)  //temp1,,2,3 有相等值平均数为NaN
        //        return null;
        //    RFIDData.medium = MathUtil.getMediumFromArray(PhaseAngleInRadians, temp1, temp2);
        //    System.Diagnostics.Debug.WriteLine("getSection: 开始阶段平均数为" + RFIDData.medium);
        //    for (int i = temp2; i < temp3; i++)
        //    {
        //        RFIDData.AddPhase(timestampList[i], PhaseAngleInRadians[i]);
        //    }
        //    return RFIDData;
        //}
    }
}
