using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace advent_console._12
{
    internal class twelve_one : IPart
    {
        public void DoIt()
        {
            // meta
            Stopwatch sw = new Stopwatch();
            var generations = 20;
            var index_of_zero = 0;

            string[] lines = File.ReadAllLines("12/e.txt");

            //initial state: #..#.#..##......###...###
            string initial_state = lines[0].Split(' ')[2];
            
            // get rules
            List<Rule> rules = new List<Rule>();
            foreach (var line in lines.Skip(2))
            {
                var arr = line.Split(' ');
                rules.Add(new Rule
                {
                    RuleAsString = arr[0],
                    Result = arr[2]
                });
            }

            // start
            var current_gen = initial_state;
            
            // setup complete
            PrintGeneration(".." + initial_state, 0);
            

            for(long g = 1; g<generations; g++) 
            {
                sw.Start();

                var next_gen = "";
                // apply rules
                foreach (var i in Enumerable.Range(0,current_gen.Length))
                {
                    string fives = GetFives(i, current_gen);

                    var applying_rule = rules.FirstOrDefault(r => r.RuleAsString == fives);
                    var append = 0;

                    //Console.WriteLine(fives + (applying_rule != null ? ": rule!":""));

                    if (applying_rule != null)
                    {
                        var prepend = false;
                            
                        if (i == 0)
                        {
                            next_gen += ".." + applying_rule.Result;
                            index_of_zero += 2;
                            prepend = true;
                        }
                        else if (i == 1 && !prepend)
                        {
                            next_gen += "." + applying_rule.Result;
                            index_of_zero += 1;
                            prepend = true;
                        }
                        else if (i == current_gen.Length - 1)
                        {
                            next_gen += applying_rule.Result;
                            append = 1;
                        }
                        else if (i == current_gen.Length)
                        {
                            next_gen += applying_rule.Result;
                            append = 2;
                        }
                        else
                        {
                            next_gen += applying_rule.Result;
                        }
                    }
                    else
                    {
                        next_gen += ".";
                    }

                    if (append > 0)
                    {
                        foreach (var x in Enumerable.Range(1, append))
                        {
                            next_gen += ".";
                        }
                    }
                }

                //PrintGeneration(next_gen, g);
                current_gen = next_gen;
                if (g % 1000 == 0)
                {
                    var elapsed = sw.Elapsed;
                    var times = generations / g;
                    Console.WriteLine($"{g}: took: {elapsed.TotalSeconds} seconds. Estimate till end: {TimeSpan.FromTicks(elapsed.Ticks * times).ToString("G")}");
                }
            }
            sw.Stop();

            Console.WriteLine("index of zero:" + index_of_zero);
            int sum = 0;
            
            foreach (var i in Enumerable.Range(0, current_gen.Length))
            {
                if (current_gen[i] == '#')
                {
                    sum += i - index_of_zero;
                }   
            }

            Console.WriteLine(sum);
        }

        private string GetFives(int i, string current_gen)
        {
            if (i == 0)
            {
                return ".." + string.Join("", current_gen.Take(3));
            }
            else if (i == 1)
            {
                return "." + string.Join("", current_gen.Take(4));
            }
            else if (i == current_gen.Length - 2)
            {
                return string.Join("", current_gen.Skip(i - 2).Take(4)) + ".";
            }
            else if (i == current_gen.Length - 1)
            {
                return string.Join("", current_gen.Skip(i - 2).Take(3)) + "..";
            }
            else
            {
                return string.Join("", current_gen.Skip(i - 2).Take(5));
            }
        }

        private void PrintGeneration(string state, long gen)
        {
            Console.WriteLine($"{gen.ToString("D2")}: {state}");
        }
    }
    
    public class Rule
    {
        public int RuleAsInteger;
        public int ResultAsInteger;
        public string RuleAsString;
        public string Result;
    }
}
