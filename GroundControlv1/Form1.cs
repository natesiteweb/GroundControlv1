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
using ZedGraph;
using System.Diagnostics;

namespace GroundControlv1
{
    public partial class Form1 : Form
    {
        //GraphPane graphPane;

        //PointPairList pointPairList = new PointPairList();

        //RAW TELEM DATA
        short gyroX = 0;
        short gyroY = 0;
        short gyroZ = 0;
        int throttle = 0;
        short flight_mode = -1;
        float roll_angle = 0;
        float pitch_angle = 0;
        float yaw_angle = 0;
        float level_rate = 0;

        System.Windows.Forms.Label[] allMarkers = new System.Windows.Forms.Label[3];

        Point[] markerPoints = new Point[3];
        int[] markerSetArray = new int[3];
        int[] markerVisibleArray = new int[3];
        
        List<string> statusWriteBuffer = new List<string>();
        bool markedToClear = false;

        ZedGraphControl[] graphControlArray = new ZedGraphControl[4];
        GraphPane[] graphPaneArray = new GraphPane[4];
        PointPairList[] pointPairListArray = new PointPairList[10];
        int graphXLength = 50;
        bool[] markedToUpdateGraphs = new bool[10];

        float angle = -500f;

        int axisFontSize = 24;
        int titleSize = 26;

        float loaded_longitude = -91.39382f;
        float loaded_latitude = 40.92384f;
        int loaded_scale = 19;

        Stopwatch updateTimerStopwatch = new Stopwatch();

        float timeSinceLastTelem = 0f;

        public int click_lat, click_lon;

        float p_gain_downloaded = 0f;
        float i_gain_downloaded = 0f;
        float d_gain_downloaded = 0f;
        float p_gain_yaw_downloaded = 0f;
        float i_gain_yaw_downloaded = 0f;
        float d_gain_yaw_downloaded = 0f;
        bool updatePIDTextbox = false;

        float battery_voltage = 0f;

        int roll_output_downloaded = 0;
        int pitch_output_downloaded = 0;
        int yaw_output_downloaded = 0;
        bool updatePIDOutputTextbox = false;

        bool askforpid = false;
        bool waitingforpidreply = false;
        bool updatepid = false;
        bool calibrateGyro = false;

        short flightModeToSend = 0;

        Stopwatch waitingforpidTimer = new Stopwatch();

