using System;
using System.Collections.Generic;
using advent_console._11;
using advent_console._12;
using advent_console._13;

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
            tasklist.Add(new thirteen_one());


            foreach (IPart part in tasklist)
            {
                part.DoIt();
            }
            
            Console.WriteLine("End of Code. Press a key to exit.");
            Console.Read();
        }
    }
}
