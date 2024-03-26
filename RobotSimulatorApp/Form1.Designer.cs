using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotSimulatorApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            glControl = new OpenTK.WinForms.GLControl();
            Position = new Label();
            splitter1 = new Splitter();
            splitter2 = new Splitter();
            PitchTrackBar = new TrackBar();
            YawTrackBar = new TrackBar();
            PosXTrackBar = new TrackBar();
            PosYTrackBar = new TrackBar();
            PosZTrackBar = new TrackBar();
            OpenControlsButton = new Button();
            NorthButton = new Button();
            CameraPanel = new Panel();
            StartButton = new Button();
            BackButton = new Button();
            TopButton = new Button();
            SouthButton = new Button();
            WestButton = new Button();
            EastButton = new Button();
            PitchLabel = new Label();
            YawLabel = new Label();
            XLabel = new Label();
            YLabel = new Label();
            ZLabel = new Label();
            YawTextBox = new TextBox();
            PitchTextBox = new TextBox();
            PositionZTextBox = new TextBox();
            PositionYTextBox = new TextBox();
            PositionXTextBox = new TextBox();
            TraceCheckbox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)PitchTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)YawTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PosXTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PosYTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PosZTrackBar).BeginInit();
            CameraPanel.SuspendLayout();
            SuspendLayout();
            // 
            // glControl
            // 
            glControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl.APIVersion = new Version(3, 3, 0, 0);
            glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl.IsEventDriven = true;
            glControl.Location = new Point(12, 12);
            glControl.Name = "glControl";
            glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glControl.SharedContext = null;
            glControl.Size = new Size(1058, 621);
            glControl.TabIndex = 0;
            glControl.Text = "glControl";
            // 
            // Position
            // 
            Position.AutoSize = true;
            Position.Font = new Font("Segoe UI", 15F);
            Position.Location = new Point(1172, 0);
            Position.Name = "Position";
            Position.Size = new Size(78, 28);
            Position.TabIndex = 12;
            Position.Text = "Camera";
            // 
            // splitter1
            // 
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 654);
            splitter1.TabIndex = 24;
            splitter1.TabStop = false;
            // 
            // splitter2
            // 
            splitter2.Location = new Point(3, 0);
            splitter2.Name = "splitter2";
            splitter2.Size = new Size(3, 654);
            splitter2.TabIndex = 25;
            splitter2.TabStop = false;
            // 
            // PitchTrackBar
            // 
            PitchTrackBar.AutoSize = false;
            PitchTrackBar.Location = new Point(1113, 277);
            PitchTrackBar.Maximum = 89;
            PitchTrackBar.Minimum = -89;
            PitchTrackBar.Name = "PitchTrackBar";
            PitchTrackBar.Size = new Size(212, 33);
            PitchTrackBar.TabIndex = 26;
            PitchTrackBar.Scroll += PitchTrackBar_Scroll;
            // 
            // YawTrackBar
            // 
            YawTrackBar.AutoSize = false;
            YawTrackBar.Location = new Point(1117, 351);
            YawTrackBar.Maximum = 180;
            YawTrackBar.Minimum = -180;
            YawTrackBar.Name = "YawTrackBar";
            YawTrackBar.Size = new Size(208, 33);
            YawTrackBar.TabIndex = 27;
            YawTrackBar.Scroll += YawTrackBar_Scroll;
            // 
            // PosXTrackBar
            // 
            PosXTrackBar.AutoSize = false;
            PosXTrackBar.Location = new Point(1117, 75);
            PosXTrackBar.Maximum = 500;
            PosXTrackBar.Minimum = -500;
            PosXTrackBar.Name = "PosXTrackBar";
            PosXTrackBar.Size = new Size(208, 33);
            PosXTrackBar.TabIndex = 29;
            PosXTrackBar.Scroll += PosXTrackBar_Scroll;
            // 
            // PosYTrackBar
            // 
            PosYTrackBar.AutoSize = false;
            PosYTrackBar.Location = new Point(1113, 140);
            PosYTrackBar.Maximum = 500;
            PosYTrackBar.Minimum = -500;
            PosYTrackBar.Name = "PosYTrackBar";
            PosYTrackBar.Size = new Size(214, 33);
            PosYTrackBar.TabIndex = 30;
            PosYTrackBar.Scroll += PosYTrackBar_Scroll;
            // 
            // PosZTrackBar
            // 
            PosZTrackBar.AutoSize = false;
            PosZTrackBar.Location = new Point(1113, 204);
            PosZTrackBar.Maximum = 500;
            PosZTrackBar.Minimum = -500;
            PosZTrackBar.Name = "PosZTrackBar";
            PosZTrackBar.Size = new Size(212, 33);
            PosZTrackBar.TabIndex = 31;
            PosZTrackBar.Scroll += PosZTrackBar_Scroll;
            // 
            // OpenControlsButton
            // 
            OpenControlsButton.Location = new Point(1119, 403);
            OpenControlsButton.Name = "OpenControlsButton";
            OpenControlsButton.Size = new Size(208, 23);
            OpenControlsButton.TabIndex = 37;
            OpenControlsButton.Text = "Open Limb controls";
            OpenControlsButton.UseVisualStyleBackColor = true;
            OpenControlsButton.Click += OpenControlsButton_Click;
            // 
            // NorthButton
            // 
            NorthButton.Location = new Point(60, 3);
            NorthButton.Name = "NorthButton";
            NorthButton.Size = new Size(51, 27);
            NorthButton.TabIndex = 38;
            NorthButton.Text = "North";
            NorthButton.UseVisualStyleBackColor = true;
            NorthButton.Click += NorthButton_Click;
            // 
            // CameraPanel
            // 
            CameraPanel.BorderStyle = BorderStyle.FixedSingle;
            CameraPanel.Controls.Add(StartButton);
            CameraPanel.Controls.Add(BackButton);
            CameraPanel.Controls.Add(TopButton);
            CameraPanel.Controls.Add(SouthButton);
            CameraPanel.Controls.Add(WestButton);
            CameraPanel.Controls.Add(EastButton);
            CameraPanel.Controls.Add(NorthButton);
            CameraPanel.Location = new Point(1150, 525);
            CameraPanel.Name = "CameraPanel";
            CameraPanel.Size = new Size(179, 108);
            CameraPanel.TabIndex = 39;
            // 
            // StartButton
            // 
            StartButton.Location = new Point(3, 3);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(51, 27);
            StartButton.TabIndex = 44;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // BackButton
            // 
            BackButton.Location = new Point(117, 69);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(51, 27);
            BackButton.TabIndex = 43;
            BackButton.Text = "Back";
            BackButton.UseVisualStyleBackColor = true;
            BackButton.Click += BackButton_Click;
            // 
            // TopButton
            // 
            TopButton.Location = new Point(60, 36);
            TopButton.Name = "TopButton";
            TopButton.Size = new Size(51, 27);
            TopButton.TabIndex = 42;
            TopButton.Text = "Top";
            TopButton.UseVisualStyleBackColor = true;
            TopButton.Click += TopButton_Click;
            // 
            // SouthButton
            // 
            SouthButton.Location = new Point(60, 69);
            SouthButton.Name = "SouthButton";
            SouthButton.Size = new Size(51, 27);
            SouthButton.TabIndex = 41;
            SouthButton.Text = "South";
            SouthButton.UseVisualStyleBackColor = true;
            SouthButton.Click += SouthButton_Click;
            // 
            // WestButton
            // 
            WestButton.Location = new Point(3, 36);
            WestButton.Name = "WestButton";
            WestButton.Size = new Size(51, 27);
            WestButton.TabIndex = 40;
            WestButton.Text = "West";
            WestButton.UseVisualStyleBackColor = true;
            WestButton.Click += WestButton_Click;
            // 
            // EastButton
            // 
            EastButton.Location = new Point(117, 36);
            EastButton.Name = "EastButton";
            EastButton.Size = new Size(51, 27);
            EastButton.TabIndex = 39;
            EastButton.Text = "East";
            EastButton.UseVisualStyleBackColor = true;
            EastButton.Click += EastButton_Click;
            // 
            // PitchLabel
            // 
            PitchLabel.AutoSize = true;
            PitchLabel.Location = new Point(1117, 254);
            PitchLabel.Name = "PitchLabel";
            PitchLabel.Size = new Size(37, 15);
            PitchLabel.TabIndex = 40;
            PitchLabel.Text = "Pitch:";
            // 
            // YawLabel
            // 
            YawLabel.AutoSize = true;
            YawLabel.Location = new Point(1119, 325);
            YawLabel.Name = "YawLabel";
            YawLabel.Size = new Size(31, 15);
            YawLabel.TabIndex = 41;
            YawLabel.Text = "Yaw:";
            // 
            // XLabel
            // 
            XLabel.AutoSize = true;
            XLabel.ForeColor = SystemColors.ControlText;
            XLabel.Location = new Point(1113, 49);
            XLabel.Name = "XLabel";
            XLabel.Size = new Size(17, 15);
            XLabel.TabIndex = 42;
            XLabel.Text = "X:";
            // 
            // YLabel
            // 
            YLabel.AutoSize = true;
            YLabel.Location = new Point(1117, 117);
            YLabel.Name = "YLabel";
            YLabel.Size = new Size(17, 15);
            YLabel.TabIndex = 43;
            YLabel.Text = "Y:";
            // 
            // ZLabel
            // 
            ZLabel.AutoSize = true;
            ZLabel.Location = new Point(1119, 182);
            ZLabel.Name = "ZLabel";
            ZLabel.Size = new Size(17, 15);
            ZLabel.TabIndex = 44;
            ZLabel.Text = "Z:";
            // 
            // YawTextBox
            // 
            YawTextBox.BackColor = SystemColors.Control;
            YawTextBox.Location = new Point(1152, 322);
            YawTextBox.Name = "YawTextBox";
            YawTextBox.ReadOnly = true;
            YawTextBox.Size = new Size(167, 23);
            YawTextBox.TabIndex = 46;
            // 
            // PitchTextBox
            // 
            PitchTextBox.Location = new Point(1158, 251);
            PitchTextBox.Name = "PitchTextBox";
            PitchTextBox.ReadOnly = true;
            PitchTextBox.Size = new Size(161, 23);
            PitchTextBox.TabIndex = 47;
            // 
            // PositionZTextBox
            // 
            PositionZTextBox.Location = new Point(1136, 179);
            PositionZTextBox.Name = "PositionZTextBox";
            PositionZTextBox.ReadOnly = true;
            PositionZTextBox.Size = new Size(181, 23);
            PositionZTextBox.TabIndex = 48;
            // 
            // PositionYTextBox
            // 
            PositionYTextBox.Location = new Point(1136, 114);
            PositionYTextBox.Name = "PositionYTextBox";
            PositionYTextBox.ReadOnly = true;
            PositionYTextBox.Size = new Size(183, 23);
            PositionYTextBox.TabIndex = 49;
            // 
            // PositionXTextBox
            // 
            PositionXTextBox.Location = new Point(1136, 46);
            PositionXTextBox.Name = "PositionXTextBox";
            PositionXTextBox.ReadOnly = true;
            PositionXTextBox.Size = new Size(183, 23);
            PositionXTextBox.TabIndex = 50;
            // 
            // TraceCheckbox
            // 
            TraceCheckbox.AutoSize = true;
            TraceCheckbox.Location = new Point(1124, 461);
            TraceCheckbox.Name = "TraceCheckbox";
            TraceCheckbox.Size = new Size(85, 19);
            TraceCheckbox.TabIndex = 51;
            TraceCheckbox.Text = "Show Trace";
            TraceCheckbox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1341, 654);
            Controls.Add(TraceCheckbox);
            Controls.Add(PositionXTextBox);
            Controls.Add(PositionYTextBox);
            Controls.Add(PositionZTextBox);
            Controls.Add(PitchTextBox);
            Controls.Add(YawTextBox);
            Controls.Add(ZLabel);
            Controls.Add(YLabel);
            Controls.Add(XLabel);
            Controls.Add(YawLabel);
            Controls.Add(PitchLabel);
            Controls.Add(CameraPanel);
            Controls.Add(OpenControlsButton);
            Controls.Add(PosZTrackBar);
            Controls.Add(PosYTrackBar);
            Controls.Add(PosXTrackBar);
            Controls.Add(YawTrackBar);
            Controls.Add(PitchTrackBar);
            Controls.Add(splitter2);
            Controls.Add(splitter1);
            Controls.Add(Position);
            Controls.Add(glControl);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)PitchTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)YawTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PosXTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PosYTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PosZTrackBar).EndInit();
            CameraPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenTK.WinForms.GLControl glControl;
        private CheckBox captureMouseCheckBox;
        private TextBox releaseMouseTextBox;
        private Label Position;
        private Button getCameraButton;
        private Button setCameraButton;
        private Button returnDefaultButton;
        private Splitter splitter1;
        private Splitter splitter2;
        private TrackBar PitchTrackBar;
        private TrackBar YawTrackBar;
        private TrackBar PosXTrackBar;
        private TrackBar PosYTrackBar;
        private TrackBar PosZTrackBar;
        private TextBox positionTextBox;
        private TextBox frontTextBox;
        private Button OpenControlsButton;
        private Button NorthButton;
        private Panel CameraPanel;
        private Button SouthButton;
        private Button WestButton;
        private Button EastButton;
        private Button TopButton;
        private Button BackButton;
        private Button StartButton;
        private Label PitchLabel;
        private Label YawLabel;
        private Label XLabel;
        private Label YLabel;
        private Label ZLabel;
        private TextBox YawTextBox;
        private TextBox PitchTextBox;
        private TextBox PositionZTextBox;
        private TextBox PositionYTextBox;
        private TextBox PositionXTextBox;
        private CheckBox TraceCheckbox;
    }
}
