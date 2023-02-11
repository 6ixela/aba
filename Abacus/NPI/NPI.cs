using System;
using System.Collections.Generic;
using Abacus.Analyse;

namespace Abacus.NPI
{
    public class NPI
    {
        private List<Token> tokens;

        public  List<Token> Tokens => this.tokens;
        
        
        public NPI(List<Token> lToken)
        {
            this.tokens = lToken;
        }

        /*public List<Token> Evaluate()
        {
            Stack<Token> stack = new Stack<Token>();
            List<Token> outputStack = new List<Token>();
            
            for (int i = 0; i < tokens.Count; i++)
            {
                //si on a un nombre
                if (tokens[i].Operateur == null)
                    outputStack.Add(tokens[i]);
                else
                {
                    //on doit verifier si on a un moins avant un nombre pour signifier
                    ////un nbr negatif

                    //ici, -5 + 3, on verifie si le moins est au debut
                    
                    if (i == 0 && i + 1 < tokens.Count &&
                        tokens[i + 1].Operateur == null
                        && (tokens[i].Operateur == "-" ||
                            tokens[i].Operateur == "+"))
                    {
                        i += 1; 
                        tokens[i].Valeur *= -1;
                        outputStack.Add(tokens[i]);
                    }
                    else
                    {
                        //si on a par exemple 5 + (-3)
                        //parenthese pas obligatoire ici
                        if (i - 1 >= 0 &&
                            tokens[i - 1].Operateur != null &&
                            i + 1 < tokens.Count &&
                            tokens[i + 1].Operateur == null &&
                            (tokens[i].Operateur == "-" ||
                             tokens[i].Operateur == "+"))
                        {
                            i += 1;
                            tokens[i].Valeur *= -1;
                            outputStack.Add(tokens[i]);
                        }
                        else
                        {
                            //si on tombe sur ')', on destack tous les operateur entre les ()
                            if (tokens[i].Operateur == ")")
                            {
                                int j = 0;
                                bool trouver = false;
                                int stackCount = stack.Count;
                                while (j < stackCount && !trouver)
                                {
                                    var token = stack.Pop();
                                    if (token.Operateur != "(")
                                        outputStack.Add(token);
                                    else
                                        trouver = true;
                                    j += 1;
                                }

                                //si on a pas trouver la '('
                                if (!trouver)
                                {
                                    Console.Error.WriteLine("Syntax error : no '('");
                                    Environment.Exit(2);
                                }
                            }
                            //si le dernier operateur de la pile est une ft, alors le depiler
                            else
                            {
                                //si on a un operateur
                                if (tokens[i].Operateur != "(")
                                {
                                    Token token;
                                    bool fini = false;
                                    while (stack.Count > 0 && !fini)
                                    {
                                        token = stack.Pop();
                                        if (token.Precedence >= tokens[i].Precedence)
                                            outputStack.Add(token);
                                        else
                                        {
                                            stack.Push(token);
                                            fini = true;
                                        }
                                    }
                                }

                                stack.Push(tokens[i]);
                            }
                        }
                    }
                }
            }
            
            //a la fin, on ajoute la fin de stack dans outputStack
            int countStack = stack.Count;
            for (int i = 0; i < countStack; i++)
                outputStack.Add(stack.Pop());

            return outputStack;
        }*/
        
        public List<Token> Evaluate()
        {
            Stack<Token> stack = new Stack<Token>();
            List<Token> outputStack = new List<Token>();

            for (int i = 0; i < tokens.Count; i++)
            {
                //si on a un nombre
                if (tokens[i].Operateur == null)
                {
                    if (i+1 < tokens.Count && 
                        tokens[i+1].Operateur == "(")
                    {
                        Token token = new Token(0, "*");
                    }
                    outputStack.Add(tokens[i]);

                }
                else
                {
                    
                    //si on tombe sur ')', on destack tous les operateur entre les ()
                    if (tokens[i].Operateur == ")")
                    {
                        int j = 0;
                        bool trouver = false;
                        int stackCount = stack.Count;
                        while (j < stackCount && !trouver)
                        {
                            var token = stack.Pop();
                            if (token.Operateur != "(")
                                outputStack.Add(token);
                            else
                                trouver = true;
                            j += 1;
                        }
                        //si on a pas trouver la '('

                        if (!trouver)
                        {
                            Console.Error.WriteLine("Syntax error : no '('");
                            Environment.Exit(2);
                        }
                        //sinon 
                        else
                        {
                            //le dernier operateur de la pile est une ft, alors le depiler
                            if (stack.Count > 0)
                            {
                                Token token;
                                bool ft = true;
                                while (stack.Count > 0 && ft)
                                {
                                    token = stack.Pop();
                                    if (token.Fonction)
                                        outputStack.Add(token);
                                    else
                                    {
                                        ft = false;
                                        stack.Push(token);
                                    }
                                }
                            }
                        }
                    }
                    else
                    { 
                        //si on a un operateur
                        if (tokens[i].Operateur != "(") 
                        { 
                            //on doit verifier si on a un moins avant un nombre pour signifier
                            //un nbr negatif
                            
                            /*Ici, on verifie si on a :
                             *-5 + ...
                             * -min(...)
                             * ou avec des plus
                             */
                            if (i == 0 && i + 1 < tokens.Count &&
                                (tokens[i + 1].Operateur == null
                                 || tokens[i + 1].Fonction
                                 || tokens[i+1].Precedence == 2)
                                && tokens[i].Precedence == 2) 
                            { 
                                if (stack.Count > 0)
                                {
                                    Token t = stack.Pop();
                                    tokens[i].Precedence = t.Precedence + 1;
                                    stack.Push(t);
                                }
                                //i += 1; 
                                outputStack.Add(new Token(0, ""));

                                    //tokens[i].Valeur *= -1;
                                    //outputStack.Add(tokens[i]);
                            }
                            else 
                            { 
                                //si on a par exemple 5 + (-3)
                                //parenthese pas obligatoire ici
                                    
                                if (i - 1 >= 0 && 
                                    tokens[i - 1].Operateur != null && 
                                    !tokens[i - 1].Fonction &&
                                    i + 1 < tokens.Count && 
                                    (tokens[i + 1].Operateur == null ||
                                     tokens[i+1].Precedence == 2 ||
                                     tokens[i+1].Fonction) && 
                                    tokens[i].Precedence == 2)//precedence == 2 soit + soit -
                                {
                                    if (stack.Count > 0)
                                    {
                                        Token t = stack.Pop();
                                        tokens[i].Precedence = t.Precedence + 1;
                                        stack.Push(t);
                                    }
                                    outputStack.Add(new Token(0, ""));

                                    //tokens[i].Valeur *= -1;
                                }
                                //pour les operateurs avec la pile
                                Token token; 
                                bool fini = false; 
                                while (stack.Count > 0 && !fini) 
                                { 
                                    token = stack.Pop(); 
                                    if (token.Precedence >= tokens[i].Precedence) 
                                        outputStack.Add(token);
                                    else
                                    { 
                                        stack.Push(token); 
                                        fini = true;
                                    }
                                }

                            }

                        }
                        stack.Push(tokens[i]);

                    }
                }
            }

            //a la fin, on ajoute la fin de stack dans outputStack
            int countStack = stack.Count;
            for (int i = 0; i < countStack; i++)
                outputStack.Add(stack.Pop());

            return outputStack;
        }
        
        //objectif, savoir si les plus ou moins foute le bordel
        private static void BcpDeMoinsOuPlus(List<Token> lToken, int i)
        {
            
        }
    }
}