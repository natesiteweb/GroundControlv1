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
        short flight_mode = -1;
        float roll_angle = 0;
        float pitch_angle = 0;
        float yaw_angle = 0;
        float level_rate = 0;
        int loopTime;

        string loggingPath = "";
        bool isRecording = false;
        int dataFileCounter = 0;
        int dataLineCounter = 0;
        bool dataFirstLine = true;
        double dataTimeCounter = 0;

        System.Windows.Forms.Label[] allMarkers = new System.Windows.Forms.Label[3];

        Point[] markerPoints = new Point[3];
        int[] markerSetArray = new int[3];
        int[] markerVisibleArray = new int[3];
        
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

        float p_gain_altitude_captured = 0f;
        float i_gain_altitude_captured = 0f;
        float d_gain_altitude_captured = 0f;

        bool updatePIDTextbox = false;

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

        short flightModeToSend = 0;

        Stopwatch waitingforpidTimer = new Stopwatch();

        Stopwatch waitingsecondPIDTimer = new Stopwatch();
        Stopwatch waitingsecondPIDTimer2 = new Stopwatch();

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

                    switch (cmd)
                    {
                        case (byte)SerialHelper.CommandFromSerial.GYRO_PACKET:  //Gyro

                            gyroX = SerialHelper.ReadInt16();
                            gyroY = SerialHelper.ReadInt16();
                            gyroZ = SerialHelper.ReadInt16();

                            raw_battery_voltage = SerialHelper.ReadInt16();
                            battery_voltage = (float)Math.Truncate(raw_battery_voltage * 5.6734f) / 100f;

                            loopTime = SerialHelper.ReadInt32();

                            roll_angle = SerialHelper.ReadFloat();
                            pitch_angle = SerialHelper.ReadFloat();
                            yaw_angle = SerialHelper.ReadFloat();

                            flight_mode = SerialHelper.ReadInt16();

                            graphScales[0] = 0.001;
                            graphScales[1] = -0.001;
                            graphScales[4] = 0.001;
                            graphScales[5] = -0.001;
                            graphScales[10] = 0.001;
                            graphScales[11] = -0.001;

                            UpdateGraph(0, 0, ((double)gyroX) / 65.5f);
                            UpdateGraph(0, 1, ((double)gyroY) / 65.5f);
                            UpdateGraph(0, 2, ((double)gyroZ) / 65.5f);

                            UpdateGraph(3, 0, (double)roll_angle);
                            UpdateGraph(3, 1, (double)pitch_angle);
                            UpdateGraph(3, 2, (double)yaw_angle);

                            UpdateGraph(5, 0, (double)loopTime);

                            markedToUpdateGraphs[0] = true;

                            markedToUpdateGraphs[3] = true;

                            markedToUpdateGraphs[5] = true;

                            /*byte[] valX = new byte[2];
                            byte[] valY = new byte[2];
                            byte[] valZ = new byte[2];

                            valX[0] = (byte)serialPort1.ReadByte();
                            valX[1] = (byte)serialPort1.ReadByte();

                            valY[0] = (byte)serialPort1.ReadByte();
                            valY[1] = (byte)serialPort1.ReadByte();

                            valZ[0] = (byte)serialPort1.ReadByte();
                            valZ[1] = (byte)serialPort1.ReadByte();

                            gyroX = System.BitConverter.ToInt16(valX, 0);
                            gyroY = System.BitConverter.ToInt16(valY, 0);
                            gyroZ = System.BitConverter.ToInt16(valZ, 0);

                            byte[] roll_angle_output = new byte[4];
                            byte[] pitch_angle_output = new byte[4];
                            byte[] yaw_angle_output = new byte[4];

                            byte[] flight_mode_byte_array = new byte[2];

                            byte[] loopTime_byte_array = new byte[4];

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

                            flight_mode_byte_array[0] = (byte)serialPort1.ReadByte();
                            flight_mode_byte_array[1] = (byte)serialPort1.ReadByte();

                            flight_mode = BitConverter.ToInt16(flight_mode_byte_array, 0);

                            //battery_voltage = (float)serialPort1.ReadByte();
                            //battery_voltage += (float)Math.Truncate((double)serialPort1.ReadByte()) / 100f;
                            raw_battery_voltage = ((short)((((byte)serialPort1.ReadByte()) << 8) | ((byte)serialPort1.ReadByte())));
                            battery_voltage = (float)Math.Truncate(raw_battery_voltage * 5.6734f) / 100f;

                            loopTime_byte_array[0] = (byte)serialPort1.ReadByte();
                            loopTime_byte_array[1] = (byte)serialPort1.ReadByte();
                            loopTime_byte_array[2] = (byte)serialPort1.ReadByte();
                            loopTime_byte_array[3] = (byte)serialPort1.ReadByte();

                            loopTime = BitConverter.ToInt32(loopTime_byte_array, 0);

                            roll_angle = System.BitConverter.ToSingle(roll_angle_output, 0);
                            pitch_angle = System.BitConverter.ToSingle(pitch_angle_output, 0);
                            yaw_angle = System.BitConverter.ToSingle(yaw_angle_output, 0);

                            graphScales[0] = 0.001;
                            graphScales[1] = -0.001;
                            graphScales[4] = 0.001;
                            graphScales[5] = -0.001;
                            graphScales[10] = 0.001;
                            graphScales[11] = -0.001;

                            UpdateGraph(0, 0, ((double)gyroX) / 65.5f);
                            UpdateGraph(0, 1, ((double)gyroY) / 65.5f);
                            UpdateGraph(0, 2, ((double)gyroZ) / 65.5f);

                            UpdateGraph(3, 0, (double)roll_angle);
                            UpdateGraph(3, 1, (double)pitch_angle);
                            UpdateGraph(3, 2, (double)yaw_angle);

                            UpdateGraph(5, 0, (double)loopTime);*/

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
                            ultrasonicDistance = SerialHelper.ReadFloat();

                            graphScales[8] = 0.001;
                            graphScales[9] = -0.001;

                            UpdateGraph(4, 0, (double)ultrasonicDistance);
                            UpdateGraph(4, 1, (double)barometerDistance);
                            UpdateGraph(4, 2, (double)0.500);

                            markedToUpdateGraphs[4] = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    statusWriteBuffer.Add("Failed");
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
                statusWriteBuffer.Add("Connected");
                connect_btn.Visible = false;
                disconnect_btn.Visible = true;
                refreshcom_btn.Visible = false;

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

            SerialHelper.serialPort.Close();
            disconnect_btn.Visible = false;
            connect_btn.Visible = true;
            refreshcom_btn.Visible = true;

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
            if (markedToUpdateGraphs[graphIndex])
                return;

            int localIndex = curveIndex;

            for (int i = 0; i < graphCount; i++)
            {
                if (i == graphIndex)
                    break;

                localIndex += graphCurveCounts[i];
            }

            Debug.Print(localIndex.ToString() + " : " + graphIndex.ToString());

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
                if(askforpid)
                {
                    waitingforpidTimer.Stop();

                    waitingforpidTimer.Reset();
                    waitingforpidTimer.Start();

                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.PID_GAIN_FIRST_REQUEST);
                    askforpid = false;
                    askforpid2 = true;
                    statusWriteBuffer.Add("Downloaded PID values.");

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
                    statusWriteBuffer.Add("Uploaded PID values.");

                    p_gain_altitude_captured = float.Parse(pgainaltitude_textbox.Text.ToString());
                    i_gain_altitude_captured = float.Parse(igainaltitude_textbox.Text.ToString());
                    d_gain_altitude_captured = float.Parse(dgainaltitude_textbox.Text.ToString());

                    pgain_textbox.Text = "~";
                    igain_textbox.Text = "~";
                    dgain_textbox.Text = "~";

                    pgainyaw_textbox.Text = "~";
                    igainyaw_textbox.Text = "~";
                    dgainyaw_textbox.Text = "~";

                    pgainaltitude_textbox.Text = "~";
                    igainaltitude_textbox.Text = "~";
                    dgainaltitude_textbox.Text = "~";

                    waitingsecondPIDTimer.Reset();
                    waitingsecondPIDTimer.Stop();
                    waitingsecondPIDTimer.Start();
                }
                else if (updatepid2 && waitingsecondPIDTimer.IsRunning && waitingsecondPIDTimer.ElapsedMilliseconds > 200)
                {
                    float[] gains = new float[3] { p_gain_altitude_captured, i_gain_altitude_captured, d_gain_altitude_captured };
                    byte[] p = new byte[13];
                    p[0] = (byte)SerialHelper.CommandFromSerial.PID_GAIN_SECOND_UPDATE_REQUEST;
                    System.Buffer.BlockCopy(gains, 0, p, 1, 12);
                    SerialHelper.serialPort.Write(p, 0, 13);

                    waitingsecondPIDTimer.Stop();
                    updatepid2 = false;
                }

                if(calibrateGyro)
                {
                    statusWriteBuffer.Add("Calibrating Gyro...");
                    SerialHelper.SetPacketID((byte)SerialHelper.CommandFromSerial.CALIBRATE_REQUEST);
                    calibrateGyro = false;
                }

                /*else if(flightModeToSend > 0)
                {
                    statusWriteBuffer.Add("Changing Flight Mode...");
                    serialPort1.Write(new byte[3] { 0x04, (byte)((flightModeToSend >> 8) & 0xFF), (byte)(flightModeToSend & 0xFF) }, 0, 3);

                    flightModeToSend = 0;
                }*/
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
            looptime_label.Text = loopTime.ToString();

            battery_voltage_label.Text = "Battery: " + battery_voltage.ToString() + "V";
            batteryraw_label1.Text = raw_battery_voltage.ToString();

            double x = Math.Truncate(roll_angle * 100) / 100;
            double y = Math.Truncate(pitch_angle * 100) / 100;
            double z = Math.Truncate(yaw_angle * 100) / 100;
            double z1 = Math.Truncate(ultrasonicDistance * 100) / 100;
            double z2 = Math.Truncate(barometerDistance * 100) / 100;

            roll_label.Text = string.Format("{0:N2}", x);
            pitch_label.Text = string.Format("{0:N2}", y);
            yaw_label.Text = string.Format("{0:N2}", z);
            ultrasonicraw_label.Text = string.Format("{0:N2}", z1);
            barometerraw_label.Text = string.Format("{0:N2}", z2);

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
                    flight_mode_label.Text = "Flight Mode: Altitude Hold - Ultrasonic";
                    break;
                case 5:
                    flight_mode_label.Text = "Flight Mode: Altitude Hold - Barometer";
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

        private void refreshcom_btn_Click(object sender, EventArgs e)
        {
            RefreshComPorts();
        }

        private void recorddata_btn_Click(object sender, EventArgs e)
        {
            stoprecording_btn.Visible = true;
            recorddata_btn.Visible = false;

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
                recorddata_btn.Visible = true;
                isRecording = false;
            }
        }

        private void stoprecording_btn_Click(object sender, EventArgs e)
        {
            stoprecording_btn.Visible = false;
            recorddata_btn.Visible = true;
            isRecording = false;
            dataFirstLine = true;
            dataTimeCounter = 0;
        }

        private void logging_timer_Tick(object sender, EventArgs e)
        {
            if(isRecording)
            {
                string dataToLog;

                if(dataFirstLine)
                {
                    dataToLog = "time(ms)\tgyro_x(deg/s)\tgyro_y(deg/s)\tgyro_z(deg/s)\torientation_roll(deg)\torientation_pitch(deg)\torientation_yaw(deg)\tthrottle\tbattery(V)\tloop_time(uS)\tpid_output_roll\tpid_output_pitch\tpid_output_yaw\tpid_output_throttle\tultrasonic_alt\tbaro_alt\tflight_mode\n";
                    dataFirstLine = false;
                    File.WriteAllText(loggingPath + "-" + dataFileCounter.ToString() + ".txt", dataToLog);
                    dataLineCounter++;
                }
                else
                {
                    //dataToLog = File.ReadAllText(loggingPath);
                    dataTimeCounter += dataLogStopwatch.ElapsedMilliseconds;

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
                    dataToLog += flight_mode.ToString() + "\n";

                    File.AppendAllText(loggingPath + "-" + dataFileCounter.ToString() + ".txt", dataToLog);
                    dataLineCounter++;

                    if(dataLineCounter > 300)
                    {
                        dataLineCounter = 0;
                        dataFirstLine = true;
                        dataFileCounter++;
                    }
                }

                dataLogStopwatch.Stop();
                dataLogStopwatch.Reset();
                dataLogStopwatch.Start();
            }
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
