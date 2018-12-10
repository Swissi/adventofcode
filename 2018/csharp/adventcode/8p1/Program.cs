using System;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace _8p1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Read unsorted file
            var lines = File.ReadAllLines("i.txt");
            var arr = lines[0].Split(' ').Select(Int32.Parse).ToList();
            int sum = 0;

            var i = 0;
            var root = ReadNode(arr, ref i);
            Console.WriteLine(root.Sum());
            Console.WriteLine(root.Value());


            GetNext(arr, 0, ref sum);
            Console.WriteLine(sum);
            Console.Read();
        }

        private static int GetNext(List<int> arr, int start_index, ref int sum)
        {
            var children = arr[start_index++];
            var meta = arr[start_index++];

            for (int i = 0; i < children; i++)
            {
                start_index = GetNext(arr, start_index, ref sum);
            }

            for (int x = 0; x < meta; x++)
            {
                sum += arr[start_index++];
            }

            return start_index;
        }

        public static Node ReadNode(List<int> numbers, ref int i)
        {
            var node = new Node();
            var children = numbers[i++];
            var metadata = numbers[i++];
            for (int j = 0; j < children; j++)
            {
                node.Nodes.Add(ReadNode(numbers, ref i));
            }

            for (int j = 0; j < metadata; j++)
            {
                node.Metadata.Add(numbers[i++]);
            }

            return node;
        }
    }

    public class Node
        {
            public List<int> Metadata { get; set; } = new List<int>();
            public List<Node> Nodes { get; set; } = new List<Node>();

            public int Sum()
            {
                return Metadata.Sum() + Nodes.Sum(x => x.Sum());
            }

            public int Value()
            {
                if (!Nodes.Any())
                {
                    return Metadata.Sum();
                }

                var value = 0;
                foreach (var m in Metadata)
                {
                    if (m <= Nodes.Count)
                    {
                        value += Nodes[m - 1].Value();
                    }
                }

                return value;
            }
        }

}
