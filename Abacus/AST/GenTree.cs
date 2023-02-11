using System;
using System.Collections.Generic;
using Abacus.Analyse;

namespace Abacus.AST
{
    public class GenTree
    {
        private GenTree right;

        private GenTree left;
        
        private Token name;
        
        //private List<Token> listToken = new List<Token>();
        
        public GenTree Left => left;

        public Token Name => name;

        public GenTree Right => right;

        //public  List<Token> ListTokens => this.listToken;

        
        public GenTree(Token name, GenTree left, GenTree right)
        {
            this.name = name;
            this.left = left;
            this.right = right;
        }

        //cree l'ast
        public static GenTree Build(List<Token> lToken)
        {
            Stack<GenTree> stack = new Stack<GenTree>();
            int i = 0;
            GenTree tree = null;
            while (i < lToken.Count)
            {
                tree = new GenTree(lToken[i], null, null);
                //si on a un operateur
                if (lToken[i].Operateur != null)
                {
                    try
                    {
                        tree.right = stack.Pop();
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Syntax error");
                        Environment.Exit(2);
                    }

                    if (lToken[i].NbArg != 1)
                    {

                        try
                        {
                            tree.left = stack.Pop();
                        }
                        catch (Exception)
                        {
                            Console.Error.WriteLine("Syntax error");
                            Environment.Exit(2);
                        }
                    }
                }
                stack.Push(tree);
                i += 1;
            }

            if (stack.Count == 1)
            {
                tree = stack.Pop();
            }
            else
            {
                Console.Error.WriteLine("Syntax Error: operator expected");
                Environment.Exit(2);
            }
            return tree;
        }

        //calcule le resultat de l'ast
        public static int Resultat(GenTree tree)
        {
            int res = tree.name.Valeur;
            // si on a un operateur
            if (tree.name.Operateur != null)
            {
                int v1 = 0;
                int v2;
                if (tree.left !=null)
                    v1 = Resultat(tree.left);
                

                string op = tree.name.Operateur;
                v2 = Resultat(tree.right);
                if (tree.Name.NbArg == 1)
                {
                    res = Operation.OperatorAction(v2, op);
                }
                else
                {
                    res = Operation.OperatorAction(v1, op, v2);
                }
            }

            return res;

        }
        
    }
}