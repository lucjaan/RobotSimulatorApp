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
        public List<Vector3> Vertices = [];
        public Cube(string name, Vector3 position, float length, float width, float height)
        {
            Name = name;


            //Create vertices responsible for generating a cube and add them for later use:
            Vertices.AddRange(CreateWall(position, length, width, 0, "z"));
            Vertices.AddRange(CreateWall(position, length, 0, height, "y"));
            Vertices.AddRange(CreateWall(position, 0, width, height, "x"));
            Vertices.AddRange(CreateWall(position, length, width, height, "z"));
            Vertices.AddRange(CreateWall(position, length, width, height, "y"));
            Vertices.AddRange(CreateWall(position, length, width, height, "x"));






            var x = CreateWall(position, length, width, height, "x");
        }

        public Vector3[] ReturnInternalVectors()
        {
            Vector3[] result = new Vector3[Vertices.Count];
            //int i = 0;
            //foreach (Vector3 v in Vertices) 
            //{
            //    result.(v);
            //}

            for (int i = 0; i < Vertices.Count; i++)
            {
                result[i] = Vertices[i];
            }
            return result;
        }
        
        private List<Vector3> CreateWall(Vector3 position, float x, float y, float z, string dimension)
        {
            //Vector3[] result = Array.Empty<Vector3>();
            List<Vector3> result = [];

            switch (dimension)
            {
                case "x":
                    foreach (Vector2 v in CreateWallRectangle(y,z))
                    {
                        result.Add(new Vector3(position.X, v.X + position.Y, v.Y + position.Z));
                    }
                    break;

                case "y":
                    foreach (Vector2 v in CreateWallRectangle(x, z))
                    {
                        result.Add(new Vector3(v.X + position.X, position.Y, v.Y + position.Z));
                    }
                    break;

                case "z":
                    foreach (Vector2 v in CreateWallRectangle(x, y))
                    {
                        result.Add(new Vector3(v.X + position.X, v.Y + position.Y, position.Z));
                    }
                    break;
            }
            return result;
        }

        private Vector2[] CreateWallRectangle(float a, float b)
            => new Vector2[] { new(0, 0), new(a, 0), new(a, b), new(0, b) };
    }
}
