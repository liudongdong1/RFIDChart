using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeChart
{
    public class TimeUtil
    {
        private static Double programReletiveSeconds=10; //系统启动时间
        public static void setProgramStartedTime(DateTime dateTime)
        {
            if (programReletiveSeconds <100)
            {
                System.Diagnostics.Debug.WriteLine("programReletiveSeconds is null");       
                DateTime centuryBegin = new DateTime(2001, 1, 1);
                long elapsedTicks = dateTime.Ticks - centuryBegin.Ticks;  //  单位/个  计时周期
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                System.Diagnostics.Debug.WriteLine("programReletiveSeconds: " + elapsedSpan.TotalSeconds);
                programReletiveSeconds= elapsedSpan.TotalSeconds;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("programReletiveSeconds is not null");
            }
        }
       public static void TestUTCNow_Now_Time()
        {
            DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            System.Diagnostics.Debug.WriteLine("UTCNow Time:String: " + DateTime.UtcNow);
            System.Diagnostics.Debug.WriteLine("UTCNow Time:String: " + DateTime.UtcNow.ToString());
            System.Diagnostics.Debug.WriteLine("UTCNow Time:String: " + DateTime.UtcNow.Ticks);
            System.Diagnostics.Debug.WriteLine("UTCNow Time:String: " + DateTime.UtcNow.TimeOfDay);

            System.Diagnostics.Debug.WriteLine("Now Time:String: " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("Now Time:String: " + DateTime.Now.ToString());
            System.Diagnostics.Debug.WriteLine("Now Time:String: " + DateTime.Now.Ticks);
            System.Diagnostics.Debug.WriteLine("Now Time:String: " + DateTime.Now.TimeOfDay);
            long unixTime = (DateTime.UtcNow - startTime).Ticks / 10;
            System.Diagnostics.Debug.WriteLine("unixTime: " + unixTime);
            //UTCNow Time:String: 2019 / 12 / 31 2:42:43      //UTC 时间
            //UTCNow Time:String: 2019 / 12 / 31 2:42:43
            //UTCNow Time:String: 637133569633693202
            //UTCNow Time:String: 02:42:43.3693202
            //Now Time:String: 2019 / 12 / 31 10:42:43        //本地电脑时间
            //Now Time:String: 2019 / 12 / 31 10:42:43
            //Now Time:String: 637133857633693202
            //Now Time:String: 10:42:43.3693202
            //unixTime: 1577760163369320

            //36488999580 
        }
        static double ToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (double)span.TotalSeconds;
        }

        static DateTime ConvertTimestamp(double timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddSeconds(timestamp);
            return newDateTime.ToLocalTime();
        }
        public  static void TestMouseLess()
        {
            //Unix时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数。
            //转换： 1 second(s) = 1000 millisecond(ms) = 10 x 100 0000 one ten-millionth of a second(Ticks)
            //DateTime.Now.Ticks / 10000 即为当前时间的总毫秒值
            //1 秒间隔的判断
            //if (DateTime.Now.Ticks / 10000 - _lastCmdTime_ms >= 1000)
            System.Diagnostics.Debug.WriteLine("green :" + new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime().Ticks);
            System.Diagnostics.Debug.WriteLine("获取毫秒" + (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
           
        }
        /// <summary>
        /// 获取相对程序启动的 时间s*100 000
        /// </summary>
        /// <returns>实际的秒数*1000</returns>
        public static long getReletiveToStartProgramSeconds()
        {
            DateTime centuryBegin = new DateTime(2001, 1, 1);
            DateTime currentDate = DateTime.Now;
            //  一个计时周期表示一百纳秒，即一千万分之一秒。 1 毫秒内有 10,000 个计时周期，即 1 秒内有 1,000 万个计时周期。
            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;  //  单位/个  计时周期
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            //temp.ToString("F3") 获取3位小数
           // System.Diagnostics.Debug.WriteLine("getReletiveToStartProgramSeconds:"+(elapsedSpan.TotalSeconds - programReletiveSeconds)+" - "+ elapsedSpan.TotalSeconds+"- "+ programReletiveSeconds);
            long temp = (int)((elapsedSpan.TotalSeconds - programReletiveSeconds)*100000);
            //System.Diagnostics.Debug.WriteLine("getReletiveToStartProgramSeconds: " + elapsedSpan.TotalSeconds+"  "+temp);
            return temp;
            //System.Diagnostics.Debug.WriteLine("Elapsed from the beginning of the century to {0:f}:",
            //                   currentDate);
            //System.Diagnostics.Debug.WriteLine("   {0:N0} nanoseconds", elapsedTicks * 100);
            //System.Diagnostics.Debug.WriteLine("   {0:N0} ticks", elapsedTicks);
            //System.Diagnostics.Debug.WriteLine("   {0:N2} seconds", elapsedSpan.TotalSeconds);
            //System.Diagnostics.Debug.WriteLine("   {0:N2} minutes", elapsedSpan.TotalMinutes);
            //System.Diagnostics.Debug.WriteLine("   {0:N0} days, {1} hours, {2} minutes, {3} seconds",
            //                  elapsedSpan.Days, elapsedSpan.Hours,
            //                  elapsedSpan.Minutes, elapsedSpan.Seconds);
            // This example displays an output similar to the following:
            // 
            // Elapsed from the beginning of the century to Thursday, 14 November 2019 18:21:
            //    595,448,498,171,000,000 nanoseconds
            //    5,954,484,981,710,000 ticks
            //    595,448,498.17 seconds
            //    9,924,141.64 minutes
            //    6,891 days, 18 hours, 21 minutes, 38 seconds
            //此属性的值表示自0001年1月1日午夜12月1日午夜 12:00:00 0:00:00 （公历日期为0001年1月1日）的100纳秒间隔数，表示 DateTime.MinValue。 它不包括由闰秒组成的计时周期数。
        }
        public static long DateTimeToTimeStamp(DateTime time)
        {
            DateTime startTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            long unixTime = (time - startTime).Ticks / 10;
            return unixTime;
        }

        public static DateTime TimeStampToDateTime(string timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToDouble(timestamp) / 1000);
        }
    }
}
