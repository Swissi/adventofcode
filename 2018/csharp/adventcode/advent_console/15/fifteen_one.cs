using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent_console._15
{
    internal class FifteenOne : IPart
    {
        public void DoIt()
        {
            Dictionary<string, int> inputs_part1 = new Dictionary<string, int>
            {
                { "e", 27730 },
                { "e2", 36334 },
                { "e3", 39514 },
                { "e4", 27755 },
                { "e5", 28944 },
                { "e6", 18740 },
                { "i", 0 }
            };

            Dictionary<string, int> inputs_part2 = new Dictionary<string, int>
            {
                { "e", 4988 },
                { "e2", 0 },
                { "e3", 31284 },
                { "e4", 3478 },
                { "e5", 6474 },
                { "e6", 1140 },
                { "i", 0 }
            };

            List<int> results = new List<int>();
            List<int> aps = new List<int>();
            

            foreach (KeyValuePair<string, int> input in inputs_part2)
            {
                // Part 2: change base_ap behaviour if not wanted
                int base_ap = 3;
                bool elf_died = false;

                while (true)
                {
                    Console.WriteLine("AP is " + base_ap);
                    string[] lines = File.ReadAllLines($"15/{input.Key}.txt");

                    // setup
                    // - Read map
                    // - Read units
                    char[,] map = new char[lines[0].Length, lines.Length];
                    List<Unit> units = new List<Unit>();
                    for (int y = 0; y < lines.Length; y++)
                    {
                        for (int x = 0; x < lines[y].Length; x++)
                        {
                            char current = lines[y][x];
                            switch (current)
                            {
                                case '#':
                                case '.':
                                    map[x, y] = current;
                                    break;
                                case 'E':
                                    map[x, y] = '.';
                                    units.Add(new Unit(current, x, y, base_ap, ref map, ref units));
                                    break;
                                case 'G':
                                    map[x, y] = '.';
                                    units.Add(new Unit(current, x, y, 3, ref map, ref units));
                                    break;
                            }
                        }
                    }

                    DrawMap(map, null, units, "Initial");
                    //Console.ReadLine();

                    int block_counter = 0;
                    int round = 0;
                    bool nothing_to_kill = false;

                    bool debug = false;
                    bool livemode = !debug;
                    bool verbose = debug;
                    bool pauseeachround = debug;
                    bool pausebeforeround = debug;

                    while (true)
                    {
                        // each unit by reading order (top-bottom-left-right)
                        foreach (Unit unit in units.OrderBy(u => u.Y).ThenBy(u => u.X).ToList())
                        {
                            if (unit.Hitpoints < 1)
                            {
                                continue;
                            }

                            Unit target = null;
                            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - can be set via variable
                            if (verbose)
                            {
                                DrawMap(map, unit, units, round.ToString(), null, true);
                            }

                            if (pausebeforeround)
                            {
                                Console.ReadLine();
                            }


                            if (units.GroupBy(u => u.Type).Count() == 1)
                            {
                                if (verbose)
                                {
                                    Console.WriteLine("all enemies are dead");
                                }

                                nothing_to_kill = true;
                                break;
                            }
                            else
                            {
                                // is in combat range?
                                if (unit.IsInCombatRange())
                                {
                                    if (verbose)
                                    {
                                        Console.WriteLine("I am in combat range! Harrr. Shooting!");
                                    }

                                    target = unit.AcquireCombatTarget();
                                    target.Hitpoints = target.Hitpoints - unit.AttackPower;

                                    if (verbose)
                                    {
                                        Console.WriteLine($"target has now {target.Hitpoints} HP");
                                    }

                                    if (target.Hitpoints <= 0)
                                    {
                                        if (target.Type == 'E')
                                        {
                                            elf_died = true;
                                            break;
                                        }

                                        if (verbose)
                                        {
                                            Console.WriteLine($"target is killed.");
                                        }

                                        // target is killed
                                        units.Remove(target);
                                    }
                                }
                                else
                                {
                                    if (verbose)
                                    {
                                        Console.WriteLine("I need to move!");
                                    }

                                    // move
                                    if (unit.Move(verbose))
                                    {
                                        if (verbose)
                                        {
                                            Console.WriteLine("I moved");
                                        }

                                        if (unit.IsInCombatRange())
                                        {
                                            if (verbose)
                                            {
                                                Console.WriteLine("I am in combat range! Harrr. Shooting!");
                                            }

                                            target = unit.AcquireCombatTarget();
                                            target.Hitpoints = target.Hitpoints - unit.AttackPower;

                                            if (verbose)
                                            {
                                                Console.WriteLine($"target has now {target.Hitpoints} HP");
                                            }

                                            if (target.Hitpoints < 0)
                                            {
                                                if (verbose)
                                                {
                                                    Console.WriteLine($"target is killed.");
                                                }

                                                // target is killed
                                                units.Remove(target);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (verbose)
                                        {
                                            Console.WriteLine("I cant move... :(");
                                        }

                                        // no target -> blocked 
                                        block_counter++;
                                    }
                                }
                            }

                            if (verbose)
                            {
                                DrawMap(map, unit, units, round.ToString(), target);
                            }

                            if (pauseeachround)
                            {
                                Console.ReadLine();
                            }
                        }

                        if (livemode)
                        {
                            DrawMap(map, null, units, round.ToString(), null, true);
                            //Thread.Sleep(200);
                        }

                        // check if game is over
                        if (block_counter == units.Count || nothing_to_kill || elf_died)
                        {
                            break;
                        }

                        round++;
                        block_counter = 0;
                    }

                    

                    if (elf_died)
                    {
                        elf_died = false;
                        base_ap++;
                    }
                    else
                    {
                        int hps = units.Sum(u => u.Hitpoints);
                        results.Add(hps * round);
                        aps.Add(base_ap);
                        break;
                    }
                }
            }

            foreach (int result in results)
            {
                if (inputs_part2.ContainsValue(result))
                {
                    KeyValuePair<string, int> input = inputs_part2.First(i => i.Value == result);
                    Console.WriteLine($"Score={result}, should be {input.Value}.");
                }
                else
                {
                    Console.WriteLine($"Score={result}");
                }
            }

            foreach (var ap in aps)
            {
                Console.WriteLine(ap);
            }
        }

        private void DrawMap(char[,] map, Unit curUnit, IReadOnlyCollection<Unit> units, string v, Unit tarUnit = null,
            bool clear = false)
        {
            if (clear)
            {
                Console.Clear();
            }

            Console.WriteLine("ROUND: " + v);

            Console.Write(' ');
            Console.Write(' ');
            Console.Write(' ');


            int x_l = map.GetLength(0);
            if (x_l > 9)
            {
                for (int i = 0; i < x_l; i++)
                {
                    if (i > 9)
                    {
                        Console.Write(i / 10);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
            Console.WriteLine();

            Console.Write(' ');
            Console.Write(' ');
            Console.Write(' ');

            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.Write(i % 10);
            }
            Console.WriteLine();


            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (y < 10)
                {
                    Console.Write(' ');
                    Console.Write(y);
                    Console.Write(' ');
                }
                else
                {
                    Console.Write(y);
                    Console.Write(' ');
                }

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Unit unit = units.FirstOrDefault(u => u.X == x && u.Y == y);
                    if (unit != null)
                    {
                        // now determine color
                        if (curUnit != null && curUnit.X == x && curUnit.Y == y)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        if (tarUnit != null && tarUnit.X == x && tarUnit.Y == y)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }

                        Console.Write(unit.Type);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(map[x, y]);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"There are {units.Count} units on the field.");
            if (curUnit != null)
            {
                Console.WriteLine($"Current Unit is {curUnit.Type} and has {curUnit.Hitpoints} HP");
            }
        }
    }
}