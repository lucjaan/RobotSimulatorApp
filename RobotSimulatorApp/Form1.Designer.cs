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
            SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControl.APIVersion = new Version(3, 3, 0, 0);
            glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControl.IsEventDriven = true;
            glControl.Location = new Point(12, 12);
            glControl.Name = "glControl";
            glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glControl.SharedContext = null;
            glControl.Size = new Size(609, 424);
            glControl.TabIndex = 0;
            glControl.Text = "glControl";
            // 
            // captureMouseCheckBox
            // 
            captureMouseCheckBox.AutoSize = true;
            captureMouseCheckBox.Location = new Point(658, 359);
            captureMouseCheckBox.Name = "captureMouseCheckBox";
            captureMouseCheckBox.Size = new Size(98, 19);
            captureMouseCheckBox.TabIndex = 1;
            captureMouseCheckBox.Text = "Move camera";
            captureMouseCheckBox.UseVisualStyleBackColor = true;
            captureMouseCheckBox.CheckedChanged += captureMouseCheckBox_CheckedChanged;

            // 
            // releaseMouseTextBox
            // 
            releaseMouseTextBox.Location = new Point(658, 384);
            releaseMouseTextBox.Name = "releaseMouseTextBox";
            releaseMouseTextBox.Size = new Size(155, 23);
            releaseMouseTextBox.TabIndex = 2;
            releaseMouseTextBox.Text = "Press ESC to release mouse";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(642, 68);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(226, 213);
            textBox1.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(892, 457);
            Controls.Add(textBox1);
            Controls.Add(releaseMouseTextBox);
            Controls.Add(captureMouseCheckBox);
            Controls.Add(glControl);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private OpenTK.WinForms.GLControl glControl;
        private CheckBox captureMouseCheckBox;
        private TextBox releaseMouseTextBox;
        private TextBox textBox1;
    }
}
