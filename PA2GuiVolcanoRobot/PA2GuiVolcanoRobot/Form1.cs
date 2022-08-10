using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using AForge.Video.DirectShow;
using AForge.Video;

namespace PA2GuiVolcanoRobot
{
    public partial class Form1 : Form
    {
        private const int UAV = 0;
        private const int UGV = 1;
        public int selectUgvUav = UGV;//if select UGV 1,if select UAV 0

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

        int[,] sensor4digit = new int[2, 15];
        double[,] sensor15digit = new double[2, 2];

        stringSerial seriall = new stringSerial();

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        GMapOverlay markers = new GMapOverlay("markers");

        const char header = '#';
        const char Tail = '%';
        const int maxData = 15;
        //char saveCharacter = '\0';

        bool[] status = new bool[2];
        bool[] connect_status = new bool[2];
        int[] indeks = new int[2];
        string[] saveCom = new string[2];
        string[] saveBaudrate = new string[2];

        int[] counter = new int[2];
        int[] sendPing = new int[2];

        char[] dataSerialUGV = new char[maxData];
        char[] dataSerialUAV = new char[maxData];


        public bool form2Active = false;


        //int selectMenu = 0;
        Form1 form2;
        UserControl[] Submenu = new UserControl[4];
        Panel[] menu = new Panel[4];

        visual3d visual3D;

        // Create new stopwatch
        Stopwatch stopwatch1 = new Stopwatch();
        Stopwatch stopwatch2 = new Stopwatch();

