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
            textBox1 = new TextBox();
            Position = new Label();
            FrontLabel = new Label();
            frontX = new NumericUpDown();
            frontY = new NumericUpDown();
            frontZ = new NumericUpDown();
            positionX = new NumericUpDown();
            positionY = new NumericUpDown();
            positionZ = new NumericUpDown();
            getCameraButton = new Button();
            setCameraButton = new Button();
            returnDefaultButton = new Button();
            splitter1 = new Splitter();
            splitter2 = new Splitter();
            FrontXtrackBar = new TrackBar();
            FrontYtrackBar = new TrackBar();
            FrontZtrackBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)frontX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)frontY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)frontZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)positionX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)positionY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)positionZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrontXtrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrontYtrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FrontZtrackBar).BeginInit();
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
            // textBox1
            // 
            textBox1.Location = new Point(1103, 19);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(226, 213);
            textBox1.TabIndex = 3;
            // 
            // Position
            // 
            Position.AutoSize = true;
            Position.Location = new Point(1103, 285);
            Position.Name = "Position";
            Position.Size = new Size(50, 15);
            Position.TabIndex = 12;
            Position.Text = "Position";
            // 
            // FrontLabel
            // 
            FrontLabel.AutoSize = true;
            FrontLabel.Location = new Point(1103, 241);
            FrontLabel.Name = "FrontLabel";
            FrontLabel.Size = new Size(35, 15);
            FrontLabel.TabIndex = 13;
            FrontLabel.Text = "Front";
            // 
            // frontX
            // 
            frontX.Location = new Point(1103, 259);
            frontX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            frontX.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            frontX.Name = "frontX";
            frontX.Size = new Size(63, 23);
            frontX.TabIndex = 14;
            // 
            // frontY
            // 
            frontY.Location = new Point(1183, 259);
            frontY.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            frontY.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            frontY.Name = "frontY";
            frontY.Size = new Size(63, 23);
            frontY.TabIndex = 15;
            // 
            // frontZ
            // 
            frontZ.Location = new Point(1266, 259);
            frontZ.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            frontZ.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            frontZ.Name = "frontZ";
            frontZ.Size = new Size(63, 23);
            frontZ.TabIndex = 16;
            // 
            // positionX
            // 
            positionX.Location = new Point(1103, 303);
            positionX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            positionX.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            positionX.Name = "positionX";
            positionX.Size = new Size(63, 23);
            positionX.TabIndex = 17;
            // 
            // positionY
            // 
            positionY.Location = new Point(1183, 303);
            positionY.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            positionY.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            positionY.Name = "positionY";
            positionY.Size = new Size(63, 23);
            positionY.TabIndex = 18;
            // 
            // positionZ
            // 
            positionZ.Location = new Point(1266, 303);
            positionZ.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            positionZ.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            positionZ.Name = "positionZ";
            positionZ.Size = new Size(63, 23);
            positionZ.TabIndex = 19;
            // 
            // getCameraButton
            // 
            getCameraButton.Location = new Point(1243, 332);
            getCameraButton.Name = "getCameraButton";
            getCameraButton.Size = new Size(86, 25);
            getCameraButton.TabIndex = 21;
            getCameraButton.Text = "Get Camera";
            getCameraButton.UseVisualStyleBackColor = true;
            getCameraButton.Click += getCameraButton_Click;
            // 
            // setCameraButton
            // 
            setCameraButton.Location = new Point(1243, 363);
            setCameraButton.Name = "setCameraButton";
            setCameraButton.Size = new Size(86, 25);
            setCameraButton.TabIndex = 22;
            setCameraButton.Text = "Set Camera";
            setCameraButton.UseVisualStyleBackColor = true;
            setCameraButton.Click += setCameraButton_Click;
            // 
            // returnDefaultButton
            // 
            returnDefaultButton.Location = new Point(1151, 332);
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
            FrontXtrackBar.Location = new Point(1121, 394);
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
            FrontYtrackBar.Location = new Point(1121, 433);
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
            FrontZtrackBar.Location = new Point(1121, 472);
            FrontZtrackBar.Maximum = 180;
            FrontZtrackBar.Minimum = -180;
            FrontZtrackBar.Name = "FrontZtrackBar";
            FrontZtrackBar.Size = new Size(208, 33);
            FrontZtrackBar.TabIndex = 28;
            FrontZtrackBar.Scroll += FrontZtrackBar_Scroll;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1341, 654);
            Controls.Add(FrontZtrackBar);
            Controls.Add(FrontYtrackBar);
            Controls.Add(FrontXtrackBar);
            Controls.Add(splitter2);
            Controls.Add(splitter1);
            Controls.Add(returnDefaultButton);
            Controls.Add(setCameraButton);
            Controls.Add(getCameraButton);
            Controls.Add(positionZ);
            Controls.Add(positionY);
            Controls.Add(positionX);
            Controls.Add(frontZ);
            Controls.Add(frontY);
            Controls.Add(frontX);
            Controls.Add(FrontLabel);
            Controls.Add(Position);
            Controls.Add(textBox1);
            Controls.Add(releaseMouseTextBox);
            Controls.Add(captureMouseCheckBox);
            Controls.Add(glControl);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)frontX).EndInit();
            ((System.ComponentModel.ISupportInitialize)frontY).EndInit();
            ((System.ComponentModel.ISupportInitialize)frontZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)positionX).EndInit();
            ((System.ComponentModel.ISupportInitialize)positionY).EndInit();
            ((System.ComponentModel.ISupportInitialize)positionZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrontXtrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrontYtrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)FrontZtrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenTK.WinForms.GLControl glControl;
        private CheckBox captureMouseCheckBox;
        private TextBox releaseMouseTextBox;
        private TextBox textBox1;
        private Label Position;
        private Label FrontLabel;
        private NumericUpDown frontX;
        private NumericUpDown frontY;
        private NumericUpDown frontZ;
        private NumericUpDown positionX;
        private NumericUpDown positionY;
        private NumericUpDown positionZ;
        private Button getCameraButton;
        private Button setCameraButton;
        private Button returnDefaultButton;
        private Splitter splitter1;
        private Splitter splitter2;
        private TrackBar FrontXtrackBar;
        private TrackBar FrontYtrackBar;
        private TrackBar FrontZtrackBar;
    }
}
