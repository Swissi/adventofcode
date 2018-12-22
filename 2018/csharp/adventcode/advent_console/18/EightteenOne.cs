using System;
using System.Diagnostics;
using System.IO;

namespace advent_console._18
{
    internal class EightteenOne : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("18/i.txt");

            int x_max = lines[0].Length;
            int y_max = lines.Length;

            int[,] map = new int[x_max, y_max];

            for (int y = 0; y < y_max; y++)
            {
                for (int x = 0; x < x_max; x++)
                {
                    switch (lines[y][x])
                    {
                        case '.':
                            map[x, y] = 0;
                            break;
                        case '|':
                            map[x, y] = 1;
                            break;
                        case '#':
                            map[x, y] = 2;
                            break;
                    }
                }
            }

            int minutes = 1000000000;
            //int minutes = 10;
            int[,] current_map = map;


            DisplayMap(map);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            

            for (int i = 0; i < minutes; i++)
            {
                int[,] next_map = new int[x_max, y_max];

                for (int y = 0; y < y_max; y++)
                {
                    for (int x = 0; x < x_max; x++)
                    {
                        next_map[x, y] = Calculate(current_map, x, y, x_max, y_max);
                    }
                }

                //DisplayMap(current_map);
                //Console.ReadLine();
                current_map = next_map;

                if (i>0 && i % 100000 == 0)
                {
                    Console.Clear();
                    Console.WriteLine(i);

                    double pct = (double)i / (double)minutes;
                    var multi = minutes / i;

                    var elapsed = sw.ElapsedMilliseconds;
                    var totaltime = sw.ElapsedMilliseconds * multi;

                    var remaining = totaltime - elapsed;


                    Console.WriteLine("it took " + sw.ElapsedMilliseconds + "ms");
                    Console.WriteLine("total time " + totaltime/60000 + " minutes");
                    Console.WriteLine("time remaining " + remaining / 60000 + " minutes");
                    Console.WriteLine($"{pct.ToString("0.00%")} % calculated");

                    
                }

            }

            map = current_map;

            DisplayMap(map);

            int trees = 0;
            int lumberyards = 0;
            for (int y = 0; y < y_max; y++)
            {
                for (int x = 0; x < x_max; x++)
                {
                    if (map[x, y] == 1)
                    {
                        trees++;
                    }
                    if (map[x, y] == 2)
                    {
                        lumberyards++;
                    }
                }
            }

            Console.WriteLine(trees + " trees");
            Console.WriteLine(lumberyards + " lumberyards");
            Console.WriteLine("score=" + trees * lumberyards);

        }

        private int Calculate(int[,] map, int x, int y, int x_max, int y_max)
        {
            switch (map[x, y])
            {
                case 0:
                    if (HasAmount(map, 1, 3, x, y, x_max, y_max))
                    {
                        return 1;
                    }
                    return 0;
                case 1:
                    if (HasAmount(map, 2, 3, x, y, x_max, y_max))
                    {
                        return 2;
                    }

                    return 1;
                case 2:
                    if (HasAmount(map, 2, 1, x, y, x_max, y_max) && HasAmount(map, 1, 1, x, y, x_max, y_max))
                    {
                        return 2;
                    }

                    return 0;
            }

            throw new Exception("something not right here");
        }

        private bool HasAmount(int[,] map, int search_type, int amount, int x, int y, int x_max, int y_max)
        {
            int trees = 0;
            int lumberyards = 0;
            int open = 0;

            int value = y - 1 < 0 || x - 1 < 0 ? -1 : map[x - 1, y - 1];
            
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = y - 1 < 0 ? -1 : map[x, y - 1];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = (y - 1 < 0) || (x + 1 > x_max - 1) ? -1 : map[x + 1, y - 1];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = y + 1 > y_max - 1 || x - 1 < 0 ? -1 : map[x - 1, y + 1];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = y + 1 > y_max - 1 ? -1 : map[x, y + 1];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = y + 1 > y_max - 1 || x + 1 > x_max - 1 ? -1 : map[x + 1, y + 1];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = x - 1 < 0 ? -1 : map[x - 1, y];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }
            value = x + 1 > x_max - 1 ? -1 : map[x + 1, y];
            switch (value)
            {
                case 0:
                    open++;
                    break;
                case 1:
                    trees++;
                    break;
                case 2:
                    lumberyards++;
                    break;
            }
            if (value != -1)
            {
                switch (search_type)
                {
                    case 0:
                        if (open >= amount)
                        {
                            return true;
                        }

                        break;
                    case 1:
                        if (trees >= amount)
                        {
                            return true;
                        }

                        break;
                    case 2:
                        if (lumberyards >= amount)
                        {
                            return true;
                        }

                        break;
                }
            }



            return false;
        }

        private void DisplayMap(int[,] map)
        {
            int x_max = map.GetLength(1);
            int y_max = map.GetLength(0);

            for (int y = 0; y < y_max; y++)
            {
                for (int x = 0; x < x_max; x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
