using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeChart.util
{
    public class MathUtil
    {
        public static double varianceSequence(List<double> data)
        {
            double mean = data.Average();
            double squaresum = data.Sum(t =>Math.Pow(t-mean, 2));
            return squaresum /(data.Count() - 1);
        }
        public static double MaxMinDistance(List<double> data)
        {
            return data.Max() - data.Min();
        }
    }
}
