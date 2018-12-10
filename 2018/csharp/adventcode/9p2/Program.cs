using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _9p2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read unsorted file
            string[] lines = File.ReadAllLines("i.txt");

            foreach (string line in lines)
            {
                //9 players; last marble is worth 25 points: high score is 32
                var arr = line.Split(' ');
                long[] players = new long[long.Parse(arr[0])];

                int marbles = int.Parse(arr[6]);
                int score = 0;

                if (line.Contains("score"))
                {
                    score = int.Parse(arr[11]);
                }

                LinkedList<int> circle = new LinkedList<int>();

                circle.AddLast(0);
                PrintTurn(circle, 0, "-", 0);

                circle.AddLast(1);
                PrintTurn(circle, 1, "0", 1);

                int marble = 2;
                int turn = 2;
                int player = 1;
                var last_current = circle.Last;

                while (marble <= marbles)
                {
                    //int last_current_index = circle.Find(last_current).Value;

                    if (marble % 23 == 0)
                    {
                        players[player] += marble;
                        LinkedListNode<int> special_marble = last_current;
                        for (int i = 0; i < 7; i++)
                        {
                            special_marble = special_marble.Previous ?? circle.Last;
                        }
                        

                        players[player] += special_marble.Value;
                        last_current = special_marble.Next;
                        circle.Remove(special_marble);
                        PrintTurn(circle, turn, player.ToString(), last_current.Value);
                        marble++;
                    }
                    else
                    {
                        var desired_marble = last_current.Next ?? circle.First;
                        last_current = circle.AddAfter(desired_marble, marble);
                        PrintTurn(circle, turn, player.ToString(), last_current.Value);
                        marble++;
                        turn++;
                    }

                    if (player < players.Length - 1)
                    {
                        player++;
                    }
                    else
                    {
                        player = 0;
                    }
                }

                players = players.OrderByDescending(i => i).ToArray();


                Console.WriteLine($"high score = {players.First()} should be {score}");

            }

            Console.Read();
        }

        private static void PrintTurn(LinkedList<int> circle, int turn, string player, int current)
        {
            return;
            Console.Write($"[{turn}] [{player}] ");
            foreach (var node in circle)
            {
                if (node == current)
                {
                    Console.Write("(" + node + ") ");
                }
                else
                {
                    Console.Write(node + " ");
                }
            }
            Console.WriteLine();
        }
    }
}
