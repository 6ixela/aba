using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Abacus.AST;
using Microsoft.VisualBasic.CompilerServices;

namespace Abacus.Analyse
{
    
    public class Token
    {
        private int valeur;

        private string operateur;

        private int precedence;

        private bool fonction;

        private int nbArg;

        public int NbArg => nbArg;

        public int Valeur
        {
            get { return valeur; }
            set { valeur = value; }
        }

        public string Operateur => operateur;

        public int Precedence
        {
            get { return precedence; }
            set { precedence = value; }
        }

        public bool Fonction => fonction;

        public Token(int val, string ope)
        {
            switch (ope)
            {
                case "" or " ":
                    this.operateur = null;
                    this.valeur = val;
                    this.precedence = 0;
                    this.fonction = false;
                    this.nbArg = 0;
                    break;
                //il faudrait verifier operateur
                default:
                    this.operateur = ope;
                    this.valeur = 0;
                    switch (ope)
                    {
                        case ")" or "(":
                            this.precedence = -1;
                            this.fonction = true;
                            this.nbArg = 0;
                            break;
                        case "%":
                            this.precedence = 1;
                            this.fonction = false;
                            this.nbArg = 0;
                            break;
                        case "+" or "-":
                            this.precedence = 2;
                            this.fonction = false;
                            this.nbArg = 0;
                            break;
                        case "*" or "/":
                            this.precedence = 3;
                            this.fonction = false;
                            this.nbArg = 0;
                            break;
                        case "^":
                            this.precedence = 4;
                            this.fonction = false;
                            this.nbArg = 0;
                            break;
                        case  "min" or "max" or 
                              "gcd":
                            this.precedence = 5;
                            this.fonction = true;
                            this.nbArg = 2;
                            break;
                        case  "sqrt" or "fact" or "isprime" or "fibo":
                            this.precedence = 5;
                            this.fonction = true;
                            this.nbArg = 1;
                            break;
                        case ",":
                            this.precedence = -1;
                            this.fonction = false;
                            this.nbArg = 0;
                            break;
                        case "=":
                            this.Precedence = 0;
                            this.fonction = true;
                            this.nbArg = 2;
                            break;
                        default:
                            Console.Error.WriteLine("Error, Unknown " + ope);
                            Environment.Exit(1);
                            break;
                    }

                    break;
            }
        }

        private static readonly string[] _operator =
        {
            "+", "-", "/", "*", "^", "%",
             "=", "(", ")"
        };

        private static readonly string[] _fonction =
        {
            "sqrt", "min", "max", "fact",
            "isprime", "fibo", "gcd"
        };
        
        //verifie si le char en entré est un operateur
        private static bool IsOperator(char c)
        {
            bool res = false;
            int i = 0;
            while (i < _operator.Length && !res)
            {
                res = c.ToString() == _operator[i];
                i += 1;
            }

            return res;
        }
        
        public static List<Token> BuildTokens(string path)
        {
            List<Token> res = new List<Token>();
            int nbVirgule = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == ',')
                {
                    if (nbVirgule <= 0)
                    {
                        Console.Error.WriteLine("Syntax error.");
                        Environment.Exit(2);
                    }
                    else
                    {
                        nbVirgule -= 1;
                    }
                }
                else
                {
                    if (path[i] != ' ')
                    {
                        int val = 0;
                        string op = "";
                        //si on a une fonction
                        if (Char.IsLetter(path[i]))
                        {
                            while (i < path.Length && Char.IsLetter(path[i]))
                            {
                                op += path[i];
                                i += 1;
                            }
                        }
                        else
                        {
                            //si on a un nombre ou chiffre
                            if (Char.IsNumber(path[i]))
                            {
                                while (i < path.Length && Char.IsNumber(path[i]))
                                {
                                    val = val * 10 + path[i] - '0';
                                    i += 1;
                                }
                            }
                            //si on a une operation
                            else
                            {
                                if (IsOperator(path[i]))
                                {
                                    op += path[i];
                                    i += 1;
                                }
                            }
                        }

                        i -= 1;
                        Token token = new Token(val, op);
                        res.Add(token);
                        if (token.nbArg == 2)
                        {
                            nbVirgule += 1;
                        }
                    }
                }
            }

            if (nbVirgule != 0)
            {
                Console.Error.WriteLine("Syntax error.");
                Environment.Exit(2);
            }
            return res;
        }
    }
}