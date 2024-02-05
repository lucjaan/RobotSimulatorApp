using OpenTK.Mathematics;
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
            if (!J1TrackBar.Focused)
            {
                J1TrackBar.Focus();
            }

            SendJointValues();
            Scara.MoveRobot();

            float value = (float)J1TrackBar.Value;
            Scara.MoveJoint(0, value);

            // float value = (float)J1TrackBar.Value;
            // Scara.MoveJoint(0, value);
        }

        private void J2TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J2TrackBar.Focused)
            {
                J2TrackBar.Focus();
            }

            SendJointValues();
            Scara.MoveRobot();


            float value = (float)J2TrackBar.Value;
            Scara.MoveJoint(1, value);

            //float value = (float)J2TrackBar.Value;
            //Scara.MoveJoint(1, value);
        }

        private void J3TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J3TrackBar.Focused)
            {
                J3TrackBar.Focus();
            }

            float value = (float)J3TrackBar.Value;
            Scara.MoveJoint(2, value);
        }

        private void J4TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J4TrackBar.Focused)
            {
                J4TrackBar.Focus();
            }
            float value = (float)J4TrackBar.Value;
            Scara.MoveJoint(3, value);
        }

        private void J1TrackBar_GotFocus(object sender, EventArgs e)
        {
            //Scara.RobotJoints[0].Cube.UpdateBaseModel();
                
            //Scara.RobotJoints[0].UpdateModel();
            //Scara.RobotJoints[0].ce;

        }

        private void J1TrackBar_LostFocus(object sender, EventArgs e)
        {
            //for (int i = 1; i < Scara.RobotJoints.Count; i++)
            //{
            //    Scara.RobotJoints[i].UpdateModel((float)J1TrackBar.Value, Scara.RobotJoints[0].RotationCenter);
            //}
        }

        private void J2TrackBar_GotFocus(object sender, EventArgs e)
        {
            ////Scara.RobotJoints[1].Cube.UpdateBaseModel();
            //Scara.RobotJoints[1].UpdateModel();

            //foreach (RobotLimb rl in Scara.RobotJoints)
            //{
            //    rl.UpdateModel();
            //}
        }

        private void J2TrackBar_LostFocus(object sender, EventArgs e)
        {
            //for (int  i = 2; i < Scara.RobotJoints.Count; i++)
            //{
            //    Scara.RobotJoints[i].UpdateModel((float)J2TrackBar.Value, Scara.RobotJoints[1].RotationCenter);
            //}
        }

        private void J3TrackBar_GotFocus(object sender, EventArgs e)
        {
            //Scara.RobotJoints[2].Cube.UpdateBaseModel();
            //Scara.RobotJoints[2].UpdateModel();

            //foreach (RobotLimb rl in Scara.RobotJoints)
            //{
            //    rl.UpdateModel();
            //}
        }



        private void J4TrackBar_GotFocus(object sender, EventArgs e)
        {
            //Scara.RobotJoints[3].Cube.UpdateBaseModel();
            //Scara.RobotJoints[3].UpdateModel();

            //foreach (RobotLimb rl in Scara.RobotJoints)
            //{
            //    rl.UpdateModel();
            //}
        }

        private void SendJointValues()
        {
            Scara.UpdateJointValues(
                MathHelper.DegreesToRadians((float)J1TrackBar.Value), 
                MathHelper.DegreesToRadians((float)J2TrackBar.Value), 
                (float)J3TrackBar.Value, 
                MathHelper.DegreesToRadians((float)J4TrackBar.Value)
           );
        }
    }
}
