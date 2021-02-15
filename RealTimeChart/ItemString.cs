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
        public static string baseFolder="C:\\project\\RealtimeChart\\RealtimeChart\\ouputFolder\\";
        public static string RFIDEPC = "101A";
    }
}
