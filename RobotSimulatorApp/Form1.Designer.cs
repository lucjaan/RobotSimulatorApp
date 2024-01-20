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
            ((System.ComponentModel.ISupportInitialize)frontX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)frontY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)frontZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)positionX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)positionY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)positionZ).BeginInit();
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
            glControl.Size = new Size(1019, 527);
            glControl.TabIndex = 0;
            glControl.Text = "glControl";
            // 
            // captureMouseCheckBox
            // 
            captureMouseCheckBox.AutoSize = true;
            captureMouseCheckBox.Location = new Point(1192, 520);
            captureMouseCheckBox.Name = "captureMouseCheckBox";
            captureMouseCheckBox.Size = new Size(98, 19);
            captureMouseCheckBox.TabIndex = 1;
            captureMouseCheckBox.Text = "Move camera";
            captureMouseCheckBox.UseVisualStyleBackColor = true;
            captureMouseCheckBox.CheckedChanged += captureMouseCheckBox_CheckedChanged;
            // 
            // releaseMouseTextBox
            // 
            releaseMouseTextBox.Location = new Point(1135, 491);
            releaseMouseTextBox.Name = "releaseMouseTextBox";
            releaseMouseTextBox.Size = new Size(155, 23);
            releaseMouseTextBox.TabIndex = 2;
            releaseMouseTextBox.Text = "Press ESC to release mouse";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(1064, 12);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(226, 213);
            textBox1.TabIndex = 3;
            // 
            // Position
            // 
            Position.AutoSize = true;
            Position.Location = new Point(1069, 288);
            Position.Name = "Position";
            Position.Size = new Size(50, 15);
            Position.TabIndex = 12;
            Position.Text = "Position";
            // 
            // FrontLabel
            // 
            FrontLabel.AutoSize = true;
            FrontLabel.Location = new Point(1066, 235);
            FrontLabel.Name = "FrontLabel";
            FrontLabel.Size = new Size(35, 15);
            FrontLabel.TabIndex = 13;
            FrontLabel.Text = "Front";
            // 
            // frontX
            // 
            frontX.Location = new Point(1071, 259);
            frontX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            frontX.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            frontX.Name = "frontX";
            frontX.Size = new Size(63, 23);
            frontX.TabIndex = 14;
            // 
            // frontY
            // 
            frontY.Location = new Point(1149, 259);
            frontY.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            frontY.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            frontY.Name = "frontY";
            frontY.Size = new Size(63, 23);
            frontY.TabIndex = 15;
            // 
            // frontZ
            // 
            frontZ.Location = new Point(1227, 259);
            frontZ.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            frontZ.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            frontZ.Name = "frontZ";
            frontZ.Size = new Size(63, 23);
            frontZ.TabIndex = 16;
            // 
            // positionX
            // 
            positionX.Location = new Point(1071, 315);
            positionX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            positionX.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            positionX.Name = "positionX";
            positionX.Size = new Size(63, 23);
            positionX.TabIndex = 17;
            // 
            // positionY
            // 
            positionY.Location = new Point(1149, 315);
            positionY.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            positionY.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            positionY.Name = "positionY";
            positionY.Size = new Size(63, 23);
            positionY.TabIndex = 18;
            // 
            // positionZ
            // 
            positionZ.Location = new Point(1227, 315);
            positionZ.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            positionZ.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            positionZ.Name = "positionZ";
            positionZ.Size = new Size(63, 23);
            positionZ.TabIndex = 19;
            // 
            // getCameraButton
            // 
            getCameraButton.Location = new Point(1192, 353);
            getCameraButton.Name = "getCameraButton";
            getCameraButton.Size = new Size(86, 25);
            getCameraButton.TabIndex = 21;
            getCameraButton.Text = "Get Camera";
            getCameraButton.UseVisualStyleBackColor = true;
            getCameraButton.Click += getCameraButton_Click;
            // 
            // setCameraButton
            // 
            setCameraButton.Location = new Point(1192, 384);
            setCameraButton.Name = "setCameraButton";
            setCameraButton.Size = new Size(86, 25);
            setCameraButton.TabIndex = 22;
            setCameraButton.Text = "Set Camera";
            setCameraButton.UseVisualStyleBackColor = true;
            setCameraButton.Click += setCameraButton_Click;
            // 
            // returnDefaultButton
            // 
            returnDefaultButton.Location = new Point(1071, 353);
            returnDefaultButton.Name = "returnDefaultButton";
            returnDefaultButton.Size = new Size(86, 56);
            returnDefaultButton.TabIndex = 23;
            returnDefaultButton.Text = "Return Default";
            returnDefaultButton.UseVisualStyleBackColor = true;
            returnDefaultButton.Click += returnDefaultButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1302, 560);
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
    }
}
