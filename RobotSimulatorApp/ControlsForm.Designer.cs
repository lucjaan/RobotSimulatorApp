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
            CartesianRadioButton = new System.Windows.Forms.RadioButton();
            JointRadioButton = new System.Windows.Forms.RadioButton();
            CartesianPositionTextBox = new System.Windows.Forms.TextBox();
            CartesianLabel = new System.Windows.Forms.Label();
            J1ValueLabel = new System.Windows.Forms.Label();
            J2ValueLabel = new System.Windows.Forms.Label();
            J3ValueLabel = new System.Windows.Forms.Label();
            J4ValueLabel = new System.Windows.Forms.Label();
            XLabel = new System.Windows.Forms.Label();
            YLabel = new System.Windows.Forms.Label();
            ZLabel = new System.Windows.Forms.Label();
            XTrackBar = new System.Windows.Forms.TrackBar();
            YTrackBar = new System.Windows.Forms.TrackBar();
            ZTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)J1TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)J2TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)J3TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)J4TrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)XTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)YTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ZTrackBar).BeginInit();
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
            J1TrackBar.GotFocus += J1TrackBar_GotFocus;
            // 
            // J2TrackBar
            // 
            J2TrackBar.Location = new Point(48, 114);
            J2TrackBar.Maximum = 360;
            J2TrackBar.Name = "J2TrackBar";
            J2TrackBar.Size = new Size(213, 45);
            J2TrackBar.TabIndex = 1;
            J2TrackBar.Scroll += J2TrackBar_Scroll;
            J2TrackBar.GotFocus += J2TrackBar_GotFocus;
            // 
            // J3TrackBar
            // 
            J3TrackBar.Location = new Point(50, 165);
            J3TrackBar.Maximum = 360;
            J3TrackBar.Name = "J3TrackBar";
            J3TrackBar.Size = new Size(213, 45);
            J3TrackBar.TabIndex = 2;
            J3TrackBar.Scroll += J3TrackBar_Scroll;
            J3TrackBar.GotFocus += J3TrackBar_GotFocus;
            // 
            // J4TrackBar
            // 
            J4TrackBar.Location = new Point(48, 224);
            J4TrackBar.Maximum = 360;
            J4TrackBar.Name = "J4TrackBar";
            J4TrackBar.Size = new Size(213, 45);
            J4TrackBar.TabIndex = 3;
            J4TrackBar.Scroll += J4TrackBar_Scroll;
            J4TrackBar.GotFocus += J4TrackBar_GotFocus;
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
            J1label.Location = new Point(19, 60);
            J1label.Name = "J1label";
            J1label.Size = new Size(30, 28);
            J1label.TabIndex = 5;
            J1label.Text = "J1";
            // 
            // J1TextBox
            // 
            J1TextBox.Location = new Point(48, 343);
            J1TextBox.Name = "J1TextBox";
            J1TextBox.Size = new Size(93, 23);
            J1TextBox.TabIndex = 6;
            // 
            // J2TextBox
            // 
            J2TextBox.Location = new Point(50, 285);
            J2TextBox.Name = "J2TextBox";
            J2TextBox.Size = new Size(91, 23);
            J2TextBox.TabIndex = 7;
            // 
            // J3TextBox
            // 
            J3TextBox.Location = new Point(50, 314);
            J3TextBox.Name = "J3TextBox";
            J3TextBox.Size = new Size(92, 23);
            J3TextBox.TabIndex = 8;
            // 
            // J4TextBox
            // 
            J4TextBox.Location = new Point(50, 372);
            J4TextBox.Name = "J4TextBox";
            J4TextBox.Size = new Size(91, 23);
            J4TextBox.TabIndex = 9;
            // 
            // J2label
            // 
            J2label.AutoSize = true;
            J2label.Font = new Font("Segoe UI", 15F);
            J2label.Location = new Point(21, 108);
            J2label.Name = "J2label";
            J2label.Size = new Size(30, 28);
            J2label.TabIndex = 10;
            J2label.Text = "J2";
            // 
            // J3label
            // 
            J3label.AutoSize = true;
            J3label.Font = new Font("Segoe UI", 15F);
            J3label.Location = new Point(21, 159);
            J3label.Name = "J3label";
            J3label.Size = new Size(30, 28);
            J3label.TabIndex = 11;
            J3label.Text = "J3";
            // 
            // J4label
            // 
            J4label.AutoSize = true;
            J4label.Font = new Font("Segoe UI", 15F);
            J4label.Location = new Point(19, 221);
            J4label.Name = "J4label";
            J4label.Size = new Size(30, 28);
            J4label.TabIndex = 12;
            J4label.Text = "J4";
            // 
            // CartesianRadioButton
            // 
            CartesianRadioButton.AutoSize = true;
            CartesianRadioButton.Location = new Point(187, 372);
            CartesianRadioButton.Name = "CartesianRadioButton";
            CartesianRadioButton.Size = new Size(74, 19);
            CartesianRadioButton.TabIndex = 13;
            CartesianRadioButton.Text = "Cartesian";
            CartesianRadioButton.UseVisualStyleBackColor = true;
            CartesianRadioButton.CheckedChanged += CartesianRadioButton_CheckedChanged;
            // 
            // JointRadioButton
            // 
            JointRadioButton.AutoSize = true;
            JointRadioButton.Checked = true;
            JointRadioButton.Location = new Point(187, 347);
            JointRadioButton.Name = "JointRadioButton";
            JointRadioButton.Size = new Size(50, 19);
            JointRadioButton.TabIndex = 14;
            JointRadioButton.TabStop = true;
            JointRadioButton.Text = "Joint";
            JointRadioButton.UseVisualStyleBackColor = true;
            JointRadioButton.CheckedChanged += JointRadioButton_CheckedChanged;
            // 
            // CartesianPositionTextBox
            // 
            CartesianPositionTextBox.Location = new Point(161, 310);
            CartesianPositionTextBox.Name = "CartesianPositionTextBox";
            CartesianPositionTextBox.Size = new Size(91, 23);
            CartesianPositionTextBox.TabIndex = 15;
            // 
            // CartesianLabel
            // 
            CartesianLabel.AutoSize = true;
            CartesianLabel.Font = new Font("Segoe UI", 11F);
            CartesianLabel.Location = new Point(161, 288);
            CartesianLabel.Name = "CartesianLabel";
            CartesianLabel.Size = new Size(89, 20);
            CartesianLabel.TabIndex = 16;
            CartesianLabel.Text = "Coordinates";
            // 
            // J1ValueLabel
            // 
            J1ValueLabel.AutoSize = true;
            J1ValueLabel.Font = new Font("Segoe UI", 11F);
            J1ValueLabel.Location = new Point(20, 288);
            J1ValueLabel.Name = "J1ValueLabel";
            J1ValueLabel.Size = new Size(22, 20);
            J1ValueLabel.TabIndex = 17;
            J1ValueLabel.Text = "J1";
            // 
            // J2ValueLabel
            // 
            J2ValueLabel.AutoSize = true;
            J2ValueLabel.Font = new Font("Segoe UI", 11F);
            J2ValueLabel.Location = new Point(20, 317);
            J2ValueLabel.Name = "J2ValueLabel";
            J2ValueLabel.Size = new Size(22, 20);
            J2ValueLabel.TabIndex = 18;
            J2ValueLabel.Text = "J2";
            // 
            // J3ValueLabel
            // 
            J3ValueLabel.AutoSize = true;
            J3ValueLabel.Font = new Font("Segoe UI", 11F);
            J3ValueLabel.Location = new Point(20, 345);
            J3ValueLabel.Name = "J3ValueLabel";
            J3ValueLabel.Size = new Size(22, 20);
            J3ValueLabel.TabIndex = 19;
            J3ValueLabel.Text = "J3";
            // 
            // J4ValueLabel
            // 
            J4ValueLabel.AutoSize = true;
            J4ValueLabel.Font = new Font("Segoe UI", 11F);
            J4ValueLabel.Location = new Point(20, 372);
            J4ValueLabel.Name = "J4ValueLabel";
            J4ValueLabel.Size = new Size(22, 20);
            J4ValueLabel.TabIndex = 20;
            J4ValueLabel.Text = "J4";
            // 
            // XLabel
            // 
            XLabel.AutoSize = true;
            XLabel.Font = new Font("Segoe UI", 15F);
            XLabel.Location = new Point(20, 91);
            XLabel.Name = "XLabel";
            XLabel.Size = new Size(24, 28);
            XLabel.TabIndex = 21;
            XLabel.Text = "X";
            XLabel.Visible = false;
            // 
            // YLabel
            // 
            YLabel.AutoSize = true;
            YLabel.Font = new Font("Segoe UI", 15F);
            YLabel.Location = new Point(19, 145);
            YLabel.Name = "YLabel";
            YLabel.Size = new Size(23, 28);
            YLabel.TabIndex = 22;
            YLabel.Text = "Y";
            YLabel.Visible = false;
            // 
            // ZLabel
            // 
            ZLabel.AutoSize = true;
            ZLabel.Font = new Font("Segoe UI", 15F);
            ZLabel.Location = new Point(21, 193);
            ZLabel.Name = "ZLabel";
            ZLabel.Size = new Size(23, 28);
            ZLabel.TabIndex = 23;
            ZLabel.Text = "Z";
            ZLabel.Visible = false;
            // 
            // XTrackBar
            // 
            XTrackBar.Location = new Point(48, 91);
            XTrackBar.Maximum = 360;
            XTrackBar.Name = "XTrackBar";
            XTrackBar.Size = new Size(213, 45);
            XTrackBar.TabIndex = 24;
            XTrackBar.Visible = false;
            // 
            // YTrackBar
            // 
            YTrackBar.Location = new Point(48, 142);
            YTrackBar.Maximum = 360;
            YTrackBar.Name = "YTrackBar";
            YTrackBar.Size = new Size(213, 45);
            YTrackBar.TabIndex = 25;
            YTrackBar.Visible = false;
            // 
            // ZTrackBar
            // 
            ZTrackBar.Location = new Point(48, 191);
            ZTrackBar.Maximum = 360;
            ZTrackBar.Name = "ZTrackBar";
            ZTrackBar.Size = new Size(213, 45);
            ZTrackBar.TabIndex = 26;
            ZTrackBar.Visible = false;
            // 
            // ControlsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(292, 433);
            Controls.Add(ZTrackBar);
            Controls.Add(YTrackBar);
            Controls.Add(XTrackBar);
            Controls.Add(ZLabel);
            Controls.Add(YLabel);
            Controls.Add(XLabel);
            Controls.Add(J4ValueLabel);
            Controls.Add(J3ValueLabel);
            Controls.Add(J2ValueLabel);
            Controls.Add(J1ValueLabel);
            Controls.Add(CartesianLabel);
            Controls.Add(CartesianPositionTextBox);
            Controls.Add(JointRadioButton);
            Controls.Add(CartesianRadioButton);
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
            ((System.ComponentModel.ISupportInitialize)XTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)YTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)ZTrackBar).EndInit();
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
        private System.Windows.Forms.RadioButton CartesianRadioButton;
        private System.Windows.Forms.RadioButton JointRadioButton;
        private System.Windows.Forms.TextBox CartesianPositionTextBox;
        private System.Windows.Forms.Label CartesianLabel;
        private System.Windows.Forms.Label J1ValueLabel;
        private System.Windows.Forms.Label J2ValueLabel;
        private System.Windows.Forms.Label J3ValueLabel;
        private System.Windows.Forms.Label J4ValueLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label ZLabel;
        private System.Windows.Forms.TrackBar XTrackBar;
        private System.Windows.Forms.TrackBar YTrackBar;
        private System.Windows.Forms.TrackBar ZTrackBar;
    }
}