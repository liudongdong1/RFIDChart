using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeChart
{
    public class ItemString
    {
        public static string ReaderIP = "192.168.0.221";   //RFID 阅读器标签
        public static bool RFIDControlReaderState = false;  //是否记录RFID 数据，true 记录，false 不记录
        public static string baseFolder= "D:\\RFID_project\\RealtimeChartOutput\\";
        public static string RFIDEPC = "D113";

        public static int port = 12000;   //CNN port
        public static string host = "127.0.0.1";  //CNN port

        public static double phasethreshold = 1;
        public static int datawindowlength = 800;       // 一般一个完整的过程 数据在 400左右
        public static int judgewindowlength = 60;

        public static string windowinformation = "Status";

    }
}