        private void setAllToZero(int mode)
        {
            for(int i = 0; i < 15; i++)
            {
                sensor4digit[mode, i] = 0;
            }
        }

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sensor4digit[i, j] = 0;
                }
                for (int j = 0; j < 2; j++)
                {
                    sensor15digit[i, j] = 0;
                }
            }

            status[UGV] = false; status[UAV] = false;
            connect_status[UGV] = false; connect_status[UAV] = true;
            indeks[UGV] = 0; indeks[UAV] = 0;
            counter[UGV] = 0; counter[UAV] = 0;
            sendPing[UGV] = 0; sendPing[UAV] = 0;

            menu[0] = this.panel8;
            menu[1] = this.panel9;
            menu[2] = this.panel10;
            menu[3] = this.panel17;
            Submenu[0] = this.connectionPanel;
            Submenu[1] = this.sensor1;
            Submenu[2] = this.graph1;
            Submenu[3] = this.setting1;

            label10.Text = "H" + '\u2082' + "S";
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            connectionPanel.pressConnectSerial += connect_OnUpdateStatus;
            connectionPanel.pressPingSerial += ping_OnUpdateStatus;
            connectionPanel.pressRefreshVideo += refreshVideo_OnUpdateStatus;
            connectionPanel.pressConnectVideo += connectVideo_OnUpdateStatus;

            sensor1.cameraSelect += changeCamera_OnUpdateStatus;
            sensor1.gpsSelect += changeGps_OnUpdateStatus;
            sensor1.show3d += visual3d_OnUpdateStatus;

            selectUgvUav = UGV;
            connectionPanel.label12.Text = "UGV";
            /*
            if (serialPort1.IsOpen == false)
            {
                connectionPanel.enableCombobox(selectUgvUav);
            }
            else
            {
                //connectionPanel.comboBox1.Text = saveCom[UGV];
                connectionPanel.disableCombobox(selectUgvUav);
            }*/




            form2 = new Form1();
            visual3D = new visual3d();

            timer1.Start();

            pictureBox6.Visible = false;
            gMapControl1.Dock = DockStyle.Fill;
            pictureBox6.Dock = DockStyle.Fill;


            changeMenuColor(0);

            //akses gmaps
            gMapControl1.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            GMap.NET.MapProviders.OpenStreet4UMapProvider.UserAgent = "IE";
            gMapControl1.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            GMaps.Instance.OptimizeMapDb(null);
            gMapControl1.CacheLocation = @"C:\Users\haris\source\repos\PA2GuiVolcanoRobot\GoogleMap";
            gMapControl1.Zoom = 17;
            //gMapControl1.Size = new Size(this.Width, this.Height);
            //gMapControl1.ShowCenter = false;
            gMapControl1.Position = new GMap.NET.PointLatLng(-7.2766120, 112.7940801);

            //penentuan marker gmaps
            double lat = -7.2766120;
            double lng = 112.7940801;
            PointLatLng point = new PointLatLng(lat, lng);
            GMapMarker marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(point, GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green);
            markers.Markers.Add(marker);
            gMapControl1.Overlays.Add(markers);

            setAllToZero(0);
            setAllToZero(1);

            //video connection
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterinfo in filterInfoCollection)
            connectionPanel.comboBox7.Items.Add(filterinfo.Name);
            connectionPanel.comboBox7.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice();
        }
        
        private void SaveData(string s, int mode)
        {

            if (seriall.checkFCS(s))
            {
                if (seriall.checkUNIT(0, s))
                {
                    int j = seriall.parseStringToInt(s, 1, 2);
                    string value = s.Substring(3, 4);
                    if (j == altitude)
                    {
                        //output attitude
                        sensor4digit[mode, altitude] = (Int16.Parse(value)+(setting1.getZero(altitude)))*(setting1.getSpan(altitude))/100;

                    }
                    else if (j == humadity)
                    {
                        //output humadity
                        sensor4digit[mode, humadity] = (Int16.Parse(value) + (setting1.getZero(humadity)))*(setting1.getSpan(humadity))/100;

                    }
                    else if (j == co)
                    {
                        //output CO
                        sensor4digit[mode, co] = (Int16.Parse(value) + (setting1.getZero(co)))*(setting1.getSpan(co))/100;

                    }
                    else if (j == battery)
                    {
                        //output battery
                        sensor4digit[mode, battery] = (Int16.Parse(value) + (setting1.getZero(battery)))*(setting1.getSpan(battery))/100;

                    }
                    else if (j == compass)
                    {
                        //output compass

                        sensor4digit[mode, compass] = (Int16.Parse(value) + (setting1.getZero(compass)*10))*(setting1.getSpan(compass))/100;

                    }
                    else if (j == temperature)
                    {
                        //output temperature
                        sensor4digit[mode, temperature] = (Int16.Parse(value) + (setting1.getZero(temperature)))*(setting1.getSpan(temperature)) / 100;

                    }
                    else if (j == h2s)
                    {
                        //output h2s
                        sensor4digit[mode, h2s] = (Int16.Parse(value) + (setting1.getZero(h2s)))*(setting1.getSpan(h2s)) / 100;

                    }
                    else if (j == speed)
                    {
                        //output speed
                        sensor4digit[mode, speed] = (Int16.Parse(value) + (setting1.getZero(speed)))*(setting1.getSpan(speed)) / 100;

                    }
                    else if (j == roll)
                    {
                        //output roll
                        int value2 = (Int16.Parse(value) + (setting1.getZero(roll))) * (setting1.getSpan(roll)) / 100;
                        if(value2 > 360) { value2 = value2 - 360; }
                        if(value2 < -360) { value2 = value2 + 360; }
                        sensor4digit[mode, roll] = value2;                       
                    }
                    else if (j == pitch)
                    {
                        //output pitch
                        int value2 = (Int16.Parse(value) + (setting1.getZero(pitch))) * (setting1.getSpan(pitch)) / 100;
                        if (value2 > 360) { value2 = value2 - 360; }
                        if (value2 < -360) { value2 = value2 + 360; }
                        sensor4digit[mode, pitch] = value2;
                    }
                    else if (j == yaw)
                    {
                        //output yaw
                        int value2 = (Int16.Parse(value) + (setting1.getZero(yaw))) * (setting1.getSpan(yaw)) / 100;
                        if (value2 > 360) { value2 = value2 - 360; }
                        if (value2 < -360) { value2 = value2 + 360; }
                        sensor4digit[mode, yaw] = value2;

                    }
                    else if (j == co2)
                    {
                        //output co2
                        sensor4digit[mode, co2] = (Int16.Parse(value) + (setting1.getZero(co2)))*(setting1.getSpan(co2)) / 100;

                    }
                    else if (j == nh4)
                    {
                        //output nh4
                        sensor4digit[mode, nh4] = (Int16.Parse(value) + (setting1.getZero(nh4)))*(setting1.getSpan(nh4)) / 100;

                    }
                    else if (j == latt)
                    {
                        //output lattitude
                        sensor15digit[mode, 0] = Convert.ToDouble(s.Substring(6, 7));

                    }
                    else if (j == lngg)
                    {
                        //output longtitude
                        sensor15digit[mode, 1] = Convert.ToDouble(s.Substring(5, 9));

                    }
                }
            }
        }

        private void sendPingConnection(int mode)
        {
            if (mode == UGV)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write("#");
                    serialPort1.Write(sendPing[UGV].ToString());
                    serialPort1.Write("%");
                    sendPing[UGV]++;
                    if (sendPing[UGV] > 13)
                    {
                        sendPing[UGV] = 0;
                    }
                    timer2.Start();
                    stopwatch1.Reset();
                    stopwatch1.Start();
                }
            }
            else
            {
                if (serialPort2.IsOpen)
                {
                    serialPort2.Write("#");
                    serialPort2.Write(sendPing[UAV].ToString());
                    serialPort2.Write("%");
                    sendPing[UAV]++;
                    if (sendPing[UAV] > 13)
                    {
                        sendPing[UAV] = 0;
                    }
                    timer3.Start();
                    stopwatch2.Reset();
                    stopwatch2.Start();
                }
            }
        }

        private void changeMaps_OnUpdateStatus(object sender, EventArgs e)
        {
            ///Handle your event here

        }

        private void changeCamera_OnUpdateStatus(object sender, EventArgs e)
        {
            ///Handle your event here
            label7.Text = "CAMERA";
            if (selectUgvUav == UGV)
            {
                label7.Text = label7.Text + " UGV ";
            }
            else
            {
                label7.Text = label7.Text + " UAV ";
            }
            label7.Text = label7.Text + "VIEW";
            pictureBox6.Visible = true;
            gMapControl1.Visible = false;
        }
        private void changeGps_OnUpdateStatus(object sender, EventArgs e)
        {
            ///Handle your event here
            label7.Text = "GPS";
            if (selectUgvUav == UGV)
            {
                label7.Text = label7.Text + " UGV ";
            }
            else
            {
                label7.Text = label7.Text + " UAV ";
            }
            label7.Text = label7.Text + "VIEW";
            pictureBox6.Visible = false;
            gMapControl1.Visible = true;
        }
        private void ping_OnUpdateStatus(object sender, EventArgs e)
        {
            sendPingConnection(selectUgvUav);
        }
        private void visual3d_OnUpdateStatus(object sender, EventArgs e)
        {
            visual3D.Show();
        }
        private void refreshVideo_OnUpdateStatus(object sender, EventArgs e)
        {
            //video connection
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterinfo in filterInfoCollection)
                connectionPanel.comboBox7.Items.Add(filterinfo.Name);
            connectionPanel.comboBox7.SelectedIndex = 0;
        }
        private void connectVideo_OnUpdateStatus(object sender, EventArgs e)
        {
            if (!videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[connectionPanel.comboBox7.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();
                connectionPanel.comboBox7.Enabled = false;
                connectionPanel.button4.Enabled = false;
                connectionPanel.button3.Text = "DISCONNECT";
            }
            else
            {
                //pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("temperature");
                videoCaptureDevice.Stop();
                connectionPanel.comboBox7.Enabled = true;
                connectionPanel.button4.Enabled = true;
                connectionPanel.button3.Text = "CONNECT";
                pictureBox6.Image = (Image)Properties.Resources.ResourceManager.GetObject("kawahijen");
                sensor1.pictureBox1.Image = pictureBox6.Image;
            }
        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs e)
        {
            pictureBox6.Image = (Bitmap)e.Frame.Clone();
            sensor1.pictureBox1.Image = pictureBox6.Image;
        }
        private void connect_OnUpdateStatus(object sender, EventArgs e)
        {
            try
            {
                if(selectUgvUav == UGV)
                {
                    if(serialPort1.IsOpen == false)
                    {
                        serialPort1.PortName = connectionPanel.comboBox1.Text;
                        serialPort1.BaudRate = int.Parse(connectionPanel.comboBox2.Text);
                        serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), connectionPanel.comboBox3.SelectedIndex.ToString());
                        serialPort1.DataBits = int.Parse(connectionPanel.comboBox4.Text);

                        serialPort1.Open(); //buka port yang dipilih
                        if (serialPort1.IsOpen == true)
                        {
                            connectionPanel.disableCombobox();//aktifkan tombol disconnect untuk disconnect
                            saveCom[UGV] = serialPort1.PortName;
                            saveBaudrate[UGV] = connectionPanel.comboBox2.Text;
                            connectionPanel.comboBox1.Enabled = false;
                            connect_status[UGV] = true;

                            timer2.Start();
                        }
                    }
                    else
                    {
                        timer2.Stop();
                        serialPort1.Close();
                        connectionPanel.enableCombobox();//aktifkan tombol disconnect untuk disconnect
                        connectionPanel.comboBox1.Enabled = true;
                        connect_status[UGV] = false;
                        setAllToZero(UGV);

                    }

                }
                if (selectUgvUav == UAV)
                {
                    if (serialPort2.IsOpen == false)
                    {
                        serialPort2.PortName = connectionPanel.comboBox1.Text;
                        serialPort2.BaudRate = int.Parse(connectionPanel.comboBox2.Text);
                        serialPort2.Parity = (Parity)Enum.Parse(typeof(Parity), connectionPanel.comboBox3.SelectedIndex.ToString());
                        serialPort2.DataBits = int.Parse(connectionPanel.comboBox4.Text);

                        serialPort2.Open(); //buka port yang dipilih
                        if (serialPort2.IsOpen == true)
                        {
                            connectionPanel.disableCombobox();//aktifkan tombol disconnect untuk disconnect
                            saveCom[UAV] = serialPort2.PortName;
                            saveBaudrate[UAV] = connectionPanel.comboBox2.Text;
                            connectionPanel.comboBox1.Enabled = false;
                            connect_status[UAV] = true;

                            timer3.Start();
                        }
                    }
                    else
                    {

                        timer3.Stop();
                        serialPort2.Close();
                        connectionPanel.enableCombobox();
                        //connectionPanel.comboBox1.Enabled = true;
                        connect_status[UAV] = false;
                        setAllToZero(UGV);
                    }
                }
            }
            catch
            {

            }
        }
        

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            panel1.Width = 83 + (this.Width / 200 - 4);
            panel2.Width = 180 + (this.Width / 3 - 266);
            panel3.Height = 49 + (this.Height / 10 - 60);
            panel4.Height = 403 + (int)(this.Height / 1.5 - 600 / 1.5);
            panel7.Height = 80 + (this.Height / 6 - 100);
            //label4.Text = this.Width.ToString();
            //label5.Text = this.Height.ToString();

        }

        private void changeMenuColor(int selectMenu) 
        {
            for (int i = 0; i < menu.Length; i++)
            {
                if(i == selectMenu)
                {
                    menu[i].BackColor = Color.FromArgb(40, 40, 40);
                    Submenu[i].Visible = true;
                    continue;
                }
                menu[i].BackColor = Color.FromArgb(29, 25, 26);
                Submenu[i].Visible = false;
            }
        }
        private void panel8_Click(object sender, EventArgs e)
        {
            changeMenuColor(0);
        }

        private void panel9_Click(object sender, EventArgs e)
        {
            changeMenuColor(1);
        }

        private void panel10_Click(object sender, EventArgs e)
        {
            changeMenuColor(2);
        }
        private void panel17_Click(object sender, EventArgs e)
        {
            changeMenuColor(3);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            connectionPanel.increaseSizeText();
            sensor11.increaseSizeText();
            setting1.increaseSizeText();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            connectionPanel.decreaseSizeText();
            sensor11.decreaseSizeText();
            setting1.decreaseSizeText();
        }
        
        private void label5_Click(object sender, EventArgs e)
        {
            if(selectUgvUav != UGV)
            {
                if (serialPort1.IsOpen == false)
                {
                    connectionPanel.enableCombobox();
                }
                else
                {
                    connectionPanel.comboBox1.Text = saveCom[UGV];
                    connectionPanel.comboBox2.Text = saveBaudrate[UGV];
                    connectionPanel.disableCombobox();
                }
                selectUgvUav = UGV;
                setting1.selectUGVUAV = UGV;
                graph1.selectUGVUAV = UGV;
                connectionPanel.label12.Text = "UGV";
                String s = label7.Text;
                label7.Text = s.Replace("UAV", "UGV");
                label5.BackColor = Color.FromArgb(40, 40, 40);
                label4.BackColor = Color.FromArgb(29, 25, 26);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if(selectUgvUav != UAV)
            {

                if (serialPort2.IsOpen == false)
                {
                    connectionPanel.enableCombobox();
                }
                else
                {
                    connectionPanel.comboBox1.Text = saveCom[UAV];
                    connectionPanel.comboBox2.Text = saveBaudrate[UAV];
                    connectionPanel.disableCombobox();
                }

                selectUgvUav = UAV;
                setting1.selectUGVUAV = UAV;
                graph1.selectUGVUAV = UAV;
                connectionPanel.label12.Text = "UAV";


                String s = label7.Text;
                label7.Text = s.Replace("UGV", "UAV");
                label4.BackColor = Color.FromArgb(40, 40, 40);
                label5.BackColor = Color.FromArgb(29, 25, 26);
            }
            
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if(selectUgvUav == UGV)
            {
                form2.Show();
                form2.form2Active = true;
                form2.connectionPanel.Visible = false;
                form2.selectUgvUav = UAV;
                form2.setting1.selectUGVUAV = UAV;
                form2.label6.Visible = false;
                form2.label5.Visible = false;
                form2.panel8.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer2.Stop();
            timer3.Stop();
            if (serialPort1.IsOpen) { serialPort1.Close(); }
            if (serialPort2.IsOpen) { serialPort2.Close(); }
            if (form2Active == true)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {//untuk ugv
            int j;
            int i;
            char buffer;
            string hasil;
            j = serialPort1.BytesToRead;
            for (i = 0; i < j; i++)
            {
                try
                {
                    buffer = (char)serialPort1.ReadChar();
                    if (status[UGV] == false)
                    {
                        if (buffer == header)
                        {
                            status[UGV] = true;
                            indeks[UGV] = 0;
                        }
                    }
                    else
                    {
                        if (buffer == Tail)
                        {

                            hasil = new string(dataSerialUGV);

                            this.Invoke((MethodInvoker)delegate ()
                            {
                                SaveData(hasil, UGV);
                                if (serialPort1.IsOpen == true)
                                {
                                    counter[UGV] = 0;
                                    timer2.Stop();
                                    stopwatch1.Stop();
                                    graph1.saveConnection(stopwatch1.Elapsed.TotalMilliseconds);
                                    sendPingConnection(UGV);
                                }
                            }
                            );

                            status[UGV] = false;
                            indeks[UGV] = 0;
                            setEmptChrUGV();
                        }
                        else
                        {
                            if (indeks[UGV] >= maxData)
                            {
                                indeks[UGV] = 0;
                                status[UGV] = false;
                                setEmptChrUGV();
                            }
                            else
                            {
                                dataSerialUGV[indeks[UGV]] = buffer;
                                indeks[UGV]++;
                            }
                        }
                    }
                }
                catch
                {
                    break;
                }
                
            }
        }
        private void setEmptChrUGV()
        {
            for (int i = 0; i < maxData; i++)
            {
                dataSerialUGV[i] = '\0';
            }
        }

        private void serialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {//untuk UAV
            int j;
            int i;
            char buffer;
            string hasil;
            j = serialPort2.BytesToRead;
            for (i = 0; i < j; i++)
            {
                try
                {
                    buffer = (char)serialPort2.ReadChar();
                    if (status[UAV] == false)
                    {
                        if (buffer == header)
                        {
                            status[UAV] = true;
                            indeks[UAV] = 0;
                        }
                    }
                    else
                    {
                        if (buffer == Tail)
                        {

                            hasil = new string(dataSerialUAV);

                            this.Invoke((MethodInvoker)delegate ()
                            {
                                SaveData(hasil, UAV);
                                if (serialPort2.IsOpen == true)
                                {
                                    counter[UAV] = 0;
                                    timer3.Stop();
                                    stopwatch2.Stop();
                                    graph1.saveConnection(stopwatch2.Elapsed.TotalMilliseconds);
                                    sendPingConnection(UAV);
                                }
                            }
                            );
                            status[UAV] = false;
                            indeks[UAV] = 0;
                            setEmptChrUAV();
                        }
                        else
                        {
                            if (indeks[UAV] >= maxData)
                            {
                                indeks[UAV] = 0;
                                status[UAV] = false;
                                setEmptChrUAV();
                            }
                            else
                            {
                                dataSerialUAV[indeks[UAV]] = buffer;
                                indeks[UAV]++;
                            }
                        }
                    }
                }
                catch
                {
                    break;
                }
            }

        }
        private void setEmptChrUAV()
        {
            for (int i = 0; i < maxData; i++)
            {
                dataSerialUAV[i] = '\0';
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //label7.Text = UGVText;
            //label3.Text = UAVText;
            for(int i = 0; i < 12; i++)
            {
                sensor11.setValueSensor(i, sensor4digit[selectUgvUav,i]);
                graph1.setValueSensor(i, sensor4digit[selectUgvUav, i]);
            }
            visual3D.changeRoll(sensor4digit[selectUgvUav,roll]);
            visual3D.changePitch(sensor4digit[selectUgvUav,pitch]);
            visual3D.changeYaw(sensor4digit[selectUgvUav,yaw]);
            if (sensor4digit[selectUgvUav, co] > 1000)
            {
                label9.Visible = true;
            }
            else
            {
                label9.Visible = false;
            }
            if (sensor4digit[selectUgvUav, h2s] > 500)
            {
                label10.Visible = true;
            }
            else
            {
                label10.Visible = false;
            }
            if ((sensor15digit[selectUgvUav, 0] != 0)&& (sensor15digit[selectUgvUav, 1] != 0)){
                try
                {
                    PointLatLng point = new PointLatLng(sensor15digit[selectUgvUav, 0], sensor15digit[selectUgvUav, 1]);
                    gMapControl1.Overlays[0].Markers[0].Position = point;
                    gMapControl1.Refresh();

                    sensor1.gMapControl1.Overlays[0].Markers[0].Position = point;
                    sensor1.gMapControl1.Refresh();
                }
                catch
                {

                }
            }
            //label1.Text = sensor15digit[selectUgvUav, 0].ToString();
            //label2.Text = sensor15digit[selectUgvUav, 1].ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            counter[UGV]++;
            if(counter[UGV] >= 10)
            {
                if (connect_status[UGV])
                {
                    sendPingConnection(UGV);
                }
            }
            if (counter[UGV] >= 3000)
            {
                counter[UGV] = 0;
                timer2.Stop();
                stopwatch1.Stop();
                setAllToZero(UGV);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            counter[UAV]++;
            if (counter[UAV] >= 10)
            {
                if (connect_status[UAV])
                {
                    sendPingConnection(UAV);
                }
            }
            if (counter[UAV] >= 3000)
            {
                counter[UAV] = 0;
                timer3.Stop();
                stopwatch2.Stop();
                setAllToZero(UAV);
            }
        }
    }
}
