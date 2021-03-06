﻿using System;
using System.Collections.Generic;
using System.Linq;
using Impinj.OctaneSdk;
namespace RealtimeChart
{
    
    public class RFIDDeviceOp
    {
        private static Object thisLock = new Object();  //Queue 队列获取枷锁
        private static List<RFIDData> rFIDDatas;       //record all the RFID datas
        private static readonly ImpinjReader reader = new ImpinjReader(); //  Create a instance of the ImpinjReader class
        
        public RFIDDeviceOp()
        {
            rFIDDatas = new List<RFIDData>();
        }
      
        /// <summary>
        /// 这里后期更据需要存储什么数据进行修改
        /// </summary>
        /// <param name="id"></param>
        public void saveRFIDData(string desc)
        {
            try
            {
                foreach(RFIDData rFIDData in rFIDDatas)
                {
                    System.Diagnostics.Debug.WriteLine("saveRFIDData:" + rFIDData.getID());
                    rFIDData.saveFile(desc);
                    System.Diagnostics.Debug.WriteLine("saveRFIDData: OK" + rFIDData.getID());
                    //if (rFIDData.getID()==id)
                    //{
                    //    FileUtil.writeRFIDData(rFIDData, rFIDData.getID() + "_" + rFIDData.getTimestamps()[0]);
                    //    System.Diagnostics.Debug.WriteLine("saveRFIDData: OK" + rFIDData.getID() + " id=" + id);
                    //}
                }
                
            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("saveRFIDData:存储数据发生异常"+e.StackTrace);
            }
           
        }
         /// <summary>
         /// RFID Reader 相关设置
         /// </summary>
         /// <param name="settings"></param>
         /// <returns></returns>
        private Settings settingReportInformation(Settings settings)
        {
            // reader antenna port settings
            settings.Antennas.DisableAll();
            settings.Antennas.GetAntenna(3).IsEnabled = true;
            settings.Antennas.GetAntenna(3).MaxRxSensitivity = true;
            // reader readerMode settings
            settings.ReaderMode = ReaderMode.AutoSetDenseReader;
            settings.SearchMode = SearchMode.DualTarget; // for samll amount of tags
            //settings.SearchMode = SearchMode.SingleTarget; // for large amount of tags
            // Setup a tag filter.
            settings.Filters.TagFilter1.MemoryBank = MemoryBank.Epc;
            settings.Filters.TagFilter1.BitPointer = BitPointers.Epc;
            settings.Filters.TagFilter1.TagMask = ItemString.RFIDEPC;
            settings.Filters.TagFilter1.BitCount = 4 * ItemString.RFIDEPC.Length;
            settings.Filters.Mode = TagFilterMode.OnlyFilter1;
            // Report Settings
            settings.Report.IncludeAntennaPortNumber = true;
            settings.Report.Mode = ReportMode.Individual;
            settings.Report.IncludePhaseAngle = true;
            settings.Report.IncludePeakRssi = true;
            settings.Report.IncludeLastSeenTime = true;
            return settings;
        }

