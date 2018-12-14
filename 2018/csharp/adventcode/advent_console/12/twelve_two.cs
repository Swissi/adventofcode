using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace advent_console._12
{
    internal class twelve_two : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("12/i.txt");
            string i_state = lines[0].Split(' ')[2];

            int last_zero = 0;

            // create linked list
            List<int> list = new List<int>();
            list.Add(0);
            list.Add(0);
            foreach (char c in i_state)
            {
                list.Add(c == '#' ? 1 : 0);
            }
            list.Add(0);
            list.Add(0);
            last_zero = 2;

            // get rules
            List<RuleInt> rules = new List<RuleInt>();
            foreach (string line in lines.Skip(2))
            {
                string[] arr = line.Split(' ');

                RuleInt rule = new RuleInt
                {
                    Result = arr[2] == "#" ? 1 : 0,
                    Rule = new List<int>()
                };

                foreach (char c in arr[0])
                {
                    rule.Rule.Add(c == '#' ? 1 : 0);
                }

                rules.Add(rule);
            }

            // setup complete
            PrintGeneration(list, 0);
            List<int> current_state = list;

            int gen = 5000;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (long g = 1; g <= gen; g++)
            {
                List<int> next_state = new List<int>();
                next_state.Add(0);
                next_state.Add(0);
                next_state.Add(0);
                next_state.Add(0);
                for (int i = 2; i < current_state.Count - 2; i++)
                {
                    bool found = false;
                    foreach (RuleInt rule in rules)
                    {
                        if (ApplyRule(i, rule, current_state))
                        {
                            next_state.Add(rule.Result);
                            found = true;
                            break;
                        }
                    }

                    if (!found) { 
                        next_state.Add(0);
                    }
                }

                next_state.Add(0);
                next_state.Add(0);
                next_state.Add(0);
                next_state.Add(0);

                //PrintGeneration(next_state, g);
                current_state = next_state;
                last_zero += 2;

                if (g % 1000 == 0)
                {
                    var elapsed = sw.Elapsed;
                    var times = gen / g;
                    Console.WriteLine($"{g}: took: {elapsed.TotalSeconds} seconds. Estimate till end: {TimeSpan.FromTicks(elapsed.Ticks * times).ToString("G")}");
                }
            }
            sw.Stop();

            PrintGeneration(current_state, gen + 1);
            Console.WriteLine(current_state[last_zero]);

            Console.WriteLine("index of zero:" + last_zero);
            int sum = 0;

            foreach (var i in Enumerable.Range(0, current_state.Count))
            {
                if (current_state[i] == 1)
                {
                    sum += i - last_zero;
                }
            }

            Console.WriteLine(sum);
        }

        private bool ApplyRule(int index, RuleInt rule, List<int> list)
        {
            // get 2 before and 2 after
            List<int> five = list.GetRange(index-2,5);
            return five.SequenceEqual(rule.Rule);
        }

        private void PrintGeneration(List<int> state, long gen)
        {
            Console.Write($"{gen.ToString("D2")}: ");
            foreach (int node in state)
            {
                Console.Write(node);
            }

            Console.WriteLine();
        }
    }


    public class RuleInt
    {
        public List<int> Rule;
        public int Result;
    }
}
