using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace advent_console._19
{
    class NineteenOne : IPart
    {
        public void DoIt()
        {
            string[] lines = File.ReadAllLines("19/i.txt");

            int ip_register = int.Parse(lines[0].Split(' ')[1]);

            List<Input> inputs = new List<Input>();
            for (int i = 1; i < lines.Length; i++)
            {
                Input inp = new Input();
                var arr = lines[i].Split(' ');
                var classname = arr[0].First().ToString().ToUpper() + arr[0].Substring(1);
                var optype = Type.GetType("advent_console._19." + classname);
                inp.Op = (IOperation)Activator.CreateInstance(optype);
                inp.Data = new[] {int.Parse(arr[1]), int.Parse(arr[2]), int.Parse(arr[3])};
                inputs.Add(inp);
            }

            // part1:
            int[] register = new[] {0, 0, 0, 0, 0, 0};

            // part 2:
            register[0] = 1;

            int cycle = 0;

            while (true)
            {
                cycle++;
                // get instruction
                int instruction = register[ip_register];
                var input = inputs[instruction];
                input.Op.Invoke(ref register, input.Data);
                if (register[ip_register] + 1 >= inputs.Count)
                {
                    break;
                }
                else
                {
                    register[ip_register]++;
                }
            }

            Console.WriteLine("Register 0 is:" + register[0]);
        }
    }

    public class Input
    {
        public IOperation Op;
        public int[] Data;
    }


    /// <summary>
    /// addr (add register) stores into register C the result of adding register A and register B.
    /// </summary>
    public class Addr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = register[input[1]];
            int result = value1 + value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// addi (add immediate) stores into register C the result of adding register A and value B.
    /// </summary>
    public class Addi : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = input[1];
            int result = value1 + value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// mulr (multiply register) stores into register C the result of multiplying register A and register B.
    /// </summary>
    public class Mulr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = register[input[1]];
            int result = value1 * value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// muli (multiply immediate) stores into register C the result of multiplying register A and value B.
    /// </summary>
    public class Muli : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = input[1];
            int result = value1 * value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
    /// </summary>
    public class Banr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = register[input[1]];
            int result = value1 & value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
    /// </summary>
    public class Bani : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = input[1];
            int result = value1 & value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
    /// </summary>
    public class Borr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = register[input[1]];
            int result = value1 | value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
    /// </summary>
    public class Bori : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = input[1];
            int result = value1 | value2;
            register[input[2]] = result;
        }
    }

    /// <summary>
    /// setr (set register) copies the contents of register A into register C. (Input B is ignored.)
    /// </summary>
    public class Setr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            register[input[2]] = value1;
        }
    }

    /// <summary>
    /// seti (set immediate) stores value A into register C. (Input B is ignored.)
    /// </summary>
    public class Seti : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = input[0];
            register[input[2]] = value1;
        }
    }

    /// <summary>
    /// gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
    /// </summary>
    public class Gtir : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = input[0];
            int value2 = register[input[1]];
            if (value1 > value2)
            {
                register[input[2]] = 1;
            }
            else
            {
                register[input[2]] = 0;
            }
        }
    }

    /// <summary>
    /// gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
    /// </summary>
    public class Gtri : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = input[1];
            if (value1 > value2)
            {
                register[input[2]] = 1;
            }
            else
            {
                register[input[2]] = 0;
            }
        }
    }

    /// <summary>
    /// gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
    /// </summary>
    public class Gtrr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = register[input[1]];
            if (value1 > value2)
            {
                register[input[2]] = 1;
            }
            else
            {
                register[input[2]] = 0;
            }
        }
    }

    /// <summary>
    /// eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
    /// </summary>
    public class Eqir : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = input[0];
            int value2 = register[input[1]];
            if (value1 == value2)
            {
                register[input[2]] = 1;
            }
            else
            {
                register[input[2]] = 0;
            }
        }
    }

    /// <summary>
    /// eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
    /// </summary>
    public class Eqri : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = input[1];
            if (value1 == value2)
            {
                register[input[2]] = 1;
            }
            else
            {
                register[input[2]] = 0;
            }
        }
    }

    /// <summary>
    /// eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
    /// </summary>
    public class Eqrr : IOperation
    {
        public void Invoke(ref int[] register, int[] input)
        {
            int value1 = register[input[0]];
            int value2 = register[input[1]];
            if (value1 == value2)
            {
                register[input[2]] = 1;
            }
            else
            {
                register[input[2]] = 0;
            }
        }
    }

    public interface IOperation
    {
        void Invoke(ref int[] register, int[] input);
    }
}
