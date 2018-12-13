using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace advent_console._11
{
    class eleven_one : IPart
    {
        public void DoIt()
        {
            var lines = File.ReadAllLines("11/i.txt");
            var grid_x = Int32.Parse(lines[0].Split(',')[0]);
            var grid_y = Int32.Parse(lines[0].Split(',')[1]);
            var inp = Int32.Parse(lines[1]);
            int[,] plevels = new int[grid_x, grid_y];

            Console.WriteLine($"Starting with the grid: {grid_x}, {grid_y}. Input is: {inp}");

            foreach (int y in Enumerable.Range(1, grid_y))
            {
                foreach (int x in Enumerable.Range(1, grid_x))
                {
                    int plevel = CalcPowerLevel(x, y, inp);
                    plevels[x - 1, y - 1] = plevel;
                }
            }
            Console.WriteLine("PLevels calculated");

            // Test level calc
            TestPowerLevel(3, 5, 8, 4);
            TestPowerLevel(122, 79, 57, -5);
            TestPowerLevel(217, 196, 39, 0);
            TestPowerLevel(101, 153, 71, 4);

            Dictionary<string, int> p3levels = new Dictionary<string, int>();

            Console.WriteLine("Calculating 3x3 Levels..");
            foreach (int y in Enumerable.Range(1, grid_y))
            {
                foreach (int x in Enumerable.Range(1, grid_x))
                {
                    p3levels.Add(x+","+y, Get3x3PowerLevel(plevels, x, y));
                }
                
                Console.WriteLine("y:" + y);
            }

            var highestp3 = p3levels.OrderByDescending(o => o.Value).First();
            Console.WriteLine($"Highest 3x3 Powerlevel at {highestp3.Key}: {highestp3.Value}");
        }

        private int Get3x3PowerLevel(int[,] plevels, int top_x, int top_y)
        {
            int sum = 0;
            foreach (int y in Enumerable.Range(top_y, 3))
            {
                foreach (int x in Enumerable.Range(top_x, 3))
                {
                    sum += GetPL(plevels, x, y);
                }
            }

            return sum;
        }

        private int GetPL(int[,] plevels, int x, int y)
        {
            try
            {
                return plevels[x-1, y-1];
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void TestPowerLevel(int x, int y, int i, int s)
        {
            Console.WriteLine($"Testing {x},{y} with input {i} should be {s} and is {CalcPowerLevel(x,y,i)}");
        }

        private int CalcPowerLevel(int x, int y, int inp)
        {
            var id = x + 10;
            var start_level = id * y;
            var increase = start_level + inp;
            var end_level = increase * id;
            var hundred_digit = (int) Math.Abs(end_level / 100 % 10);
            var result = hundred_digit - 5;
            return result;
        }
    }
}
