using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent_console._20
{
    internal class Twenty : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("20/i.txt");

            foreach (string line in lines)
            {
                // create grid
                // char[,] map = new char[line.Length,line.Length];
                string l = line; // ^ENWWW(NEEE|SSE(EE|N))$

                string result = CheckLine(l.Substring(1, l.Length - 2)); // ENWWW(NEEE|SSE(EE|N))


                // all done
                Console.WriteLine("Source= " + line);
                Console.WriteLine("Result=" + result);
                Console.WriteLine("The Path is " + result.Length + " doors long");

            }
        }

        private string CheckLine(string l)
        {
            if (string.IsNullOrEmpty(l))
            {
                return l;
            }

            int firstopenbracket = l.IndexOf('(');
            int firstpipe = l.IndexOf('|');

            // if no special characters return string
            if (firstopenbracket == -1 && firstpipe == -1)
            {
                return l;
            }

            // if just a pipe, split 
            if (firstopenbracket == -1 && firstpipe > -1)
            {
                List<string> arrpipe = l.Split('|').ToList();

                return arrpipe.OrderBy(s => s.Length).First();
            }
            
            // pipe but no brackets
            List<string> arr = SplitByPipes(l);
            List<string> list = new List<string>();

            if (arr.Count > 1)
            {
                foreach (string subline in arr)
                {
                    list.Add(CheckLine(subline));
                }

                return list.OrderByDescending(s => s.Length).First();
            }
            else
            {
                int si = 0;
                int brackets = 0;
                List<string> array = new List<string>();

                for (int i = 0; i < l.Length; i++)
                {
                    char c = l[i];

                    if (c == '(')
                    {
                        if (brackets == 0)
                        {
                            string sub1 = l.Substring(si, i - si);
                            if (!string.IsNullOrEmpty(sub1))
                            {
                                array.Add(sub1);
                            }
                            si = i;
                        }

                        brackets++;
                    }

                    if (c == ')')
                    {
                        if (brackets == 1)
                        {
                            string sub1 = l.Substring(si + 1, i - si - 1);
                            if (!string.IsNullOrEmpty(sub1))
                            {
                                array.Add(sub1);
                            }
                            si = i + 1;
                        }
                        brackets--;
                    }
                }

                string rest = l.Substring(si);
                if (!string.IsNullOrEmpty(rest))
                {
                    array.Add(rest);
                }


                string result = "";
                foreach (string line in array)
                {
                    result += CheckLine(line);
                }

                return result;
            }
        }

        private List<string> SplitByPipes(string l)
        {
            //NEEE|SSE(EE|N)
            List<string> array = new List<string>();
            int i = 0;
            int start_i = 0;
            int brackets = 0;


            foreach (char c in l)
            {
                if (c == '|' && brackets == 0)
                {
                    array.Add(l.Substring(start_i, i - start_i));
                    start_i = i + 1;
                }

                if (c == '(')
                {
                    brackets++;
                }

                if (c == ')')
                {
                    brackets--;
                }

                i++;
            }

            array.Add(l.Substring(start_i));

            return array;
        }
    }
}
