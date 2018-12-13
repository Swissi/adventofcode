using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace advent_console._13
{
    class thirteen_one : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("13/i.txt");
            bool draw = false;
            char[,] map = new char[lines.First().Length,lines.Length];
            List<Cart> carts = new List<Cart>();
            int y = 0;
            foreach (var line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    var current = line[x];
                    switch (line[x])
                    {
                        case '<':
                            carts.Add(new Cart(Direction.West, x,y));
                            map[x, y] = '-';
                            break;
                        case '^':
                            carts.Add(new Cart(Direction.North, x, y));
                            map[x, y] = '|';
                            break;
                        case '>':
                            carts.Add(new Cart(Direction.East, x, y));
                            map[x, y] = '-';
                            break;
                        case 'v':
                            carts.Add(new Cart(Direction.South, x, y));
                            map[x, y] = '|';
                            break;
                        default:
                            map[x, y] = line[x];
                            break;
                    }
                }
                y++;
            }

            bool crash = false;
            int tick = 0;
            Console.WriteLine("tick: " + tick);
            DrawMapAndCheckCrash(map, carts, draw);
            //Console.ReadLine();
            tick = 1;
            
            while (carts.Count > 1 && tick < 100000)
            {
                foreach (var cart in carts.OrderBy(c => c.X).OrderBy(c => c.Y))
                {
                    cart.Move(map, carts);
                }

                //Console.Clear();
                //Console.WriteLine("tick: " + tick);
                crash = DrawMapAndCheckCrash(map, carts, draw);
                //Console.WriteLine();
                if (crash)
                {
                    //Console.ReadLine();
                    Console.WriteLine(tick + ": crash happened. carts remaining: " + carts.Count());
                }
                tick++;
                //Thread.Sleep(1000);
                //Console.ReadLine();
            }

            Console.WriteLine(carts.First().X + "," + carts.First().Y);

        }

        private bool DrawMapAndCheckCrash(char[,] map, List<Cart> carts, bool draw)
        {
            StringBuilder sb = new StringBuilder();
            bool crash = false;
            var x_crash = 0;
            var y_crash = 0;
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (carts.Where(c => c.X == x && c.Y == y).Count() == 2)
                    {
                        if (draw) { 
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            Console.ResetColor();
                        }
                        crash = true;
                        x_crash = x;
                        y_crash = y;
                        continue;
                    }
                    else
                    {
                        var cart = carts.FirstOrDefault(c => c.X == x && c.Y == y);
                        if (cart != null)
                        {
                            if (draw)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(cart.DrawDirection());
                                Console.ResetColor();
                            }
                            
                        }
                        else
                        {
                            if (draw)
                            {
                                Console.Write(map[x, y]);
                            }
                            
                        }
                    }                    
                }

                if (draw)
                {
                    Console.WriteLine();
                }
            }

            if (crash)
            {
                Console.WriteLine("Crash happened at " + x_crash + ", " + y_crash);
            }

            Console.Write(sb);
            return crash;
        }
    }

    class Cart
    {
        public Direction Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Turn LastTurn { get; set; }

        public Cart(Direction dir, int x, int y)
        {
            Direction = dir;
            X = x;
            Y = y;
            LastTurn = Turn.Right;
        }

        public Turn GetNextTurn()
        {
            switch (LastTurn)
            {
                case Turn.Right:
                    LastTurn = Turn.Left;
                    break;
                case Turn.Left:
                    LastTurn = Turn.Straight;
                    break;
                case Turn.Straight:
                    LastTurn = Turn.Right;
                    break;
            }

            return LastTurn;
        }

        internal string DrawDirection()
        {
            string draw = "";
            switch (Direction)
            {
                case Direction.West:
                    draw = "<";
                    break;
                case Direction.North:
                    draw = "^";
                    break;
                case Direction.East:
                    draw = ">";
                    break;
                case Direction.South:
                    draw = "v";
                    break;

            }

            return draw;
        }

        internal void Move(char[,] map, List<Cart> carts)
        {
            // Get new coordinate
            SetNewCoordinates();

            // Check Crash Situation
            CheckCrash(carts);

            // Check for new Directions in case of curve or intersection
            SetNewDirection(map);
        }

        private void CheckCrash(List<Cart> carts)
        {
            if (carts.Where(c => c.X == X && c.Y == Y).Count() > 1)
            {
                
                carts.RemoveAll(c => c.X == X && c.Y == Y);
                Console.WriteLine("Crash happens at " + X + ", " + Y + ". Remaining Carts: " + carts.Count);
            }
        }

        private void SetNewDirection(char[,] map)
        {
            Direction currentDir = Direction;
            var currenttrack = map[X, Y];

            switch (currenttrack)
            {
                case '\\':
                    switch (Direction)
                    {
                        case Direction.West:
                            Direction = Direction.North;
                            break;
                        case Direction.North:
                            Direction = Direction.West;
                            break;
                        case Direction.East:
                            Direction = Direction.South;
                            break;
                        case Direction.South:
                            Direction = Direction.East;
                            break;
                    }
                    break;
                case '/':
                    switch (Direction)
                    {
                        case Direction.West:
                            Direction = Direction.South;
                            break;
                        case Direction.North:
                            Direction = Direction.East;
                            break;
                        case Direction.East:
                            Direction = Direction.North;
                            break;
                        case Direction.South:
                            Direction = Direction.West;
                            break;
                    }
                    break;
                case '+':
                    Turn next = GetNextTurn();
                    switch (next)
                    {
                        case Turn.Right:
                            switch (Direction)
                            {
                                case Direction.West:
                                    Direction = Direction.North;
                                    break;
                                case Direction.North:
                                    Direction = Direction.East;
                                    break;
                                case Direction.East:
                                    Direction = Direction.South;
                                    break;
                                case Direction.South:
                                    Direction = Direction.West;
                                    break;
                            }
                            break;
                        case Turn.Left:
                            switch (Direction)
                            {
                                case Direction.West:
                                    Direction = Direction.South;
                                    break;
                                case Direction.North:
                                    Direction = Direction.West;
                                    break;
                                case Direction.East:
                                    Direction = Direction.North;
                                    break;
                                case Direction.South:
                                    Direction = Direction.East;
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void SetNewCoordinates()
        {
            switch (Direction)
            {
                case Direction.West:
                    X--;
                    break;
                case Direction.North:
                    Y--;
                    break;
                case Direction.East:
                    X++;
                    break;
                case Direction.South:
                    Y++;
                    break;
            }
        }

        internal void CheckCrash(char[,] map, List<Cart> carts)
        {
            int newx = X;
            int newy = Y;
            switch (Direction)
            {
                case Direction.West:
                    newx--;
                    break;
                case Direction.North:
                    newy--;
                    break;
                case Direction.East:
                    newx++;
                    break;
                case Direction.South:
                    newy++;
                    break;
            }

            var cart = carts.FirstOrDefault(c => c.X == newx && c.Y == newy);
            if (cart != null)
            {
                Console.WriteLine("Crash at: " + newx + "," + newy);
            }
        }
    }

    enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    enum Turn
    {
        Left = 0,
        Straight = 1,
        Right = 2
    }
}
