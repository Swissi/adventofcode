using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace advent_console._16
{
    internal class SixteenOne : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("16/i.txt");

            List<Sample> samples = new List<Sample>();
            int lastsample = 0;
            // collect samples
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith("Before"))
                {
                    string line2 = lines[i + 1];
                    string line3 = lines[i + 2];

                    Sample sample = new Sample
                    {
                        Before = line.Substring(9, 10).Split(',').Select(c => int.Parse(c)).ToArray(),
                        Instruction = line2.Split(' ').Select(c => int.Parse(c)).ToArray(),
                        After = line3.Substring(9, 10).Split(',').Select(c => int.Parse(c)).ToArray()
                    };
                    samples.Add(sample);

                    i = i + 3;
                    lastsample = i;
                }
            }

            Console.WriteLine($"there are {samples.Count} samples");

            List<IOperation> ops = new List<IOperation>
            {
                new Addr(),
                new Addi(),
                new Mulr(),
                new Muli(),
                new Banr(),
                new Bani(),
                new Borr(),
                new Bori(),
                new Setr(),
                new Seti(),
                new Gtir(),
                new Gtri(),
                new Gtrr(),
                new Eqir(),
                new Eqri(),
                new Eqrr()
            };

            Console.WriteLine($"there are {ops.Count} operations");
            
            int inputswithmultipleops = 0;
            Dictionary<int,List<int>> logs = new Dictionary<int, List<int>>();
            Dictionary<int, string> logs_string = new Dictionary<int, string>();
            foreach (Sample sample in samples)
            {
                List<bool> results = new List<bool>();
                foreach (IOperation op in ops)
                {
                    int opcode = sample.Instruction[0];
                    bool result = op.Test(sample.Before, sample.After, sample.Instruction);

                    if (result)
                    {
                        if (!logs.ContainsKey(opcode))
                        {
                            logs.Add(opcode, new List<int>());
                            logs_string.Add(opcode, "");
                        }

                        if (!logs[opcode].Contains(ops.IndexOf(op)))
                        {
                            logs[opcode].Add(ops.IndexOf(op));
                            logs_string[opcode] += op.GetType() + " ";
                        }
                    }

                    results.Add(op.Test(sample.Before, sample.After, sample.Instruction));
                }

                if (results.Where(b => b == true).Count() >= 3)
                {
                    inputswithmultipleops++;
                }
            }

            Console.WriteLine($"{inputswithmultipleops} have multiple possibilities");

            DisplayLog(logs);

            foreach (var log in logs_string)
            {
                Console.WriteLine(log.Key + ": " + log.Value);
            }

            while (logs.Any(l => l.Value.Count > 0))
            {
                var log = logs.FirstOrDefault(l => l.Value.Count == 1);

                int key = log.Key;
                int value = log.Value[0];

                Console.WriteLine(value + " is alone");

                ops[value].Code = key;

                Console.WriteLine("removing all " + value);
                foreach (var entry in logs)
                {
                    entry.Value.Remove(value);
                }

                DisplayLog(logs);
            }


            

            List<int[]> inputs = new List<int[]>();
            for (int i = 3298; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!line.Contains(":") && line != "")
                {
                    var arr = line.Split(' ').Select(c => int.Parse(c)).ToArray();
                    if (arr.Count() > 0)
                    {
                        inputs.Add(new[] { arr[0], arr[1], arr[2], arr[3] });
                    }
                }
            }


            //int[] register = lines[0].Substring(9, 10).Split(',').Select(c => int.Parse(c)).ToArray();
            int[] register = new[] {0, 0, 0, 0};

            Console.WriteLine("start");
            Console.WriteLine(string.Join(" ", register));
            // go through each input and invoke op
            foreach (var input in inputs)
            {
                int opcode = input[0];
                Console.WriteLine("input: " + string.Join(" ", input));
                Console.WriteLine(ops[opcode].GetType());
                ops.First(o => o.Code == opcode).Invoke(ref register, input);
                Console.WriteLine(string.Join(" ", register));
            }

            Console.Write("Register 0 is: " + register[0]);
        }

        private static void DisplayLog(Dictionary<int, List<int>> logs)
        {
            foreach (var log in logs.OrderBy(o => o.Key))
            {
                Console.Write(log.Key + ": ");
                foreach (var entry in log.Value)
                {
                    Console.Write(entry + " ");
                }

                Console.WriteLine();
            }
        }

        public class Sample
        {
            public int[] Before { get; set; }
            public int[] After { get; set; }
            public int[] Instruction { get; set; }

            public Sample()
            {
                Before = new int[4];
                After = new int[4];
                Instruction = new int[4];
            }
        }

        public interface IOperation
        {
            int Code { get; set; }

            int Usage { get; set; }

            int Trues { get; set; }

            bool Test(int[] before, int[] after, int[] input);

            void Invoke(ref int[] register, int[] input);
        }

        /// <summary>
        /// addr (add register) stores into register C the result of adding register A and register B.
        /// </summary>
        public class Addr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int value1 = register[input[1]];
                int value2 = register[input[2]];
                int result = value1 + value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = before[input[2]];
                int result = value1 + value2;

                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// addi (add immediate) stores into register C the result of adding register A and value B.
        /// </summary>
        public class Addi : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = input[2];
                int result = value1 + value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = input[2];
                int result = value1 + value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// mulr (multiply register) stores into register C the result of multiplying register A and register B.
        /// </summary>
        public class Mulr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = register[input[2]];
                int result = value1 * value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = before[input[2]];
                int result = value1 * value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// muli (multiply immediate) stores into register C the result of multiplying register A and value B.
        /// </summary>
        public class Muli : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = input[2];
                int result = value1 * value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = input[2];
                int result = value1 * value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
        /// </summary>
        public class Banr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = register[input[2]];
                int result = value1 & value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = before[input[2]];
                int result = value1 & value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
        /// </summary>
        public class Bani : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = input[2];
                int result = value1 & value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = input[2];
                int result = value1 & value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
        /// </summary>
        public class Borr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = register[input[2]];
                int result = value1 | value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = before[input[2]];
                int result = value1 | value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
        /// </summary>
        public class Bori : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = input[2];
                int result = value1 | value2;
                register[input[3]] = result;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = input[2];
                int result = value1 | value2;
                Trues += result == after[input[3]] ? 1 : 0;
                return result == after[input[3]];
            }
        }

        /// <summary>
        /// setr (set register) copies the contents of register A into register C. (Input B is ignored.)
        /// </summary>
        public class Setr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                register[input[3]] = value1;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                Trues += after[input[3]] == value1 ? 1 : 0;
                return after[input[3]] == value1;
            }
        }

        /// <summary>
        /// seti (set immediate) stores value A into register C. (Input B is ignored.)
        /// </summary>
        public class Seti : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = input[1];
                register[input[3]] = value1;
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = input[1];
                Trues += after[input[3]] == value1 ? 1 : 0;
                return after[input[3]] == value1;
            }
        }

        /// <summary>
        /// gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
        /// </summary>
        public class Gtir : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = input[1];
                int value2 = register[input[2]];
                if (value1 > value2)
                {
                    register[input[3]] = 1;
                }
                else
                {
                    register[input[3]] = 0;
                }
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = input[1];
                int value2 = before[input[2]];
                if (value1 > value2)
                {
                    Trues += after[input[3]] == 1 ? 1 : 0;
                    return after[input[3]] == 1;
                }
                else
                {
                    Trues += after[input[3]] == 0 ? 1 : 0;
                    return after[input[3]] == 0;
                }
            }
        }

        /// <summary>
        /// gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
        /// </summary>
        public class Gtri : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = input[2];
                if (value1 > value2)
                {
                    register[input[3]] = 1;
                }
                else
                {
                    register[input[3]] = 0;
                }
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = input[2];
                if (value1 > value2)
                {
                    Trues += after[input[3]] == 1 ? 1 : 0;
                    return after[input[3]] == 1;
                }
                else
                {
                    Trues += after[input[3]] == 0 ? 1 : 0;
                    return after[input[3]] == 0;
                }
            }
        }

        /// <summary>
        /// gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
        /// </summary>
        public class Gtrr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = register[input[2]];
                if (value1 > value2)
                {
                    register[input[3]] = 1;
                }
                else
                {
                    register[input[3]] = 0;
                }
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = before[input[2]];
                if (value1 > value2)
                {
                    Trues += after[input[3]] == 1 ? 1 : 0;
                    return after[input[3]] == 1;
                }
                else
                {
                    Trues += after[input[3]] == 0 ? 1 : 0;
                    return after[input[3]] == 0;
                }
            }
        }

        /// <summary>
        /// eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
        /// </summary>
        public class Eqir : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = input[1];
                int value2 = register[input[2]];
                if (value1 == value2)
                {
                    register[input[3]] = 1;
                }
                else
                {
                    register[input[3]] = 0;
                }
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = input[1];
                int value2 = before[input[2]];
                if (value1 == value2)
                {
                    Trues += after[input[3]] == 1 ? 1 : 0;
                    return after[input[3]] == 1;
                }
                else
                {
                    Trues += after[input[3]] == 0 ? 1 : 0;
                    return after[input[3]] == 0;
                }
            }
        }

        /// <summary>
        /// eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
        /// </summary>
        public class Eqri : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = input[2];
                if (value1 == value2)
                {
                    register[input[3]] = 1;
                }
                else
                {
                    register[input[3]] = 0;
                }
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = input[2];
                if (value1 == value2)
                {
                    Trues += after[input[3]] == 1 ? 1 : 0;
                    return after[input[3]] == 1;
                }
                else
                {
                    Trues += after[input[3]] == 0 ? 1 : 0;
                    return after[input[3]] == 0;
                }
            }
        }

        /// <summary>
        /// eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
        /// </summary>
        public class Eqrr : IOperation
        {
            public int Code { get; set; }
            public int Usage { get; set; }
            public int Trues { get; set; }

            public void Invoke(ref int[] register, int[] input)
            {
                int opcode = input[0];
                int value1 = register[input[1]];
                int value2 = register[input[2]];
                if (value1 == value2)
                {
                    register[input[3]] = 1;
                }
                else
                {
                    register[input[3]] = 0;
                }
            }

            public bool Test(int[] before, int[] after, int[] input)
            {
                Usage++;
                int opcode = input[0];
                int value1 = before[input[1]];
                int value2 = before[input[2]];
                if (value1 == value2)
                {
                    Trues += after[input[3]] == 1 ? 1 : 0;
                    return after[input[3]] == 1;
                }
                else
                {
                    Trues += after[input[3]] == 0 ? 1 : 0;
                    return after[input[3]] == 0;
                }
            }
        }
    }
}
