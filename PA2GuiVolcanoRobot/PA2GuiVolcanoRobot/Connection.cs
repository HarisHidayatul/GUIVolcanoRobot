using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace PA2GuiVolcanoRobot
{
    public partial class Connection : UserControl
    {
        public bool statusConnect = false;
        public delegate void StatusUpdateHandler(object sender, EventArgs e);
        public event StatusUpdateHandler pressConnectSerial;
        public event StatusUpdateHandler pressPingSerial;

        public event StatusUpdateHandler pressRefreshVideo;
        public event StatusUpdateHandler pressConnectVideo;

        private void videoRefresh()
        {
            //Create arguments.  You should also have custom one, or else return EventArgs.Empty();
            EventArgs args = new EventArgs();

            //Call any listeners
            pressRefreshVideo?.Invoke(this, args);
        }
        private void connectVideo()
        {
            //Create arguments.  You should also have custom one, or else return EventArgs.Empty();
            EventArgs args = new EventArgs();

            //Call any listeners
            pressConnectVideo?.Invoke(this, args);
        }
        private void ConnectPress()
        {
            //Create arguments.  You should also have custom one, or else return EventArgs.Empty();
            EventArgs args = new EventArgs();

            //Call any listeners
            pressConnectSerial?.Invoke(this, args);
        }
        private void PingPress()
        {
            //Create arguments.  You should also have custom one, or else return EventArgs.Empty();
            EventArgs args = new EventArgs();

            //Call any listeners
            pressPingSerial?.Invoke(this, args);

        }
        public void disableCombobox()
        {
            button2.Text = "Disconnect";
            statusConnect = true;
            button1.Text = "Ping";
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
        }
        public void enableCombobox()
        {
            button2.Text = "Connect";
            statusConnect = false;
            button1.Text = "Refresh";
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            comboBox6.Enabled = true;
        }
        public Connection()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            videoRefresh();
        }
        public void increaseSizeText()
        {
            label1.Font = new Font(label1.Font.Name,label1.Font.Size+1,FontStyle.Bold);
            panel2.Height = label1.Height + 5;

            label2.Font = new Font(label2.Font.Name, label2.Font.Size + 1,FontStyle.Bold);
            panel6.Height = label2.Height + 5;

            label9.Font = label2.Font;
            panel31.Height = label2.Height + 5;

            comboBox1.Font = new Font(comboBox1.Font.Name, comboBox1.Font.Size + 1,comboBox1.Font.Style);
            label3.Font = new Font(label3.Font.Name, comboBox1.Font.Size + 1);
            panel12.Height = comboBox1.Height;
            panel28.Height = comboBox1.Height;
            panel42.Height = comboBox1.Height;

            comboBox2.Font = comboBox1.Font;
            comboBox3.Font = comboBox1.Font;
            comboBox4.Font = comboBox1.Font;
            comboBox5.Font = comboBox1.Font;
            comboBox6.Font = comboBox1.Font;
            comboBox7.Font = comboBox1.Font;
            comboBox8.Font = comboBox1.Font;

            button1.Font = comboBox1.Font;
            button2.Font = comboBox1.Font;
            button3.Font = comboBox1.Font;

            label4.Font = label3.Font;
            label5.Font = label3.Font;
            label6.Font = label3.Font;
            label7.Font = label3.Font;
            label8.Font = label3.Font;

            label10.Font = label3.Font;
            label11.Font = label3.Font;

            panel11.Height = panel12.Height;
            panel17.Height = panel12.Height;
            panel20.Height = panel12.Height;
            panel23.Height = panel12.Height;
            panel26.Height = panel12.Height;

            panel36.Height = panel12.Height;
            panel39.Height = panel12.Height;

            panel9.Height = (panel12.Height + panel14.Height)*8;
            panel33.Height = (panel12.Height + panel14.Height) * 4;
        }
        public void decreaseSizeText()
        {
            label1.Font = new Font(label1.Font.Name, label1.Font.Size - 1,FontStyle.Bold);
            panel2.Height = label1.Height + 5;

            label2.Font = new Font(label2.Font.Name, label2.Font.Size - 1,FontStyle.Bold);
            panel6.Height = label2.Height + 5;

            label9.Font = label2.Font;
            panel31.Height = label2.Height + 5;

            comboBox1.Font = new Font(comboBox1.Font.Name, comboBox1.Font.Size - 1,comboBox1.Font.Style);
            label3.Font = new Font(label3.Font.Name, comboBox1.Font.Size + 1);
            panel12.Height = comboBox1.Height;
            panel28.Height = comboBox1.Height;
            panel42.Height = comboBox1.Height;

            comboBox2.Font = comboBox1.Font;
            comboBox3.Font = comboBox1.Font;
            comboBox4.Font = comboBox1.Font;
            comboBox5.Font = comboBox1.Font;
            comboBox6.Font = comboBox1.Font;
            comboBox7.Font = comboBox1.Font;
            comboBox8.Font = comboBox1.Font;
            button1.Font = comboBox1.Font;
            button2.Font = comboBox1.Font;
            button3.Font = comboBox1.Font;

            label4.Font = label3.Font;
            label5.Font = label3.Font;
            label6.Font = label3.Font;
            label7.Font = label3.Font;
            label8.Font = label3.Font;

            label10.Font = label3.Font;
            label11.Font = label3.Font;

            panel11.Height = panel12.Height;
            panel17.Height = panel12.Height;
            panel20.Height = panel12.Height;
            panel23.Height = panel12.Height;
            panel26.Height = panel12.Height;


            panel36.Height = panel12.Height;
            panel39.Height = panel12.Height;

            panel9.Height = (panel12.Height + panel14.Height) * 8;
            panel33.Height = (panel12.Height + panel14.Height) * 4;
        }
        private void Connection_SizeChanged(object sender, EventArgs e)
        {
            panel3.Width = 14 + (this.Width/4 - 45);
            panel8.Width = 161 + (int)(this.Width / 1.5 - 110);
            panel32.Width = panel8.Width;

            label3.Width = 80 + (this.Width / 4 - 45);
            panel13.Width = 80 + (this.Width / 4 - 45);
            panel12.Height = comboBox1.Height;
            panel28.Height = comboBox1.Height;

            label4.Width = label3.Width;
            label5.Width = label3.Width;
            label6.Width = label3.Width;
            label7.Width = label3.Width;
            label8.Width = label3.Width;
            label10.Width = label3.Width - 7;
            label11.Width = label3.Width - 7;

            button1.Width = label3.Width - 7;
            button2.Width = label3.Width - 7;
            button3.Width = label3.Width - 7;
            panel43.Width = button1.Width + panel29.Width;

            panel15.Width = panel13.Width;
            panel18.Width = panel13.Width;
            panel21.Width = panel13.Width;
            panel24.Width = panel13.Width;
            panel27.Width = panel13.Width;

            panel37.Width = panel13.Width+10;
            panel40.Width = panel13.Width+10;

            panel11.Height = comboBox1.Height;
            panel17.Height = comboBox1.Height;
            panel20.Height = comboBox1.Height;
            panel23.Height = comboBox1.Height;
            panel26.Height = comboBox1.Height;

            panel36.Height = comboBox1.Height;
            panel39.Height = comboBox1.Height;

            panel9.Height = (panel12.Height + panel14.Height) * 8;
            panel33.Height = (panel12.Height + panel14.Height) * 4;
        }

        private void addCom()
        {

            comboBox1.Items.Clear();
            comboBox1.Text = "\0";
            try
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    comboBox1.Items.Add(s); //dapatkan port dan tampilkan
                }
                comboBox1.Text = comboBox1.Items[0].ToString();
            }
            catch
            { //tampilkan ketika port tidak tersedia
                /*
                MessageBox.Show("port is not available\n" +
                                "please check port at device manager",//isi pesan
                                "Port Available",//judul program
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                */
            }
        }

        private void Connection_Load(object sender, EventArgs e)
        {
            /*
            comboBox1.Items.Add("COM 1");
            comboBox1.Items.Add("COM 2");
            comboBox1.Items.Add("COM 3");
            comboBox1.Text = comboBox1.Items[0].ToString();
            */
            addCom();
            //Tambahkan list untuk pilihan baudrate
            comboBox2.Items.Add("1200");
            comboBox2.Items.Add("2400");
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("57600");
            comboBox2.Text = comboBox2.Items[3].ToString();

            //Tambahkan List Parity
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                comboBox3.Items.Add(s);
            }
            comboBox3.Text = comboBox3.Items[0].ToString();
            //Tambahkan Data Bits
            comboBox4.Items.Add("8");
            comboBox4.Items.Add("7");
            comboBox4.Text = comboBox4.Items[0].ToString();
            //Tambahkan Stop Bits
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                comboBox5.Items.Add(s);
            }
            comboBox5.Text = comboBox3.Items[0].ToString();
            //Tambahkan timeout
            comboBox6.Items.Add("1000");
            comboBox6.Items.Add("2000");
            comboBox6.Items.Add("3000");
            comboBox6.Text = comboBox6.Items[0].ToString();

            //Tambahkan camera
            //comboBox7.Items.Add("AVI TO VGA");
            //comboBox7.Items.Add("AVI TO VGA");
            //comboBox7.Items.Add("AVI TO VGA");
            //comboBox7.Text = comboBox7.Items[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(statusConnect == false)
            {
                addCom();
            }
            else
            {
                //ping connection
                PingPress();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConnectPress();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connectVideo();
        }
    }
}
