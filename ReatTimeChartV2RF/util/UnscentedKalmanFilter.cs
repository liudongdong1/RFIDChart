using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnscentedKalmanFilter;

//using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.LinearAlgebra.Double;
//using MathNet.Numerics.LinearAlgebra.Factorization;


namespace RealtimeChart
{

    public class FilterMethod
    {
        /// <summary>
        /// UnscentedKalmanFilter 对一维数据进行处理
        /// </summary>
        /// <param name="data">使用引用传值方式修改data</param>
        public static void UnscentedKalmanFilterMethod(List<double> data)
        {
            var filter = new UKF();
            for (int i = 0; i < data.Count; i++)
            {
                filter.Update(new[] { data[i] });
                // filter.Update(new[] { data1[i], data2[i] });
                //filter.Update(new[] { data1[i], data2[i], data3[i] });
                try
                {
                    data[i] = filter.getState()[0];
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("UnscentedKalmanFilterMethod:filter index 超出");
                }
            }
        }
    }
}
