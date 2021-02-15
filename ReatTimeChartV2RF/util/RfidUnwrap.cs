using System;
using System.Collections.Generic;

namespace RealtimeChart
{
    public  class RfidUnwrap
    {
        /// <summary>
        /// UNWRAP（P）通过将大于pi的绝对跳跃改变为2 * pi补码来展开弧度相位P.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="cutoff"></param>
        /// <returns></returns>        
        public static List<double> MatUnwrap(List<double> vector, double cutoff = Math.PI)
        {
            //判空
            if (vector == null || vector.Count <= 0)
            {
                System.Diagnostics.Debug.WriteLine("RfidUnwrap:MatUnwrap:warring: The vector is empty!");
                return vector;
            }
            //初始化变量
            int m = vector.Count;
            List<double> dp = new List<double>();
            List<double> dp_corr = new List<double>();
            List<bool> roundDown = new List<bool>();
            List<double> cumsum = new List<double>();

            //计算递增的相位值
            //List<double> dp = new List<double>();
            for (int i = 1; i < m; i++)
            {
                dp.Add(vector[i] - vector[i - 1]);
            }

            //计算递增的相位值偏离多少个 2pi 
            //List<double> dp_corr = new List<double>();
            for (int i = 0; i < dp.Count; i++)
            {
                dp_corr.Add(dp[i] / (2 * Math.PI));
            }

            //对dp_corr进行舍入，以达到（2n+1）pi被规整到2n*pi，而不是（2n+2）pi
            //List<bool> roundDown = new List<bool>();
            for (int i = 0; i < dp_corr.Count; i++)
            {
                roundDown.Add(Math.Abs(dp_corr[i] % 1) <= 0.5);
            }

            //按以上规则初步整理数据
            for (int i = 0; i < dp_corr.Count; i++)
            {
                //朝零四舍五入
                if (roundDown[i] == true)
                {
                    if (dp_corr[i] > 0)
                    {
                        dp_corr[i] = Math.Floor(dp_corr[i]);
                    }
                    else
                    {
                        dp_corr[i] = Math.Ceiling(dp_corr[i]);
                    }
                }
                //朝最近整数四舍五入
                else
                {
                    dp_corr[i] = Math.Round(dp_corr[i]);
                }
            }

            //处理跳变，跳变大于cutoff才处理
            //cumsum，用来记录，前i个跳变的累计影响，每前一个元素的加减，后续元素都要相应变化
            for (int i = 0; i < dp.Count; i++)
            {
                if (Math.Abs(dp[i]) < cutoff)
                {
                    dp_corr[i] = 0;
                }
                if (i == 0)
                {
                    cumsum.Add(dp_corr[i]);
                }
                else
                {
                    cumsum.Add(dp_corr[i] + cumsum[i - 1]);
                }
            }

            //规整
            for (int i = 1; i < vector.Count; i++)
            {
                vector[i] -= cumsum[i - 1] * 2 * Math.PI;
            }

            //对于数据点稀疏的情况，有些点的跳变没有超过2pi,需要进一步微调
            double jumppoint = 2;
            for (int i = 0; i < vector.Count - 1; i++)
            {
                if (vector[i + 1] - vector[i] > jumppoint)
                {
                    for (int j = i + 1; j < vector.Count; j++)
                    {
                        vector[j] = vector[j] - 2 * Math.PI;
                    }
                    continue;
                }
                if (vector[i] - vector[i + 1] > jumppoint)
                {
                    for (int j = i + 1; j < vector.Count; j++)
                    {
                        vector[j] = vector[j] + 2 * Math.PI;
                    }
                }
            }

            return vector;
        }
        /// <summary>
        /// RFID phase wrapping operation
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="worth"></param>
        /// <returns></returns>
        public static List<double> Unwrap(List<double> vector, double worth)
        {
            int piCount = 0;
            for (int j = 1; j < vector.Count; j++)
            {
                vector[j] += 2 * Math.PI * piCount;
                if (vector[j] - vector[j - 1] > worth)
                {
                    piCount -= 1;
                    vector[j] -= 2 * Math.PI;
                }
                else if (vector[j] - vector[j - 1] < -worth)
                {
                    piCount += 1;
                    vector[j] += 2 * Math.PI;
                }
            }
            return vector;
        }
    }
}
