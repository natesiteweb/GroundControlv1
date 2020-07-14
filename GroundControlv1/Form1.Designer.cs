namespace GroundControlv1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.connect_btn = new System.Windows.Forms.Button();
            this.disconnect_btn = new System.Windows.Forms.Button();
            this.port_list = new System.Windows.Forms.ComboBox();
            this.baud_list = new System.Windows.Forms.ComboBox();
            this.gyroGraphControl = new ZedGraph.ZedGraphControl();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.throttleGraphControl = new ZedGraph.ZedGraphControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.battery_voltage_label = new System.Windows.Forms.Label();
            this.flight_mode_label = new System.Windows.Forms.Label();
            this.lasttelem_label = new System.Windows.Forms.Label();
            this.orientationGraphControl = new ZedGraph.ZedGraphControl();
            this.pidoutputGraphControl = new ZedGraph.ZedGraphControl();
            this.tuning_groupBox = new System.Windows.Forms.GroupBox();
            this.levelrate_textbox = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.dgainyaw_textbox = new System.Windows.Forms.TextBox();
            this.igainyaw_textbox = new System.Windows.Forms.TextBox();
            this.pgainyaw_textbox = new System.Windows.Forms.TextBox();
            this.downloadtuning_btn = new System.Windows.Forms.Button();
            this.dgain_textbox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.igain_textbox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.pgain_textbox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.uploadtuning_btn = new System.Windows.Forms.Button();
            this.statusterminal_groupBox = new System.Windows.Forms.GroupBox();
            this.clearstatus_btn = new System.Windows.Forms.Button();
            this.statusterminal_textbox = new System.Windows.Forms.TextBox();
            this.sendcmd_groupBox = new System.Windows.Forms.GroupBox();
            this.disarm_btn = new System.Windows.Forms.Button();
            this.ratemode_btn = new System.Windows.Forms.Button();
            this.levelmode_btn = new System.Windows.Forms.Button();
            this.gyro_callibrate_btn = new System.Windows.Forms.Button();
            this.poshold_btn = new System.Windows.Forms.Button();
            this.rth_btn = new System.Windows.Forms.Button();
            this.gpstelem_groupBox = new System.Windows.Forms.GroupBox();
            this.gps_sats_label = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.gps_lat_label = new System.Windows.Forms.Label();
            this.gps_lon_label = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.zoomControls = new System.Windows.Forms.GroupBox();
            this.zoomin_btn = new System.Windows.Forms.Button();
            this.zoomout_btn = new System.Windows.Forms.Button();
            this.waypointGroupBox = new System.Windows.Forms.GroupBox();
            this.resetmarkers_btn = new System.Windows.Forms.Button();
            this.mapInput = new System.Windows.Forms.GroupBox();
            this.long_label = new System.Windows.Forms.Label();
            this.lati_label = new System.Windows.Forms.Label();
            this.loadMap_btn = new System.Windows.Forms.Button();
            this.lati_input = new System.Windows.Forms.TextBox();
            this.long_input = new System.Windows.Forms.TextBox();
            this.markerGroupBox = new System.Windows.Forms.GroupBox();
            this.uploadhomeandpos_btn = new System.Windows.Forms.Button();
            this.clearhold_btn = new System.Windows.Forms.Button();
            this.setholdpos_btn = new System.Windows.Forms.Button();
            this.sethome_btn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.home_marker = new System.Windows.Forms.Label();
            this.marker2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.positionhold_marker = new System.Windows.Forms.Label();
            this.telemraw_groupBox = new System.Windows.Forms.GroupBox();
            this.yaw_label = new System.Windows.Forms.Label();
            this.pitch_label = new System.Windows.Forms.Label();
            this.roll_label = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.flight_mode_raw_label = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.throttle_label = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.gyrozraw_label = new System.Windows.Forms.Label();
            this.gyroyraw_label = new System.Windows.Forms.Label();
            this.gyroxraw_label = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.mainLoopTimer = new System.Windows.Forms.Timer(this.components);
            this.label25 = new System.Windows.Forms.Label();
            this.batteryraw_label = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.tuning_groupBox.SuspendLayout();
            this.statusterminal_groupBox.SuspendLayout();
            this.sendcmd_groupBox.SuspendLayout();
            this.gpstelem_groupBox.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.zoomControls.SuspendLayout();
            this.waypointGroupBox.SuspendLayout();
            this.mapInput.SuspendLayout();
            this.markerGroupBox.SuspendLayout();
            this.telemraw_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 115200;
            this.serialPort1.DtrEnable = true;
            this.serialPort1.PortName = "COM13";
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // connect_btn
            // 
            this.connect_btn.Location = new System.Drawing.Point(165, 13);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(75, 23);
            this.connect_btn.TabIndex = 0;
            this.connect_btn.Text = "Connect";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // disconnect_btn
            // 
            this.disconnect_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.disconnect_btn.Location = new System.Drawing.Point(165, 13);
            this.disconnect_btn.Name = "disconnect_btn";
            this.disconnect_btn.Size = new System.Drawing.Size(75, 23);
            this.disconnect_btn.TabIndex = 2;
            this.disconnect_btn.Text = "Disconnect";
            this.disconnect_btn.UseVisualStyleBackColor = true;
            this.disconnect_btn.Visible = false;
            this.disconnect_btn.Click += new System.EventHandler(this.disconnect_btn_Click);
            // 
            // port_list
            // 
            this.port_list.FormattingEnabled = true;
            this.port_list.Location = new System.Drawing.Point(47, 6);
            this.port_list.Name = "port_list";
            this.port_list.Size = new System.Drawing.Size(121, 21);
            this.port_list.TabIndex = 3;
            // 
            // baud_list
            // 
            this.baud_list.FormattingEnabled = true;
            this.baud_list.Location = new System.Drawing.Point(47, 39);
            this.baud_list.Name = "baud_list";
            this.baud_list.Size = new System.Drawing.Size(121, 21);
            this.baud_list.TabIndex = 4;
            // 
            // gyroGraphControl
            // 
            this.gyroGraphControl.BackColor = System.Drawing.SystemColors.Control;
            this.gyroGraphControl.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gyroGraphControl.Location = new System.Drawing.Point(9, 238);
            this.gyroGraphControl.Name = "gyroGraphControl";
            this.gyroGraphControl.ScrollGrace = 0D;
            this.gyroGraphControl.ScrollMaxX = 0D;
            this.gyroGraphControl.ScrollMaxY = 0D;
            this.gyroGraphControl.ScrollMaxY2 = 0D;
            this.gyroGraphControl.ScrollMinX = 0D;
            this.gyroGraphControl.ScrollMinY = 0D;
            this.gyroGraphControl.ScrollMinY2 = 0D;
            this.gyroGraphControl.Size = new System.Drawing.Size(340, 200);
            this.gyroGraphControl.TabIndex = 5;
            this.gyroGraphControl.UseExtendedPrintDialog = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(503, 81);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(1474, 830);
            this.webBrowser1.TabIndex = 6;
            this.webBrowser1.Url = new System.Uri("https://www.openstreetmap.org/?mlat=40.92372&mlon=-91.39403#map=19/40.92372/-91.3" +
        "9403&layers=N", System.UriKind.Absolute);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Baud:";
            // 
            // throttleGraphControl
            // 
            this.throttleGraphControl.Location = new System.Drawing.Point(346, 247);
            this.throttleGraphControl.Name = "throttleGraphControl";
            this.throttleGraphControl.ScrollGrace = 0D;
            this.throttleGraphControl.ScrollMaxX = 0D;
            this.throttleGraphControl.ScrollMaxY = 0D;
            this.throttleGraphControl.ScrollMaxY2 = 0D;
            this.throttleGraphControl.ScrollMinX = 0D;
            this.throttleGraphControl.ScrollMinY = 0D;
            this.throttleGraphControl.ScrollMinY2 = 0D;
            this.throttleGraphControl.Size = new System.Drawing.Size(340, 200);
            this.throttleGraphControl.TabIndex = 9;
            this.throttleGraphControl.UseExtendedPrintDialog = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(229, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.battery_voltage_label);
            this.panel3.Controls.Add(this.flight_mode_label);
            this.panel3.Controls.Add(this.lasttelem_label);
            this.panel3.Controls.Add(this.orientationGraphControl);
            this.panel3.Controls.Add(this.pidoutputGraphControl);
            this.panel3.Controls.Add(this.tuning_groupBox);
            this.panel3.Controls.Add(this.statusterminal_groupBox);
            this.panel3.Controls.Add(this.sendcmd_groupBox);
            this.panel3.Controls.Add(this.throttleGraphControl);
            this.panel3.Controls.Add(this.gpstelem_groupBox);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.disconnect_btn);
            this.panel3.Controls.Add(this.connect_btn);
            this.panel3.Location = new System.Drawing.Point(9, -9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(898, 1064);
            this.panel3.TabIndex = 12;
            // 
            // battery_voltage_label
            // 
            this.battery_voltage_label.AutoSize = true;
            this.battery_voltage_label.Location = new System.Drawing.Point(352, 32);
            this.battery_voltage_label.Name = "battery_voltage_label";
            this.battery_voltage_label.Size = new System.Drawing.Size(46, 13);
            this.battery_voltage_label.TabIndex = 16;
            this.battery_voltage_label.Text = "Battery: ";
            // 
            // flight_mode_label
            // 
            this.flight_mode_label.AutoSize = true;
            this.flight_mode_label.Location = new System.Drawing.Point(352, 52);
            this.flight_mode_label.Name = "flight_mode_label";
            this.flight_mode_label.Size = new System.Drawing.Size(68, 13);
            this.flight_mode_label.TabIndex = 15;
            this.flight_mode_label.Text = "Flight Mode: ";
            // 
            // lasttelem_label
            // 
            this.lasttelem_label.AutoSize = true;
            this.lasttelem_label.Location = new System.Drawing.Point(174, 53);
            this.lasttelem_label.Name = "lasttelem_label";
            this.lasttelem_label.Size = new System.Drawing.Size(89, 13);
            this.lasttelem_label.TabIndex = 12;
            this.lasttelem_label.Text = "Last Packet: 0ms";
            // 
            // orientationGraphControl
            // 
            this.orientationGraphControl.Location = new System.Drawing.Point(0, 659);
            this.orientationGraphControl.Name = "orientationGraphControl";
            this.orientationGraphControl.ScrollGrace = 0D;
            this.orientationGraphControl.ScrollMaxX = 0D;
            this.orientationGraphControl.ScrollMaxY = 0D;
            this.orientationGraphControl.ScrollMaxY2 = 0D;
            this.orientationGraphControl.ScrollMinX = 0D;
            this.orientationGraphControl.ScrollMinY = 0D;
            this.orientationGraphControl.ScrollMinY2 = 0D;
            this.orientationGraphControl.Size = new System.Drawing.Size(340, 200);
            this.orientationGraphControl.TabIndex = 14;
            this.orientationGraphControl.UseExtendedPrintDialog = true;
            // 
            // pidoutputGraphControl
            // 
            this.pidoutputGraphControl.Location = new System.Drawing.Point(0, 453);
            this.pidoutputGraphControl.Name = "pidoutputGraphControl";
            this.pidoutputGraphControl.ScrollGrace = 0D;
            this.pidoutputGraphControl.ScrollMaxX = 0D;
            this.pidoutputGraphControl.ScrollMaxY = 0D;
            this.pidoutputGraphControl.ScrollMaxY2 = 0D;
            this.pidoutputGraphControl.ScrollMinX = 0D;
            this.pidoutputGraphControl.ScrollMinY = 0D;
            this.pidoutputGraphControl.ScrollMinY2 = 0D;
            this.pidoutputGraphControl.Size = new System.Drawing.Size(340, 200);
            this.pidoutputGraphControl.TabIndex = 13;
            this.pidoutputGraphControl.UseExtendedPrintDialog = true;
            // 
            // tuning_groupBox
            // 
            this.tuning_groupBox.Controls.Add(this.levelrate_textbox);
            this.tuning_groupBox.Controls.Add(this.label24);
            this.tuning_groupBox.Controls.Add(this.label19);
            this.tuning_groupBox.Controls.Add(this.label20);
            this.tuning_groupBox.Controls.Add(this.label21);
            this.tuning_groupBox.Controls.Add(this.dgainyaw_textbox);
            this.tuning_groupBox.Controls.Add(this.igainyaw_textbox);
            this.tuning_groupBox.Controls.Add(this.pgainyaw_textbox);
            this.tuning_groupBox.Controls.Add(this.downloadtuning_btn);
            this.tuning_groupBox.Controls.Add(this.dgain_textbox);
            this.tuning_groupBox.Controls.Add(this.label18);
            this.tuning_groupBox.Controls.Add(this.igain_textbox);
            this.tuning_groupBox.Controls.Add(this.label17);
            this.tuning_groupBox.Controls.Add(this.pgain_textbox);
            this.tuning_groupBox.Controls.Add(this.label16);
            this.tuning_groupBox.Controls.Add(this.uploadtuning_btn);
            this.tuning_groupBox.Location = new System.Drawing.Point(692, 247);
            this.tuning_groupBox.Name = "tuning_groupBox";
            this.tuning_groupBox.Size = new System.Drawing.Size(180, 200);
            this.tuning_groupBox.TabIndex = 12;
            this.tuning_groupBox.TabStop = false;
            this.tuning_groupBox.Text = "Tuning";
            // 
            // levelrate_textbox
            // 
            this.levelrate_textbox.Location = new System.Drawing.Point(9, 113);
            this.levelrate_textbox.Name = "levelrate_textbox";
            this.levelrate_textbox.Size = new System.Drawing.Size(79, 20);
            this.levelrate_textbox.TabIndex = 27;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 97);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(62, 13);
            this.label24.TabIndex = 26;
            this.label24.Text = "Level Rate:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(89, 71);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(25, 13);
            this.label19.TabIndex = 25;
            this.label19.Text = "DY:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(94, 45);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(20, 13);
            this.label20.TabIndex = 24;
            this.label20.Text = "IY:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(90, 19);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(24, 13);
            this.label21.TabIndex = 23;
            this.label21.Text = "PY:";
            // 
            // dgainyaw_textbox
            // 
            this.dgainyaw_textbox.Location = new System.Drawing.Point(115, 67);
            this.dgainyaw_textbox.Name = "dgainyaw_textbox";
            this.dgainyaw_textbox.Size = new System.Drawing.Size(59, 20);
            this.dgainyaw_textbox.TabIndex = 22;
            // 
            // igainyaw_textbox
            // 
            this.igainyaw_textbox.Location = new System.Drawing.Point(115, 41);
            this.igainyaw_textbox.Name = "igainyaw_textbox";
            this.igainyaw_textbox.Size = new System.Drawing.Size(59, 20);
            this.igainyaw_textbox.TabIndex = 21;
            // 
            // pgainyaw_textbox
            // 
            this.pgainyaw_textbox.Location = new System.Drawing.Point(115, 15);
            this.pgainyaw_textbox.Name = "pgainyaw_textbox";
            this.pgainyaw_textbox.Size = new System.Drawing.Size(59, 20);
            this.pgainyaw_textbox.TabIndex = 20;
            // 
            // downloadtuning_btn
            // 
            this.downloadtuning_btn.Location = new System.Drawing.Point(6, 171);
            this.downloadtuning_btn.Name = "downloadtuning_btn";
            this.downloadtuning_btn.Size = new System.Drawing.Size(80, 23);
            this.downloadtuning_btn.TabIndex = 19;
            this.downloadtuning_btn.Text = "Download";
            this.downloadtuning_btn.UseVisualStyleBackColor = true;
            this.downloadtuning_btn.Click += new System.EventHandler(this.downloadtuning_btn_Click);
            // 
            // dgain_textbox
            // 
            this.dgain_textbox.Location = new System.Drawing.Point(29, 67);
            this.dgain_textbox.Name = "dgain_textbox";
            this.dgain_textbox.Size = new System.Drawing.Size(59, 20);
            this.dgain_textbox.TabIndex = 18;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 71);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(18, 13);
            this.label18.TabIndex = 17;
            this.label18.Text = "D:";
            // 
            // igain_textbox
            // 
            this.igain_textbox.Location = new System.Drawing.Point(29, 41);
            this.igain_textbox.Name = "igain_textbox";
            this.igain_textbox.Size = new System.Drawing.Size(59, 20);
            this.igain_textbox.TabIndex = 4;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 44);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(13, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "I:";
            // 
            // pgain_textbox
            // 
            this.pgain_textbox.Location = new System.Drawing.Point(29, 15);
            this.pgain_textbox.Name = "pgain_textbox";
            this.pgain_textbox.Size = new System.Drawing.Size(59, 20);
            this.pgain_textbox.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 19);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "P:";
            // 
            // uploadtuning_btn
            // 
            this.uploadtuning_btn.Enabled = false;
            this.uploadtuning_btn.Location = new System.Drawing.Point(94, 171);
            this.uploadtuning_btn.Name = "uploadtuning_btn";
            this.uploadtuning_btn.Size = new System.Drawing.Size(80, 23);
            this.uploadtuning_btn.TabIndex = 0;
            this.uploadtuning_btn.Text = "Upload";
            this.uploadtuning_btn.UseVisualStyleBackColor = true;
            this.uploadtuning_btn.Click += new System.EventHandler(this.uploadtuning_btn_Click);
            // 
            // statusterminal_groupBox
            // 
            this.statusterminal_groupBox.Controls.Add(this.clearstatus_btn);
            this.statusterminal_groupBox.Controls.Add(this.statusterminal_textbox);
            this.statusterminal_groupBox.Location = new System.Drawing.Point(686, 78);
            this.statusterminal_groupBox.Name = "statusterminal_groupBox";
            this.statusterminal_groupBox.Size = new System.Drawing.Size(186, 163);
            this.statusterminal_groupBox.TabIndex = 11;
            this.statusterminal_groupBox.TabStop = false;
            this.statusterminal_groupBox.Text = "Status Terminal";
            // 
            // clearstatus_btn
            // 
            this.clearstatus_btn.Location = new System.Drawing.Point(6, 138);
            this.clearstatus_btn.Name = "clearstatus_btn";
            this.clearstatus_btn.Size = new System.Drawing.Size(174, 23);
            this.clearstatus_btn.TabIndex = 1;
            this.clearstatus_btn.Text = "Clear";
            this.clearstatus_btn.UseVisualStyleBackColor = true;
            this.clearstatus_btn.Click += new System.EventHandler(this.clearstatus_btn_Click);
            // 
            // statusterminal_textbox
            // 
            this.statusterminal_textbox.Location = new System.Drawing.Point(6, 19);
            this.statusterminal_textbox.Multiline = true;
            this.statusterminal_textbox.Name = "statusterminal_textbox";
            this.statusterminal_textbox.ReadOnly = true;
            this.statusterminal_textbox.Size = new System.Drawing.Size(174, 113);
            this.statusterminal_textbox.TabIndex = 0;
            // 
            // sendcmd_groupBox
            // 
            this.sendcmd_groupBox.Controls.Add(this.disarm_btn);
            this.sendcmd_groupBox.Controls.Add(this.ratemode_btn);
            this.sendcmd_groupBox.Controls.Add(this.levelmode_btn);
            this.sendcmd_groupBox.Controls.Add(this.gyro_callibrate_btn);
            this.sendcmd_groupBox.Controls.Add(this.poshold_btn);
            this.sendcmd_groupBox.Controls.Add(this.rth_btn);
            this.sendcmd_groupBox.Location = new System.Drawing.Point(479, 78);
            this.sendcmd_groupBox.Name = "sendcmd_groupBox";
            this.sendcmd_groupBox.Size = new System.Drawing.Size(201, 163);
            this.sendcmd_groupBox.TabIndex = 10;
            this.sendcmd_groupBox.TabStop = false;
            this.sendcmd_groupBox.Text = "Send Commands";
            // 
            // disarm_btn
            // 
            this.disarm_btn.Enabled = false;
            this.disarm_btn.Location = new System.Drawing.Point(101, 24);
            this.disarm_btn.Name = "disarm_btn";
            this.disarm_btn.Size = new System.Drawing.Size(94, 23);
            this.disarm_btn.TabIndex = 9;
            this.disarm_btn.Text = "Disarm";
            this.disarm_btn.UseVisualStyleBackColor = true;
            this.disarm_btn.Click += new System.EventHandler(this.disarm_btn_Click);
            // 
            // ratemode_btn
            // 
            this.ratemode_btn.Enabled = false;
            this.ratemode_btn.Location = new System.Drawing.Point(101, 53);
            this.ratemode_btn.Name = "ratemode_btn";
            this.ratemode_btn.Size = new System.Drawing.Size(94, 23);
            this.ratemode_btn.TabIndex = 8;
            this.ratemode_btn.Text = "Rate Mode";
            this.ratemode_btn.UseVisualStyleBackColor = true;
            this.ratemode_btn.Click += new System.EventHandler(this.ratemode_btn_Click);
            // 
            // levelmode_btn
            // 
            this.levelmode_btn.Enabled = false;
            this.levelmode_btn.Location = new System.Drawing.Point(101, 82);
            this.levelmode_btn.Name = "levelmode_btn";
            this.levelmode_btn.Size = new System.Drawing.Size(94, 23);
            this.levelmode_btn.TabIndex = 7;
            this.levelmode_btn.Text = "Level Mode";
            this.levelmode_btn.UseVisualStyleBackColor = true;
            this.levelmode_btn.Click += new System.EventHandler(this.levelmode_btn_Click);
            // 
            // gyro_callibrate_btn
            // 
            this.gyro_callibrate_btn.Enabled = false;
            this.gyro_callibrate_btn.Location = new System.Drawing.Point(6, 82);
            this.gyro_callibrate_btn.Name = "gyro_callibrate_btn";
            this.gyro_callibrate_btn.Size = new System.Drawing.Size(94, 23);
            this.gyro_callibrate_btn.TabIndex = 6;
            this.gyro_callibrate_btn.Text = "Calibrate Gyro";
            this.gyro_callibrate_btn.UseVisualStyleBackColor = true;
            this.gyro_callibrate_btn.Click += new System.EventHandler(this.gyro_callibrate_btn_Click);
            // 
            // poshold_btn
            // 
            this.poshold_btn.Enabled = false;
            this.poshold_btn.Location = new System.Drawing.Point(6, 24);
            this.poshold_btn.Name = "poshold_btn";
            this.poshold_btn.Size = new System.Drawing.Size(94, 23);
            this.poshold_btn.TabIndex = 5;
            this.poshold_btn.Text = "Hold Position At P";
            this.poshold_btn.UseVisualStyleBackColor = true;
            // 
            // rth_btn
            // 
            this.rth_btn.Enabled = false;
            this.rth_btn.Location = new System.Drawing.Point(6, 53);
            this.rth_btn.Name = "rth_btn";
            this.rth_btn.Size = new System.Drawing.Size(94, 23);
            this.rth_btn.TabIndex = 4;
            this.rth_btn.Text = "Return To Home";
            this.rth_btn.UseVisualStyleBackColor = true;
            // 
            // gpstelem_groupBox
            // 
            this.gpstelem_groupBox.Controls.Add(this.gps_sats_label);
            this.gpstelem_groupBox.Controls.Add(this.label12);
            this.gpstelem_groupBox.Controls.Add(this.gps_lat_label);
            this.gpstelem_groupBox.Controls.Add(this.gps_lon_label);
            this.gpstelem_groupBox.Controls.Add(this.label11);
            this.gpstelem_groupBox.Controls.Add(this.label10);
            this.gpstelem_groupBox.Location = new System.Drawing.Point(346, 78);
            this.gpstelem_groupBox.Name = "gpstelem_groupBox";
            this.gpstelem_groupBox.Size = new System.Drawing.Size(127, 163);
            this.gpstelem_groupBox.TabIndex = 9;
            this.gpstelem_groupBox.TabStop = false;
            this.gpstelem_groupBox.Text = "GPS Telemetry";
            // 
            // gps_sats_label
            // 
            this.gps_sats_label.Location = new System.Drawing.Point(37, 68);
            this.gps_sats_label.Name = "gps_sats_label";
            this.gps_sats_label.Size = new System.Drawing.Size(84, 23);
            this.gps_sats_label.TabIndex = 5;
            this.gps_sats_label.Text = "~";
            this.gps_sats_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 73);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Sats:";
            // 
            // gps_lat_label
            // 
            this.gps_lat_label.Location = new System.Drawing.Point(37, 44);
            this.gps_lat_label.Name = "gps_lat_label";
            this.gps_lat_label.Size = new System.Drawing.Size(84, 23);
            this.gps_lat_label.TabIndex = 3;
            this.gps_lat_label.Text = "~";
            this.gps_lat_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gps_lon_label
            // 
            this.gps_lon_label.Location = new System.Drawing.Point(37, 19);
            this.gps_lon_label.Name = "gps_lon_label";
            this.gps_lon_label.Size = new System.Drawing.Size(84, 23);
            this.gps_lon_label.TabIndex = 2;
            this.gps_lon_label.Text = "~";
            this.gps_lon_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Lat:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Lon:";
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(902, 798);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1048, 246);
            this.panel4.TabIndex = 13;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(902, -12);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1048, 213);
            this.panel5.TabIndex = 12;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.zoomControls);
            this.panel6.Controls.Add(this.waypointGroupBox);
            this.panel6.Controls.Add(this.mapInput);
            this.panel6.Controls.Add(this.markerGroupBox);
            this.panel6.Location = new System.Drawing.Point(0, 51);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(958, 153);
            this.panel6.TabIndex = 12;
            // 
            // zoomControls
            // 
            this.zoomControls.Controls.Add(this.zoomin_btn);
            this.zoomControls.Controls.Add(this.zoomout_btn);
            this.zoomControls.Location = new System.Drawing.Point(760, 27);
            this.zoomControls.Name = "zoomControls";
            this.zoomControls.Size = new System.Drawing.Size(184, 115);
            this.zoomControls.TabIndex = 10;
            this.zoomControls.TabStop = false;
            this.zoomControls.Text = "Map Zoom(This will clear markers!)";
            // 
            // zoomin_btn
            // 
            this.zoomin_btn.Location = new System.Drawing.Point(6, 86);
            this.zoomin_btn.Name = "zoomin_btn";
            this.zoomin_btn.Size = new System.Drawing.Size(75, 23);
            this.zoomin_btn.TabIndex = 8;
            this.zoomin_btn.Text = "Zoom In";
            this.zoomin_btn.UseVisualStyleBackColor = true;
            this.zoomin_btn.Click += new System.EventHandler(this.zoomin_btn_Click);
            // 
            // zoomout_btn
            // 
            this.zoomout_btn.Location = new System.Drawing.Point(103, 86);
            this.zoomout_btn.Name = "zoomout_btn";
            this.zoomout_btn.Size = new System.Drawing.Size(75, 23);
            this.zoomout_btn.TabIndex = 9;
            this.zoomout_btn.Text = "Zoom Out";
            this.zoomout_btn.UseVisualStyleBackColor = true;
            this.zoomout_btn.Click += new System.EventHandler(this.zoomout_btn_Click);
            // 
            // waypointGroupBox
            // 
            this.waypointGroupBox.Controls.Add(this.resetmarkers_btn);
            this.waypointGroupBox.Location = new System.Drawing.Point(503, 27);
            this.waypointGroupBox.Name = "waypointGroupBox";
            this.waypointGroupBox.Size = new System.Drawing.Size(251, 115);
            this.waypointGroupBox.TabIndex = 14;
            this.waypointGroupBox.TabStop = false;
            this.waypointGroupBox.Text = "Waypoints";
            // 
            // resetmarkers_btn
            // 
            this.resetmarkers_btn.Location = new System.Drawing.Point(6, 86);
            this.resetmarkers_btn.Name = "resetmarkers_btn";
            this.resetmarkers_btn.Size = new System.Drawing.Size(93, 23);
            this.resetmarkers_btn.TabIndex = 4;
            this.resetmarkers_btn.Text = "Clear Waypoints";
            this.resetmarkers_btn.UseVisualStyleBackColor = true;
            // 
            // mapInput
            // 
            this.mapInput.Controls.Add(this.long_label);
            this.mapInput.Controls.Add(this.lati_label);
            this.mapInput.Controls.Add(this.loadMap_btn);
            this.mapInput.Controls.Add(this.lati_input);
            this.mapInput.Controls.Add(this.long_input);
            this.mapInput.Location = new System.Drawing.Point(11, 27);
            this.mapInput.Name = "mapInput";
            this.mapInput.Size = new System.Drawing.Size(186, 115);
            this.mapInput.TabIndex = 11;
            this.mapInput.TabStop = false;
            this.mapInput.Text = "Map Coordinates";
            // 
            // long_label
            // 
            this.long_label.AutoSize = true;
            this.long_label.Location = new System.Drawing.Point(6, 19);
            this.long_label.Name = "long_label";
            this.long_label.Size = new System.Drawing.Size(57, 13);
            this.long_label.TabIndex = 1;
            this.long_label.Text = "Longitude:";
            // 
            // lati_label
            // 
            this.lati_label.AutoSize = true;
            this.lati_label.Location = new System.Drawing.Point(6, 48);
            this.lati_label.Name = "lati_label";
            this.lati_label.Size = new System.Drawing.Size(48, 13);
            this.lati_label.TabIndex = 2;
            this.lati_label.Text = "Latitude:";
            // 
            // loadMap_btn
            // 
            this.loadMap_btn.Location = new System.Drawing.Point(6, 86);
            this.loadMap_btn.Name = "loadMap_btn";
            this.loadMap_btn.Size = new System.Drawing.Size(174, 23);
            this.loadMap_btn.TabIndex = 0;
            this.loadMap_btn.Text = "Go";
            this.loadMap_btn.UseVisualStyleBackColor = true;
            this.loadMap_btn.Click += new System.EventHandler(this.loadMap_btn_Click);
            // 
            // lati_input
            // 
            this.lati_input.Location = new System.Drawing.Point(69, 45);
            this.lati_input.Name = "lati_input";
            this.lati_input.Size = new System.Drawing.Size(100, 20);
            this.lati_input.TabIndex = 4;
            // 
            // long_input
            // 
            this.long_input.Location = new System.Drawing.Point(69, 19);
            this.long_input.Name = "long_input";
            this.long_input.Size = new System.Drawing.Size(100, 20);
            this.long_input.TabIndex = 3;
            // 
            // markerGroupBox
            // 
            this.markerGroupBox.Controls.Add(this.uploadhomeandpos_btn);
            this.markerGroupBox.Controls.Add(this.clearhold_btn);
            this.markerGroupBox.Controls.Add(this.setholdpos_btn);
            this.markerGroupBox.Controls.Add(this.sethome_btn);
            this.markerGroupBox.Controls.Add(this.label9);
            this.markerGroupBox.Controls.Add(this.label8);
            this.markerGroupBox.Controls.Add(this.label7);
            this.markerGroupBox.Controls.Add(this.label6);
            this.markerGroupBox.Controls.Add(this.label5);
            this.markerGroupBox.Controls.Add(this.label3);
            this.markerGroupBox.Location = new System.Drawing.Point(220, 27);
            this.markerGroupBox.Name = "markerGroupBox";
            this.markerGroupBox.Size = new System.Drawing.Size(277, 115);
            this.markerGroupBox.TabIndex = 12;
            this.markerGroupBox.TabStop = false;
            this.markerGroupBox.Text = "Marker Legend";
            // 
            // uploadhomeandpos_btn
            // 
            this.uploadhomeandpos_btn.Enabled = false;
            this.uploadhomeandpos_btn.Location = new System.Drawing.Point(203, 86);
            this.uploadhomeandpos_btn.Name = "uploadhomeandpos_btn";
            this.uploadhomeandpos_btn.Size = new System.Drawing.Size(68, 23);
            this.uploadhomeandpos_btn.TabIndex = 11;
            this.uploadhomeandpos_btn.Text = "Upload";
            this.uploadhomeandpos_btn.UseVisualStyleBackColor = true;
            // 
            // clearhold_btn
            // 
            this.clearhold_btn.Location = new System.Drawing.Point(85, 86);
            this.clearhold_btn.Name = "clearhold_btn";
            this.clearhold_btn.Size = new System.Drawing.Size(112, 23);
            this.clearhold_btn.TabIndex = 10;
            this.clearhold_btn.Text = "Clear Position Hold";
            this.clearhold_btn.UseVisualStyleBackColor = true;
            this.clearhold_btn.Visible = false;
            this.clearhold_btn.Click += new System.EventHandler(this.clearhold_btn_Click);
            // 
            // setholdpos_btn
            // 
            this.setholdpos_btn.Location = new System.Drawing.Point(85, 86);
            this.setholdpos_btn.Name = "setholdpos_btn";
            this.setholdpos_btn.Size = new System.Drawing.Size(112, 23);
            this.setholdpos_btn.TabIndex = 9;
            this.setholdpos_btn.Text = "Set Hold Position";
            this.setholdpos_btn.UseVisualStyleBackColor = true;
            this.setholdpos_btn.Click += new System.EventHandler(this.setholdpos_btn_Click);
            // 
            // sethome_btn
            // 
            this.sethome_btn.Location = new System.Drawing.Point(6, 86);
            this.sethome_btn.Name = "sethome_btn";
            this.sethome_btn.Size = new System.Drawing.Size(73, 23);
            this.sethome_btn.TabIndex = 8;
            this.sethome_btn.Text = "Set Home";
            this.sethome_btn.UseVisualStyleBackColor = true;
            this.sethome_btn.Click += new System.EventHandler(this.sethome_btn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Lime;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(205, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 16);
            this.label9.TabIndex = 7;
            this.label9.Text = "C";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(127, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Craft Position:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Red;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(85, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "P";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Position Hold:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ControlText;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(84, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "H";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Home:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = " (Right click to change map center)";
            // 
            // home_marker
            // 
            this.home_marker.AutoSize = true;
            this.home_marker.BackColor = System.Drawing.Color.Black;
            this.home_marker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.home_marker.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.home_marker.Location = new System.Drawing.Point(913, 767);
            this.home_marker.Name = "home_marker";
            this.home_marker.Size = new System.Drawing.Size(19, 16);
            this.home_marker.TabIndex = 0;
            this.home_marker.Text = "H";
            this.home_marker.Visible = false;
            // 
            // marker2
            // 
            this.marker2.AutoSize = true;
            this.marker2.BackColor = System.Drawing.Color.DarkRed;
            this.marker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marker2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.marker2.Location = new System.Drawing.Point(960, 767);
            this.marker2.Name = "marker2";
            this.marker2.Size = new System.Drawing.Size(16, 16);
            this.marker2.TabIndex = 14;
            this.marker2.Text = "1";
            this.marker2.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(1846, 194);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(104, 598);
            this.panel2.TabIndex = 15;
            // 
            // positionhold_marker
            // 
            this.positionhold_marker.AutoSize = true;
            this.positionhold_marker.BackColor = System.Drawing.Color.Red;
            this.positionhold_marker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.positionhold_marker.Location = new System.Drawing.Point(1001, 755);
            this.positionhold_marker.Name = "positionhold_marker";
            this.positionhold_marker.Size = new System.Drawing.Size(18, 16);
            this.positionhold_marker.TabIndex = 16;
            this.positionhold_marker.Text = "P";
            this.positionhold_marker.Visible = false;
            // 
            // telemraw_groupBox
            // 
            this.telemraw_groupBox.Controls.Add(this.label25);
            this.telemraw_groupBox.Controls.Add(this.batteryraw_label);
            this.telemraw_groupBox.Controls.Add(this.yaw_label);
            this.telemraw_groupBox.Controls.Add(this.pitch_label);
            this.telemraw_groupBox.Controls.Add(this.roll_label);
            this.telemraw_groupBox.Controls.Add(this.label27);
            this.telemraw_groupBox.Controls.Add(this.label28);
            this.telemraw_groupBox.Controls.Add(this.label29);
            this.telemraw_groupBox.Controls.Add(this.flight_mode_raw_label);
            this.telemraw_groupBox.Controls.Add(this.label22);
            this.telemraw_groupBox.Controls.Add(this.throttle_label);
            this.telemraw_groupBox.Controls.Add(this.label23);
            this.telemraw_groupBox.Controls.Add(this.gyrozraw_label);
            this.telemraw_groupBox.Controls.Add(this.gyroyraw_label);
            this.telemraw_groupBox.Controls.Add(this.gyroxraw_label);
            this.telemraw_groupBox.Controls.Add(this.label15);
            this.telemraw_groupBox.Controls.Add(this.label14);
            this.telemraw_groupBox.Controls.Add(this.label13);
            this.telemraw_groupBox.Location = new System.Drawing.Point(9, 69);
            this.telemraw_groupBox.Name = "telemraw_groupBox";
            this.telemraw_groupBox.Size = new System.Drawing.Size(340, 163);
            this.telemraw_groupBox.TabIndex = 8;
            this.telemraw_groupBox.TabStop = false;
            this.telemraw_groupBox.Text = "Telemetry Raw";
            // 
            // yaw_label
            // 
            this.yaw_label.AutoSize = true;
            this.yaw_label.Location = new System.Drawing.Point(158, 73);
            this.yaw_label.Name = "yaw_label";
            this.yaw_label.Size = new System.Drawing.Size(14, 13);
            this.yaw_label.TabIndex = 23;
            this.yaw_label.Text = "~";
            // 
            // pitch_label
            // 
            this.pitch_label.AutoSize = true;
            this.pitch_label.Location = new System.Drawing.Point(158, 49);
            this.pitch_label.Name = "pitch_label";
            this.pitch_label.Size = new System.Drawing.Size(14, 13);
            this.pitch_label.TabIndex = 22;
            this.pitch_label.Text = "~";
            // 
            // roll_label
            // 
            this.roll_label.AutoSize = true;
            this.roll_label.Location = new System.Drawing.Point(158, 22);
            this.roll_label.Name = "roll_label";
            this.roll_label.Size = new System.Drawing.Size(14, 13);
            this.roll_label.TabIndex = 21;
            this.roll_label.Text = "~";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(97, 73);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(63, 13);
            this.label27.TabIndex = 20;
            this.label27.Text = "Yaw Amgle:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(97, 48);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(64, 13);
            this.label28.TabIndex = 19;
            this.label28.Text = "Pitch Angle:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(97, 22);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(58, 13);
            this.label29.TabIndex = 18;
            this.label29.Text = "Roll Angle:";
            // 
            // flight_mode_raw_label
            // 
            this.flight_mode_raw_label.AutoSize = true;
            this.flight_mode_raw_label.Location = new System.Drawing.Point(69, 122);
            this.flight_mode_raw_label.Name = "flight_mode_raw_label";
            this.flight_mode_raw_label.Size = new System.Drawing.Size(14, 13);
            this.flight_mode_raw_label.TabIndex = 17;
            this.flight_mode_raw_label.Text = "~";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 122);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(68, 13);
            this.label22.TabIndex = 16;
            this.label22.Text = "Flight Mode: ";
            // 
            // throttle_label
            // 
            this.throttle_label.AutoSize = true;
            this.throttle_label.Location = new System.Drawing.Point(50, 98);
            this.throttle_label.Name = "throttle_label";
            this.throttle_label.Size = new System.Drawing.Size(14, 13);
            this.throttle_label.TabIndex = 13;
            this.throttle_label.Text = "~";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 98);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(49, 13);
            this.label23.TabIndex = 12;
            this.label23.Text = "Throttle: ";
            // 
            // gyrozraw_label
            // 
            this.gyrozraw_label.AutoSize = true;
            this.gyrozraw_label.Location = new System.Drawing.Point(44, 73);
            this.gyrozraw_label.Name = "gyrozraw_label";
            this.gyrozraw_label.Size = new System.Drawing.Size(14, 13);
            this.gyrozraw_label.TabIndex = 11;
            this.gyrozraw_label.Text = "~";
            // 
            // gyroyraw_label
            // 
            this.gyroyraw_label.AutoSize = true;
            this.gyroyraw_label.Location = new System.Drawing.Point(44, 48);
            this.gyroyraw_label.Name = "gyroyraw_label";
            this.gyroyraw_label.Size = new System.Drawing.Size(14, 13);
            this.gyroyraw_label.TabIndex = 10;
            this.gyroyraw_label.Text = "~";
            // 
            // gyroxraw_label
            // 
            this.gyroxraw_label.AutoSize = true;
            this.gyroxraw_label.Location = new System.Drawing.Point(44, 22);
            this.gyroxraw_label.Name = "gyroxraw_label";
            this.gyroxraw_label.Size = new System.Drawing.Size(14, 13);
            this.gyroxraw_label.TabIndex = 9;
            this.gyroxraw_label.Text = "~";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 73);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "GyroZ: ";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "GyroY: ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "GyroX: ";
            // 
            // mainLoopTimer
            // 
            this.mainLoopTimer.Enabled = true;
            this.mainLoopTimer.Interval = 10;
            this.mainLoopTimer.Tick += new System.EventHandler(this.mainLoopTimer_Tick);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(159, 98);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(14, 13);
            this.label25.TabIndex = 25;
            this.label25.Text = "~";
            // 
            // batteryraw_label
            // 
            this.batteryraw_label.AutoSize = true;
            this.batteryraw_label.Location = new System.Drawing.Point(98, 98);
            this.batteryraw_label.Name = "batteryraw_label";
            this.batteryraw_label.Size = new System.Drawing.Size(43, 13);
            this.batteryraw_label.TabIndex = 24;
            this.batteryraw_label.Text = "Battery:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.positionhold_marker);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.telemraw_groupBox);
            this.Controls.Add(this.marker2);
            this.Controls.Add(this.home_marker);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.gyroGraphControl);
            this.Controls.Add(this.port_list);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.baud_list);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Ground Control v1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tuning_groupBox.ResumeLayout(false);
            this.tuning_groupBox.PerformLayout();
            this.statusterminal_groupBox.ResumeLayout(false);
            this.statusterminal_groupBox.PerformLayout();
            this.sendcmd_groupBox.ResumeLayout(false);
            this.gpstelem_groupBox.ResumeLayout(false);
            this.gpstelem_groupBox.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.zoomControls.ResumeLayout(false);
            this.waypointGroupBox.ResumeLayout(false);
            this.mapInput.ResumeLayout(false);
            this.mapInput.PerformLayout();
            this.markerGroupBox.ResumeLayout(false);
            this.markerGroupBox.PerformLayout();
            this.telemraw_groupBox.ResumeLayout(false);
            this.telemraw_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Button disconnect_btn;
        private System.Windows.Forms.ComboBox port_list;
        private System.Windows.Forms.ComboBox baud_list;
        private ZedGraph.ZedGraphControl gyroGraphControl;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ZedGraph.ZedGraphControl throttleGraphControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox lati_input;
        private System.Windows.Forms.TextBox long_input;
        private System.Windows.Forms.Label lati_label;
        private System.Windows.Forms.Label long_label;
        private System.Windows.Forms.Button loadMap_btn;
        private System.Windows.Forms.Label home_marker;
        private System.Windows.Forms.Label marker2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button zoomout_btn;
        private System.Windows.Forms.Button zoomin_btn;
        private System.Windows.Forms.GroupBox zoomControls;
        private System.Windows.Forms.GroupBox mapInput;
        private System.Windows.Forms.GroupBox markerGroupBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button resetmarkers_btn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox waypointGroupBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button poshold_btn;
        private System.Windows.Forms.Button rth_btn;
        private System.Windows.Forms.Button setholdpos_btn;
        private System.Windows.Forms.Button sethome_btn;
        private System.Windows.Forms.Button clearhold_btn;
        private System.Windows.Forms.Label positionhold_marker;
        private System.Windows.Forms.GroupBox gpstelem_groupBox;
        private System.Windows.Forms.GroupBox telemraw_groupBox;
        private System.Windows.Forms.GroupBox statusterminal_groupBox;
        private System.Windows.Forms.TextBox statusterminal_textbox;
        private System.Windows.Forms.GroupBox sendcmd_groupBox;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label gps_sats_label;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label gps_lat_label;
        private System.Windows.Forms.Label gps_lon_label;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer mainLoopTimer;
        private System.Windows.Forms.Button clearstatus_btn;
        private System.Windows.Forms.GroupBox tuning_groupBox;
        private System.Windows.Forms.Button uploadtuning_btn;
        private System.Windows.Forms.Button uploadhomeandpos_btn;
        private ZedGraph.ZedGraphControl orientationGraphControl;
        private ZedGraph.ZedGraphControl pidoutputGraphControl;
        private System.Windows.Forms.Label gyrozraw_label;
        private System.Windows.Forms.Label gyroyraw_label;
        private System.Windows.Forms.Label gyroxraw_label;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lasttelem_label;
        private System.Windows.Forms.Button downloadtuning_btn;
        private System.Windows.Forms.TextBox dgain_textbox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox igain_textbox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox pgain_textbox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox dgainyaw_textbox;
        private System.Windows.Forms.TextBox igainyaw_textbox;
        private System.Windows.Forms.TextBox pgainyaw_textbox;
        private System.Windows.Forms.Label throttle_label;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button gyro_callibrate_btn;
        private System.Windows.Forms.Label flight_mode_label;
        private System.Windows.Forms.Label flight_mode_raw_label;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label yaw_label;
        private System.Windows.Forms.Label pitch_label;
        private System.Windows.Forms.Label roll_label;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label battery_voltage_label;
        private System.Windows.Forms.Button ratemode_btn;
        private System.Windows.Forms.Button levelmode_btn;
        private System.Windows.Forms.Button disarm_btn;
        private System.Windows.Forms.TextBox levelrate_textbox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label batteryraw_label;
    }
}

