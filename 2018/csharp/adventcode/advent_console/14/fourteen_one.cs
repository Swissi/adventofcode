using System;
using System.Collections.Generic;
using System.Linq;

namespace advent_console._14
{
    internal class fourteen_one : IPart
    {
        public void DoIt()
        {
            LinkedList<int> list = new LinkedList<int>();
            LinkedListNode<int> elf1 = list.AddFirst(3);
            LinkedListNode<int> elf2 = list.AddLast(7);

            string buffer_string = "37";

            Dictionary<string, string> findrecipes = new Dictionary<string, string>
            {
                {"51589", "9"},
                {"01245", "5"},
                {"92510", "18"},
                {"59414", "2018"},
                {"824501", ""}
            };

            var index = 0;
            for (int i = 0; findrecipes.Count > 0; i++)
            {
                
                // create
                int sum = elf1.Value + elf2.Value;
                if (sum > 9)
                {
                    list.AddLast(1);
                    buffer_string += "1";
                    list.AddLast(sum - 10);
                    buffer_string += list.Last.Value;
                }
                else
                {
                    list.AddLast(sum);
                    buffer_string += list.Last.Value;
                }

                // move
                MoveElf(ref elf1);
                MoveElf(ref elf2);

                if (buffer_string.Length > 10000)
                {
                    if (findrecipes.Any(f => buffer_string.Contains(f.Key)))
                    {
                        var sublist = findrecipes.Where(f => buffer_string.Contains(f.Key)).ToList();

                        foreach (var entry in sublist)
                        {
                            // find entry in list
                            var sub_index = buffer_string.IndexOf(entry.Key);
                            Console.Write($"{entry} found in last string: ");
                            foreach (var node in list.Skip(index + sub_index).Take(entry.Key.Length))
                            {
                                Console.Write(node);
                            }
                            Console.WriteLine();

                            Console.WriteLine("Recipe = " + (sub_index + index));
                            findrecipes.Remove(entry.Key);
                        }
                    }
                    index += buffer_string.Length;
                    buffer_string = "";
                }
            }

            CheckRecipe(9, ref list, "5158916779");
            CheckRecipe(5, ref list, "0124515891");
            CheckRecipe(18, ref list, "9251071085");
            CheckRecipe(2018, ref list, "5941429882");
            CheckRecipe(824501, ref list);
        }

        private void CheckRecipe(int skip, ref LinkedList<int> list, string result = "")
        {
            Console.WriteLine($"Checking with {skip}. Result = {GetTen(skip, list)} {(result != "" ? "should be " + result : "")}");
        }

        private string GetTen(int v, LinkedList<int> list)
        {
            string ten = "";

            foreach (int node in list.Skip(v).Take(10))
            {
                ten += node.ToString();
            }

            return ten;
        }

        private void ShowRecipes(LinkedListNode<int> elf1, LinkedListNode<int> elf2)
        {
            for (LinkedListNode<int> node = elf1.List.First; node != null; node = node.Next)
            {
                if (node == elf1)
                {
                    Console.Write("(" + node.Value + ") ");
                }
                else if (node == elf2)
                {
                    Console.Write("[" + elf2.Value + "] ");
                }
                else
                {
                    Console.Write(node.Value + " ");
                }


            }

            Console.WriteLine();
        }

        private void MoveElf(ref LinkedListNode<int> elf)
        {
            int moves = elf.Value + 1;
            for (int i = 0; i < moves; i++)
            {
                elf = elf.Next ?? elf.List.First;
            }
        }
    }
}
