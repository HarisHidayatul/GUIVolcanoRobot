using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace PA2GuiVolcanoRobot
{
    public partial class setting : UserControl
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

        private int selectTab = 0;
        public int selectUGVUAV = 0;
        struct sensorr
        {
            public string id_name;
            public int zeroSetting;
            public int spanSetting;
        }
        sensorr[] sensor = new sensorr[12];

        private SqlCeCommand cmd;
        private SqlCeConnection conn;
        private SqlCeDataAdapter da;
        private DataTable[] dt = new DataTable[2];

        public int getZero(int accss)
        {
            //if (dt[accss] != null) { return dt[accss]; }
            //return new DataTable();
            switch (accss)
            {
                case humadity:
                    return sensor[0].zeroSetting;
                case temperature:
                    return sensor[1].zeroSetting;
                case battery:
                    return sensor[2].zeroSetting;
                case altitude:
                    return sensor[3].zeroSetting;
                case co:
                    return sensor[4].zeroSetting;
                case h2s:
                    return sensor[5].zeroSetting;
                case compass:
                    return sensor[8].zeroSetting;
                case roll:
                    return sensor[9].zeroSetting;
                case pitch:
                    return sensor[10].zeroSetting;
                case yaw:
                    return sensor[11].zeroSetting;
                default:
                    return 0;
            }

        }
        public int getSpan(int accss)
        {
            //if (dt[accss] != null) { return dt[accss]; }
            //return new DataTable();
            switch (accss)
            {
                case humadity:
                    return sensor[0].spanSetting;
                case temperature:
                    return sensor[1].spanSetting;
                case battery:
                    return sensor[2].spanSetting;
                case altitude:
                    return sensor[3].spanSetting;
                case co:
                    return sensor[4].spanSetting;
                case h2s:
                    return sensor[5].spanSetting;
                case compass:
                    return sensor[8].spanSetting;
                case roll:
                    return sensor[9].spanSetting;
                case pitch:
                    return sensor[10].spanSetting;
                case yaw:
                    return sensor[11].spanSetting;
                default:
                    return 100;
            }
        }

        private readonly string[] dbName = { "dbUGV", "dbUAV" };

        private string c = "DataSource=\"setting.sdf\"; Password=\"mypassword\"";

        containPanel[] containpanel = new containPanel[4];
        struct containPanel
        {
            public Panel panelMain,panelZeroBtn,panelSpanBtn,panelZeroAll,panelSpanAll;
            public Label labelTitle, lblTxtZero, lblTxtSpan, lblValZero, lblVanSpan;
            public Button buttonZero1, buttonZero2, buttonSpan1, buttonSpan2;
        }
        private void initContain(int indeks,Panel mainPanel,Label labelTitle,
                                 Label labelZero,Label labelSpan,Button buttonZero1,
                                 Button buttonZero2,Panel panelZeroBtn,Button buttonSpan1,
                                 Button buttonSpan2,Panel panelSpanBtn,Label labelValZero,
                                 Label labelValSpan,Panel panelZeroAll,Panel panelSpanAll)
        {
            containpanel[indeks].panelMain = mainPanel;
            containpanel[indeks].labelTitle = labelTitle;
            containpanel[indeks].lblTxtZero = labelZero;
            containpanel[indeks].lblTxtSpan = labelSpan;
            containpanel[indeks].buttonZero1 = buttonZero1;
            containpanel[indeks].buttonZero2 = buttonZero2;
            containpanel[indeks].panelZeroBtn = panelZeroBtn;
            containpanel[indeks].buttonSpan1 = buttonSpan1;
            containpanel[indeks].buttonSpan2 = buttonSpan2;
            containpanel[indeks].panelSpanBtn = panelSpanBtn;
            containpanel[indeks].lblValZero = labelValZero;
            containpanel[indeks].lblVanSpan = labelValSpan;
            containpanel[indeks].panelZeroAll = panelZeroAll;
            containpanel[indeks].panelSpanAll = panelSpanAll;
        }
        public setting()
        {
            InitializeComponent();
            initContain(0, this.panel4, this.label2, this.label3, this.label6, this.button2, this.button1, this.panel7, this.button4, this.button3, this.panel9, this.label4, this.label5, this.panel6, this.panel8);
            initContain(1, this.panel10, this.label11, this.label10, this.label8, this.button8, this.button7, this.panel15, this.button6, this.button5, this.panel13, this.label9, this.label7, this.panel14, this.panel12);
            initContain(2, this.panel16, this.label16, this.label15, this.label13, this.button12, this.button11, this.panel21, this.button10, this.button9, this.panel19, this.label14, this.label12, this.panel20, this.panel18);
            initContain(3, this.panel22, this.label21, this.label20, this.label18, this.button16, this.button15, this.panel27, this.button14, this.button13, this.panel25, this.label19, this.label17, this.panel26, this.panel24);

            initSensor();

            for (int i = 0; i < 4; i++)
            {
                //it's important to have this; closing over the loop variable would be bad
                containpanel[i].buttonZero2.Click += new EventHandler(addZero);
                containpanel[i].buttonZero1.Click += new EventHandler(minZero);
                containpanel[i].buttonSpan2.Click += new EventHandler(addSpan);
                containpanel[i].buttonSpan1.Click += new EventHandler(minSpan);
            }


        }
        private void setting_Load(object sender, EventArgs e)
        {
            
            
            changeSensor(sensor[0], sensor[1], sensor[2], sensor[3]);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (!File.Exists("setting.sdf"))
            {
                createDB();
                fillinitDB(0);
                fillinitDB(1);
                refreshDBTable();
                for (int i = 0; i < 12; i++)
                {
                    sensor[i].zeroSetting = 0;
                    sensor[i].spanSetting = 100;
                }
            }
            else
            {
                refreshDBTable();
                for (int i = 0; i < 12; i++)
                {
                    sensor[i].zeroSetting = int.Parse(dt[selectUGVUAV].Rows[i][3].ToString());
                    sensor[i].spanSetting = int.Parse(dt[selectUGVUAV].Rows[i][2].ToString());
                }
            }
            changeSensor(sensor[4 * selectTab], sensor[4 * selectTab + 1], sensor[4 * selectTab + 2], sensor[4 * selectTab + 3]);
        }
        private void addSpan(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                if (sender == containpanel[i].buttonSpan2) { break; }
            }
            int tempAdd = int.Parse(containpanel[i].lblVanSpan.Text);
            tempAdd++;
            containpanel[i].lblVanSpan.Text = tempAdd.ToString();
        }
        private void minSpan(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                if (sender == containpanel[i].buttonSpan1) { break; }
            }
            int tempAdd = int.Parse(containpanel[i].lblVanSpan.Text);
            tempAdd--;
            containpanel[i].lblVanSpan.Text = tempAdd.ToString();
        }
        private void addZero(object sender, EventArgs e)
        {
            int i = 0;
            for(i = 0; i < 4; i++)
            {
                if(sender == containpanel[i].buttonZero2) { break; }
            }
            int tempAdd = int.Parse(containpanel[i].lblValZero.Text);
            tempAdd++;
            containpanel[i].lblValZero.Text = tempAdd.ToString();
        }
        private void minZero(object sender,EventArgs e)
        {
            int i = 0;
            for (i = 0; i < 4; i++)
            {
                if (sender == containpanel[i].buttonZero1) { break; }
            }
            int tempAdd = int.Parse(containpanel[i].lblValZero.Text);
            tempAdd--;
            containpanel[i].lblValZero.Text = tempAdd.ToString();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 4; i++)
            {
                updateDB(selectUGVUAV, selectTab * 4 + i+1, 0,int.Parse(containpanel[i].lblValZero.Text));
                updateDB(selectUGVUAV, selectTab * 4 + i+1, 1, int.Parse(containpanel[i].lblVanSpan.Text));
            }
            refreshDBTable();
        }
        

        private void initSensor()
        {
            sensor[0].id_name = "HUMIDITY";
            sensor[1].id_name = "TEMPERATURE";
            sensor[2].id_name = "BATTERY";
            sensor[3].id_name = "ALTITUDE";
            sensor[4].id_name = "CO";
            sensor[5].id_name = "H" + '\u2082' + "S";
            sensor[6].id_name = "\0";
            sensor[7].id_name = "\0";
            sensor[8].id_name = "COMPASS";
            sensor[9].id_name = "ROLL";
            sensor[10].id_name = "PITCH";
            sensor[11].id_name = "YAW";

            for(int i = 0; i < 12; i++)
            {
                sensor[i].zeroSetting = 0;
                sensor[i].spanSetting = 100;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            selectTab++;
            if (selectTab > 2) selectTab = 2;
            changeSensor(sensor[4 * selectTab], sensor[4 * selectTab + 1], sensor[4 * selectTab + 2], sensor[4 * selectTab + 3]);

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            selectTab--;
            if (selectTab < 0) selectTab = 0;
            changeSensor(sensor[4 * selectTab], sensor[4 * selectTab + 1], sensor[4 * selectTab + 2], sensor[4 * selectTab + 3]);

        }
        private void setting_SizeChanged(object sender, EventArgs e)
        {
            //panel6.Width = (this.Width - 20) / 2;
            for(int i = 0; i < 4; i++)
            {
                containpanel[i].panelZeroAll.Width = (this.Width - 20) / 2;
                containpanel[i].panelSpanAll.Width = (this.Width - 20) / 2;
                containpanel[i].buttonZero1.Width = (this.Width - 20) / 4;
                containpanel[i].buttonZero2.Width = (this.Width - 20) / 4;
                containpanel[i].buttonSpan1.Width = (this.Width - 20) / 4;
                containpanel[i].buttonSpan2.Width = (this.Width - 20) / 4;
            }
            panel29.Width = (this.Width - 20) / 4;
            //button1.Width = panel6.Width / 2;
        }
        public void increaseSizeText()
        {          
            for(int i = 0; i < 4; i++)
            {
                containpanel[i].buttonSpan1.Font = new Font(containpanel[i].buttonSpan1.Font.Name, containpanel[i].buttonSpan1.Font.Size-1, containpanel[i].buttonSpan1.Font.Style);
                containpanel[i].buttonSpan2.Font = containpanel[i].buttonSpan1.Font;
                containpanel[i].buttonZero1.Font = containpanel[i].buttonSpan1.Font;
                containpanel[i].buttonZero2.Font = containpanel[i].buttonSpan1.Font;

                containpanel[i].labelTitle.Font = new Font(containpanel[i].labelTitle.Font.Name,containpanel[i].labelTitle.Font.Size+1,containpanel[i].labelTitle.Font.Style);

            }
        }
        public void decreaseSizeText()
        {
        }
        private void changeSensor(sensorr sensor1, sensorr sensor2, sensorr sensor3, sensorr sensor4)
        {
            containpanel[0].labelTitle.Text = sensor1.id_name;
            containpanel[0].lblValZero.Text = sensor1.zeroSetting.ToString();
            containpanel[0].lblVanSpan.Text = sensor1.spanSetting.ToString();

            containpanel[1].labelTitle.Text = sensor2.id_name;
            containpanel[1].lblValZero.Text = sensor2.zeroSetting.ToString();
            containpanel[1].lblVanSpan.Text = sensor2.spanSetting.ToString();

            containpanel[2].labelTitle.Text = sensor3.id_name;
            containpanel[2].lblValZero.Text = sensor3.zeroSetting.ToString();
            containpanel[2].lblVanSpan.Text = sensor3.spanSetting.ToString();

            containpanel[3].labelTitle.Text = sensor4.id_name;
            containpanel[3].lblValZero.Text = sensor4.zeroSetting.ToString();
            containpanel[3].lblVanSpan.Text = sensor4.spanSetting.ToString();
        }
        private void createDB()
        {
            try
            {
                SqlCeEngine en = new SqlCeEngine(c);
                en.CreateDatabase();
                conn = new SqlCeConnection(c);
                conn.Open();
                cmd = new SqlCeCommand(@"create table dbUGV
                      (
                      id int primary key identity(1,1),
                      sensor nvarchar(50),
                      span int,
                      zero int
                      )", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCeCommand(@"create table dbUAV
                      (
                      id int primary key identity(1,1),
                      sensor nvarchar(50),
                      span int,
                      zero int
                      )", conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Cannot make database dbUGV dbUAV");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
        }
        private string sensorName(int accss)
        {
            if (accss == 0) return "HUMIDITY";
            else if (accss == 1) return "TEMPERATURE";
            else if (accss == 2) return "BATTERY";
            else if (accss == 3) return "ALTITUDE";
            else if (accss == 4) return "CO";
            else if (accss == 5) return "H2S";
            else if (accss == 6) return "\0";
            else if (accss == 7) return "\0";
            else if (accss == 8) return "COMPASS";
            else if (accss == 9) return "ROLL";
            else if (accss == 10) return "PITCH";
            else if (accss == 11) return "YAW";
            else return "\0";


        }
        private void refreshDBTable()
        {
            try
            {
                conn = new SqlCeConnection(c);
                conn.Open();
                cmd = new SqlCeCommand(@"SELECT * FROM dbUGV ", conn);
                da = new SqlCeDataAdapter(cmd);
                dt[0] = new DataTable();
                da.Fill(dt[0]);

                cmd = new SqlCeCommand(@"SELECT * FROM dbUAV ", conn);
                da = new SqlCeDataAdapter(cmd);
                dt[1] = new DataTable();
                da.Fill(dt[1]);

                for(int i = 0; i < 12; i++)
                {
                    sensor[i].zeroSetting = int.Parse(dt[selectUGVUAV].Rows[i][3].ToString());
                    sensor[i].spanSetting = int.Parse(dt[selectUGVUAV].Rows[i][2].ToString());
                }
            }
            catch
            {
                MessageBox.Show("Cannot Refresh Database");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
            
        }
        private void updateDB(int accss, int id, int accZeroSpan, int value)
        {
            //accZero =0 accSpan =1
            if ((accss < 2) && (accss >= 0))
            {
                try
                {
                    conn = new SqlCeConnection(c);
                    conn.Open();
                    for (int i = 0; i < 12; i++)
                    {
                        string s = "UPDATE " + dbName[accss] + " SET ";
                        if (accZeroSpan == 0)
                        {
                            s = s + "zero = ";
                        }
                        else
                        {
                            s = s + "span = ";
                        }
                        s = s + value.ToString();
                        s = s + " WHERE id = ";
                        s = s + id.ToString();
                        s = s + ";";
                        cmd = new SqlCeCommand(@s, conn);
                        //cmd.Parameters.Add("@sensor", sensorName(i));
                        cmd.ExecuteNonQuery();
                    }

                }
                catch
                {
                    MessageBox.Show("Cannot update DB" + dbName[accss]);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }
        }
        private void fillinitDB(int accss)
        {
            if ((accss < 2) && (accss >= 0))
            {
                try
                {
                    conn = new SqlCeConnection(c);
                    conn.Open();
                    for (int i = 0; i < 12; i++)
                    {
                        string s = "INSERT INTO " + dbName[accss] + " (sensor,span,zero) VALUES (@sensor,100,0)";
                        cmd = new SqlCeCommand(@s, conn);
                        cmd.Parameters.Add("@sensor", sensorName(i));
                        cmd.ExecuteNonQuery();
                    }

                }
                catch
                {
                    MessageBox.Show("Cannot fill init DB" + dbName[accss]);
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                }
            }

        }

    }
}
