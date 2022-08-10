using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PA2GuiVolcanoRobot
{
    public partial class graph : UserControl
    {
        int selectTab = 0;

        public const int altitude = 0;
        public const int humadity = 1;
        public const int co = 2;
        public const int battery = 3;
        public const int compass = 4;
        public const int temperature = 5;
        public const int h2s = 6;
        public const int speed = 7;
        public const int roll = 8;
        public const int pitch = 9;
        public const int yaw = 10;
        public const int co2 = 11;
        public const int nh4 = 12;
        public const int latt = 13;
        public const int lngg = 14;
        struct sensorr
        {
            public string id_name;
            public int value;
        }
        sensorr[] sensor = new sensorr[12];
        public int selectUGVUAV = 0;

        Chart[] chartGraph = new Chart[4];
        int[] indeksGraph = new int[4];
        int[] minValue = new int[4];
        int[] maxValue = new int[4];

        bool record = false;

        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string dirName;

        string dirNameUGV;
        string dirNameUAV;
        string[,] textFile = new string[2, 12];
        string[] conTextFile = new string[2];

        public graph()
        {
            initSensor();

            dirName = $@"{docPath}\Record UGV UAV";
            dirNameUGV = $@"{dirName}\" + "Record UGV";
            dirNameUAV = $@"{dirName}\" + "Record UAV";
            conTextFile[0] = $@"{dirNameUGV}\" + "recordConnection.txt";
            conTextFile[1] = $@"{dirNameUAV}\" + "recordConnection.txt";

            for (int i = 0; i < 12; i++)
            {
                textFile[0, i] = $@"{dirNameUGV}\" + sensor[i].id_name + ".txt";
                textFile[1, i] = $@"{dirNameUAV}\" + sensor[i].id_name + ".txt";
            }

            for (int i = 0; i < 4; i++) 
            {
                indeksGraph[i] = 0;
                minValue[i] = 0;
                maxValue[i] = 0;
            }
            InitializeComponent();
        }
        public void saveConnection(double val)
        {
            if (record)
            {
                File.AppendAllText(conTextFile[selectUGVUAV], val.ToString() + "\n");
            }
        }
        public void setValueSensor(int codeSensor, int val)
        {
            switch (codeSensor)
            {
                case humadity:
                    sensor[0].value = val;
                    if (record)
                    {
                        writeText(0, val);
                    }
                    break;
                case temperature:
                    sensor[1].value = val/10;
                    if (record)
                    {
                        writeText(1, val);
                    }
                    break;
                case battery:
                    sensor[2].value = val;
                    if (record)
                    {
                        writeText(2, val);
                    }
                    break;
                case altitude:
                    sensor[3].value = val;
                    if (record)
                    {
                        writeText(3, val);
                    }
                    break;
                case co:
                    sensor[4].value = val;
                    if (record)
                    {
                        writeText(4, val);
                    }
                    break;
                case h2s:
                    sensor[5].value = val;
                    if (record)
                    {
                        writeText(5, val);
                    }
                    break;
                case compass:
                    sensor[8].value = val/10;
                    if (record)
                    {
                        writeText(8, val);
                    }
                    break;
                case roll:
                    sensor[9].value = val;
                    if (record)
                    {
                        writeText(9, val);
                    }
                    break;
                case pitch:
                    sensor[10].value = val;
                    if (record)
                    {
                        writeText(10, val);
                    }
                    break;
                case yaw:
                    sensor[11].value = val;
                    if (record)
                    {
                        writeText(11, val);
                    }
                    break;
                default:
                    break;
            }
            if (codeSensor < 12)
            {
                //processValueString(codeSensor);

                
            }
        }
        private void writeText(int access, int value)
        {
            File.AppendAllText(textFile[selectUGVUAV, access], value.ToString() + "\n");
        }
        private void initSensor()
        {
            //pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("temperature");
            sensor[0].id_name = "HUMIDITY";
            sensor[0].value = 0;

            sensor[1].id_name = "TEMPERATURE";
            sensor[1].value = 0;

            sensor[2].id_name = "BATTERY";
            sensor[2].value = 0;

            sensor[3].id_name = "ALTITUDE";
            sensor[3].value = 0;

            sensor[4].id_name = "CO";
            sensor[4].value = 0;

            sensor[5].id_name = "H" + '\u2082' + "S";
            sensor[5].value = 0;

            sensor[6].id_name = "\0";
            sensor[6].value = 0;

            sensor[7].id_name = "\0";
            sensor[7].value = 0;

            sensor[8].id_name = "COMPASS";
            sensor[8].value = 0;

            sensor[9].id_name = "ROLL";
            sensor[9].value = 0;

            sensor[10].id_name = "PITCH";
            sensor[10].value = 0;

            sensor[11].id_name = "YAW";
            sensor[11].value = 0;
        }
        private void changeSensor(sensorr sensor1, sensorr sensor2, sensorr sensor3, sensorr sensor4)
        {
            label4.Text = sensor1.id_name;
            label1.Text = sensor2.id_name;
            label2.Text = sensor3.id_name;
            label3.Text = sensor4.id_name;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void graph_Load(object sender, EventArgs e)
        {
            changeSensor(sensor[0], sensor[1], sensor[2], sensor[3]);
            chartGraph[0] = this.chart1;
            chartGraph[1] = this.chart2;
            chartGraph[2] = this.chart3;
            chartGraph[3] = this.chart4;
            for(int i = 0; i < 4; i++)
            {
                chartGraph[i].Series[0].ChartType = SeriesChartType.Line;
            }
            initPathfolder();
            timer1.Tick += timer1_Tick;
            timer1.Interval = 100;
            timer1.Start();
        }
        private void initPathfolder()
        {
            if (!Directory.Exists(dirName))
            {
                //Buat directory record UGV UAV
                Directory.CreateDirectory(dirName);
            }

            if (!Directory.Exists(dirNameUGV))
            {
                Directory.CreateDirectory(dirNameUGV);
            }

            if (!Directory.Exists(dirNameUAV))
            {
                Directory.CreateDirectory(dirNameUAV);
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 6) { continue; }
                if (i == 7) { continue; }
                if (!File.Exists(textFile[0, i]))
                {
                    File.Create(textFile[0, i]);
                }
                if (!File.Exists(textFile[1, i]))
                {
                    File.Create(textFile[1, i]);
                }
            }
            if (!File.Exists(conTextFile[0]))
            {
                File.Create(conTextFile[0]);
            }
            if (!File.Exists(conTextFile[1]))
            {
                File.Create(conTextFile[1]);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i < 4; i++)
            {
                chartGraph[i].Series[0].Points.AddXY(indeksGraph[i],sensor[4*selectTab+i].value);
                if(chartGraph[i].Series[0].Points.Count > 500)
                {
                    chartGraph[i].Series[0].Points.RemoveAt(0);
                }
                minValue[i] = (int)chartGraph[i].Series[0].Points[0].XValue;
                maxValue[i] = indeksGraph[i];
                if (minValue[i] > maxValue[i])
                {
                    minValue[i] = maxValue[i];
                }
                chartGraph[i].ChartAreas[0].AxisX.Minimum = minValue[i];
                chartGraph[i].ChartAreas[0].AxisX.Maximum = indeksGraph[i];
                indeksGraph[i]++;
            }
            /*
            int minValue0, maxValue0;
            int minValue1, maxValue1;
            int minValue2, maxValue2;
            int minValue3, maxValue3;
            chart1.Series[0].Points.AddXY(indeksGraph[0], 3 * Math.Sin(5 * indeksGraph[0]));
            chart2.Series[0].Points.AddXY(indeksGraph[1], 30 * Math.Sin(5 * indeksGraph[1]));
            chart3.Series[0].Points.AddXY(indeksGraph[2], 3 * Math.Sin(5 * indeksGraph[2]));
            chart4.Series[0].Points.AddXY(indeksGraph[3], 3 * Math.Sin(5 * indeksGraph[3]));
            if (chart1.Series[0].Points.Count > 500)
            {
                chart1.Series[0].Points.RemoveAt(0);
            }
            if(chart2.Series[0].Points.Count > 500)
            {
                chart2.Series[0].Points.RemoveAt(0);
            }
            if (chart3.Series[0].Points.Count > 500)
            {
                chart3.Series[0].Points.RemoveAt(0);
            }
            if (chart4.Series[0].Points.Count > 500)
            {
                chart4.Series[0].Points.RemoveAt(0);
            }
            minValue0 = (int)chart1.Series[0].Points[0].XValue;
            maxValue0 = indeksGraph[0];
            if (minValue0 > maxValue0)
            {
                minValue0 = maxValue0;
            }
            chart1.ChartAreas[0].AxisX.Minimum = minValue0;
            chart1.ChartAreas[0].AxisX.Maximum = indeksGraph[0];


            minValue1 = (int)chart2.Series[0].Points[0].XValue;
            maxValue1 = indeksGraph[1];
            if (minValue1 > maxValue1)
            {
                minValue1 = maxValue1;
            }
            chart2.ChartAreas[0].AxisX.Minimum = minValue1;
            chart2.ChartAreas[0].AxisX.Maximum = indeksGraph[1];

            minValue2 = (int)chart3.Series[0].Points[0].XValue;
            maxValue2 = indeksGraph[2];
            if (minValue2 > maxValue2)
            {
                minValue2 = maxValue2;
            }
            chart3.ChartAreas[0].AxisX.Minimum = minValue2;
            chart3.ChartAreas[0].AxisX.Maximum = indeksGraph[2];

            minValue3 = (int)chart4.Series[0].Points[0].XValue;
            maxValue3 = indeksGraph[3];
            if (minValue3 > maxValue3)
            {
                minValue3 = maxValue3;
            }
            chart4.ChartAreas[0].AxisX.Minimum = minValue3;
            chart4.ChartAreas[0].AxisX.Maximum = indeksGraph[3];



            indeksGraph[0]++;
            indeksGraph[1]++;
            indeksGraph[2]++;
            indeksGraph[3]++;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {/*
            timer1.Stop();
            foreach (var series in chart2.Series)
            {
                series.Points.Clear();
            }
            indeksGraph[1] = 0;
            timer1.Start();*/
        }

        private void graph_SizeChanged(object sender, EventArgs e)
        {
            chart1.Height = 100 + (int)(this.Height/4.5-561/4.5);
            chart2.Height = chart1.Height;
            chart3.Height = chart1.Height;
            chart4.Height = chart1.Height;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            selectTab++;
            if (selectTab > 2) selectTab = 2;
            changeSensor(sensor[4 * selectTab], sensor[4 * selectTab + 1], sensor[4 * selectTab + 2], sensor[4 * selectTab + 3]);
            timer1.Stop();
            for(int i = 0; i < 4; i++)
            {
                foreach (var series in chartGraph[i].Series)
                {
                    series.Points.Clear();
                }
                indeksGraph[i] = 0;
            }
            timer1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            selectTab--;
            if (selectTab < 0) selectTab = 0;
            changeSensor(sensor[4 * selectTab], sensor[4 * selectTab + 1], sensor[4 * selectTab + 2], sensor[4 * selectTab + 3]);
            timer1.Stop();
            for (int i = 0; i < 4; i++)
            {
                foreach (var series in chartGraph[i].Series)
                {
                    series.Points.Clear();
                }
                indeksGraph[i] = 0;
            }
            timer1.Start(); 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (record)
            {
                record = false;
                button1.Text = "RECORD";
            }
            else
            {
                DateTime now = DateTime.Now;
                for(int i = 0; i < 12; i++)
                {
                    if (i == 6) { continue; }
                    if (i == 7) { continue; }
                    string s = sensor[i].id_name;
                    s = s + " record at " + now.Date
                        + "\n" + now.Hour + ":" + now.Minute + ":" + now.Second;
                    s = s + "\n"+"Created : Haris Hidayatulloh / 3110181011";
                    s = s + "\n" + "\n" + "\n";
                    File.AppendAllText(textFile[selectUGVUAV, i],s);
                }
                string s2 = "Connection";
                s2 = s2 + " record at " + now.Date
                        + "\n" + now.Hour + ":" + now.Minute + ":" + now.Second;
                s2 = s2 + "\n" + "Created : Haris Hidayatulloh / 3110181011";
                s2 = s2 + "\n" + "\n" + "\n";
                File.AppendAllText(conTextFile[selectUGVUAV],s2);
                record = true;
                button1.Text = "STOP";
            }
        }
    }
}
