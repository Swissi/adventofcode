using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace advent_console._17
{
    internal class SeventeenOne : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("17/i.txt");

            List<int> x = new List<int>();
            List<int> y = new List<int>();
            foreach (string line in lines)
            {
                //x=495, y=2..7
                string[] arr = line.Split(", ");


                //x = 495
                string left = arr[0];
                int value1 = int.Parse(left.Split('=')[1]);
                if (left.StartsWith("x"))
                {
                    x.Add(value1);
                }
                else
                {
                    y.Add(value1);
                }

                //y=2..7
                string right = arr[1];
                int[] range = right.Split('=')[1].Split("..").Select(c => int.Parse(c)).ToArray();
                if (right.TrimStart().StartsWith("x"))
                {
                    for (int i = range[0]; i <= range[1]; i++)
                    {
                        x.Add(i);
                    }
                }
                else
                {
                    for (int i = range[0]; i <= range[1]; i++)
                    {
                        y.Add(i);
                    }
                }
            }

            // map range
            int y_max = y.Max();
            int x_min = x.Min();
            int x_max = x.Max();

            char[,] map = new char[x_max + 2, y_max + 1];

            // add water spring
            map[500, 0] = '+';

            foreach (string line in lines)
            {
                //x=495, y=2..7
                string[] arr = line.Split(", ");

                bool x_y = false;
                //x = 495
                string left = arr[0];
                int value1 = int.Parse(left.Split('=')[1]);
                if (left.StartsWith("x"))
                {
                    x_y = true;
                }

                //y=2..7
                string right = arr[1];
                int[] range = right.Split('=')[1].Split("..").Select(c => int.Parse(c)).ToArray();
                if (x_y)
                {
                    for (int i = range[0]; i <= range[1]; i++)
                    {
                        map[value1, i] = '#';
                    }
                }
                else
                {
                    for (int i = range[0]; i <= range[1]; i++)
                    {
                        map[i, value1] = '#';
                    }
                }
            }

            // Display map
            //DisplayMap(map);

            // the stage is set
            // current ist the spring
            List<Water> settled_water = new List<Water>();
            List<Water> moving_water = new List<Water>();
            int source_x = 500;
            int source_y = 0;
            int counter = 0;


            while (true) // while max y level is not reached
            {
                if (settled_water.Any(l => l.X == source_x && l.Y == source_y))
                {
                    Water source = settled_water.First(l => l.X == source_x && l.Y == source_y);
                    settled_water.Remove(source);
                    break; // no space for additional water
                }
                else
                {
                    if (counter % 2 == 0)
                    {
                        // every 2 cycles add a water
                        Water water = new Water(map, ref settled_water);
                        moving_water.Add(water);
                    }

                    foreach (Water water in moving_water.ToList())
                    {
                        if (!water.Move()) // if water cant move, add it to the settled list.
                        {
                            water.Settled = true;
                            moving_water.Remove(water);
                            settled_water.Add(water);
                        }
                    }

                    // just some debug info every 100 steps
                    if (counter % 100 == 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Moving: {moving_water.Count}");
                        Console.WriteLine($"Settled: {settled_water.Count}");
                    }

                    counter++;
                }
            }
            // takes some time to draw the map -> commented
            //DisplayMap(map);
            Console.WriteLine($"{settled_water.Count} water drops created");
            Console.WriteLine($"min={y.Min()} max={y.Max()}");
            Console.WriteLine($"{settled_water.Where(w => w.Y >= y.Min() && w.Y <= y.Max()).Count()}");
            Console.WriteLine($"{moving_water.Count} water drops still moving?");


            // Part two
            CalculateStandingWater(map, y.Min(), y.Max());
        }

        private void CalculateStandingWater(char[,] map, int y_min, int y_max)
        {
            int count = 0;
            for (int y = y_min; y < y_max; y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == 'W' && IsBetweenWalls(map,x,y))
                    {
                        count++;
                    }
                }
            }

            Console.WriteLine(count + " drops are standing");
        }

        private bool IsBetweenWalls(char[,] map, int x, int y)
        {
            bool leftwall = false;
            bool rightwall = false;

            int temp_x = x;

            // check right
            temp_x++;
            while (true)
            {
                char type = map[temp_x, y];

                if (type == ' ' || type == 'E')
                {
                    return false;
                }
                
                if (type == 'W')
                {
                    temp_x++;
                    continue;
                }

                if (type == '#')
                {
                    rightwall = true;
                    temp_x = x;
                    break;
                }
            }

            // check left
            temp_x--;
            while (true)
            {
                char type = map[temp_x, y];

                if (type == ' ' || type == 'E')
                {
                    return false;
                }

                if (type == 'W')
                {
                    temp_x--;
                    continue;
                }

                if (type == '#')
                {
                    leftwall = true;
                    temp_x = x;
                    break;
                }
            }

            return leftwall && rightwall;
        }

        public static void DisplayMap(char[,] map, Water water = null)
        {
            int x_min = 0;
            int y_min = 0;

            int x_max = map.GetLength(0);
            int y_max = map.GetLength(1);

            if (water != null)
            {
                int range = 20;
                x_min = (water.X - range) < x_min ? x_min : (water.X - range);
                x_max = (water.X + range) > x_max ? x_max : (water.X + range);
                y_min = (water.Y - range) < y_min ? y_min : (water.Y - range);
                y_max = (water.Y + range) > y_max ? y_max : (water.Y + range);
            }

            for (int y = y_min; y < y_max; y++)
            {
                for (int x = x_min; x < x_max; x++)
                {
                    if (map[x, y] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write('#');
                        Console.ResetColor();
                        continue;
                    }

                    if (map[x,y] == 'W')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('W');
                        Console.ResetColor();
                        continue;
                    }

                    if (map[x, y] == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write('E');
                        Console.ResetColor();
                        continue;
                    }

                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }

    internal class Water
    {
        public int X { get; set; }
        public int Y { get; set; }

        private List<Water> ledger;
        private readonly char[,] map;

        public Water(char[,] _map, ref List<Water> _ledger)
        {
            ledger = _ledger;
            map = _map;
            X = 500;
            Y = 0;
        }

        public bool Settled { get; set; }

        private bool wentdown;
        private bool wentleft;
        private bool wentright;

        internal bool Move()
        {
            if (IsEnd(X,Y+1))
            {
                map[X, Y] = 'E';
                return false;
            }

            // if down is free...lets do gravity
            if (IsEmpty(X, Y + 1))
            {

                // fast forward downfall
                int temp_x = X;
                int temp_y = Y + 1;
                while (temp_y < map.GetLength(1) && IsEmpty(temp_x, temp_y))
                {
                    temp_y++;
                }
                
                map[X, Y] = '\0';
                Y = temp_y - 1;
                map[X, Y] = 'W';
                wentdown = true;
                wentleft = false;
                wentright = false;

                
                if (Y+1 == map.GetLength(1)) // check if its at the end
                {
                    map[X, Y] = 'E';
                    return false;
                }

                if (IsWater(X, Y + 1) && (AnyWaterFallLeft() || AnyWaterFallRight())) // tricky check. if multiple waterfalls are detected, treat is as end-state
                {
                    map[X, Y] = 'E';
                    return false;
                }

                return true;
            }
            else
            {
                if (wentdown)
                {
                    if (IsEmpty(X - 1, Y)) // Left first
                    {
                        map[X, Y] = '\0';
                        X--;
                        map[X, Y] = 'W';

                        wentleft = true;
                        wentright = false;
                        wentdown = false;
                        return true;
                    }
                    else if (IsEmpty(X + 1, Y)) // Right second
                    {
                        map[X, Y] = '\0';
                        X++;
                        map[X, Y] = 'W';
                        wentright = true;
                        wentleft = false;
                        wentdown = false;


                        return true;
                    }

                    if (IsEnd(X-1,Y))
                    {
                        map[X, Y] = 'E';
                    }
                    if (IsEnd(X+1,Y))
                    {
                        map[X, Y] = 'E';
                    }
                }
                else
                {
                    if (wentleft) // if the drop went left, keep going left
                    {
                        if (IsEmpty(X - 1, Y))
                        {
                            map[X, Y] = '\0';
                            X--;
                            map[X, Y] = 'W';
                            return true;
                        }
                    }

                    if (wentright) // if the drop went right, keep going right
                    {
                        if (IsEmpty(X + 1, Y))
                        {
                            map[X, Y] = '\0';
                            X++;
                            map[X, Y] = 'W';
                            return true;
                        }
                    }

                    if (IsEnd(X - 1, Y)) // if neighbour is end-state, declare this as endstate too
                    {
                        map[X, Y] = 'E';
                    }
                    if (IsEnd(X + 1, Y))
                    {
                        map[X, Y] = 'E';
                    }
                }
            }

            return false;
        }

        private bool AnyWaterFallRight()
        {
            int temp_x = X + 1;
            while (temp_x < map.GetLength(0) - 1 && IsWater(temp_x,Y+1))
            {
                
                if (IsWall(temp_x, Y))
                {
                    return false;
                }

                if (IsWater(temp_x, Y) && IsWater(temp_x, Y-1))
                {
                    return true;
                }

                temp_x++;
            }

            return false;
        }

        private bool AnyWaterFallLeft()
        {
            int temp_x = X-1;
            while (temp_x > 0 && IsWater(temp_x, Y + 1))
            {
                if (IsWall(temp_x, Y))
                {
                    return false;
                }

                if (IsWater(temp_x, Y) && IsWater(temp_x, Y - 1))
                {
                    return true;
                }

                temp_x--;
            }

            return false;
        }

        private bool IsEnd(int x, int y)
        {
            return map[x, y] == 'E';
        }

        private bool IsWall(int x, int y)
        {
            return map[x, y] == '#';
        }

        private bool IsWater(int x, int y)
        {
            return map[x, y] == 'W' || map[x, y] == 'E';
        }

        private bool IsEmpty(int x, int y)
        {
            return map[x, y] == '\0';
        }
    }
}
