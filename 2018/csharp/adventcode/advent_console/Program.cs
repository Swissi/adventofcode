using System;
using System.Collections.Generic;
using advent_console._11;
using advent_console._12;
using advent_console._13;
using advent_console._14;
using advent_console._15;
using advent_console._16;
using advent_console._17;

namespace advent_console
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IPart> tasklist = new List<IPart>();
            //tasklist.Add(new eleven_one());
            //tasklist.Add(new eleven_two());
            //tasklist.Add(new twelve_one());
            //tasklist.Add(new twelve_two());
            //tasklist.Add(new thirteen_one());
            //tasklist.Add(new fourteen_one());
            //tasklist.Add(new FifteenOne());
            //tasklist.Add(new SixteenOne());
            tasklist.Add(new SeventeenOne());


            foreach (IPart part in tasklist)
            {
                part.DoIt();
            }
            
            Console.WriteLine("End of Code. Press a key to exit.");
            Console.Read();
        }
    }
}