        /// <summary>
        /// Initializes RFID configuration
        /// </summary>
        public void initRFID()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("initRFID 阅读器链接" + ItemString.ReaderIP);
                // Connect to the reader
                reader.Connect(ItemString.ReaderIP);
                // Get the default settings
                Settings settings = reader.QueryDefaultSettings();
                // My setting
                settings = settingReportInformation(settings);
                //只能读到一个信号时的配置
                //settings.Antennas.GetAntenna(1).TxPowerInDbm = power;
                //settings.Antennas.GetAntenna(2).TxPowerInDbm = power;
                // Apply the newly modified settins
                reader.ApplySettings(settings);
                // Assign the TagsReported event handler.
                reader.TagsReported += OnTagsReported;
                if (!reader.IsConnected)
                    System.Diagnostics.Debug.WriteLine("initRFID 阅读器链接失败");
                reader.Start();                               //这里 如果Reader.stop() 执行后在执行start（） 则就报错
                System.Diagnostics.Debug.WriteLine("initRFID 阅读器设置完成");

            }
            catch (OctaneSdkException e)
            {
                //Headle Octane SDK errors.
                System.Diagnostics.Debug.WriteLine("initRFID： OctaneSdkException : {0} 阅读器链接失败", e.Message);
            }
            catch (Exception e)
            {
                //Handle other .Net errors.
                System.Diagnostics.Debug.WriteLine("initRFID： Exception : {0}", e.Message);
            }
        }
        /// <summary>
        /// This event handler is called asynchronously
        /// when tag reports are available.
        /// Loop through each tag in the report
        /// and print the data.
        /// My Output to the file
        /// </summary>
        private void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            foreach (Tag tag in report)
            {
                if (!ItemString.RFIDControlReaderState)
                {
                   // System.Diagnostics.Debug.WriteLine("OnTagsReported:RFID 绘制结束stop");
                    continue;
                }
                else
                {
                    try
                    {
                        //System.Diagnostics.Debug.WriteLine("OnTagsReported:add data:EPC" + tag.Epc.ToString());
                        //if (tag.Epc.ToString() == ItemString.RFIDEPC)
                        //{
                            int rId = rFIDDatas.FindIndex(a => a.getID() == tag.Epc.ToString());
                            //System.Diagnostics.Debug.WriteLine("OnTagsReported:EPC" + tag.Epc.ToString());
                            if (rId < 0)
                            {
                                rFIDDatas.Add(new RFIDData(tag.Epc.ToString()));
                                rId = 0;
                            }
                            //System.Diagnostics.Debug.WriteLine("OnTagsReported:add data:EPC" + tag.Epc.ToString());
                            rFIDDatas[rId].stateMachineChanging(tag.PhaseAngleInRadians, TimeUtil.getReletiveToStartProgramSeconds());
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("OnTagsReported:出现异常 "+e.StackTrace);
                    }
                }
            }
        }
        /// <summary>
        /// 通过置为 标志位 实现数据是否存储
        /// </summary>
        public void restartReader()
        {
            if(!ItemString.RFIDControlReaderState)
            {
                ItemString.RFIDControlReaderState = true;
                System.Diagnostics.Debug.WriteLine("restartReader 打开阅读器");
            }
        }
        /// <summary>
        /// 通过置为标志位，实现不存储
        /// </summary>
        public void stopReader()
        {
            if (ItemString.RFIDControlReaderState)
            {
                ItemString.RFIDControlReaderState = false;
                System.Diagnostics.Debug.WriteLine("stopReader 关闭阅读器");
            }
        }
        public void clearRfidAllData()
        {
            rFIDDatas.Clear();

        }
        /// <summary>
        /// 获取RFID 所有数据
        /// </summary>
        /// <returns></returns>
        public List<RFIDData> getRFIDDatas()
        {
            return rFIDDatas;
        }
        /// <summary>
        /// 关闭RFID 连接
        /// </summary>
        public void close()
        {
            // Close RFID
            reader.Stop();
            reader.Disconnect();
        }


        /// <summary>
        /// 用于每个画手势之间  存储所有的RFID 相位比较图片
        /// </summary>
        /// <param name="description">文件特征名</param>
        //public void testSaveAllComparebleRFIDPhase(string description)
        //{
        //    List<DoubleVector> doubleVectors=new List<DoubleVector>();
        //    for (int i = 0; i < RFIDDatas.Count(); i++)
        //    {
        //        if (!DataProcessing.isRFIDDataAvailable(RFIDDatas[i].getPhaseAngleInRadians())&&DataProcessing.isRFIDStatic(RFIDDatas[i].getPhaseAngleInRadians().val,1.0))
        //            continue;
        //        doubleVectors.Add(RFIDDatas[i].getPhaseAngleInRadians());
        //        DataRecord.writeKinectPhase(RFIDDatas[i].getPhaseAngleInRadians(), RFIDDatas[i].getTimestampList(),description+RFIDDatas[i].ID+".txt");
        //    }
        //    System.Diagnostics.Debug.WriteLine("testSaveAllComparebleRFIDPhase：写入RFID比较数据到文件中,RFID数据大小："+doubleVectors.Count);
        //    DataRecord.drawComparePicture(description, doubleVectors);
        //}
        /// <summary>
        /// 存储所有的RFID 相位数据到文件中
        /// </summary>
        /// <param name="description">文件描述名</param>
        //public void saveAllRFIDPhaseToFile(string description)
        //{
        //    for (int i = 0; i < RFIDDatas.Count(); i++)
        //    {
        //        if (!DataProcessing.isRFIDDataAvailable(RFIDDatas[i].getPhaseAngleInRadians()))
        //            continue;
        //        //Toto 添加RFID过滤判断
        //        System.Diagnostics.Debug.WriteLine("saveAllRFIDPhaseToFile 开始写入: ID = " + RFIDDatas[i].ID + "  count = " + RFIDDatas[i].getPhaseAngleInRadians().Length);
        //        //绘制RFIDphase 到文件并作图
        //        DataRecord.writeRFIDPhase(RFIDDatas[i].getPhaseAngleInRadians(), RFIDDatas[i].getTimestampList(), description +"_"+ RFIDDatas[i].ID + ".txt");
        //        DataRecord.drawPicutre(description + "_" + RFIDDatas[i].ID, RFIDDatas[i].getPhaseAngleInRadians());
        //        List<double> rfidUnwrapVectorList = MatUnwrap(RFIDDatas[i].getPhaseAngleInRadians().val);
        //        FilterMethod.UnscentedKalmanFilterMethod(rfidUnwrapVectorList);
        //        DataRecord.drawPicutre(description + "_Unwarp" + RFIDDatas[i].ID, rfidUnwrapVectorList);
        //    }
        //}
        /// <summary>
        /// 用于观察RFID 相关处理操作，   其中RFID数据都是原始数据
        /// 保存所有RFID 数据到文件中
        /// 保存原始数据
        /// 保存经过UKF 算法过滤后的数据
        /// 保存经过去重处理后的数据
        /// 保存unwarp 后的数据
        /// 保存经过前后静止去除后数据
        /// </summary>
        /// <param name="filename">保存的文件名，高度概括该文件内容</param>
        //public void testSaveHandleAllRelevantAllRFIDPhaseToFile(string description)
        //{
        //    for (int i = 0; i < RFIDDatas.Count(); i++)
        //    {
        //        System.Diagnostics.Debug.WriteLine("saveAllRFIDPhaseToFile 开始写入: ID = " + RFIDDatas[i].ID + "  count = " + RFIDDatas[i].getPhaseAngleInRadians().Length);
        //        //绘制RFIDphase 到文件并作图
        //        DataRecord.writeRFIDPhase(RFIDDatas[i].getPhaseAngleInRadians(), RFIDDatas[i].getTimestampList(), description + RFIDDatas[i].ID + ".txt");
        //        DataRecord.drawPicutre(description + RFIDDatas[i].ID, RFIDDatas[i].getPhaseAngleInRadians());
        //        UKFFilterOneRFIDPhase(RFIDDatas[i]);
        //        DataRecord.drawPicutre(description + RFIDDatas[i].ID + "UKF" + RFIDDatas[i].ID, RFIDDatas[i].getPhaseAngleInRadians());
        //        //-----------以下测试代码-----------------
        //        DataProcessing.getDistinctTimeStamp(RFIDDatas[i].getTimestampList().ToArray(), RFIDDatas[i].getPhaseAngleInRadians().val);   //rfidTimeStampList, rfidVectorList  原始的RFID时间戳和相位数据
        //        //过滤掉相位为空的标签
        //        if (RFIDDatas[i].getPhaseAngleInRadians().Length <= 15)     //  这里设置 匹配的RFID 最小长度为 15
        //        {
        //            System.Diagnostics.Debug.WriteLine("RFID distint count: " + RFIDDatas[i].getPhaseAngleInRadians().Length);
        //            continue;
        //        }
        //        //-----------以下测试代码-----------------
        //        //存储时间戳去重后的的RFID phase-timestamp 数据
        //        //DataRecord.writeRFIDPhase(RFIDDatas[i].getPhaseAngleInRadians(), RFIDDatas[i].getTimestampList(), description+ RFIDDatas[i].ID + "_rfidUKFDphase.txt");
        //        DataRecord.drawPicutre(description + RFIDDatas[i].ID + "rfidAfterUKFDoriphase", RFIDDatas[i].getPhaseAngleInRadians());

        //        List<double> rfidUnwrapVectorList = MatUnwrap(RFIDDatas[i].getPhaseAngleInRadians().val);
        //        //-----------以下测试代码-----------------
        //        //存储phase unwrapping 后的的RFID phase-timestamp 数据
        //        //DataRecord.writeRFIDPhase(RFIDData.getPhaseAngleInRadians(), RFIDData.getTimestampList(), description+ RFIDDatas[i].ID + "_rfidUnwrappingphase.txt");
        //        DataRecord.drawPicutre(description + RFIDDatas[i].ID + "_rfidUnwrappingphase", RFIDDatas[i].getPhaseAngleInRadians());
        //        // rfidUnwrapVectorList  RFID 相位unwrap 后的数据
        //        //if (rfidUnwrapVectorList.Max() - rfidUnwrapVectorList.Min() < Math.PI)
        //        //{
        //        //    System.Diagnostics.Debug.WriteLine("经过unwrapping处理，相位变化比较小的，匹配下一个 RFID相位信息");
        //        //    continue;
        //        //}
        //        //这个地方确定单人和多人情况下的影响,观察方差是否一致
        //        if (DataProcessing.isRFIDStatic(RFIDDatas[i].getPhaseAngleInRadians().ToArray(), 0.03))                  //这里的方差 设置合理吗
        //        {
        //            System.Diagnostics.Debug.WriteLine(" 方差比较小");
        //            continue;
        //        }
        //        if (!DataProcessing.handleRFIDPhase(RFIDDatas[i].getPhaseAngleInRadians(), RFIDDatas[i].getTimestampList(), description + RFIDDatas[i].ID))
        //            continue;
        //    }
        //}
    }
}