        public Form1()
        {
            InitializeComponent();

            allMarkers[0] = home_marker;
            allMarkers[1] = positionhold_marker;
            allMarkers[2] = marker2;

            markerPoints[0] = new Point(0, 0);
            markerPoints[1] = new Point(0, 0);
            markerPoints[2] = new Point(0, 0);

            markerVisibleArray[0] = 0;
            markerVisibleArray[1] = 0;
            markerVisibleArray[2] = 0;

            graphPaneArray[0] = gyroGraphControl.GraphPane;
            graphPaneArray[1] = throttleGraphControl.GraphPane;
            graphPaneArray[3] = pidoutputGraphControl.GraphPane;
            graphPaneArray[2] = orientationGraphControl.GraphPane;
            graphControlArray[0] = gyroGraphControl;
            graphControlArray[1] = throttleGraphControl;
            graphControlArray[3] = pidoutputGraphControl;
            graphControlArray[2] = orientationGraphControl;

            graphPaneArray[0].YAxis.Title.Text = "deg/s";
            graphPaneArray[1].YAxis.Title.Text = "Value (uS)";
            graphPaneArray[2].YAxis.Title.Text = "Value (uS)";
            graphPaneArray[3].YAxis.Title.Text = "Angle (deg)";

            for (int i = 0; i < markedToUpdateGraphs.Length; i++)
            {
                markedToUpdateGraphs[i] = false;
            }

            for (int i = 0; i < graphPaneArray.Length; i++)
            {
                graphPaneArray[i].XAxis.Title.Text = "Samples";
                graphPaneArray[i].XAxis.Title.FontSpec.Size = axisFontSize;
                graphPaneArray[i].YAxis.Title.FontSpec.Size = axisFontSize;
                graphPaneArray[i].XAxis.Scale.FontSpec.Size = 20;
                graphPaneArray[i].YAxis.Scale.FontSpec.Size = 20;
                graphPaneArray[i].Title.FontSpec.Size = titleSize;
                graphPaneArray[i].Legend.FontSpec.Size = axisFontSize;
            }

            //graphPane = gyroGraphControl.GraphPane;
            //graphPane.XAxis.Title.Text = "Time";
            //graphPane.XAxis.Title.FontSpec.Size = axisFontSize;
            //graphPane.YAxis.Title.Text = "Radians";
            //graphPane.YAxis.Title.FontSpec.Size = axisFontSize;
            //graphPane.XAxis.Scale.FontSpec.Size = 20;
            //graphPane.YAxis.Scale.FontSpec.Size = 20;
            //graphPane.Title.Text = "Gryo";
            //graphPane.Title.FontSpec.Size = titleSize;
            //graphPane.Legend.FontSpec.Size = axisFontSize;

            long_input.Text = loaded_longitude.ToString();
            lati_input.Text = loaded_latitude.ToString();

            for(int i = 0; i < pointPairListArray.Length; i++)
            {
                pointPairListArray[i] = new PointPairList();

                for (int j = -graphXLength; j < 0; j++)
                {
                    pointPairListArray[i].Add(new PointPair(j, 0));
                }
            }

            graphPaneArray[0].Title.Text = "Gryo";
            LineItem lineItemX = graphPaneArray[0].AddCurve("X", pointPairListArray[0], Color.Red, SymbolType.None);
            LineItem lineItemY = graphPaneArray[0].AddCurve("Y", pointPairListArray[1], Color.FromArgb(0, 200, 0), SymbolType.None);
            LineItem lineItemZ = graphPaneArray[0].AddCurve("Z", pointPairListArray[2], Color.Blue, SymbolType.None);

            graphPaneArray[1].Title.Text = "RC Input";
            LineItem lineItemThrottle = graphPaneArray[1].AddCurve("Throttle", pointPairListArray[3], Color.Red, SymbolType.None);

            graphPaneArray[2].Title.Text = "PID Outputs";
            LineItem lineItemRollOutput = graphPaneArray[2].AddCurve("Roll", pointPairListArray[4], Color.Red, SymbolType.None);
            LineItem lineItemPitchOutput = graphPaneArray[2].AddCurve("Pitch", pointPairListArray[5], Color.FromArgb(0, 200, 0), SymbolType.None);
            LineItem lineItemYawOutput = graphPaneArray[2].AddCurve("Yaw", pointPairListArray[6], Color.Blue, SymbolType.None);

            graphPaneArray[3].Title.Text = "Orientation";
            LineItem lineItemRollAngle = graphPaneArray[3].AddCurve("Roll", pointPairListArray[7], Color.Red, SymbolType.None);
            LineItem lineItemPitchAngle = graphPaneArray[3].AddCurve("Pitch", pointPairListArray[8], Color.FromArgb(0, 200, 0), SymbolType.None);
            LineItem lineItemYawAngle = graphPaneArray[3].AddCurve("Yaw", pointPairListArray[9], Color.Blue, SymbolType.None);

            //marker1.Location = new Point(webBrowser1.Location.X + (webBrowser1.Width / 2) - 8, webBrowser1.Location.Y + (webBrowser1.Height / 2) - 8);
            //marker1.Visible = true;

            ((Control)webBrowser1).Enabled = false;
            LoadMap();
            //webBrowser1.Navigate("https://www.google.com/maps/@" + loaded_latitude.ToString() + "," + loaded_longitude.ToString() + "," + loaded_scale.ToString() + "m/data=!3m1!1e3");
            //webBrowser1.Navigate(new Uri("https://google.com/maps", UriKind.Absolute), string.Empty, null, string.Format("User-Agent: {0}", "Mozilla/5.0 (Linux; Android 10; SM-G970U) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36 OPR/59.0.2926.53920"));
            //graphPane.Chart.Fill = new Fill(Color.FromArgb(0, 0, 0));
            //graphPane.YAxis.Color = Color.FromArgb(255, 255, 255);
            baud_list.Items.Add(9600);
            baud_list.Items.Add(19200);
            baud_list.Items.Add(57600);
            baud_list.Items.Add(115200);
            baud_list.SelectedIndex = 3;

            RefreshComPorts();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                byte cmd = (byte)serialPort1.ReadByte();

                switch (cmd)
                {
                    case 0x01:  //Gyro
                        short valX = (short)((((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()));
                        short valY = (short)((((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()));
                        short valZ = (short)((((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()));

                        gyroX = valX;
                        gyroY = valY;
                        gyroZ = valZ;

                        byte[] roll_angle_output = new byte[4];
                        byte[] pitch_angle_output = new byte[4];
                        byte[] yaw_angle_output = new byte[4];

                        roll_angle_output[0] = (byte)serialPort1.ReadByte();
                        roll_angle_output[1] = (byte)serialPort1.ReadByte();
                        roll_angle_output[2] = (byte)serialPort1.ReadByte();
                        roll_angle_output[3] = (byte)serialPort1.ReadByte();

                        pitch_angle_output[0] = (byte)serialPort1.ReadByte();
                        pitch_angle_output[1] = (byte)serialPort1.ReadByte();
                        pitch_angle_output[2] = (byte)serialPort1.ReadByte();
                        pitch_angle_output[3] = (byte)serialPort1.ReadByte();

                        yaw_angle_output[0] = (byte)serialPort1.ReadByte();
                        yaw_angle_output[1] = (byte)serialPort1.ReadByte();
                        yaw_angle_output[2] = (byte)serialPort1.ReadByte();
                        yaw_angle_output[3] = (byte)serialPort1.ReadByte();

                        flight_mode = (short)((((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()));

                        //battery_voltage = (float)serialPort1.ReadByte();
                        //battery_voltage += (float)Math.Truncate((double)serialPort1.ReadByte()) / 100f;
                        battery_voltage = (float)Math.Truncate(((short)((((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()))) * 5.6734f) / 100f;

                        roll_angle = System.BitConverter.ToSingle(roll_angle_output, 0);
                        pitch_angle = System.BitConverter.ToSingle(pitch_angle_output, 0);
                        yaw_angle = System.BitConverter.ToSingle(yaw_angle_output, 0);

                        UpdateGraph(0, 0, ((double)valX) / 65.5f);
                        UpdateGraph(0, 1, ((double)valY) / 65.5f);
                        UpdateGraph(0, 2, ((double)valZ) / 65.5f);

                        UpdateGraph(3, 7, (double)roll_angle);
                        UpdateGraph(3, 8, (double)pitch_angle);
                        UpdateGraph(3, 9, (double)yaw_angle);

                        break;

                    case 0x02:  //Download PID
                        byte[] p_gain = new byte[4];
                        byte[] i_gain = new byte[4];
                        byte[] d_gain = new byte[4];

                        byte[] p_gain_yaw = new byte[4];
                        byte[] i_gain_yaw = new byte[4];
                        byte[] d_gain_yaw = new byte[4];

                        byte[] levelrateBytes = new byte[4];

                        p_gain[0] = (byte)serialPort1.ReadByte();
                        p_gain[1] = (byte)serialPort1.ReadByte();
                        p_gain[2] = (byte)serialPort1.ReadByte();
                        p_gain[3] = (byte)serialPort1.ReadByte();

                        i_gain[0] = (byte)serialPort1.ReadByte();
                        i_gain[1] = (byte)serialPort1.ReadByte();
                        i_gain[2] = (byte)serialPort1.ReadByte();
                        i_gain[3] = (byte)serialPort1.ReadByte();

                        d_gain[0] = (byte)serialPort1.ReadByte();
                        d_gain[1] = (byte)serialPort1.ReadByte();
                        d_gain[2] = (byte)serialPort1.ReadByte();
                        d_gain[3] = (byte)serialPort1.ReadByte();

                        p_gain_yaw[0] = (byte)serialPort1.ReadByte();
                        p_gain_yaw[1] = (byte)serialPort1.ReadByte();
                        p_gain_yaw[2] = (byte)serialPort1.ReadByte();
                        p_gain_yaw[3] = (byte)serialPort1.ReadByte();

                        i_gain_yaw[0] = (byte)serialPort1.ReadByte();
                        i_gain_yaw[1] = (byte)serialPort1.ReadByte();
                        i_gain_yaw[2] = (byte)serialPort1.ReadByte();
                        i_gain_yaw[3] = (byte)serialPort1.ReadByte();

                        d_gain_yaw[0] = (byte)serialPort1.ReadByte();
                        d_gain_yaw[1] = (byte)serialPort1.ReadByte();
                        d_gain_yaw[2] = (byte)serialPort1.ReadByte();
                        d_gain_yaw[3] = (byte)serialPort1.ReadByte();

                        levelrateBytes[0] = (byte)serialPort1.ReadByte();
                        levelrateBytes[1] = (byte)serialPort1.ReadByte();
                        levelrateBytes[2] = (byte)serialPort1.ReadByte();
                        levelrateBytes[3] = (byte)serialPort1.ReadByte();

                        p_gain_downloaded = System.BitConverter.ToSingle(p_gain, 0);
                        i_gain_downloaded = System.BitConverter.ToSingle(i_gain, 0);
                        d_gain_downloaded = System.BitConverter.ToSingle(d_gain, 0);

                        p_gain_yaw_downloaded = System.BitConverter.ToSingle(p_gain_yaw, 0);
                        i_gain_yaw_downloaded = System.BitConverter.ToSingle(i_gain_yaw, 0);
                        d_gain_yaw_downloaded = System.BitConverter.ToSingle(d_gain_yaw, 0);

                        level_rate = System.BitConverter.ToSingle(levelrateBytes, 0);

                        updatePIDTextbox = true;

                        waitingforpidreply = false;

                        break;

                    case 0x03:  //RC Input
                        int throttleVal = (int)((((byte)serialPort1.ReadByte()) << 24) | (((byte)serialPort1.ReadByte()) << 16) | (((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()));

                        if (throttleVal != 0)
                        {
                            throttle = throttleVal;
                            UpdateGraph(1, 3, ((double)throttleVal));
                        }

                        break;

                    case 0x04:  //PID Outputs
                        byte[] roll_output = new byte[4];
                        byte[] pitch_output = new byte[4];
                        byte[] yaw_output = new byte[4];

                        roll_output[0] = (byte)serialPort1.ReadByte();
                        roll_output[1] = (byte)serialPort1.ReadByte();
                        roll_output[2] = (byte)serialPort1.ReadByte();
                        roll_output[3] = (byte)serialPort1.ReadByte();

                        pitch_output[0] = (byte)serialPort1.ReadByte();
                        pitch_output[1] = (byte)serialPort1.ReadByte();
                        pitch_output[2] = (byte)serialPort1.ReadByte();
                        pitch_output[3] = (byte)serialPort1.ReadByte();

                        yaw_output[0] = (byte)serialPort1.ReadByte();
                        yaw_output[1] = (byte)serialPort1.ReadByte();
                        yaw_output[2] = (byte)serialPort1.ReadByte();
                        yaw_output[3] = (byte)serialPort1.ReadByte();

                        roll_output_downloaded = System.BitConverter.ToInt32(roll_output, 0);
                        pitch_output_downloaded = System.BitConverter.ToInt32(pitch_output, 0);
                        yaw_output_downloaded = System.BitConverter.ToInt32(yaw_output, 0);

                        UpdateGraph(2, 4, (double)roll_output_downloaded);
                        UpdateGraph(2, 5, (double)pitch_output_downloaded);
                        UpdateGraph(2, 6, (double)yaw_output_downloaded);

                        //updatePIDOutputTextbox = true;
                        break;
                }
            }

            while(serialPort1.IsOpen && serialPort1.BytesToRead > 0)
            {
                serialPort1.ReadByte();
            }

            timeSinceLastTelem = updateTimerStopwatch.ElapsedMilliseconds;

            updateTimerStopwatch.Stop();

            updateTimerStopwatch.Reset();
            updateTimerStopwatch.Start();
        }

        private void RefreshComPorts()
        {
            port_list.Items.Clear();

            foreach (string s in SerialPort.GetPortNames())
            {
                port_list.Items.Add(s);
            }

            port_list.SelectedIndex = 0;
        }

        private void connect_btn_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = port_list.SelectedItem.ToString();
            serialPort1.BaudRate = int.Parse(baud_list.SelectedItem.ToString());

            try
            {
                serialPort1.Open();
                statusWriteBuffer.Add("Connected");
                connect_btn.Visible = false;
                disconnect_btn.Visible = true;

                poshold_btn.Enabled = true;
                rth_btn.Enabled = true;
                uploadtuning_btn.Enabled = true;
                downloadtuning_btn.Enabled = true;
                uploadhomeandpos_btn.Enabled = true;
                gyro_callibrate_btn.Enabled = true;
                levelmode_btn.Enabled = true;
                ratemode_btn.Enabled = true;
                disarm_btn.Enabled = true;
            }
            catch (Exception ex)
            {
                statusWriteBuffer.Add("Failed");
            }
        }

        private void disconnect_btn_Click(object sender, EventArgs e)
        {
            statusWriteBuffer.Add("Disconnected");

            serialPort1.Close();
            disconnect_btn.Visible = false;
            connect_btn.Visible = true;

            poshold_btn.Enabled = false;
            rth_btn.Enabled = false;
            uploadtuning_btn.Enabled = false;
            downloadtuning_btn.Enabled = false;
            uploadhomeandpos_btn.Enabled = false;
            gyro_callibrate_btn.Enabled = false;
            levelmode_btn.Enabled = false;
            ratemode_btn.Enabled = false;
            disarm_btn.Enabled = false;
        }

        private void UpdateGraph(int graphIndex, int curveIndex, double y)
        {
            for (int i = 0; i < pointPairListArray[curveIndex].Count - 1; i++)
            {
                pointPairListArray[curveIndex][i].Y = pointPairListArray[curveIndex][i + 1].Y;
            }

            double x = 0;

            if(pointPairListArray[curveIndex].Count >= graphXLength)
                pointPairListArray[curveIndex].RemoveAt(pointPairListArray[curveIndex].Count - 1);

            PointPair pointPair = new PointPair(x, y);

            pointPairListArray[curveIndex].Add(pointPair);

            if (graphIndex == 0)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = 10;
                graphPaneArray[graphIndex].YAxis.Scale.Min = -10;
            }
            else if(graphIndex == 1)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = 2000;
                graphPaneArray[graphIndex].YAxis.Scale.Min = 1000;
            }
            else if (graphIndex == 2)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = 450;
                graphPaneArray[graphIndex].YAxis.Scale.Min = -450;
            }
            else if (graphIndex == 3)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = 180;
                graphPaneArray[graphIndex].YAxis.Scale.Min = -180;
            }

            graphPaneArray[graphIndex].XAxis.Scale.Max = 0;
            graphPaneArray[graphIndex].XAxis.Scale.Min = -graphXLength;

            graphPaneArray[graphIndex].AxisChange();
            markedToUpdateGraphs[graphIndex] = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            click_lat = e.Y;
            click_lon = e.X;

            if(e.Button == MouseButtons.Left)
            {
                if (click_lat > webBrowser1.Location.Y && click_lat < webBrowser1.Location.Y + webBrowser1.Size.Height && click_lon > webBrowser1.Location.X && click_lon < webBrowser1.Location.X + webBrowser1.Size.Width)
                {
                    //string url = webBrowser1.Url.ToString();
                    //string[] splits = url.Split(',');
                    //string[] splits2 = splits[2].Split('m');

                    //loaded_scale = int.Parse(splits2[0]);

                    for (int i = 0; i < allMarkers.Length; i++)
                    {
                        if(markerSetArray[i] == 1)
                        {
                            int x = (webBrowser1.Location.X + (webBrowser1.Width / 2)) - click_lon;
                            int y = (webBrowser1.Location.Y + (webBrowser1.Height / 2)) - click_lat + 24;


                            //click_lat -= 35;
                            //click_lat -= (webBrowser1.Size.Height / 2) + webBrowser1.Location.Y;
                            //click_lon -= (webBrowser1.Size.Width / 2) + webBrowser1.Location.X;

                            float scale_factor;

                            if (loaded_scale == 19)
                            {
                                scale_factor = 0.298f;
                            }
                            else if (loaded_scale == 18)
                            {
                                scale_factor = 0.596f;
                            }
                            else if (loaded_scale == 17)
                            {
                                scale_factor = 1.193f;
                            }
                            else if (loaded_scale == 16)
                            {
                                scale_factor = 2.387f;
                            }
                            else if (loaded_scale == 15)
                            {
                                scale_factor = 4.773f;
                            }
                            else if (loaded_scale == 14)
                            {
                                scale_factor = 9.547f;
                            }
                            else if (loaded_scale == 13)
                            {
                                scale_factor = 19.093f;
                            }
                            else if (loaded_scale == 12)
                            {
                                scale_factor = 38.187f;
                            }
                            else
                            {
                                scale_factor = 0.298f;
                            }

                            scale_factor *= (float)Math.Cos(loaded_latitude * 0.017453f);

                            //loaded_latitude -= ((float)click_lat * 1.664f) / 1000000f;
                            //loaded_longitude += (float)((float)click_lon * (1.664f / Math.Cos(loaded_latitude * 0.017453f))) / 1000000f;

                            //loaded_longitude = x * scale_factor;
                            //loaded_latitude += ((float)y * scale_factor) / 111111f;
                            //loaded_longitude -= ((float)x * scale_factor) / (111111f * (float)Math.Cos(loaded_latitude * 0.017453f));

                            //homePos.Y = (int)((loaded_latitude + ((float)y * scale_factor) / 111111f) * 1000000f);
                            //homePos.X = (int)((loaded_longitude - ((float)x * scale_factor) / (111111f * (float)Math.Cos(loaded_latitude * 0.017453f))) * 1000000f);

                            markerPoints[i] = new Point((int)((loaded_longitude - ((float)x * scale_factor) / (111111f * (float)Math.Cos(loaded_latitude * 0.017453f))) * 1000000f), (int)((loaded_latitude + ((float)y * scale_factor) / 111111f) * 1000000f));


                            if (i == 0)
                                sethome_btn.Text = "Set Home";
                            else if (i == 1)
                            {
                                setholdpos_btn.Text = "Set Hold Position";
                                setholdpos_btn.Visible = false;
                                clearhold_btn.Visible = true;

                                poshold_btn.Text = markerPoints[i].X.ToString();
                                rth_btn.Text = markerPoints[i].Y.ToString();
                            }

                            allMarkers[i].Location = new Point(click_lon - 8, click_lat - 8);
                            markerVisibleArray[i] = 1;
                            markerSetArray[i] = 0;
                            allMarkers[i].Visible = true;
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                int x = (webBrowser1.Location.X + (webBrowser1.Width / 2)) - click_lon;
                int y = (webBrowser1.Location.Y + (webBrowser1.Height / 2)) - click_lat + 24;

                float scale_factor;

                if(loaded_scale == 19)
                {
                    scale_factor = 0.298f;
                }
                else if (loaded_scale == 18)
                {
                    scale_factor = 0.596f;
                }
                else if (loaded_scale == 17)
                {
                    scale_factor = 1.193f;
                }
                else if (loaded_scale == 16)
                {
                    scale_factor = 2.387f;
                }
                else if (loaded_scale == 15)
                {
                    scale_factor = 4.773f;
                }
                else if (loaded_scale == 14)
                {
                    scale_factor = 9.547f;
                }
                else if (loaded_scale == 13)
                {
                    scale_factor = 19.093f;
                }
                else if (loaded_scale == 12)
                {
                    scale_factor = 38.187f;
                }
                else
                {
                    scale_factor = 0.298f;
                }

                scale_factor *= (float)Math.Cos(loaded_latitude * 0.017453f);

                //loaded_latitude -= ((float)click_lat * 1.664f) / 1000000f;
                //loaded_longitude += (float)((float)click_lon * (1.664f / Math.Cos(loaded_latitude * 0.017453f))) / 1000000f;

                //loaded_longitude = x * scale_factor;
                loaded_latitude += ((float)y * scale_factor) / 111111f;
                loaded_longitude -= ((float)x * scale_factor) / (111111f * (float)Math.Cos(loaded_latitude * 0.017453f));

                long_input.Text = loaded_longitude.ToString();
                lati_input.Text = loaded_latitude.ToString();

                RefreshMarkerPositions();

                LoadMap();
            }
        }

        private void zoomin_btn_Click(object sender, EventArgs e)
        {
            if (loaded_scale < 19)
            {
                loaded_scale += 1;

                RefreshMarkerPositions();
                LoadMap();
            }
        }

        private void zoomout_btn_Click(object sender, EventArgs e)
        {
            if (loaded_scale > 12)
            {
                loaded_scale -= 1;

                RefreshMarkerPositions();
                LoadMap();
            }
        }

        private void loadMap_btn_Click(object sender, EventArgs e)
        {
            loaded_longitude = float.Parse(long_input.Text);
            loaded_latitude = float.Parse(lati_input.Text);

            LoadMap();
        }

        private void RefreshMarkerPositions()
        {
            for(int i = 0; i < allMarkers.Length; i++)
            {
                if (markerPoints[i].X != 0 && markerPoints[i].Y != 0)
                {
                    float scale_factor;

                    if (loaded_scale == 19)
                    {
                        scale_factor = 0.298f;
                    }
                    else if (loaded_scale == 18)
                    {
                        scale_factor = 0.596f;
                    }
                    else if (loaded_scale == 17)
                    {
                        scale_factor = 1.193f;
                    }
                    else if (loaded_scale == 16)
                    {
                        scale_factor = 2.387f;
                    }
                    else if (loaded_scale == 15)
                    {
                        scale_factor = 4.773f;
                    }
                    else if (loaded_scale == 14)
                    {
                        scale_factor = 9.547f;
                    }
                    else if (loaded_scale == 13)
                    {
                        scale_factor = 19.093f;
                    }
                    else if (loaded_scale == 12)
                    {
                        scale_factor = 38.187f;
                    }
                    else
                    {
                        scale_factor = 0.298f;
                    }

                    scale_factor *= (float)Math.Cos(loaded_latitude * 0.017453f);

                    float homeLon = markerPoints[i].X / 1000000f;
                    float homeLat = markerPoints[i].Y / 1000000f;

                    allMarkers[i].Location = new Point((int)((homeLon - loaded_longitude) * (111111f * (float)Math.Cos(loaded_latitude * 0.017453f)) / scale_factor) + (webBrowser1.Location.X + (webBrowser1.Width / 2)) - 8, (int)((homeLat - loaded_latitude) * -111111f / scale_factor) + (webBrowser1.Location.Y + (webBrowser1.Height / 2)) - 8 + 24);

                    if (allMarkers[i].Location.X > (webBrowser1.Location.X + (webBrowser1.Width / 2)) + 600 || allMarkers[i].Location.X < (webBrowser1.Location.X + (webBrowser1.Width / 2)) - 300
                        || allMarkers[i].Location.Y > (webBrowser1.Location.Y + (webBrowser1.Height / 2)) + 300 || allMarkers[i].Location.Y < (webBrowser1.Location.Y + (webBrowser1.Height / 2)) - 250)
                    {
                        Debug.WriteLine("TOO FAR");
                        allMarkers[i].Visible = false;
                    }
                    else if (markerVisibleArray[i] == 1)
                    {
                        allMarkers[i].Visible = true;
                    }
                    else
                    {
                        allMarkers[i].Visible = false;
                    }
                }
            }
        }

        private void sethome_btn_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < allMarkers.Length; i++)
            {
                if (i == 0)
                    markerSetArray[0] = 1;
                else
                    markerSetArray[i] = 0;
            }

            sethome_btn.Text = "Click Map...";
        }

        private void setholdpos_btn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < allMarkers.Length; i++)
            {
                if (i == 1)
                    markerSetArray[1] = 1;
                else
                    markerSetArray[i] = 0;
            }

            setholdpos_btn.Text = "Click Map...";
        }

        private void StatusWriteLine(string text)
        {
            if(statusterminal_textbox.Text == "")
                statusterminal_textbox.AppendText(text);
            else
                statusterminal_textbox.AppendText(Environment.NewLine + text);
        }

        private void StatusClear()
        {
            statusterminal_textbox.Text = "";
            //statusWriteBuffer.Clear();
        }

        private void clearhold_btn_Click(object sender, EventArgs e)
        {
            markerPoints[1] = new Point(0, 0);
            allMarkers[1].Visible = false;
            markerVisibleArray[1] = 0;

            clearhold_btn.Visible = false;
            setholdpos_btn.Visible = true;
        }

        private void mainLoopTimer_Tick(object sender, EventArgs e)
        {
            //gps_lon_label.Text = timeSinceLastTelem.ToString();
            lasttelem_label.Text = "Last Packet: " + timeSinceLastTelem.ToString() + "ms";

            UpdateRawTelem();

            for (int i = 0; i < markedToUpdateGraphs.Length; i++)
            {
                if (markedToUpdateGraphs[i])
                {
                    graphControlArray[i].Refresh();
                    markedToUpdateGraphs[i] = false;
                }
            }

            if(updatePIDTextbox)
            {
                updatePIDTextbox = false;

                pgain_textbox.Text = p_gain_downloaded.ToString();
                igain_textbox.Text = i_gain_downloaded.ToString();
                dgain_textbox.Text = d_gain_downloaded.ToString();

                pgainyaw_textbox.Text = p_gain_yaw_downloaded.ToString();
                igainyaw_textbox.Text = i_gain_yaw_downloaded.ToString();
                dgainyaw_textbox.Text = d_gain_yaw_downloaded.ToString();

                levelrate_textbox.Text = level_rate.ToString();
            }

            /*if (updatePIDOutputTextbox)
            {
                updatePIDOutputTextbox = false;

                pgain_textbox.Text = p_gain_downloaded.ToString();
                igain_textbox.Text = i_gain_downloaded.ToString();
                dgain_textbox.Text = d_gain_downloaded.ToString();

                pgainyaw_textbox.Text = p_gain_yaw_downloaded.ToString();
                igainyaw_textbox.Text = i_gain_yaw_downloaded.ToString();
                dgainyaw_textbox.Text = d_gain_yaw_downloaded.ToString();
            }*/

            if (waitingforpidreply && waitingforpidTimer.ElapsedMilliseconds > 500)
            {
                //askforpid = true;
            }

            if(serialPort1.IsOpen && serialPort1.BytesToRead == 0)
            {
                if(askforpid)
                {
                    waitingforpidTimer.Stop();

                    waitingforpidTimer.Reset();
                    waitingforpidTimer.Start();

                    serialPort1.Write(new byte[1] { 0x01 }, 0, 1);
                    askforpid = false;
                    statusWriteBuffer.Add("Downloaded PID values.");
                }
                else if(updatepid)
                {
                    //serialPort1.Write(new byte[1] { 0x02 }, 0, 1);
                    float[] gains = new float[7] { float.Parse(pgain_textbox.Text.ToString()), float.Parse(igain_textbox.Text.ToString()), float.Parse(dgain_textbox.Text.ToString()), float.Parse(pgainyaw_textbox.Text.ToString()), float.Parse(igainyaw_textbox.Text.ToString()), float.Parse(dgainyaw_textbox.Text.ToString()), float.Parse(levelrate_textbox.Text.ToString()) } ;
                    byte[] p = new byte[29];
                    p[0] = 0x02;
                    System.Buffer.BlockCopy(gains, 0, p, 1, 28);
                    serialPort1.Write(p, 0, 29);

                    updatepid = false;
                    statusWriteBuffer.Add("Uploaded PID values.");

                    pgain_textbox.Text = "~";
                    igain_textbox.Text = "~";
                    dgain_textbox.Text = "~";

                    pgainyaw_textbox.Text = "~";
                    igainyaw_textbox.Text = "~";
                    dgainyaw_textbox.Text = "~";

                    levelrate_textbox.Text = "~";
                }
                else if(calibrateGyro)
                {
                    statusWriteBuffer.Add("Calibrating Gyro...");
                    serialPort1.Write(new byte[1] { 0x03 }, 0, 1);
                    calibrateGyro = false;
                }
                else if(flightModeToSend > 0)
                {
                    statusWriteBuffer.Add("Changing Flight Mode...");
                    serialPort1.Write(new byte[3] { 0x04, (byte)((flightModeToSend >> 8) & 0xFF), (byte)(flightModeToSend & 0xFF) }, 0, 3);

                    flightModeToSend = 0;
                }
            }

            if (markedToClear)
            {
                markedToClear = false;
                statusterminal_textbox.Text = "";
                statusWriteBuffer.Clear();
            }

            for (int i = 0; i < statusWriteBuffer.Count; i++)
            {
                StatusWriteLine("> " + statusWriteBuffer[i]);
            }

            statusWriteBuffer.Clear();
        }

        private void UpdateRawTelem()
        {
            gyroxraw_label.Text = gyroX.ToString();
            gyroyraw_label.Text = gyroY.ToString();
            gyrozraw_label.Text = gyroZ.ToString();
            throttle_label.Text = throttle.ToString();
            flight_mode_raw_label.Text = flight_mode.ToString();

            battery_voltage_label.Text = "Battery: " + battery_voltage.ToString() + "V";

            double x = Math.Truncate(roll_angle * 100) / 100;
            double y = Math.Truncate(pitch_angle * 100) / 100;
            double z = Math.Truncate(yaw_angle * 100) / 100;

            roll_label.Text = string.Format("{0:N2}", x);
            pitch_label.Text = string.Format("{0:N2}", y);
            yaw_label.Text = string.Format("{0:N2}", z);

            switch (flight_mode)
            {
                case 0:
                    flight_mode_label.Text = "Flight Mode: Ready - Unarmed";
                    break;
                case 1:
                    flight_mode_label.Text = "Flight Mode: Disarmed";
                    break;
                case 2:
                    flight_mode_label.Text = "Flight Mode: Rate Mode - Armed";
                    break;
                case 3:
                    flight_mode_label.Text = "Flight Mode: Auto Level - Armed";
                    break;
                case 4:
                    flight_mode_label.Text = "Flight Mode: Starting...";
                    break;
            }
        }

        private void clearstatus_btn_Click(object sender, EventArgs e)
        {
            //statusterminal_textbox.Text = "";
            //statusterminal_textbox.Text = "";
            //statusWriteBuffer.Clear();
            markedToClear = true;
        }

        private void downloadtuning_btn_Click(object sender, EventArgs e)
        {
            askforpid = true;
            waitingforpidreply = true;

            pgain_textbox.Text = "~";
            igain_textbox.Text = "~";
            dgain_textbox.Text = "~";

            pgainyaw_textbox.Text = "~";
            igainyaw_textbox.Text = "~";
            dgainyaw_textbox.Text = "~";

            levelrate_textbox.Text = "~";
        }

        private void uploadtuning_btn_Click(object sender, EventArgs e)
        {
            updatepid = true;
        }

        private void gyro_callibrate_btn_Click(object sender, EventArgs e)
        {
            calibrateGyro = true;
        }

        private void levelmode_btn_Click(object sender, EventArgs e)
        {
            flightModeToSend = 3;
        }

        private void ratemode_btn_Click(object sender, EventArgs e)
        {
            flightModeToSend = 2;
        }

        private void disarm_btn_Click(object sender, EventArgs e)
        {
            flightModeToSend = 1;
        }

        private void LoadMap()
        {
            //With Marker
            //webBrowser1.Navigate("https://www.openstreetmap.org/?mlat=" + loaded_latitude + "&mlon=" + loaded_longitude + "#map=" + loaded_scale + "/" + loaded_latitude + "/" + loaded_longitude + "&layers=N");
            //Without Marker
            webBrowser1.Navigate("https://www.openstreetmap.org/#map=" + loaded_scale + "/" + loaded_latitude + "/" + loaded_longitude);
        }
    }
}
