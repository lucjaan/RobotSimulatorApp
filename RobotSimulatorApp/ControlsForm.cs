﻿using RobotSimulatorApp.Robot.SCARA;
using System;
using System.Windows.Forms;

namespace RobotSimulatorApp
{
    public partial class ControlsForm : Form
    {
        public SCARA_Robot Scara { get; set; }

        public ControlsForm(SCARA_Robot scara)
        {
            InitializeComponent();
            Scara = scara;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            J1TextBox.Text = J1TrackBar.Value.ToString();
            J2TextBox.Text = J2TrackBar.Value.ToString();
            J3TextBox.Text = J3TrackBar.Value.ToString();
            J4TextBox.Text = J4TrackBar.Value.ToString();
        }

        private void J1TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J1TrackBar.Focused)
            {
                J1TrackBar.Focus();
            }

            float value = (float)J1TrackBar.Value;
            Scara.MoveRevoluteJoint(0, value);
            J1TextBox.Text = value.ToString();
        }

        private void J2TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J2TrackBar.Focused)
            {
                J2TrackBar.Focus();
            }

            float value = (float)J2TrackBar.Value;
            Scara.MoveRevoluteJoint(1, value);
            J2TextBox.Text = value.ToString();
        }

        private void J3TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J3TrackBar.Focused)
            {
                J3TrackBar.Focus();
            }

            float value = (float)J3TrackBar.Value;
            Scara.MoveLinearJoint(2, value);
            J3TextBox.Text = value.ToString();
        }

        private void J4TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J4TrackBar.Focused)
            {
                J4TrackBar.Focus();
            }
            float value = (float)J4TrackBar.Value;
            Scara.MoveRevoluteJoint(3, value);
            J4TextBox.Text = value.ToString();
        }

        private void J1TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
        }

        private void J2TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
        }

        private void J3TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
        }

        private void J4TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
        }

        private void SendJointValues()
        {
            Scara.UpdateJointValues(
                (float)J1TrackBar.Value,
                (float)J2TrackBar.Value,
                (float)J3TrackBar.Value,
                (float)J4TrackBar.Value
           );
        }

        private void JointRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (JointRadioButton.Checked)
            {
                CartesianRadioButton.Checked = false;
                J1TrackBar.Show();
                J2TrackBar.Show();
                J3TrackBar.Show();
                J4TrackBar.Show();
                J1label.Show();
                J2label.Show();
                J3label.Show();
                J4label.Show();

                XLabel.Hide();
                YLabel.Hide();
                ZLabel.Hide();
                XTrackBar.Hide();
                YTrackBar.Hide();
                ZTrackBar.Hide();
            }
        }

        private void CartesianRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CartesianRadioButton.Checked)
            {
                JointRadioButton.Checked = false;
                J1TrackBar.Hide();
                J2TrackBar.Hide();
                J3TrackBar.Hide();
                J4TrackBar.Hide();
                J1label.Hide();
                J2label.Hide();
                J3label.Hide();
                J4label.Hide();

                XLabel.Show();
                YLabel.Show();
                ZLabel.Show();
                XTrackBar.Show();
                YTrackBar.Show();
                ZTrackBar.Show();
            }
        }
    }
}
