using System;
using System.Collections.Generic;
using System.IO;
using ZedGraph;  // Version=1.0.1812.29779, 5.几那个版本有问题
using System.Drawing;
using System.Drawing.Imaging;
namespace RealtimeChart
{
    public static class FileUtil
    {
        /// <summary>
        /// 如果目录不存在，则创建目录
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        public static Boolean createFolder(string foldername)
        {
            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }
            return true;
        }
        /// <summary>
        /// 绘制波形图  一维度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="description">ItemString.baseFolder + description + ".png"文件名,没有后缀，建议用y轴含义</param>
        public static void drawPicutre(string description, List<double> data)
        {
            GraphPane myPane = new GraphPane(new RectangleF(0, 0, 3200, 2400), description, "number", description);
            PointPairList measurementsPairs = new PointPairList();
            for (int i = 0; i < data.Count; i++)
            {
                measurementsPairs.Add(i, data[i]);
            }
            _ = myPane.AddCurve(description, measurementsPairs, Color.Red, SymbolType.Circle);
            Bitmap bm = new Bitmap(200, 200);
            Graphics g = Graphics.FromImage(bm);
            myPane.AxisChange(g);
            Image im = myPane.Image;
            im.Save(ItemString.baseFolder + description + ".png", ImageFormat.Png);
        }
        /// <summary>
        /// 存储RFID 数据值，安装 RSS，Phase，timestamp 顺序存储到文件中
        /// </summary>
        /// <param name="rFID"></param>
        /// <param name="description"></param>
        public static void writeRFIDData(RFIDData rFID,string description)
        {
            writeThreeVector(rFID.getpeakRssiInDbm(), rFID.getUnwarpPhase(), rFID.getTimestamps(), description + rFID.getID());
        }

        public static void writeThreeVector(List<double> data1, List<double> data2, List<long> timestamp, string description)
        {
            description = ItemString.baseFolder + description+".txt";
            StreamWriter sw = new StreamWriter(description);
            for (int i = 0; i < data1.Count&&i<data2.Count; ++i)
            {
                sw.WriteLine(string.Format("{0} {1} {2}", data1[i], data2[i], timestamp[i]));
            }
            sw.Close();
        }
        /// <summary>
        /// 从文件中读取RFID data
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>RFID 类</returns>
        public static RFIDData readRFIDData(string filename)
        {
            filename = ItemString.baseFolder + filename;
            System.Diagnostics.Debug.WriteLine("readRFIDData:" + filename);
            StreamReader sr = new StreamReader(filename);
            string line = null;
            RFIDData rFIDData = new RFIDData();
            while ((line = sr.ReadLine()) != null)
            {
                string[] temp = line.Split();
                // System.Diagnostics.Debug.WriteLine(temp[0]+"  "+temp[1]+" "+temp[2]);
                if (temp.Length < 3)
                    continue;
                // System.Diagnostics.Debug.WriteLine("test: " + Convert.ToDouble(temp[1]));
                rFIDData.addRSSPhaseTime(Convert.ToDouble(temp[0]), Convert.ToDouble(temp[1]), Convert.ToInt64(temp[2]));
            }
            System.Diagnostics.Debug.WriteLine("readRFIDData：从文件读取完成--大小分别为：" + rFIDData.getTimestamps().Count);
            sr.Close();
            return rFIDData;
        }
        public static string getFolderName(string path, string key)
        {
            DirectoryInfo theFolder = new DirectoryInfo(path);
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in theFolder.GetDirectories())
            {
                if (NextFolder.Name.ToUpper().IndexOf(key.ToUpper()) >= 0)
                {
                    return NextFolder.Name;
                }
            }
            return " ";
        }
        /// <summary>
        /// 获取一个目录下所有txt 文件
        /// </summary>
        /// <param name="foldername">文件目录： baseFolder+foldername</param>
        /// <returns>返回目录下所有 txt 文件名列表</returns>
        public static List<string> getAllFolderFiles(string foldername = " ")
        {
            List<string> fileNames = new List<string>();
            string foler = ItemString.baseFolder + foldername;
            DirectoryInfo theFolder = new DirectoryInfo(foler);
            //遍历文件
            foreach (FileInfo NextFile in theFolder.GetFiles())
            {
                if (NextFile.Extension == ".txt")
                {
                    fileNames.Add(NextFile.Name);  //E:\RFIDKinecttestData\20_1_12_collectdata\phase\10586579allCompareRFIDPhase0003.txt'
                                                   //  System.Diagnostics.Debug.WriteLine(" 文件名为： " + NextFile.FullName);
                }
            }
            return fileNames;
        }
        /// <summary>
        /// 绘制 根目录下所有Kinect 的相位信息
        /// </summary>
        //public static void DrawKinectPhaseAllFolderFiles()
        //{
        //    List<string> files = getAllFolderFiles("gesture");
        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        KinectData kinectData = readKinectXYZTimestamp(files[i]);
        //        kinectData.updatePhase();
        //        drawPicutre("gesture\\" + files[i], kinectData.getPhaseList());
        //    }
        //}
    }
}
