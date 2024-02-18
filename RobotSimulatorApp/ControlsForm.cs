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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void J1TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J1TrackBar.Focused)
            {
                J1TrackBar.Focus();
            }
            float value = (float)J1TrackBar.Value;

            //SendJointValues();
            //Scara.MoveRobot(value);

            Scara.MoveJoint(0, value);
            //SendJointValues();

            // float value = (float)J1TrackBar.Value;
            // Scara.MoveJoint(0, value);
        }

        private void J2TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J2TrackBar.Focused)
            {
                J2TrackBar.Focus();
            }

            //SendJointValues();
            //Scara.MoveRobot();
            //Scara.RobotJoints[1].SetRotationCenter()

            float value = (float)J2TrackBar.Value;
            Scara.MoveJoint(1, value);
            //SendJointValues();

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
            //SendJointValues();

        }

        private void J4TrackBar_Scroll(object sender, EventArgs e)
        {
            if (!J4TrackBar.Focused)
            {
                J4TrackBar.Focus();
            }
           float value = (float)J4TrackBar.Value;
           Scara.MoveJoint(3, value);
           //SendJointValues();

        }

        private void J1TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
            Scara.CalculateCenters();
            //Scara.RobotJoints[0].Cube.UpdateBaseModel();

        }

        private void J2TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
            Scara.CalculateCenters();
            //Scara.CalculateCenters();
            //Scara.RobotJoints[1].Cube.UpdateBaseModel();

            //Scara.UpdateModels();
            ////Scara.RobotJoints[1].Cube.UpdateBaseModel();
            //Scara.RobotJoints[1].UpdateModel();

            //foreach (RobotLimb rl in Scara.RobotJoints)
            //{
            //    rl.UpdateModel();
            //}
        }

        private void J3TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
            Scara.CalculateCenters();          //Scara.RobotJoints[2].Cube.UpdateBaseModel();
            //Scara.RobotJoints[2].UpdateModel();

            //foreach (RobotLimb rl in Scara.RobotJoints)
            //{
            //    rl.UpdateModel();
            //}
        }

        private void J4TrackBar_GotFocus(object sender, EventArgs e)
        {
            SendJointValues();
            Scara.UpdateModels();
            Scara.CalculateCenters();         //Scara.RobotJoints[3].Cube.UpdateBaseModel();
            //Scara.RobotJoints[3].UpdateModel();

            //foreach (RobotLimb rl in Scara.RobotJoints)
            //{
            //    rl.UpdateModel();
            //}
        }

        private void J1TrackBar_LostFocus(object sender, EventArgs e)
        {
            //Scara.RobotJoints[0].Distance = (float)J1TrackBar.Value;
            //Scara.UpdateModels();
            Scara.RobotJoints[0].Cube.PrevTransformation = Scara.RobotJoints[0].Cube.Transformation;
            //Scara.RobotJoints[0].Cube.Transformation = Matrix4.Identity;
        }
        private void J2TrackBar_LostFocus(object sender, EventArgs e)
        {
            //Scara.RobotJoints[1].Distance = (float)J2TrackBar.Value;
            //Scara.UpdateModels();
            Scara.RobotJoints[1].Cube.PrevTransformation = Scara.RobotJoints[1].Cube.Transformation;
            //Scara.RobotJoints[1].Cube.Transformation = Matrix4.Identity;
        }
        private void J3TrackBar_LostFocus(object sender, EventArgs e)
        {
            Scara.RobotJoints[2].Cube.PrevTransformation = Scara.RobotJoints[2].Cube.Transformation;
            //Scara.RobotJoints[2].Cube.Transformation = Matrix4.Identity;
            //Scara.RobotJoints[2].Distance = (float)J3TrackBar.Value;
            //Scara.UpdateModels();

        }
        private void J4TrackBar_LostFocus(object sender, EventArgs e)
        {
            Scara.RobotJoints[3].Cube.PrevTransformation = Scara.RobotJoints[3].Cube.Transformation;
            //Scara.RobotJoints[2].Cube.Transformation = Matrix4.Identity;
           // Scara.RobotJoints[3].Distance = (float)J4TrackBar.Value;
            //Scara.UpdateModels();

        }

        private void SendJointValues()
        {
            Scara.UpdateJointValues(
                (float)J1TrackBar.Value,
                (float)J2TrackBar.Value,
                (float)J3TrackBar.Value,
                (float)J4TrackBar.Value
                //MathHelper.DegreesToRadians((float)J1TrackBar.Value), 
                //MathHelper.DegreesToRadians((float)J2TrackBar.Value), 
                //(float)J3TrackBar.Value, 
                //MathHelper.DegreesToRadians((float)J4TrackBar.Value)
           );

            

        }
    }
}
