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
            captureMouseCheckBox = new CheckBox();
            releaseMouseTextBox = new TextBox();
            Position = new Label();
            FrontLabel = new Label();
            getCameraButton = new Button();
            setCameraButton = new Button();
            returnDefaultButton = new Button();
            splitter1 = new Splitter();
            splitter2 = new Splitter();
            FrontXtrackBar = new TrackBar();
            FrontYtrackBar = new TrackBar();
            FrontZtrackBar = new TrackBar();
            PosXTrackBar = new TrackBar();
            PosYTrackBar = new TrackBar();
            PosZTrackBar = new TrackBar();
            positionTextBox = new TextBox();
            frontTextBox = new TextBox();
            OpenControlsButton = new Button();
            ((System.ComponentModel.ISupportInitialize)FrontXtrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrontYtrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrontZtrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PosXTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PosYTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PosZTrackBar).BeginInit();
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
            // captureMouseCheckBox
            // 
            captureMouseCheckBox.AutoSize = true;
            captureMouseCheckBox.Location = new Point(1231, 614);
            captureMouseCheckBox.Name = "captureMouseCheckBox";
            captureMouseCheckBox.Size = new Size(98, 19);
            captureMouseCheckBox.TabIndex = 1;
            captureMouseCheckBox.Text = "Move camera";
            captureMouseCheckBox.UseVisualStyleBackColor = true;
            captureMouseCheckBox.CheckedChanged += captureMouseCheckBox_CheckedChanged;
            // 
            // releaseMouseTextBox
            // 
            releaseMouseTextBox.Location = new Point(1174, 585);
            releaseMouseTextBox.Name = "releaseMouseTextBox";
            releaseMouseTextBox.Size = new Size(155, 23);
            releaseMouseTextBox.TabIndex = 2;
            releaseMouseTextBox.Text = "Press ESC to release mouse";
            // 
            // Position
            // 
            Position.AutoSize = true;
            Position.Location = new Point(1121, 12);
            Position.Name = "Position";
            Position.Size = new Size(50, 15);
            Position.TabIndex = 12;
            Position.Text = "Position";
            // 
            // FrontLabel
            // 
            FrontLabel.AutoSize = true;
            FrontLabel.Location = new Point(1121, 144);
            FrontLabel.Name = "FrontLabel";
            FrontLabel.Size = new Size(35, 15);
            FrontLabel.TabIndex = 13;
            FrontLabel.Text = "Front";
            // 
            // getCameraButton
            // 
            getCameraButton.Location = new Point(1243, 523);
            getCameraButton.Name = "getCameraButton";
            getCameraButton.Size = new Size(86, 25);
            getCameraButton.TabIndex = 21;
            getCameraButton.Text = "Get Camera";
            getCameraButton.UseVisualStyleBackColor = true;
            getCameraButton.Click += getCameraButton_Click;
            // 
            // setCameraButton
            // 
            setCameraButton.Location = new Point(1243, 554);
            setCameraButton.Name = "setCameraButton";
            setCameraButton.Size = new Size(86, 25);
            setCameraButton.TabIndex = 22;
            setCameraButton.Text = "Set Camera";
            setCameraButton.UseVisualStyleBackColor = true;
            setCameraButton.Click += setCameraButton_Click;
            // 
            // returnDefaultButton
            // 
            returnDefaultButton.Location = new Point(1121, 523);
            returnDefaultButton.Name = "returnDefaultButton";
            returnDefaultButton.Size = new Size(86, 56);
            returnDefaultButton.TabIndex = 23;
            returnDefaultButton.Text = "Return Default";
            returnDefaultButton.UseVisualStyleBackColor = true;
            returnDefaultButton.Click += returnDefaultButton_Click;
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
            // FrontXtrackBar
            // 
            FrontXtrackBar.AutoSize = false;
            FrontXtrackBar.Location = new Point(1121, 162);
            FrontXtrackBar.Maximum = 180;
            FrontXtrackBar.Minimum = -180;
            FrontXtrackBar.Name = "FrontXtrackBar";
            FrontXtrackBar.Size = new Size(208, 33);
            FrontXtrackBar.TabIndex = 26;
            FrontXtrackBar.Scroll += FrontXtrackBar_Scroll;
            // 
            // FrontYtrackBar
            // 
            FrontYtrackBar.AutoSize = false;
            FrontYtrackBar.Location = new Point(1121, 201);
            FrontYtrackBar.Maximum = 180;
            FrontYtrackBar.Minimum = -180;
            FrontYtrackBar.Name = "FrontYtrackBar";
            FrontYtrackBar.Size = new Size(208, 33);
            FrontYtrackBar.TabIndex = 27;
            FrontYtrackBar.Scroll += FrontYtrackBar_Scroll;
            // 
            // FrontZtrackBar
            // 
            FrontZtrackBar.AutoSize = false;
            FrontZtrackBar.Location = new Point(1121, 240);
            FrontZtrackBar.Maximum = 180;
            FrontZtrackBar.Minimum = -180;
            FrontZtrackBar.Name = "FrontZtrackBar";
            FrontZtrackBar.Size = new Size(208, 33);
            FrontZtrackBar.TabIndex = 28;
            FrontZtrackBar.Scroll += FrontZtrackBar_Scroll;
            // 
            // PosXTrackBar
            // 
            PosXTrackBar.AutoSize = false;
            PosXTrackBar.Location = new Point(1121, 30);
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
            PosYTrackBar.Location = new Point(1121, 69);
            PosYTrackBar.Maximum = 500;
            PosYTrackBar.Minimum = -500;
            PosYTrackBar.Name = "PosYTrackBar";
            PosYTrackBar.Size = new Size(208, 33);
            PosYTrackBar.TabIndex = 30;
            PosYTrackBar.Scroll += PosYTrackBar_Scroll;
            // 
            // PosZTrackBar
            // 
            PosZTrackBar.AutoSize = false;
            PosZTrackBar.Location = new Point(1121, 108);
            PosZTrackBar.Maximum = 500;
            PosZTrackBar.Minimum = -500;
            PosZTrackBar.Name = "PosZTrackBar";
            PosZTrackBar.Size = new Size(208, 33);
            PosZTrackBar.TabIndex = 31;
            PosZTrackBar.Scroll += PosZTrackBar_Scroll;
            // 
            // positionTextBox
            // 
            positionTextBox.Location = new Point(1121, 279);
            positionTextBox.Name = "positionTextBox";
            positionTextBox.Size = new Size(208, 23);
            positionTextBox.TabIndex = 32;
            // 
            // frontTextBox
            // 
            frontTextBox.Location = new Point(1121, 308);
            frontTextBox.Name = "frontTextBox";
            frontTextBox.Size = new Size(208, 23);
            frontTextBox.TabIndex = 33;
            // 
            // OpenControlsButton
            // 
            OpenControlsButton.Location = new Point(1121, 346);
            OpenControlsButton.Name = "OpenControlsButton";
            OpenControlsButton.Size = new Size(208, 23);
            OpenControlsButton.TabIndex = 37;
            OpenControlsButton.Text = "Open Limb controls";
            OpenControlsButton.UseVisualStyleBackColor = true;
            OpenControlsButton.Click += OpenControlsButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1341, 654);
            Controls.Add(OpenControlsButton);
            Controls.Add(frontTextBox);
            Controls.Add(positionTextBox);
            Controls.Add(PosZTrackBar);
            Controls.Add(PosYTrackBar);
            Controls.Add(PosXTrackBar);
            Controls.Add(FrontZtrackBar);
            Controls.Add(FrontYtrackBar);
            Controls.Add(FrontXtrackBar);
            Controls.Add(splitter2);
            Controls.Add(splitter1);
            Controls.Add(returnDefaultButton);
            Controls.Add(setCameraButton);
            Controls.Add(getCameraButton);
            Controls.Add(FrontLabel);
            Controls.Add(Position);
            Controls.Add(releaseMouseTextBox);
            Controls.Add(captureMouseCheckBox);
            Controls.Add(glControl);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)FrontXtrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrontYtrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrontZtrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PosXTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PosYTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PosZTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenTK.WinForms.GLControl glControl;
        private CheckBox captureMouseCheckBox;
        private TextBox releaseMouseTextBox;
        private Label Position;
        private Label FrontLabel;
        private Button getCameraButton;
        private Button setCameraButton;
        private Button returnDefaultButton;
        private Splitter splitter1;
        private Splitter splitter2;
        private TrackBar FrontXtrackBar;
        private TrackBar FrontYtrackBar;
        private TrackBar FrontZtrackBar;
        private TrackBar PosXTrackBar;
        private TrackBar PosYTrackBar;
        private TrackBar PosZTrackBar;
        private TextBox positionTextBox;
        private TextBox frontTextBox;
        private Button OpenControlsButton;
    }
}
