using OpenTK.Graphics.ES20;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSimulatorApp.GlConfig
{
    internal class Cube
    {
        public static string Name;
        public Cube(string name, Vector3 position, float length, float width, float height)
        {
            Name = name;

            //Create vertices responsible for generating a cube:
            CreateWall(position, length, width, height, "x");
            CreateWall(position, 0, width, height, "x");
            CreateWall(position, length, width, height, "y");
            CreateWall(position, length, 0, height, "y");
            CreateWall(position, length, width, height, "z");
            CreateWall(position, length, width, 0, "z");
        }

        public Vector3[] ReturnInternalVectors()
        {
            return null;
        }

        private Vector3[] CreateWall(Vector3 position, float x, float y, float z, string dimension)
        {
            Vector3[] result = Array.Empty<Vector3>();

            switch (dimension)
            {
                case "x":
                    foreach (Vector2 v in CreateWallRectangle(y,z))
                    {
                        result.Append(new Vector3(position.X, v.X + position.Y, v.Y + position.Z));
                    }
                    break;

                case "y":
                    foreach (Vector2 v in CreateWallRectangle(x, z))
                    {
                        result.Append(new Vector3(v.X + position.X, position.Y, v.Y + position.Z));
                    }
                    break;

                case "z":
                    foreach (Vector2 v in CreateWallRectangle(x, y))
                    {
                        result.Append(new Vector3(v.X + position.X, v.Y + position.Y, position.Z));
                    }
                    break;
            }
            return result;
        }

        private Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
