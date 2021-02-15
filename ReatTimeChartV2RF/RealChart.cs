using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RealtimeChart
{
    public partial class RealChart : Form
    {
        private List<double> showdata = new List<double>();
        private RFIDDeviceOp rFIDDeviceOp = new RFIDDeviceOp();
        private int curValue = 0;
        private int PhaseMax=12;
        private int PhaseMin = -1;
        public RealChart()
        {
            InitializeComponent();
            rFIDDeviceOp.initRFID();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btnInit_Click: init_OK");
            InitChart();
            System.Diagnostics.Debug.WriteLine("btnInit_click");
            rFIDDeviceOp.clearRfidAllData();
        }

        /// <summary>
        /// 开始事件，并开始记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btnStart_Click: Start");
            this.timer1.Start();
            rFIDDeviceOp.restartReader();
        }

        /// <summary>
        /// 暂停阅读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btnStop_Click: stop");
            this.timer1.Stop();
            rFIDDeviceOp.stopReader();
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //showdata.Clear();
         //   System.Diagnostics.Debug.WriteLine("timer1_Tick: showdata.size="+showdata.Count);
            //System.Diagnostics.Debug.WriteLine("timer1_Tick: showdata.size="+showdata.Count+"max="+showdata.Max()+" min="+showdata.Min());
            this.chart1.Series[0].Points.Clear();
            //System.Diagnostics.Debug.WriteLine("timer1_Tick：");
            if (rFIDDeviceOp.getRFIDDatas().Count > 0)
            {
                //System.Diagnostics.Debug.WriteLine("timer1_Tick: size", rFIDDeviceOp.getRFIDDatas()[0].getPhase().Count);
                this.richTextBox1.Text = rFIDDeviceOp.getRFIDDatas()[0].getResult();
                showdata = rFIDDeviceOp.getRFIDDatas()[0].getPhase();
                for (int i = 0; i < showdata.Count; i++)
                {
                    // System.Diagnostics.Debug.WriteLine("timer1_Tick: showdata=" + showdata[i]);
                    this.chart1.Series[0].Points.AddXY((i + 1), showdata[i]);
                }
            }
            
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void InitChart()
        {
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisY.Minimum = PhaseMin;
            this.chart1.ChartAreas[0].AxisY.Maximum = PhaseMax;       //设置y显示的最大最小值，这个需要后期观察
            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "RFID 数据显示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //设置图表显示样式
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Titles[0].Text = string.Format("RFID Phase 数据显示");
            this.chart1.Series[0].ChartType = SeriesChartType.Spline;
            this.chart1.Series[0].Points.Clear();
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            rFIDDeviceOp.saveRFIDData(TimeUtil.getReletiveToStartProgramSeconds().ToString()+ ItemString.RFIDEPC);        //这里输入相应的index，或者ID
            rFIDDeviceOp.clearRfidAllData();
            System.Diagnostics.Debug.WriteLine("saveBtn_Click: Ok");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //this.richTextBox1.Text = ItemString.windowinformation;
        }
    }
}
