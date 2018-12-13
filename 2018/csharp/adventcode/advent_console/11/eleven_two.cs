using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace advent_console._11
{
    class eleven_two : IPart
    {
        public void DoIt()
        {
            var lines = File.ReadAllLines("11/i.txt");
            var grid_x = 600;
            var grid_y = 600;
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

            Dictionary<string, int> pxlevels = new Dictionary<string, int>();

            Console.WriteLine("Calculating 3x3 Levels..");

            foreach (int i in Enumerable.Range(1, 300))
            {
                Console.WriteLine("i:" + i);
                foreach (int y in Enumerable.Range(1, 300))
                {
                    foreach (int x in Enumerable.Range(1, 300))
                    {
                        pxlevels.Add(x + "," + y + "," + i, GetSqPowerLevel(plevels, x, y, i));
                    }
                }
            }

            var highestp3 = pxlevels.OrderByDescending(o => o.Value).First();
            Console.WriteLine($"Highest 3x3 Powerlevel at {highestp3.Key}: {highestp3.Value}");
        }

        private int GetSqPowerLevel(int[,] plevels, int top_x, int top_y, int size)
        {
            int sum = 0;
            foreach (int y in Enumerable.Range(top_y, size))
            {
                foreach (int x in Enumerable.Range(top_x, size))
                {
                    sum += plevels[x - 1, y - 1];
                }
            }

            return sum;
        }
        
        private void TestPowerLevel(int x, int y, int i, int s)
        {
            Console.WriteLine($"Testing {x},{y} with input {i} should be {s} and is {CalcPowerLevel(x,y,i)}");
        }

        private int CalcPowerLevel(int x, int y, int inp)
        {
            if (x > 300 || y > 300)
            {
                return 0;
            }
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
