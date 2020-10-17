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
using System.IO;

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
        byte flight_mode = 1;
        float roll_angle = 0;
        float pitch_angle = 0;
        float yaw_angle = 0;
        float level_rate = 0;
        int loopTime;
        float compass_heading = 0f;

        string loggingPath = "";
        bool isRecording = false;
        int dataFileCounter = 0;
        int dataLineCounter = 0;
        bool dataFirstLine = true;
        bool recordingPaused = false;
        double dataTimeCounter = 0;

        System.Windows.Forms.Label[] allMarkers = new System.Windows.Forms.Label[4];

        Point[] markerPoints = new Point[4];
        int[] markerSetArray = new int[4];
        int[] markerVisibleArray = new int[4];

        int satelliteCount = 0;
        
        List<string> statusWriteBuffer = new List<string>();
        bool markedToClear = false;

        const int graphCount = 6;
        const int curveCount = 16;

        ZedGraphControl[] graphControlArray = new ZedGraphControl[graphCount];
        GraphPane[] graphPaneArray = new GraphPane[graphCount];
        PointPairList[] pointPairListArray = new PointPairList[curveCount];
        double[] graphScales = new double[graphCount * 2];
        int[] graphCurveCounts = new int[graphCount] { 3, 1, 4, 3, 3, 1 };
        int graphXLength = 150;
        bool[] markedToUpdateGraphs = new bool[curveCount];

        float angle = -500f;

        int axisFontSize = 24;
        int titleSize = 26;

        float loaded_longitude = -91.39382f;
        float loaded_latitude = 40.92384f;
        int loaded_scale = 19;

        Stopwatch updateTimerStopwatch = new Stopwatch();
        Stopwatch dataLogStopwatch = new Stopwatch();

        float timeSinceLastTelem = 0f;

        public int click_lat, click_lon;

        float p_gain_downloaded = 0f;
        float i_gain_downloaded = 0f;
        float d_gain_downloaded = 0f;
        float p_gain_yaw_downloaded = 0f;
        float i_gain_yaw_downloaded = 0f;
        float d_gain_yaw_downloaded = 0f;
        float p_gain_altitude_downloaded = 0f;
        float i_gain_altitude_downloaded = 0f;
        float d_gain_altitude_downloaded = 0f;
        float p_gain_gps_downloaded = 0f;
        float d_gain_gps_downloaded = 0f;
        float i_gain_gps_downloaded = 0f;

        float sonaralt_downloaded = 0f;
        float baroalt_downloaded = 0f;

        float p_gain_altitude_captured = 0f;
        float i_gain_altitude_captured = 0f;
        float d_gain_altitude_captured = 0f;
        float p_gain_gps_captured = 0f;
        float d_gain_gps_captured = 0f;
        float i_gain_gps_captured = 0f;
        float sonaralt_captured = 0f;
        float baroalt_captured = 0f;

        bool updatePIDTextbox = false;
        bool updateAltTextbox = false;
        bool newCraftPos = false;

        float battery_voltage = 0f;
        float ultrasonicDistance = 0f;
        float barometerDistance = 0f;
        short raw_battery_voltage = 0;

        int roll_output_downloaded = 0;
        int pitch_output_downloaded = 0;
        int yaw_output_downloaded = 0;
        int throttle_output_downloaded = 0;
        bool updatePIDOutputTextbox = false;

        bool askforpid = false;
        bool askforpid2 = false;
        bool waitingforpidreply = false;
        bool updatepid = false;
        bool updatepid2 = false;
        bool calibrateGyro = false;
        bool calibrateCompass = false;
        bool calibrateESC = false;
        bool updategpshold = false;

        bool askforalt = false;
        bool updatealt = false;
        bool askforgps = false;

        bool holdpos = false;

        short flightModeToSend = 0;

        int packets_received_pps = 0;

        Stopwatch waitingforpidTimer = new Stopwatch();

        Stopwatch waitingsecondPIDTimer = new Stopwatch();
        Stopwatch waitingsecondPIDTimer2 = new Stopwatch();

        public Form1()
        {
            InitializeComponent();

            allMarkers[0] = home_marker;
            allMarkers[1] = positionhold_marker;
            allMarkers[2] = marker2;
            allMarkers[3] = craft_marker;

            markerPoints[0] = new Point(0, 0);
            markerPoints[1] = new Point(0, 0);
            markerPoints[2] = new Point(0, 0);
            markerPoints[3] = new Point(-913936400, 409237500);

            markerVisibleArray[0] = 0;
            markerVisibleArray[1] = 0;
            markerVisibleArray[2] = 0;
            markerVisibleArray[3] = 1;

            graphPaneArray[0] = gyroGraphControl.GraphPane;
            graphPaneArray[1] = throttleGraphControl.GraphPane;
            graphPaneArray[3] = pidoutputGraphControl.GraphPane;
            graphPaneArray[2] = orientationGraphControl.GraphPane;
            graphPaneArray[4] = altitudegraphControl.GraphPane;
            graphPaneArray[5] = loopfrequencyGraphControl.GraphPane;
            graphControlArray[0] = gyroGraphControl;
            graphControlArray[1] = throttleGraphControl;
            graphControlArray[3] = pidoutputGraphControl;
            graphControlArray[2] = orientationGraphControl;
            graphControlArray[4] = altitudegraphControl;
            graphControlArray[5] = loopfrequencyGraphControl;

            graphPaneArray[0].YAxis.Title.Text = "deg/s";
            graphPaneArray[1].YAxis.Title.Text = "Value (uS)";
            graphPaneArray[2].YAxis.Title.Text = "Value (uS)";
            graphPaneArray[3].YAxis.Title.Text = "Angle (deg)";
            graphPaneArray[4].YAxis.Title.Text = "Altitude (m)";
            graphPaneArray[5].YAxis.Title.Text = "uS";

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

                graphScales[i] = 0.001;
                graphScales[i + 1] = -0.001;
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

            int curveIndex = 0;

            graphPaneArray[0].Title.Text = "Gryo";
            LineItem lineItemX = graphPaneArray[0].AddCurve("X", pointPairListArray[curveIndex], Color.Red, SymbolType.None);
            curveIndex++;
            LineItem lineItemY = graphPaneArray[0].AddCurve("Y", pointPairListArray[curveIndex], Color.FromArgb(0, 200, 0), SymbolType.None);
            curveIndex++;
            LineItem lineItemZ = graphPaneArray[0].AddCurve("Z", pointPairListArray[curveIndex], Color.Blue, SymbolType.None);
            curveIndex++;

            graphPaneArray[1].Title.Text = "RC Input";
            LineItem lineItemThrottle = graphPaneArray[1].AddCurve("Throttle", pointPairListArray[curveIndex], Color.Red, SymbolType.None);
            curveIndex++;

            graphPaneArray[2].Title.Text = "PID Outputs";
            LineItem lineItemRollOutput = graphPaneArray[2].AddCurve("Roll", pointPairListArray[curveIndex], Color.Red, SymbolType.None);
            curveIndex++;
            LineItem lineItemPitchOutput = graphPaneArray[2].AddCurve("Pitch", pointPairListArray[curveIndex], Color.FromArgb(0, 200, 0), SymbolType.None);
            curveIndex++;
            LineItem lineItemYawOutput = graphPaneArray[2].AddCurve("Yaw", pointPairListArray[curveIndex], Color.Blue, SymbolType.None);
            curveIndex++;
            LineItem lineItemThrottleOutput = graphPaneArray[2].AddCurve("Throttle", pointPairListArray[curveIndex], Color.Orange, SymbolType.None);
            curveIndex++;

            graphPaneArray[3].Title.Text = "Orientation";
            LineItem lineItemRollAngle = graphPaneArray[3].AddCurve("Roll", pointPairListArray[curveIndex], Color.Red, SymbolType.None);
            curveIndex++;
            LineItem lineItemPitchAngle = graphPaneArray[3].AddCurve("Pitch", pointPairListArray[curveIndex], Color.FromArgb(0, 200, 0), SymbolType.None);
            curveIndex++;
            LineItem lineItemYawAngle = graphPaneArray[3].AddCurve("Yaw", pointPairListArray[curveIndex], Color.Blue, SymbolType.None);
            curveIndex++;

            graphPaneArray[4].Title.Text = "Altitude";
            LineItem lineItemUltrasonic = graphPaneArray[4].AddCurve("Ultrasonic", pointPairListArray[curveIndex], Color.Green, SymbolType.None);
            curveIndex++;
            LineItem lineItemBarometer = graphPaneArray[4].AddCurve("Barometer", pointPairListArray[curveIndex], Color.Blue, SymbolType.None);
            curveIndex++;
            LineItem lineItemAltSetpoint = graphPaneArray[4].AddCurve("Setpoint", pointPairListArray[curveIndex], Color.Red, SymbolType.None);
            curveIndex++;

            graphPaneArray[5].Title.Text = "Control Loop Time";
            LineItem lineItemLoopTime = graphPaneArray[5].AddCurve("Control Loop", pointPairListArray[curveIndex], Color.Red, SymbolType.None);
            curveIndex++;

            saveFileDialogLogging.Title = "Choose Output File";
            saveFileDialogLogging.DefaultExt = "txt";
            saveFileDialogLogging.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialogLogging.FilterIndex = 0;

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

            SerialHelper.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

            RefreshComPorts();
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (SerialHelper.serialPort.IsOpen)
            {
                try
                { 
                    byte cmd = (byte)SerialHelper.serialPort.ReadByte();

                    packets_received_pps++;

                    switch (cmd)
                    {
                        case (byte)SerialHelper.CommandFromSerial.GYRO_PACKET:  //Gyro

                            gyroX = SerialHelper.ReadInt16();
                            gyroY = SerialHelper.ReadInt16();
                            gyroZ = SerialHelper.ReadInt16();

                            roll_angle = SerialHelper.ReadFloat();
                            pitch_angle = SerialHelper.ReadFloat();
                            yaw_angle = SerialHelper.ReadFloat();

                            loopTime = (int)SerialHelper.ReadFloat();

                            throttle = SerialHelper.ReadInt32();

                            /*raw_battery_voltage = SerialHelper.ReadInt16();
                            battery_voltage = (float)Math.Truncate(raw_battery_voltage * 5.6734f) / 100f;

                            loopTime = SerialHelper.ReadInt32();

                            roll_angle = SerialHelper.ReadFloat();
                            pitch_angle = SerialHelper.ReadFloat();
                            yaw_angle = SerialHelper.ReadFloat();

                            flight_mode = SerialHelper.ReadInt16();

                            compass_heading = SerialHelper.ReadFloat();*/

                            graphScales[0] = 0.001;
                            graphScales[1] = -0.001;
                            graphScales[4] = 0.001;
                            graphScales[5] = -0.001;
                            graphScales[10] = 0.001;
                            graphScales[11] = -0.001;

                            UpdateGraph(5, 0, (double)loopTime);

                            UpdateGraph(0, 0, ((double)gyroX) / 65.5f);
                            UpdateGraph(0, 1, ((double)gyroY) / 65.5f);
                            UpdateGraph(0, 2, ((double)gyroZ) / 65.5f);

                            /*UpdateGraph(3, 0, (double)roll_angle);
                            UpdateGraph(3, 1, (double)pitch_angle);
                            UpdateGraph(3, 2, (double)yaw_angle);

                            UpdateGraph(5, 0, (double)loopTime);*/

                            markedToUpdateGraphs[0] = true;

                            markedToUpdateGraphs[3] = true;

                            markedToUpdateGraphs[5] = true;
                            //markedToUpdateGraphs[3] = true;

                            //markedToUpdateGraphs[5] = true;

                            break;

                        case (byte)SerialHelper.CommandFromSerial.PID_GAIN_FIRST_PACKET:  //Download PID

                            p_gain_downloaded = SerialHelper.ReadFloat();
                            i_gain_downloaded = SerialHelper.ReadFloat();
                            d_gain_downloaded = SerialHelper.ReadFloat();

                            p_gain_yaw_downloaded = SerialHelper.ReadFloat();
                            i_gain_yaw_downloaded = SerialHelper.ReadFloat();
                            d_gain_yaw_downloaded = SerialHelper.ReadFloat();

                            updatePIDTextbox = true;

                            waitingforpidreply = false;

                            break;
                        case (byte)SerialHelper.CommandFromSerial.PID_GAIN_SECOND_PACKET:  //Download PID 2

                            p_gain_altitude_downloaded = SerialHelper.ReadFloat();
                            i_gain_altitude_downloaded = SerialHelper.ReadFloat();
                            d_gain_altitude_downloaded = SerialHelper.ReadFloat();

                            p_gain_gps_downloaded = SerialHelper.ReadFloat();
                            i_gain_gps_downloaded = SerialHelper.ReadFloat();
                            d_gain_gps_downloaded = SerialHelper.ReadFloat();

                            updatePIDTextbox = true;

                            waitingforpidreply = false;

                            break;

                        /*case 0x03:  //RC Input
                            int throttleVal = (int)((((byte)serialPort1.ReadByte()) << 24) | (((byte)serialPort1.ReadByte()) << 16) | (((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte()));

                            if (throttleVal != 0)
                            {
                                throttle = throttleVal;
                                UpdateGraph(1, 0, ((double)throttleVal));
                            }

                            break;*/

                        case (byte)SerialHelper.CommandFromSerial.PID_OUTPUT_PACKET:  //PID Outputs
                            //byte[] throttle_output = new byte[4];


                            /*throttle_output[0] = (byte)serialPort1.ReadByte();
                            throttle_output[1] = (byte)serialPort1.ReadByte();
                            throttle_output[2] = (byte)serialPort1.ReadByte();
                            throttle_output[3] = (byte)serialPort1.ReadByte();*/

                            roll_output_downloaded = SerialHelper.ReadInt32();
                            pitch_output_downloaded = SerialHelper.ReadInt32();
                            yaw_output_downloaded = SerialHelper.ReadInt32();
                            throttle_output_downloaded = SerialHelper.ReadInt32();
                            //throttle_output_downloaded = System.BitConverter.ToInt32(throttle_output, 0);

                            graphScales[6] = 0.001;
                            graphScales[7] = -0.001;

                            UpdateGraph(2, 0, (double)roll_output_downloaded);
                            UpdateGraph(2, 1, (double)pitch_output_downloaded);
                            UpdateGraph(2, 2, (double)yaw_output_downloaded);
                            UpdateGraph(2, 3, (double)throttle_output_downloaded);

                            markedToUpdateGraphs[2] = true;

                            //UpdateGraph(2, 3, (double)-throttle_output_downloaded);

                            //updatePIDOutputTextbox = true;
                            break;
                        case (byte)SerialHelper.CommandFromSerial.ALTITUDE_PACKET: //Altitudes
                            barometerDistance = SerialHelper.ReadFloat();
                            //ultrasonicDistance = SerialHelper.ReadFloat();
                            flight_mode = (byte)SerialHelper.serialPort.ReadByte();

                            roll_output_downloaded = SerialHelper.ReadInt32();
                            pitch_output_downloaded = SerialHelper.ReadInt32();

                            graphScales[8] = 0.001;
                            graphScales[9] = -0.001;

                            graphScales[6] = 0.001;
                            graphScales[7] = -0.001;

                            UpdateGraph(2, 0, (double)roll_output_downloaded);
                            UpdateGraph(2, 1, (double)pitch_output_downloaded);

                            //UpdateGraph(4, 0, (double)ultrasonicDistance);
                            UpdateGraph(4, 1, (double)barometerDistance);
                            //UpdateGraph(4, 2, (double)0.500);

                            markedToUpdateGraphs[2] = true;
                            markedToUpdateGraphs[4] = true;
                            break;
                        case (byte)SerialHelper.CommandFromSerial.ALTITUDE_SET_PACKET:
                            //sonaralt_downloaded = SerialHelper.ReadFloat();
                            baroalt_downloaded = SerialHelper.ReadFloat();
                            updateAltTextbox = true;
                            break;
                        case (byte)SerialHelper.CommandFromSerial.GPS_PACKET:

                            byte craftToUpdate = (byte)SerialHelper.serialPort.ReadByte();

                            if (craftToUpdate == 0x00) //Update craft position
                            {
                                satelliteCount = (byte)SerialHelper.serialPort.ReadByte();
                                markerPoints[3].Y = (int)(SerialHelper.ReadInt32() * 10);//Latitude
                                markerPoints[3].X = (int)(SerialHelper.ReadInt32() * 10);//Longitude
                            }
                            else if (craftToUpdate == 0x01) //Update home position
                            {
                                markerPoints[0].Y = (int)(SerialHelper.ReadInt32() * 10);//Latitude
                                markerPoints[0].X = (int)(SerialHelper.ReadInt32() * 10);//Longitude

                                markerVisibleArray[0] = 1;
                            }
                            else if (craftToUpdate == 0x02) //Update gps hold position
                            {
                                markerPoints[1].Y = (int)(SerialHelper.ReadInt32() * 10);//Latitude
                                markerPoints[1].X = (int)(SerialHelper.ReadInt32() * 10);//Longitude
                                markerVisibleArray[1] = 1;
                            }
                            else if (craftToUpdate >= 0x05) //Update waypoints
                            {
                                markerPoints[2].Y = (int)(SerialHelper.ReadInt32() * 10);//Latitude
                                markerPoints[2].X = (int)(SerialHelper.ReadInt32() * 10);//Longitude
                                markerVisibleArray[2] = 1;
                            }

                            newCraftPos = true;

                            break;
                        case (byte)SerialHelper.CommandFromSerial.PRINT_PACKET:
                            byte charCount = (byte)SerialHelper.serialPort.ReadByte();
                            byte[] temp_bytes = new byte[30];

                            for (int i = 0; i < charCount; i++)
                            {
                                temp_bytes[i] = (byte)SerialHelper.serialPort.ReadByte();
                            }

                            statusWriteBuffer.Add(Encoding.UTF8.GetString(temp_bytes));

                            break;
                    }
                }
                catch (Exception ex)
                {
                    statusWriteBuffer.Add("Failed\n");
                }
            }

            while(SerialHelper.serialPort.IsOpen && SerialHelper.serialPort.BytesToRead > 0)
            {
                SerialHelper.serialPort.ReadByte();
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
            SerialHelper.serialPort.DtrEnable = true;
            SerialHelper.serialPort.PortName = port_list.SelectedItem.ToString();
            SerialHelper.serialPort.BaudRate = int.Parse(baud_list.SelectedItem.ToString());

            try
            {
                SerialHelper.serialPort.Open();
                statusWriteBuffer.Add("Connected\n");
                connect_btn.Visible = false;
                disconnect_btn.Visible = true;
                refreshcom_btn.Visible = false;

                poshold_btn.Enabled = true;
                rth_btn.Enabled = true;
                uploadtuning_btn.Enabled = true;
                uploadaltsetpoint_btn.Enabled = true;
                downloadaltsetpoint_btn.Enabled = true;
                downloadtuning_btn.Enabled = true;
                uploadpos_btn.Enabled = true;
                downloadpos_btn.Enabled = true;
                gyro_callibrate_btn.Enabled = true;
                compass_callibrate_btn.Enabled = true;
                esc_callibrate_btn.Enabled = true;
                levelmode_btn.Enabled = true;
                gpshold_btn.Enabled = true;
                disarm_btn.Enabled = true;
            }
            catch (Exception ex)
            {
                statusWriteBuffer.Add("Failed\n");
            }
        }

        private void disconnect_btn_Click(object sender, EventArgs e)
        {
            statusWriteBuffer.Add("Disconnected\n");

            SerialHelper.serialPort.Close();
            disconnect_btn.Visible = false;
            connect_btn.Visible = true;
            refreshcom_btn.Visible = true;

            poshold_btn.Enabled = false;
            rth_btn.Enabled = false;
            uploadtuning_btn.Enabled = false;
            uploadaltsetpoint_btn.Enabled = false;
            downloadaltsetpoint_btn.Enabled = false;
            downloadtuning_btn.Enabled = false;
            uploadpos_btn.Enabled = false;
            downloadpos_btn.Enabled = false;
            gyro_callibrate_btn.Enabled = false;
            compass_callibrate_btn.Enabled = false;
            esc_callibrate_btn.Enabled = false;
            levelmode_btn.Enabled = false;
            gpshold_btn.Enabled = false;
            disarm_btn.Enabled = false;
        }

        private void UpdateGraph(int graphIndex, int curveIndex, double y)
        {
            if (markedToUpdateGraphs[graphIndex])
                return;

            int localIndex = curveIndex;

            for (int i = 0; i < graphCount; i++)
            {
                if (i == graphIndex)
                    break;

                localIndex += graphCurveCounts[i];
            }

            //Debug.Print(localIndex.ToString() + " : " + graphIndex.ToString());

            for (int i = 0; i < pointPairListArray[localIndex].Count - 1; i++)
            {
                pointPairListArray[localIndex][i].Y = pointPairListArray[localIndex][i + 1].Y;

                if (pointPairListArray[localIndex][i].Y > graphScales[graphIndex * 2])
                {
                    graphScales[graphIndex * 2] = pointPairListArray[localIndex][i].Y;
                }

                if (pointPairListArray[localIndex][i].Y < graphScales[graphIndex * 2 + 1])
                {
                    graphScales[graphIndex * 2 + 1] = pointPairListArray[localIndex][i].Y;
                }
            }

            double x = 0;

            if(pointPairListArray[localIndex].Count >= graphXLength)
                pointPairListArray[localIndex].RemoveAt(pointPairListArray[localIndex].Count - 1);

            PointPair pointPair = new PointPair(x, y);

            pointPairListArray[localIndex].Add(pointPair);

            float bias = 0.01f;
            float headRoom = 0f;


            if (graphIndex == 0)
            {
                headRoom = .3f;

                //graphPaneArray[graphIndex].YAxis.Scale.Max = 10;
                //graphPaneArray[graphIndex].YAxis.Scale.Min = -10;

                //graphPaneArray[graphIndex].YAxis.Scale.Max = (graphScales[graphIndex * 2] + graphScales[graphIndex * 2] * headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Max * (1f - bias);
                //graphPaneArray[graphIndex].YAxis.Scale.Min = (graphScales[graphIndex * 2 + 1] + graphScales[graphIndex * 2 + 1] * headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Min * (1f - bias);
            }
            else if(graphIndex == 1)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = 2000;
                graphPaneArray[graphIndex].YAxis.Scale.Min = 1000;

                //graphPaneArray[graphIndex].YAxis.Scale.Max = (graphScales[graphIndex * 2] + graphScales[graphIndex * 2] * 0.1f) * bias + graphPaneArray[graphIndex].YAxis.Scale.Max * (1f - bias);
                //graphPaneArray[graphIndex].YAxis.Scale.Min = (graphScales[graphIndex * 2 + 1] - graphScales[graphIndex * 2 + 1] * 0.1f) * bias + graphPaneArray[graphIndex].YAxis.Scale.Min * (1f - bias);
            }
            else if (graphIndex == 2)
            {
                //graphPaneArray[graphIndex].YAxis.Scale.Max = 450;
                //graphPaneArray[graphIndex].YAxis.Scale.Min = -450;
                headRoom = 50f;

                //graphPaneArray[graphIndex].YAxis.Scale.Max = (graphScales[graphIndex * 2] + graphScales[graphIndex * 2] * headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Max * (1f - bias);
                //graphPaneArray[graphIndex].YAxis.Scale.Min = (graphScales[graphIndex * 2 + 1] + graphScales[graphIndex * 2 + 1] * headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Min * (1f - bias);
            }
            else if (graphIndex == 3)
            {
                //graphPaneArray[graphIndex].YAxis.Scale.Max = 180;
                //graphPaneArray[graphIndex].YAxis.Scale.Min = -180;
                headRoom  = 2f;

                //graphPaneArray[graphIndex].YAxis.Scale.Max = (graphScales[graphIndex * 2] + graphScales[graphIndex * 2] * headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Max * (1f - bias);
                //graphPaneArray[graphIndex].YAxis.Scale.Min = (graphScales[graphIndex * 2 + 1] + graphScales[graphIndex * 2 + 1] * headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Min * (1f - bias);
            }
            else if(graphIndex == 4)
            {
                headRoom  = 0.3f;
            }
            else if (graphIndex == 5)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = 4500;
                graphPaneArray[graphIndex].YAxis.Scale.Min = 3800;
            }

            if (graphIndex != 1 && graphIndex != 5)
            {
                graphPaneArray[graphIndex].YAxis.Scale.Max = (graphScales[graphIndex * 2] + headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Max * (1f - bias);
                graphPaneArray[graphIndex].YAxis.Scale.Min = (graphScales[graphIndex * 2 + 1] - headRoom) * bias + graphPaneArray[graphIndex].YAxis.Scale.Min * (1f - bias);
            }

            graphPaneArray[graphIndex].XAxis.Scale.Max = 0;
            graphPaneArray[graphIndex].XAxis.Scale.Min = -graphXLength;
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

                            markerPoints[i] = new Point((int)((loaded_longitude - ((float)x * scale_factor) / (111111f * (float)Math.Cos(loaded_latitude * 0.017453f))) * 10000000f), (int)((loaded_latitude + ((float)y * scale_factor) / 111111f) * 10000000f));


                            if (i == 0)
                                sethome_btn.Text = "Set Home";
                            else if (i == 1)
                            {
                                setholdpos_btn.Text = "Set Hold Position";
                                //setholdpos_btn.Visible = false;
                                //clearhold_btn.Visible = true;

                                //poshold_btn.Text = markerPoints[i].X.ToString();
                                //rth_btn.Text = markerPoints[i].Y.ToString();
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

                    float homeLon = markerPoints[i].X / 10000000f;
                    float homeLat = markerPoints[i].Y / 10000000f;

                    Debug.Print(markerPoints[i].X.ToString());

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
            //setholdpos_btn.Visible = true;
        }

        bool waiting_for_endline = false;

        private void mainLoopTimer_Tick(object sender, EventArgs e)
        {
            //gps_lon_label.Text = timeSinceLastTelem.ToString();
            lasttelem_label.Text = "Last Packet: " + timeSinceLastTelem.ToString() + "ms";

            UpdateRawTelem();

            for (int i = 0; i < markedToUpdateGraphs.Length; i++)
            {
                if (markedToUpdateGraphs[i])
                {
                    graphPaneArray[i].AxisChange();
                    //graphControlArray[i].Update();
                    graphControlArray[i].Invalidate();
                    //graphControlArray[i].Refresh();
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

                pgainaltitude_textbox.Text = p_gain_altitude_downloaded.ToString();
                igainaltitude_textbox.Text = i_gain_altitude_downloaded.ToString();
                dgainaltitude_textbox.Text = d_gain_altitude_downloaded.ToString();

                pgaingps_textbox.Text = p_gain_gps_downloaded.ToString();
                dgaingps_textbox.Text = d_gain_gps_downloaded.ToString();
                igaingps_textbox.Text = i_gain_gps_downloaded.ToString();

                sonaralt_textbox.Text = sonaralt_downloaded.ToString();
                baroalt_textbox.Text = baroalt_downloaded.ToString();
            }

            if(updateAltTextbox)
            {
                updateAltTextbox = false;

                sonaralt_textbox.Text = sonaralt_downloaded.ToString();
                baroalt_textbox.Text = baroalt_downloaded.ToString();
            }

            
            if(satelliteCount == 0)
            {
                gps_sats_label.Text = "No Fix";
                gps_lat_label.Text = "~";
                gps_lon_label.Text = "~";
            }
            else
            {
                gps_sats_label.Text = satelliteCount.ToString();


                gps_lat_label.Text = string.Format("{0:N6}", (double)(markerPoints[3].Y / 10000000f));
                gps_lon_label.Text = string.Format("{0:N6}", (double)(markerPoints[3].X / 10000000f));

                //gps_lat_label.Text = (markerPoints[3].Y / 10000000f).ToString();
                //gps_lon_label.Text = (markerPoints[3].X / 10000000f).ToString();

                if (newCraftPos)
                {
                    if(satelliteCount >= 3)
                    {
                        markerVisibleArray[3] = 1;
                    }
                    else
                    {
                        markerVisibleArray[3] = 0;
                    }

                    newCraftPos = false;
                    RefreshMarkerPositions();
                }
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

            if(SerialHelper.serialPort.IsOpen && SerialHelper.serialPort.BytesToRead == 0)
            {
                if(updatealt)
                {
                    float[] gains = new float[1] { float.Parse(baroalt_textbox.Text.ToString()) };
                    byte[] p = new byte[5];
                    p[0] = (byte)SerialHelper.CommandFromSerial.ALTITUDE_SET_REQUEST;
                    System.Buffer.BlockCopy(gains, 0, p, 1, 4);
                    SerialHelper.serialPort.Write(p, 0, 5);

                    updatealt = false;
                    //statusWriteBuffer.Add("Uploaded altitude setpoints.");

                    sonaralt_textbox.Text = "~";
                    baroalt_textbox.Text = "~";
                }
                else if(askforalt)
                {
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.ALTITUDE_REQUEST);
                    askforalt = false;
                    //statusWriteBuffer.Add("Downloading altitude setpoints...");
                }
                else if(askforpid)
                {
                    waitingforpidTimer.Stop();

                    waitingforpidTimer.Reset();
                    waitingforpidTimer.Start();

                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.PID_GAIN_FIRST_REQUEST);
                    askforpid = false;
                    askforpid2 = true;
                    //statusWriteBuffer.Add("Downloading PID values...");

                    waitingsecondPIDTimer2.Reset();
                    waitingsecondPIDTimer2.Stop();
                    waitingsecondPIDTimer2.Start();
                }
                else if (askforpid2 && waitingsecondPIDTimer2.IsRunning && waitingsecondPIDTimer2.ElapsedMilliseconds > 200)
                {
                    waitingsecondPIDTimer.Stop();

                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.PID_GAIN_SECOND_REQUEST);
                    askforpid2 = false;
                }
                else if(updatepid)
                {
                    //serialPort1.Write(new byte[1] { 0x02 }, 0, 1);

                    float[] gains = new float[6] { float.Parse(pgain_textbox.Text.ToString()), float.Parse(igain_textbox.Text.ToString()), float.Parse(dgain_textbox.Text.ToString()), float.Parse(pgainyaw_textbox.Text.ToString()), float.Parse(igainyaw_textbox.Text.ToString()), float.Parse(dgainyaw_textbox.Text.ToString()) } ;
                    byte[] p = new byte[25];
                    p[0] = (byte)SerialHelper.CommandFromSerial.PID_GAIN_FIRST_UPDATE_REQUEST;
                    System.Buffer.BlockCopy(gains, 0, p, 1, 24);
                    SerialHelper.serialPort.Write(p, 0, 25);

                    updatepid = false;
                    updatepid2 = true;
                    //statusWriteBuffer.Add("Uploaded PID values.");

                    p_gain_altitude_captured = float.Parse(pgainaltitude_textbox.Text.ToString());
                    i_gain_altitude_captured = float.Parse(igainaltitude_textbox.Text.ToString());
                    d_gain_altitude_captured = float.Parse(dgainaltitude_textbox.Text.ToString());

                    p_gain_gps_captured = float.Parse(pgaingps_textbox.Text.ToString());
                    d_gain_gps_captured = float.Parse(dgaingps_textbox.Text.ToString());
                    i_gain_gps_captured = float.Parse(igaingps_textbox.Text.ToString());

                    pgain_textbox.Text = "~";
                    igain_textbox.Text = "~";
                    dgain_textbox.Text = "~";

                    pgainyaw_textbox.Text = "~";
                    igainyaw_textbox.Text = "~";
                    dgainyaw_textbox.Text = "~";

                    pgainaltitude_textbox.Text = "~";
                    igainaltitude_textbox.Text = "~";
                    dgainaltitude_textbox.Text = "~";

                    pgaingps_textbox.Text = "~";
                    dgaingps_textbox.Text = "~";
                    igaingps_textbox.Text = "~";


                    waitingsecondPIDTimer.Reset();
                    waitingsecondPIDTimer.Stop();
                    waitingsecondPIDTimer.Start();
                }
                else if (updatepid2 && waitingsecondPIDTimer.IsRunning && waitingsecondPIDTimer.ElapsedMilliseconds > 200)
                {
                    float[] gains = new float[6] { p_gain_altitude_captured, i_gain_altitude_captured, d_gain_altitude_captured, p_gain_gps_captured, i_gain_gps_captured, d_gain_gps_captured };
                    byte[] p = new byte[25];
                    p[0] = (byte)SerialHelper.CommandFromSerial.PID_GAIN_SECOND_UPDATE_REQUEST;
                    System.Buffer.BlockCopy(gains, 0, p, 1, 24);
                    SerialHelper.serialPort.Write(p, 0, 25);

                    waitingsecondPIDTimer.Stop();
                    updatepid2 = false;
                }
                else if(calibrateGyro)
                {
                    //statusWriteBuffer.Add("Calibrating Gyro...");
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.CALIBRATE_REQUEST);
                    calibrateGyro = false;
                }
                else if (calibrateCompass)
                {
                    //statusWriteBuffer.Add("Calibrating Compass...");
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.CALIBRATE_COMPASS_REQUEST);
                    calibrateCompass = false;
                }
                else if (calibrateESC)
                {
                    //statusWriteBuffer.Add("Calibrating ESCs...");
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.CALIBRATE_ESC_REQUEST);
                    calibrateESC = false;
                }
                else if (flightModeToSend > 0)
                {
                    //statusWriteBuffer.Add("Disarming...");

                    //Int16[] gains = new Int16[1] { (Int16)flightModeToSend };
                    byte[] p = new byte[3];
                    p[0] = (byte)SerialHelper.CommandFromSerial.FLIGHT_MODE_UPDATE_REQUEST;
                    p[1] = (byte)((flightModeToSend >> 8) & 0xFF);
                    p[2] = (byte)(flightModeToSend & 0xFF);
                    //System.Buffer.BlockCopy(gains, 0, p, 1, 2);
                    SerialHelper.serialPort.Write(p, 0, 3);

                    flightModeToSend = 0;
                }
                else if(askforgps)
                {
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.GPS_PACKET_REQUEST);
                    askforgps = false;
                }
                else if (updategpshold)
                {
                    Int32[] gains = new Int32[2] { (markerPoints[1].Y / 10), (markerPoints[1].X / 10) };
                    byte[] p = new byte[10];
                    p[0] = (byte)SerialHelper.CommandFromSerial.GPS_PACKET_UPDATE_REQUEST;
                    p[1] = (byte)0x01;
                    System.Buffer.BlockCopy(gains, 0, p, 2, 8);
                    SerialHelper.serialPort.Write(p, 0, 10);

                    updategpshold = false;
                }
                else if (holdpos)
                {
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.GPS_COPY_BUFFER_REQUEST);
                    holdpos = false;
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
                if(statusWriteBuffer[i].Contains("\n"))
                {
                    waiting_for_endline = true;
                }
                else
                {
                    waiting_for_endline = false;
                }

                if(waiting_for_endline)
                {
                    statusterminal_textbox.AppendText(Environment.NewLine + "> " + statusWriteBuffer[i]);
                }
                else
                {
                    statusterminal_textbox.AppendText(statusWriteBuffer[i]);
                }

                //StatusWriteLine("> " + statusWriteBuffer[i]);
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
            looptime_label.Text = loopTime.ToString();

            battery_voltage_label.Text = "Battery: " + battery_voltage.ToString() + "V";
            batteryraw_label1.Text = raw_battery_voltage.ToString();

            double x = Math.Truncate(roll_angle * 100) / 100;
            double y = Math.Truncate(pitch_angle * 100) / 100;
            double z = Math.Truncate(yaw_angle * 100) / 100;
            double z1 = Math.Truncate(ultrasonicDistance * 100) / 100;
            double z2 = Math.Truncate(barometerDistance * 100) / 100;
            double z3 = Math.Truncate(compass_heading * 100) / 100;

            roll_label.Text = string.Format("{0:N2}", x);
            pitch_label.Text = string.Format("{0:N2}", y);
            yaw_label.Text = string.Format("{0:N2}", z);
            ultrasonicraw_label.Text = string.Format("{0:N2}", z1);
            barometerraw_label.Text = string.Format("{0:N2}", z2);
            compassheading_label.Text = string.Format("{0:N2}", z3);

            switch (flight_mode)
            {
                case 0:
                    //flight_mode_label.Text = "Flight Mode: Ready - Unarmed";
                    break;
                case 1:
                    flight_mode_label.Text = "Flight Mode: Disarmed";
                    break;
                case 2:
                    flight_mode_label.Text = "Flight Mode: Auto Level";
                    break;
                case 3:
                    flight_mode_label.Text = "Flight Mode: Altitude Hold";
                    break;
                case 4:
                    flight_mode_label.Text = "Flight Mode: GPS Hold";
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

            pgainaltitude_textbox.Text = "~";
            igainaltitude_textbox.Text = "~";
            dgainaltitude_textbox.Text = "~";

            pgaingps_textbox.Text = "~";
            igaingps_textbox.Text = "~";
            dgaingps_textbox.Text = "~";
        }

        private void uploadtuning_btn_Click(object sender, EventArgs e)
        {
            updatepid = true;
        }

        private void gyro_callibrate_btn_Click(object sender, EventArgs e)
        {
            calibrateGyro = true;
        }

        private void gpshold_btn_Click(object sender, EventArgs e)
        {
            flightModeToSend = 8;
        }

        private void disarm_btn_Click(object sender, EventArgs e)
        {
            flightModeToSend = 1;
        }

        private void refreshcom_btn_Click(object sender, EventArgs e)
        {
            RefreshComPorts();
        }

        private void recorddata_btn_Click(object sender, EventArgs e)
        {
            stoprecording_btn.Visible = true;
            recorddata_btn.Visible = false;
            pauserecording_btn.Visible = true;

            DialogResult result = saveFileDialogLogging.ShowDialog();

            if (result == DialogResult.OK)
            {
                loggingPath = Path.GetFullPath(saveFileDialogLogging.FileName);
                loggingPath = loggingPath.Replace(".txt", "");
                isRecording = true;
                File.WriteAllText(loggingPath + "-" + dataFileCounter.ToString() + ".txt", "");
                dataFirstLine = true;
            }
            else if (result == DialogResult.Cancel)
            {
                stoprecording_btn.Visible = false;
                pauserecording_btn.Visible = false;
                recorddata_btn.Visible = true;
                isRecording = false;
            }
        }

        private void stoprecording_btn_Click(object sender, EventArgs e)
        {
            stoprecording_btn.Visible = false;
            recorddata_btn.Visible = true;
            pauserecording_btn.Visible = false;
            isRecording = false;
            dataFirstLine = true;
            dataTimeCounter = 0;

            recordingPaused = false;
            dataFirstLine = true;
            dataLineCounter = 0;
            dataFileCounter = 0;
        }

        private void logging_timer_Tick(object sender, EventArgs e)
        {
            if(isRecording)
            {
                dataTimeCounter += dataLogStopwatch.ElapsedMilliseconds;

                string dataToLog;

                if (!recordingPaused)
                {

                    if (dataFirstLine)
                    {
                        dataToLog = "time(ms)\tgyro_x(deg/s)\tgyro_y(deg/s)\tgyro_z(deg/s)\torientation_roll(deg)\torientation_pitch(deg)\torientation_yaw(deg)\tthrottle\tbattery(V)\tloop_time(uS)\tpid_output_roll\tpid_output_pitch\tpid_output_yaw\tpid_output_throttle\tultrasonic_alt\tbaro_alt\tflight_mode\tp_gain_altitude\ti_gain_altitude\td_gain_altitude\tcompass_heading\tlatitude\tlongitude\n";
                        dataFirstLine = false;
                        File.WriteAllText(loggingPath + "-" + dataFileCounter.ToString() + ".txt", dataToLog);
                        dataLineCounter++;
                    }
                    else
                    {
                        //dataToLog = File.ReadAllText(loggingPath);
                        //dataTimeCounter += dataLogStopwatch.ElapsedMilliseconds;

                        dataToLog = dataTimeCounter.ToString() + "\t";

                        dataToLog += gyroX.ToString() + "\t";
                        dataToLog += gyroY.ToString() + "\t";
                        dataToLog += gyroZ.ToString() + "\t";

                        dataToLog += roll_angle.ToString() + "\t";
                        dataToLog += pitch_angle.ToString() + "\t";
                        dataToLog += yaw_angle.ToString() + "\t";

                        dataToLog += throttle.ToString() + "\t";

                        dataToLog += battery_voltage.ToString() + "\t";

                        dataToLog += loopTime.ToString() + "\t";

                        dataToLog += roll_output_downloaded.ToString() + "\t";
                        dataToLog += pitch_output_downloaded.ToString() + "\t";
                        dataToLog += yaw_output_downloaded.ToString() + "\t";
                        dataToLog += throttle_output_downloaded.ToString() + "\t";

                        dataToLog += ultrasonicDistance.ToString() + "\t";
                        dataToLog += barometerDistance.ToString() + "\t";
                        dataToLog += flight_mode.ToString() + "\t";

                        dataToLog += p_gain_altitude_downloaded.ToString() + "\t";
                        dataToLog += i_gain_altitude_downloaded.ToString() + "\t";
                        dataToLog += d_gain_altitude_downloaded.ToString() + "\t";

                        dataToLog += compass_heading.ToString() + "\t";
                        dataToLog += (markerPoints[3].Y / 10000000f).ToString() + "\t";
                        dataToLog += (markerPoints[3].X / 10000000f).ToString() + "\n";

                        File.AppendAllText(loggingPath + "-" + dataFileCounter.ToString() + ".txt", dataToLog);
                        dataLineCounter++;

                        if (dataLineCounter > 300)
                        {
                            dataLineCounter = 0;
                            dataFirstLine = true;
                            dataFileCounter++;
                        }
                    }
                }

                dataLogStopwatch.Stop();
                dataLogStopwatch.Reset();
                dataLogStopwatch.Start();
            }
        }

        private void pauserecording_btn_Click(object sender, EventArgs e)
        {
            if (recordingPaused)
                pauserecording_btn.Text = "Pause";
            else pauserecording_btn.Text = "Resume";

            recordingPaused = !recordingPaused;
        }

        private void downloadaltsetpoint_btn_Click(object sender, EventArgs e)
        {
            askforalt = true;

            sonaralt_textbox.Text = "~";
            baroalt_textbox.Text = "~";
        }

        private void uploadaltsetpoint_btn_Click(object sender, EventArgs e)
        {
            updatealt = true;
        }

        private void compass_callibrate_btn_Click(object sender, EventArgs e)
        {
            calibrateCompass = true;
        }

        private void esc_callibrate_btn_Click(object sender, EventArgs e)
        {
            calibrateESC = true;
        }

        private void land_btn_Click(object sender, EventArgs e)
        {
            flightModeToSend = 7;
        }

        private void uploadpos_btn_Click(object sender, EventArgs e)
        {
            updategpshold = true;
        }

        private void downloadpos_btn_Click(object sender, EventArgs e)
        {
            askforgps = true;
        }

        private void poshold_btn_Click(object sender, EventArgs e)
        {
            holdpos = true;
        }

        private void PPS_timer_Tick(object sender, EventArgs e)
        {
            pps_label.Text = "PPS: " + packets_received_pps.ToString();
            packets_received_pps = 0;
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
