using System;
using System.Collections.Generic;
using System.Linq;

namespace advent_console._15
{
    public class Unit
    {
        private readonly char[,] _map;
        private readonly List<Unit> _units;
        private const char FREE = '.';

        public char Type { get; }
        private char Enemy { get; }
        public int Hitpoints { get; set; }
        public int AttackPower { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public Unit(char type, int x, int y, int ap, ref char[,] map, ref List<Unit> units)
        {
            Type = type;
            Enemy = Type == 'E' ? 'G' : 'E';
            X = x;
            Y = y;
            Hitpoints = 200;
            AttackPower = ap;
            _map = map;
            _units = units;
        }

        internal bool IsInCombatRange()
        {
            return Up() == Enemy || Left() == Enemy || Down() == Enemy || Right() == Enemy;
        }

        private bool HasFreeSpace()
        {
            return Up() == FREE || Left() == FREE || Down() == FREE || Right() == FREE;
        }

        private char Right()
        {
            Unit unit = _units.FirstOrDefault(u => u.X == X + 1 && u.Y == Y);
            return unit?.Type ?? _map[X + 1, Y];
        }

        private char Down()
        {
            Unit unit = _units.FirstOrDefault(u => u.X == X && u.Y == Y + 1);
            return unit?.Type ?? _map[X, Y + 1];
        }

        private char Left()
        {
            Unit unit = _units.FirstOrDefault(u => u.X == X - 1 && u.Y == Y);
            return unit?.Type ?? _map[X - 1, Y];
        }

        private char Up()
        {
            Unit unit = _units.FirstOrDefault(u => u.X == X && u.Y == Y - 1);
            return unit?.Type ?? _map[X, Y - 1];
        }

        internal Unit AcquireCombatTarget()
        {
            // get unit in range
            List<Unit> rangeUnits = new List<Unit>();
            if (Up() == Enemy)
            {
                rangeUnits.Add(_units.First(u => u.X == X && u.Y == Y - 1));
            }

            if (Left() == Enemy)
            {
                rangeUnits.Add(_units.First(u => u.X == X - 1 && u.Y == Y));
            }

            if (Down() == Enemy)
            {
                rangeUnits.Add(_units.First(u => u.X == X && u.Y == Y + 1));
            }

            if (Right() == Enemy)
            {
                rangeUnits.Add(_units.First(u => u.X == X + 1 && u.Y == Y));
            }

            // get unit with lowest hitpoints and order by read order
            if (rangeUnits.Count <= 1)
            {
                return rangeUnits[0];
            }

            {
                // we have multiple targets in range -> sort by hitpoints
                IOrderedEnumerable<Unit> ordered = rangeUnits.OrderBy(u => u.Hitpoints).ThenBy(u => u.Y).ThenBy(u => u.X);
                return ordered.First();
            }
        }

        internal bool Move(bool verbose)
        {
            // generate basic map
            int[,] basicmap = GenerateBasicMap(_map, _units);

            // get all free enemy targets
            IEnumerable<Unit> freetargets = _units.Where(u => u.HasFreeSpace() && u.Type == Enemy);
            List<Unit> targets = freetargets.ToList();
            if (!targets.Any())
            {
                return false; // because no enemies with free space around
            }

            if (verbose)
            {
                Console.WriteLine($"There are {targets.Count()} possible targets");
            }

            // get all coordinates of squares next to free targets
            List<Coord> coords = new List<Coord>();
            foreach (Unit enemy in targets)
            {
                coords.AddRange(enemy.GetFreeCoords());
            }

            if (verbose)
            {
                Console.WriteLine($"There are {coords.Count()} possible coordinations");
            }

            // djsktra approach
            // fill map with steps until targets are reached
            Coord winner = FillMapWithSteps(coords, ref basicmap);
            if (winner == null)
            {
                Console.WriteLine("Cant reach any of it");
                return false;
            }


            // okay got a winner, now need a path to it
            LinkedList<Coord> path = PathToWinner(winner, ref basicmap);

            // so move
            X = path.First.Value.X_Coord;
            Y = path.First.Value.Y_Coord;

            if (verbose)
            {
                Console.WriteLine($"Done");
            }

            return true;
        }

        private LinkedList<Coord> PathToWinner(Coord winner, ref int[,] basicmap)
        {
            LinkedList<Coord> path = new LinkedList<Coord>();
            int steps = winner.Distance - 1;
            Coord current = winner;
            Coord next = null;

            while (steps > 0)
            {
                List<Coord> adj_fields = GetAdjacentFields(current.X_Coord, current.Y_Coord, steps, ref basicmap);
                next = adj_fields.Where(c => c.Value == steps).OrderBy(c => c.Y_Coord).ThenBy(c => c.X_Coord).First();
                path.AddFirst(next);
                current = next;
                steps--;
            }

            // add target
            path.AddLast(winner);

            Console.Write("Path to enemy: ");
            foreach (Coord step in path)
            {
                Console.Write("[" + step.X_Coord + "," + step.Y_Coord + "], ");
            }
            Console.WriteLine(winner.Distance + " steps are needed.");

            return path;
        }

        private List<Coord> GetAdjacentFields(int x, int y, int steps, ref int[,] basicmap)
        {
            List<Coord> adj_fields = new List<Coord>
            {
                GetAdjacentField(x, y - 1, ref basicmap),
                GetAdjacentField(x - 1, y, ref basicmap),
                GetAdjacentField(x + 1, y, ref basicmap),
                GetAdjacentField(x, y + 1, ref basicmap)
            };
            return adj_fields;
        }

        private Coord GetAdjacentField(int x, int y, ref int[,] map)
        {
            // check up
            int value = map[x, y];
            return new Coord { X_Coord = x, Y_Coord = y, Value = value };
        }

        private Coord FillMapWithSteps(List<Coord> coords, ref int[,] basicmap)
        {
            // designate targets
            foreach (Coord coord in coords)
            {
                basicmap[coord.X_Coord, coord.Y_Coord] = int.MaxValue;
            }

            // now i have a map full with -1 as barriers, max value as targets, and the rest are 0's
            // recursion give me headaches but lets go

            // current is the start point
            Coord current = new Coord { X_Coord = X, Y_Coord = Y };
            basicmap[X, Y] = int.MinValue;

            bool targetfound = false;
            List<Coord> targets = new List<Coord>();
            List<Coord> steps = new List<Coord> { current };

            int step = 1;
            while (!targetfound)
            {
                List<Coord> next_steps = new List<Coord>();

                foreach (Coord field in steps)
                {
                    SetAdjacentFields(field, step, ref basicmap, ref targetfound, ref targets, ref next_steps);
                }

                if (targetfound)
                {
                    return targets.OrderBy(c => c.Y_Coord).ThenBy(c => c.X_Coord).First();
                }
                else
                {
                    if (next_steps.Count == 0)
                    {
                        return null;
                    }
                }

                steps = next_steps;
                step++;
            }

            return null;
        }

        private void SetAdjacentFields(Coord current, int step, ref int[,] map, ref bool targetfound, ref List<Coord> targets, ref List<Coord> steps)
        {
            // up
            SetAdjacentField(current.X_Coord, current.Y_Coord - 1, step, ref map, ref targetfound, ref targets, ref steps);

            // left
            SetAdjacentField(current.X_Coord - 1, current.Y_Coord, step, ref map, ref targetfound, ref targets, ref steps);

            // right
            SetAdjacentField(current.X_Coord + 1, current.Y_Coord, step, ref map, ref targetfound, ref targets, ref steps);

            // down
            SetAdjacentField(current.X_Coord, current.Y_Coord + 1, step, ref map, ref targetfound, ref targets, ref steps);
        }

        private void SetAdjacentField(int x, int y, int step, ref int[,] map, ref bool targetfound,
            ref List<Coord> targets, ref List<Coord> steps)
        {
            // check up
            int value = map[x, y];
            if (value == int.MinValue)
            {
                // this is the startpoint -> ignore
            }
            else if (value == int.MaxValue)
            {
                // found a target :D -> do something.
                targetfound = true;
                targets.Add(new Coord { Distance = step, X_Coord = x, Y_Coord = y });
            }
            else if (value == -1)
            {
                // found a barrier -> ignore
            }
            else if (value == 0)
            {
                // set step because never touched
                map[x, y] = step;
                steps.Add(new Coord { X_Coord = x, Y_Coord = y });
            }
            else
            {
                // step already set -> ignore
            }
        }

        private static int[,] GenerateBasicMap(char[,] map, IReadOnlyCollection<Unit> units)
        {
            int[,] basemap = new int[map.GetLength(0), map.GetLength(1)];
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Unit unit = units.FirstOrDefault(u => u.X == x && u.Y == y);
                    if (unit != null)
                    {
                        basemap[x, y] = -1;
                    }
                    else
                    {
                        if (map[x, y] == '#')
                        {
                            basemap[x, y] = -1;
                        }
                    }
                }
            }

            return basemap;
        }

        private IEnumerable<Coord> GetFreeCoords()
        {
            List<Coord> coords = new List<Coord>();

            if (Up() == FREE)
            {
                coords.Add(new Coord { X_Coord = X, Y_Coord = Y - 1 });
            }

            if (Left() == FREE)
            {
                coords.Add(new Coord { X_Coord = X - 1, Y_Coord = Y });
            }

            if (Down() == FREE)
            {
                coords.Add(new Coord { X_Coord = X, Y_Coord = Y + 1 });
            }

            if (Right() == FREE)
            {
                coords.Add(new Coord { X_Coord = X + 1, Y_Coord = Y });
            }

            return coords;
        }
    }
}