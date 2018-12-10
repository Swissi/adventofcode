using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _10p1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Read unsorted file
            string[] lines = File.ReadAllLines("i.txt");

            List<Light> lights = new List<Light>();
            foreach (string line in lines)
            {
                Light light = new Light();
                if (lines.Length < 100)
                {
                    light.x = int.Parse(line.Substring(10, 2));
                    light.y = int.Parse(line.Substring(14, 2));
                    light.v_x = int.Parse(line.Substring(28, 2));
                    light.v_y = int.Parse(line.Substring(32, 2));
                }
                else
                {
                    light.x = int.Parse(line.Substring(11, 6));
                    light.y = int.Parse(line.Substring(19, 6));
                    light.v_x = int.Parse(line.Substring(37, 2));
                    light.v_y = int.Parse(line.Substring(41, 2));
                }

                lights.Add(light);
            }

            int seconds = 100000;
            
            for (int t = 0; t < seconds; t++)
            {
                Console.WriteLine(t);
                Light checklight = lights[0];
                if (HasNeighbour(checklight, lights))
                {
                    DrawCanvas(checklight.y - 100, checklight.y + 100, checklight.x - 100, checklight.x + 100, lights);
                }

                foreach (Light light in lights)
                {
                    light.x += light.v_x;
                    light.y += light.v_y;
                }
            }

            Console.WriteLine("end of code");
            Console.Read();
        }

        private static bool HasNeighbour(Light light, List<Light> lights)
        {
            // check north / south
            var north = lights.Any(l => l.x == light.x && l.y == light.y - 1);
            var south = lights.Any(l => l.x == light.x && l.y == light.y + 1);

            // check west / east
            var east = lights.Any(l => l.y == light.y && l.x == light.x - 1);
            var west = lights.Any(l => l.y == light.y && l.x == light.x + 1);

            return north || south || east || west;
        }

        private static void DrawCanvas(int min_y, int max_y, int min_x, int max_x, List<Light> lights)
        {
            for (int y = min_y; y < max_y; y++)
            {
                for (int x = min_x; x < max_x; x++)
                {
                    if (lights.Any(l => l.x == x && l.y == y))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }

                Console.WriteLine();
            }
        }
    }

    internal class Light
    {
        public int x;
        public int y;
        public int v_x;
        public int v_y;
    }

}
