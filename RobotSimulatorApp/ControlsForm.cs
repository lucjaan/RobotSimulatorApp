using RobotSimulatorApp.Robot.SCARA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void J1TrackBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)J1TrackBar.Value;
            Scara.MoveJoint(0, value);
        }

        private void J2TrackBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)J2TrackBar.Value;
            Scara.MoveJoint(1, value);
        }

        private void J3TrackBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)J3TrackBar.Value;
            Scara.MoveJoint(2, value);
        }

        private void J4TrackBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)J4TrackBar.Value;
            Scara.MoveJoint(3, value);
        }
    }
}
