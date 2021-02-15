using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private List<double> peakRssiInDbm;
        private List<double> phaseAngleInRadians;
        private List<long> timestampList;

        public RFIDData()
        {
            id = null;
            peakRssiInDbm = new List<double>();
            phaseAngleInRadians = new List<double>();
            timestampList = new List<long>();
        }
        public RFIDData(string rfidid)
        {
            id = rfidid;
            peakRssiInDbm = new List<double>();
            phaseAngleInRadians = new List<double>();
            timestampList = new List<long>();
        }
        public void addRSSPhaseTime(double rss,double phase,long time)
        {
            peakRssiInDbm.Add(rss);
            phaseAngleInRadians.Add(phase);
            timestampList.Add(time);
        }
        /// <summary>
        /// 返回Unwrap后的相位数据
        /// </summary>
        /// <returns></returns>
        public List<double> getUnwarpPhase()
        {
            return RfidUnwrap.MatUnwrap(phaseAngleInRadians);
        }
        public List<double> getpeakRssiInDbm()
        {
            return peakRssiInDbm;
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
