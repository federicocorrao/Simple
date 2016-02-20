using System;

namespace Simple
{
    public class Log
    {
        private string output = string.Empty;

        private int currentEchoLevel = 0;

        public void Echo(string status)
        {
            output += status + "\n";
        }

        public void Echo(string status, int levelAction)
        {
            PrintLevel(status, currentEchoLevel);
            currentEchoLevel += levelAction;
        }

        public void Flush()
        {
            Console.WriteLine(output);
            output = string.Empty;
        }

        private void PrintLevel(string status, int level = 0)
        {
            output += "-";
            for (int i = 0; i < level; i++)
                output += "|-";
            output += "" + status + "\n";
        }

        public void PrintAST(Node.Node n, int level = 0)
        {
            if (n == null)
            {
                PrintLevel("{null}", level);
                return;
            }

            Type t = n.GetType();

            if (t == typeof(Node.Program))
            {
                PrintLevel("{program}", level);
                PrintAST((n as Node.Program).E, level);
            }
            else if (t == typeof(Node.Undefined))
            {
                PrintLevel("(undefined)", level);
            }
            else if (t == typeof(Node.IfThen))
            {
                PrintLevel("[IF-THEN]", level);
                PrintAST((n as Node.IfThen).Test, level + 1);
                PrintAST((n as Node.IfThen).If, level + 1);
            }
            else if (t == typeof(Node.IfThenElse))
            {
                PrintLevel("[IF-THEN-ELSE]", level);
                PrintAST((n as Node.IfThenElse).Test, level + 1);
                PrintAST((n as Node.IfThenElse).If, level + 1);
                PrintAST((n as Node.IfThenElse).Else, level + 1);
            }
            else if (t == typeof(Node.WhileLoop))
            {
                PrintLevel("[WHILE-LOOP]", level);
                PrintAST((n as Node.WhileLoop).Test, level + 1);
                PrintAST((n as Node.WhileLoop).Loop, level + 1);
            }
            else if (t == typeof(Node.LoopWhile))
            {
                PrintLevel("[LOOP-WHILE]", level);
                PrintAST((n as Node.LoopWhile).Loop, level + 1);
                PrintAST((n as Node.LoopWhile).Test, level + 1);
            }
            else if (t == typeof(Node.Do))
            {
                PrintLevel("[DO]", level);
                PrintAST((n as Node.Do).E, level + 1);
            }
            else if (t == typeof(Node.Async))
            {
                PrintLevel("[ASYNC]", level);
                PrintAST((n as Node.Async).E, level + 1);
            }
            else if (t == typeof(Node.Structured))
            {
                PrintLevel("{structured}", level);
                PrintAST((n as Node.Structured).V, level + 1);
                PrintAST((n as Node.Structured).C, level + 1);
            }
            else if (t == typeof(Node.Assign))
            {
                PrintLevel("{assign}", level);
                PrintAST((n as Node.Assign).E, level + 1);
            }
            else if (t == typeof(Node.Call))
            {
                PrintLevel("[call]", level);
                PrintAST((n as Node.Call).S, level + 1);
                PrintAST((n as Node.Call).C, level + 1);
            }
            else if (t == typeof(Node.Access))
            {
                PrintLevel("{access}", level);
                PrintAST((n as Node.Access).Id, level + 1);
                PrintAST((n as Node.Access).C, level + 1);
            }
            else if (t == typeof(Node.Number))
            {
                PrintLevel("(number)" + (n as Node.Number).n, level);
            }
            else if (t == typeof(Node.Identifier))
            {
                PrintLevel("(identifier)" + (n as Node.Identifier).id, level);
            }
            else if (t == typeof(Node.Function))
            {
                PrintLevel("(function)", level);
                PrintAST((n as Node.Function).N, level + 1);
                PrintAST((n as Node.Function).E, level + 1);
            }
            else if (t == typeof(Node.Block))
            {
                PrintLevel("{block}", level);
                PrintAST((n as Node.Block).S, level + 1);
            }
            else if (t == typeof(Node.Sequence))
            {
                PrintLevel("{sequence}", level);
                PrintAST((n as Node.Sequence).E, level + 1);
                PrintAST((n as Node.Sequence).S, level);
            }            
            else if (t == typeof(Node.Names))
            {
                PrintLevel("{names}", level);
                PrintAST((n as Node.Names).Id, level + 1);
                PrintAST((n as Node.Names).N, level + 1);
            }
            else if (t == typeof(Node.Wrapped))
            {
                PrintLevel("{wrapped}", level);
                PrintAST((n as Node.Wrapped).E, level + 1);
            }
            else
                throw new Exception("dunno how to print" + t.ToString());
        }

    } // Output //

} // Simple //
