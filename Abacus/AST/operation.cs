using System;

namespace Abacus.AST
{
    public class Operation
    {
        public string[] operators =
        {
            "+", "-", "/", "*", "^", "%",
            "sqrt", "min", "max", "fact",
            "isprime", "fibo", "gcd", "=",
            "(", ")"
        };

        public static int OperatorAction(int v1, string op, int v2)
        {
            int res = 0;
            switch (op)
            {
                case "-":
                    res = v1 - v2;
                    break;
                case "/":
                    if (v2 == 0)
                    {
                        Console.Error.WriteLine("Runtime Exeption: Divition by zero");
                        Environment.Exit(3);
                    }
                    else
                        res = v1 / v2;
                    break;
                case "*":
                    res = v1 * v2;
                    break;
                case "+":
                    res = v1 + v2;
                    break;
                case "%":
                    res = v1 % v2;
                    break;
                case "^":
                    res = (int) Math.Pow(v1, v2);
                    break;
                case "max":
                    res = Math.Max(v1, v2);
                    break;
                case "min":
                    res = Math.Min(v1,v2);
                    break;
                case "gcd":
                    res = Gcd(v1, v2);
                    break;
                case "=":
                    res = v1 = v2;
                    break;
                default:
                    Console.Error.WriteLine("Syntax error.");
                    Environment.Exit(2);
                    break;
            }

            return res;
        }

        public static int OperatorAction(int v, string op)
        {
            int res = 0;

            switch (op)
            {
                case "facto":
                    res = facto(v);
                    break;
                case "isprime":
                    res = Isprime(v);
                    break;
                case "fibo":
                    res = Fibo(v);
                    break;
                case "sqrt":
                    res = (int)Math.Sqrt(v);
                    break;
                default:
                    Console.Error.WriteLine("Syntax error.");
                    Environment.Exit(2);
                    break;
            }
            return res;
        }
        
        public static int Gcd(int a, int b)
        {
            return b == 0 ? a : Gcd(b, a % b);
        }

        public static int facto(int a)
        {
            if (a < 0)
            {
                Console.Error.WriteLine("Invalid operation.");
                Environment.Exit(3);
            }
            return __facto(a);
        }
        private static int __facto(int a)
        {
            return a == 1 ? 1 : a * __facto(a - 1);
        }

        public static int Fibo(int n)
        {
            int n1 = 0;
            int n2 = 1;
            int somme;
            if (n < 0)
            {
                Console.Error.WriteLine("Invalid operation.");
                Environment.Exit(3);
            }
            
            for (int i = 2; i <= n; i++)
            {
                somme = n1 + n2;
                n1 = n2;
                n2 = somme;
            }

            return n2;
        }
        
        public static int Isprime(int x)
        {
            int res = 1;
            if (x < 0)
            {
                Console.Error.WriteLine("Invalid operation.");
                Environment.Exit(3);
            }
            else
            {
                int a = 1;
                while (a * a > x && res == 1)
                {
                    res = x % a != 0 ? 1 : 0;
                    a += 1;
                }
            }

            return res;
        }
    }
}