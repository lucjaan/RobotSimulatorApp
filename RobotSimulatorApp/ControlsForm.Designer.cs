using System.Drawing;

namespace RobotSimulatorApp
{
    partial class ControlsForm
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
            J1TrackBar = new System.Windows.Forms.TrackBar();
            J2TrackBar = new System.Windows.Forms.TrackBar();
            J3TrackBar = new System.Windows.Forms.TrackBar();
            J4TrackBar = new System.Windows.Forms.TrackBar();
            TitleLabel = new System.Windows.Forms.Label();
            J1label = new System.Windows.Forms.Label();
            J1TextBox = new System.Windows.Forms.TextBox();
            J2TextBox = new System.Windows.Forms.TextBox();
            J3TextBox = new System.Windows.Forms.TextBox();
            J4TextBox = new System.Windows.Forms.TextBox();
            J2label = new System.Windows.Forms.Label();
            J3label = new System.Windows.Forms.Label();
            J4label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)J1TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)J2TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)J3TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)J4TrackBar).BeginInit();
            SuspendLayout();
            // 
            // J1TrackBar
            // 
            J1TrackBar.Location = new Point(50, 63);
            J1TrackBar.Maximum = 360;
            J1TrackBar.Name = "J1TrackBar";
            J1TrackBar.Size = new Size(213, 45);
            J1TrackBar.TabIndex = 0;
            J1TrackBar.Scroll += J1TrackBar_Scroll;
            // 
            // J2TrackBar
            // 
            J2TrackBar.Location = new Point(50, 141);
            J2TrackBar.Maximum = 360;
            J2TrackBar.Name = "J2TrackBar";
            J2TrackBar.Size = new Size(213, 45);
            J2TrackBar.TabIndex = 1;
            J2TrackBar.Scroll += J2TrackBar_Scroll;
            // 
            // J3TrackBar
            // 
            J3TrackBar.Location = new Point(50, 227);
            J3TrackBar.Maximum = 360;
            J3TrackBar.Name = "J3TrackBar";
            J3TrackBar.Size = new Size(213, 45);
            J3TrackBar.TabIndex = 2;
            J3TrackBar.Scroll += J3TrackBar_Scroll;
            // 
            // J4TrackBar
            // 
            J4TrackBar.Location = new Point(50, 304);
            J4TrackBar.Maximum = 360;
            J4TrackBar.Name = "J4TrackBar";
            J4TrackBar.Size = new Size(213, 45);
            J4TrackBar.TabIndex = 3;
            J4TrackBar.Scroll += J4TrackBar_Scroll;
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            TitleLabel.Location = new Point(89, 27);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(129, 33);
            TitleLabel.TabIndex = 4;
            TitleLabel.Text = "Robot Joints";
            TitleLabel.UseCompatibleTextRendering = true;
            // 
            // J1label
            // 
            J1label.AutoSize = true;
            J1label.Font = new Font("Segoe UI", 15F);
            J1label.Location = new Point(23, 80);
            J1label.Name = "J1label";
            J1label.Size = new Size(30, 28);
            J1label.TabIndex = 5;
            J1label.Text = "J1";
            // 
            // J1TextBox
            // 
            J1TextBox.Location = new Point(59, 97);
            J1TextBox.Name = "J1TextBox";
            J1TextBox.Size = new Size(191, 23);
            J1TextBox.TabIndex = 6;
            // 
            // J2TextBox
            // 
            J2TextBox.Location = new Point(59, 177);
            J2TextBox.Name = "J2TextBox";
            J2TextBox.Size = new Size(191, 23);
            J2TextBox.TabIndex = 7;
            // 
            // J3TextBox
            // 
            J3TextBox.Location = new Point(59, 261);
            J3TextBox.Name = "J3TextBox";
            J3TextBox.Size = new Size(191, 23);
            J3TextBox.TabIndex = 8;
            // 
            // J4TextBox
            // 
            J4TextBox.Location = new Point(59, 344);
            J4TextBox.Name = "J4TextBox";
            J4TextBox.Size = new Size(191, 23);
            J4TextBox.TabIndex = 9;
            // 
            // J2label
            // 
            J2label.AutoSize = true;
            J2label.Font = new Font("Segoe UI", 15F);
            J2label.Location = new Point(23, 158);
            J2label.Name = "J2label";
            J2label.Size = new Size(30, 28);
            J2label.TabIndex = 10;
            J2label.Text = "J2";
            // 
            // J3label
            // 
            J3label.AutoSize = true;
            J3label.Font = new Font("Segoe UI", 15F);
            J3label.Location = new Point(23, 244);
            J3label.Name = "J3label";
            J3label.Size = new Size(30, 28);
            J3label.TabIndex = 11;
            J3label.Text = "J3";
            // 
            // J4label
            // 
            J4label.AutoSize = true;
            J4label.Font = new Font("Segoe UI", 15F);
            J4label.Location = new Point(23, 321);
            J4label.Name = "J4label";
            J4label.Size = new Size(30, 28);
            J4label.TabIndex = 12;
            J4label.Text = "J4";
            // 
            // ControlsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(292, 400);
            Controls.Add(J4label);
            Controls.Add(J3label);
            Controls.Add(J2label);
            Controls.Add(J4TextBox);
            Controls.Add(J3TextBox);
            Controls.Add(J2TextBox);
            Controls.Add(J1TextBox);
            Controls.Add(J1label);
            Controls.Add(TitleLabel);
            Controls.Add(J4TrackBar);
            Controls.Add(J3TrackBar);
            Controls.Add(J2TrackBar);
            Controls.Add(J1TrackBar);
            Name = "ControlsForm";
            Text = "ControlsForm";
            ((System.ComponentModel.ISupportInitialize)J1TrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)J2TrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)J3TrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)J4TrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TrackBar J1TrackBar;
        private System.Windows.Forms.TrackBar J2TrackBar;
        private System.Windows.Forms.TrackBar J3TrackBar;
        private System.Windows.Forms.TrackBar J4TrackBar;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label J1label;
        private System.Windows.Forms.TextBox J1TextBox;
        private System.Windows.Forms.TextBox J2TextBox;
        private System.Windows.Forms.TextBox J3TextBox;
        private System.Windows.Forms.TextBox J4TextBox;
        private System.Windows.Forms.Label J2label;
        private System.Windows.Forms.Label J3label;
        private System.Windows.Forms.Label J4label;
    }
}