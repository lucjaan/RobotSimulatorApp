namespace RobotSimulatorApp.Robot
{
    public abstract class Robot
    {
        protected string Name { get; set; }
        public RobotTypes RobotType { get; set; }
        protected Robot() { }

        public void SaveToFile()
        {
            //TODO
        }

        public void LoadFromFile()
        {
            //TODO
        }
    }
}
