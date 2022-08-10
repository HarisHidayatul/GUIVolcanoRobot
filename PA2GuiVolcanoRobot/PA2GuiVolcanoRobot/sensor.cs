using GMap.NET;
using GMap.NET.WindowsForms;
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
    public partial class sensor : UserControl
    {
        GMapOverlay markers = new GMapOverlay("markers");
        //Declare a delegate and Event.  Here called StatusUpdate
        public delegate void StatusUpdateHandler(object sender, EventArgs e);
        public event StatusUpdateHandler cameraSelect;
        public event StatusUpdateHandler gpsSelect;
        public event StatusUpdateHandler show3d;
        private void sendToFormCamera()
        {
            //Create arguments.  You should also have custom one, or else return EventArgs.Empty();
            EventArgs args = new EventArgs();

            //Call any listeners
            cameraSelect?.Invoke(this, args);

        }
        private void sendToFormGPS()
        {
            //Create arguments.  You should also have custom one, or else return EventArgs.Empty();
            EventArgs args = new EventArgs();

            //Call any listeners
            gpsSelect?.Invoke(this, args);

        }
        private void sendToShow3d()
        {
            EventArgs args = new EventArgs();
            show3d?.Invoke(this, args);
        }
        public sensor()
        {
            InitializeComponent();
            gMapControl1.Visible = false;
        }

        private void sensor_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Height = pictureBox1.Width;
            gMapControl1.Height = gMapControl1.Width;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sendToFormCamera();
            label1.Text = "GPS VIEW";
            pictureBox1.Visible = false;
            gMapControl1.Visible = true;
            pictureBox1.Height = pictureBox1.Width;
            gMapControl1.Height = gMapControl1.Width;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sendToShow3d();
        }
        private void gMapControl1_Click_1(object sender, EventArgs e)
        {
            sendToFormGPS();
            label1.Text = "CAMERA VIEW";
            pictureBox1.Visible = true;
            gMapControl1.Visible = false;
            pictureBox1.Height = pictureBox1.Width;
            gMapControl1.Height = gMapControl1.Width;
        }

        private void sensor_Load(object sender, EventArgs e)
        {
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


            double lat = -7.2766120;
            double lng = 112.7940801;
            PointLatLng point = new PointLatLng(lat, lng);
            GMapMarker marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(point, GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green);
            markers.Markers.Add(marker);
            gMapControl1.Overlays.Add(markers);
        }

    }
}
