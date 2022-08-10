using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PA2GuiVolcanoRobot
{
    public partial class sensor1 : UserControl
    {
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

        int zoomText = 0;
        int selectTab = 0;
        struct sensorr
        {
            public string id_name;
            public string unit;
            public string image;
            public string valueString;
            public int value;
            public int progressBarValue;
            public int maxValue;
            public int minValue;
        }
        sensorr[] sensor = new sensorr[12];
        public void setValueSensor(int codeSensor, int val)
        {
            switch (codeSensor)
            {
                case humadity:
                    sensor[0].value = val;
                    break;
                case temperature:
                    sensor[1].value = val;
                    break;
                case battery:
                    sensor[2].value = val;
                    break;
                case altitude:
                    sensor[3].value = val;
                    break;
                case co:
                    sensor[4].value = val;
                    break;
                case h2s:
                    sensor[5].value = val;
                    break;
                case compass:
                    sensor[8].value = val;
                    break;
                case roll:
                    sensor[9].value = val;
                    break;
                case pitch:
                    sensor[10].value = val;
                    break;
                case yaw:
                    sensor[11].value = val;
                    break;
                default:
                    break;
            }
            if(codeSensor < 12)
            {
                processValueString(codeSensor);
            }
            updateSensor(4*selectTab);
        }
        private void updateSensor(int i)
        {
            label2.Text = sensor[i].valueString;
            circularProgressBar1.Value = this.sensor[i].progressBarValue;

            label6.Text = sensor[i + 1].valueString;
            circularProgressBar2.Value = sensor[i + 1].progressBarValue;

            label9.Text = this.sensor[i + 2].valueString;
            circularProgressBar3.Value = sensor[i + 2].progressBarValue;

            label12.Text = this.sensor[i + 3].valueString;
            circularProgressBar4.Value = sensor[i + 3].progressBarValue;
        }
        private void initSensor()
        {
            //pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("temperature");
            sensor[0].id_name = "HUMIDITY";
            sensor[0].unit = "BAR";
            sensor[0].image = "humidity1";
            sensor[0].maxValue = 9999;
            sensor[0].minValue = 0;
            sensor[0].value = 1000;
            sensor[0].progressBarValue = 0;

            sensor[1].id_name = "TEMPERATURE";
            sensor[1].unit = "°C";
            sensor[1].image = "temperature";
            sensor[1].maxValue = 9999;
            sensor[1].minValue = 0;
            sensor[1].value = 1000;
            sensor[1].progressBarValue = 0;

            sensor[2].id_name = "BATTERY";
            sensor[2].unit = "V";
            sensor[2].image = "battery";
            sensor[2].maxValue = 9999;
            sensor[2].minValue = 0;
            sensor[2].value = 1000;
            sensor[2].progressBarValue = 0;

            sensor[3].id_name = "ALTITUDE";
            sensor[3].unit = "m";
            sensor[3].image = "altitude";
            sensor[3].maxValue = 9999;
            sensor[3].minValue = 0;
            sensor[3].value = 1000;
            sensor[3].progressBarValue = 0;

            sensor[4].id_name = "CO";
            sensor[4].unit = "ppm";
            sensor[4].image = "gas";
            sensor[4].maxValue = 9999;
            sensor[4].minValue = 0;
            sensor[4].value = 1000;
            sensor[4].progressBarValue = 0;

            sensor[5].id_name = "H" + '\u2082' + "S";
            sensor[5].unit = "ppm";
            sensor[5].image = "gas";
            sensor[5].maxValue = 9999;
            sensor[5].minValue = 0;
            sensor[5].value = 1000;
            sensor[5].progressBarValue = 0;

            sensor[6].id_name = "\0";
            sensor[6].unit = "\0";
            sensor[6].image = "blank";
            sensor[6].maxValue = 9999;
            sensor[6].minValue = 0;
            sensor[6].value = 0;
            sensor[6].progressBarValue = 0;

            sensor[7].id_name = "\0";
            sensor[7].unit = "\0";
            sensor[7].image = "blank";
            sensor[7].maxValue = 9999;
            sensor[7].minValue = 0;
            sensor[7].value = 0;
            sensor[7].progressBarValue = 0;

            sensor[8].id_name = "COMPASS";
            sensor[8].unit = "degree";
            sensor[8].image = "compass";
            sensor[8].maxValue = 3600;
            sensor[8].minValue = 0;
            sensor[8].value = 1000;
            sensor[8].progressBarValue = 0;

            sensor[9].id_name = "ROLL";
            sensor[9].unit = "degree";
            sensor[9].image = "roll";
            sensor[9].maxValue = 360;
            sensor[9].minValue = 0;
            sensor[9].value = 0;
            sensor[9].progressBarValue = 0;

            sensor[10].id_name = "PITCH";
            sensor[10].unit = "degree";
            sensor[10].image = "pitch";
            sensor[10].maxValue = 360;
            sensor[10].minValue = 0;
            sensor[10].value = 0;
            sensor[10].progressBarValue = 0;

            sensor[11].id_name = "YAW";
            sensor[11].unit = "degree";
            sensor[11].image = "yaw";
            sensor[11].maxValue = 360;
            sensor[11].minValue = 0;
            sensor[11].value = 0;
            sensor[11].progressBarValue = 0;

            for (int i = 0; i < 12; i++)
            {
                processValueString(i);
            }
        }
        private void processValueString(int indeks)
        {
            switch (indeks)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 8:
                    sensor[indeks].valueString = (sensor[indeks].value / 10).ToString() + "." + (sensor[indeks].value % 10).ToString();
                    break;
                default:
                    if ((indeks == 6) || (indeks == 7))
                    {
                        sensor[indeks].valueString = "\0";
                    }
                    else
                    {
                        sensor[indeks].valueString = sensor[indeks].value.ToString();
                    }

                    break;
            }
            if(sensor[indeks].value < 0)
            {
                sensor[indeks].progressBarValue = (-1) * sensor[indeks].value;
            }
            else
            {
                sensor[indeks].progressBarValue = sensor[indeks].value;
            }
        }
        private void changeSensor(int i)
        {
            label4.Text = sensor[i].id_name;
            pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject(sensor[i].image);
            label2.Text = sensor[i].valueString;
            label3.Text = sensor[i].unit;
            circularProgressBar1.Maximum = sensor[i].maxValue;
            circularProgressBar1.Minimum = sensor[i].minValue;
            circularProgressBar1.Value = this.sensor[i].progressBarValue;

            label1.Text = sensor[i+1].id_name;
            pictureBox2.Image = (Image)Properties.Resources.ResourceManager.GetObject(sensor[i + 1].image);
            label6.Text = sensor[i + 1].valueString;
            label5.Text = sensor[i + 1].unit;
            circularProgressBar2.Maximum = sensor[i + 1].maxValue;
            circularProgressBar2.Minimum = sensor[i + 1].minValue;
            circularProgressBar2.Value = sensor[i + 1].progressBarValue;

            label7.Text = sensor[i + 2].id_name;
            pictureBox3.Image = (Image)Properties.Resources.ResourceManager.GetObject(sensor[i + 2].image);
            label9.Text = this.sensor[i + 2].valueString;
            label8.Text = sensor[i + 2].unit;
            circularProgressBar3.Maximum = sensor[i + 2].maxValue;
            circularProgressBar3.Minimum = sensor[i + 2].minValue;
            circularProgressBar3.Value = sensor[i + 2].progressBarValue;

            label10.Text = sensor[i + 3].id_name;
            pictureBox4.Image = (Image)Properties.Resources.ResourceManager.GetObject(sensor[i + 3].image);
            label12.Text = this.sensor[i + 3].valueString;
            label11.Text = sensor[i + 3].unit;
            circularProgressBar4.Maximum = sensor[i + 3].maxValue;
            circularProgressBar4.Minimum = sensor[i + 3].minValue;
            circularProgressBar4.Value = sensor[i + 3].progressBarValue;
        }
        public sensor1()
        {
            InitializeComponent();
            label4.Parent = circularProgressBar1;
            label2.Parent = circularProgressBar1;

            label1.Parent = circularProgressBar2;
            label7.Parent = circularProgressBar3;
            label10.Parent = circularProgressBar4;

            label6.Parent = circularProgressBar2;
            label9.Parent = circularProgressBar3;
            label12.Parent = circularProgressBar4;


            label4.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;

            label1.BackColor = System.Drawing.Color.Transparent;
            label7.BackColor = System.Drawing.Color.Transparent;
            label10.BackColor = System.Drawing.Color.Transparent;

            label6.BackColor = System.Drawing.Color.Transparent;
            label9.BackColor = System.Drawing.Color.Transparent;
            label12.BackColor = System.Drawing.Color.Transparent;

            initSensor();
            changeSensor(0);
        }
        public void increaseSizeText()
        {
            label2.Font = new Font(label2.Font.Name,label2.Font.Size + 1,label2.Font.Style);
            label3.Font = new Font(label3.Font.Name, label3.Font.Size + 1, label3.Font.Style);
            label4.Font = new Font(label4.Font.Name, label4.Font.Size + 1, label4.Font.Style);

            label1.Font = label4.Font;
            label7.Font = label4.Font;
            label10.Font = label4.Font;

            label6.Font = label2.Font;
            label9.Font = label2.Font;
            label12.Font = label2.Font;

            label5.Font = label3.Font;
            label8.Font = label3.Font;
            label11.Font = label3.Font;
            zoomText++;
            refreshLocation();

        }
        public void decreaseSizeText()
        {
            
            label2.Font = new Font(label2.Font.Name, label2.Font.Size - 1, label2.Font.Style);
            label3.Font = new Font(label3.Font.Name, label3.Font.Size - 1, label3.Font.Style);
            label4.Font = new Font(label4.Font.Name, label4.Font.Size - 1, label4.Font.Style);

            label1.Font = label4.Font;
            label7.Font = label4.Font;
            label10.Font = label4.Font;

            label6.Font = label2.Font;
            label9.Font = label2.Font;
            label12.Font = label2.Font;

            label5.Font = label3.Font;
            label8.Font = label3.Font;
            label11.Font = label3.Font;
            zoomText--;
            refreshLocation();
        }
        private void sensor1_SizeChanged(object sender, EventArgs e)
        {
            panel5.Width = panel5.Height;
            panel7.Width = panel7.Height;
            panel9.Width = panel9.Height;
            panel11.Width = panel11.Height;

            //panel6.Width = 30 + (int)(this.Width/7 - 516/7);
            panel6.Width = (this.Width - (panel3.Width * 2) - circularProgressBar1.Width * 4) / 4;
            panel8.Width = panel6.Width;
            panel10.Width = panel6.Width;

            panel4.Width = panel6.Width / 2;

            pictureBox1.Height = (circularProgressBar1.Width * 2) / 9;
            pictureBox2.Height = pictureBox1.Height;
            pictureBox3.Height = pictureBox1.Height;
            pictureBox4.Height = pictureBox1.Height;

            pictureBox1.Width = pictureBox1.Height;
            pictureBox2.Width = pictureBox1.Height;
            pictureBox3.Width = pictureBox1.Height;
            pictureBox4.Width = pictureBox1.Height;


            pictureBox1.Location = new Point(34+(circularProgressBar1.Width/3-90/3),9 + (circularProgressBar1.Width/8 - 90/8));
            pictureBox2.Location = pictureBox1.Location;
            pictureBox3.Location = pictureBox1.Location;
            pictureBox4.Location = pictureBox1.Location;

            refreshLocation();
            
        }
        private void refreshLocation()
        {
            int pos = (int)(panel5.Width / 2 - 44.5);
            //label4.Location = new Point(12 + pos - zoomText*2, 27 + pos - (int)(zoomText*1.5));
            label4.Location = new Point(12 + pos - zoomText * 2, 27 + pos);
            label2.Location = new Point(6 + pos -zoomText*2, 36 + pos);
            label3.Location = new Point(33 + pos-zoomText*2, 68 + pos+zoomText);

            label1.Location = label4.Location;
            label7.Location = label4.Location;
            label10.Location = label4.Location;

            label6.Location = label2.Location;
            label9.Location = label2.Location;
            label12.Location = label2.Location;

            label5.Location = label3.Location;
            label8.Location = label3.Location;
            label11.Location = label3.Location;
        }

        private void sensor1_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            selectTab++;
            if (selectTab > 2) selectTab = 2;
            changeSensor(4*selectTab);
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            selectTab--;
            if (selectTab < 0) selectTab = 0;
            changeSensor(4*selectTab);

        }
    }
}
