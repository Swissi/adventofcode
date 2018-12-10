using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace _4p2
{
    class Program
    {
        static void Main(string[] args)
        {


            // Read unsorted file
            var alllines = File.ReadAllLines("i.txt");
            Console.WriteLine("Filelines unsorted: " + alllines.Length);


            Dictionary<DateTime, string> unsorted_log = new Dictionary<DateTime, string>();
            foreach (string line in alllines)
            {
                /*
                 *  [1518-08-17 23:53] Guard #131 begins shift
                    [1518-10-21 00:43] falls asleep
                    [1518-08-19 00:48] wakes up
                 */

                // Get the date out and fill new list with datetimes and sort by it
                var datepart = line.Substring(1, line.IndexOf(']')-1);
                var msgpart = line.Substring(line.IndexOf("]")+2);

                unsorted_log.Add(DateTime.Parse(datepart),msgpart);
            }

            Console.WriteLine("Lines in dictionary unsorted: " + unsorted_log.Count);

            // Sort it
            var sorted_log = unsorted_log.OrderBy(s => s.Key);
            Console.WriteLine("Lines in dictionary sorted: " + unsorted_log.Count);


            List<Guard> guards = new List<Guard>();
            
            Guard last_guard = null;
            int startminute = 0;
            Dictionary<int,int> minutes = new Dictionary<int, int>();
            

            foreach (var logentry in sorted_log)
            {
                // Get Guard ID
                if (logentry.Value.Contains("#"))
                {
                    var id = logentry.Value.Split(' ')[1].Substring(1);

                    if (guards.Any(g => g.Id == id))
                    {
                        last_guard = guards.First(g => g.Id == id);
                    }
                    else
                    {
                        last_guard = new Guard {Id = id};
                        guards.Add(last_guard);
                    }
                }

                // now if the guard fell asleep. log it
                if (logentry.Value.Contains("falls asleep"))
                {
                    startminute = logentry.Key.Minute;
                }

                if (logentry.Value.Contains("wakes up"))
                {
                    for (int i = startminute; i < logentry.Key.Minute; i++)
                    {
                        if (!minutes.ContainsKey(i))
                        {
                            minutes.Add(i,0);
                        }

                        minutes[i]++;

                        if (!last_guard.Minutes.ContainsKey(i))
                        {
                            last_guard.Minutes.Add(i,0);
                        }

                        last_guard.Minutes[i]++;
                        last_guard.Total++;
                    }
                }
            }

            var guardtotalmaster = guards.OrderByDescending(o => o.Total).First();
            var guardminutemaster = guardtotalmaster.Minutes.OrderByDescending(o => o.Value).First();

            Console.WriteLine(int.Parse(guardtotalmaster.Id) * guardminutemaster.Key);

            foreach (var g in guards)
            {
                var sleepiest_minute = 0;
                var sm_tries = 0;
                if (g.Minutes.Count > 0)
                {
                    sleepiest_minute = g.Minutes.OrderBy(o => o.Value).Last().Key;
                    sm_tries = g.Minutes.OrderBy(o => o.Value).Last().Value;
                }
                
                Console.WriteLine($"Guard {g.Id}: slept {sm_tries} times on minute {sleepiest_minute}");
            }


            Console.Read();
        }
    }
}
