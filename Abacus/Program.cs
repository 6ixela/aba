using System;
using System.Collections.Generic;
using Abacus.Analyse;
using Abacus.AST;

namespace Abacus
{
    public static class Program
    {
        public static int Main(string[] args)
        {

            Console.WriteLine("Hello, Abacus!");
            for (int i = 0; i < args.Length; ++i)
            {
                Console.WriteLine("argument {0}: {1}", i, args[i]);
            }
            
            
            // Returns an error code of 0, everything went fine!


            string path = Console.ReadLine();

            //pas de --rpn
            if (args.Length == 0)
            {
                List<Token> lTokens = Analyse.Token.BuildTokens(path);
                NPI.NPI npi = new NPI.NPI(lTokens);
                List<Token> lTokensNpi = npi.Evaluate();
                GenTree tree = GenTree.Build(lTokensNpi);
                Console.WriteLine(GenTree.Resultat(tree));
            }
            else
            {
                //avec rpn
                string rpn = args[0];
                if (rpn == "--rpn")
                {
                    List<Token> lTokens = Analyse.Token.BuildTokens(path);
                    GenTree tree = GenTree.Build(lTokens);
                    Console.WriteLine(GenTree.Resultat(tree));
                }
                else
                {
                    Console.Error.WriteLine("Unknown Argument :");
                    Environment.Exit(1);
                }
            }

            return 0;

        }
    }
}
