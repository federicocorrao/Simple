using System;
using System.Collections.Generic;

namespace Simple
{
    public static partial class Parser
    {
        private static Lexer Lexer;
        public static Log Log;

        public static Node.Program Parse(Lexer lex)
        {
            Parser.Lexer = lex;
            Parser.Log = new Log();
            Node.Program tree = default(Node.Program);

            try
            {
                tree = Parser.Axiom.Parse();
            }
            catch (Exceptions.GenericException ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Parser.Log.Echo(ex.ToString());
                Parser.Log.Flush();
                Console.ResetColor();
            }
            return tree;
        }

    // //

        public static Token Match(Tokens t)
        {
            Token current = Parser.Lexer.Next();
            if (t.HasFlag(current.Name)) 
                return current;
            else
                throw new Exceptions.Parser.Mismatch(Parser.Lexer.Index, current, t);
        }

        public static Token Match(Tokens t, object attribute)
        {
            Token current = Match(t);
            if (current.Attribute.ToString() == attribute.ToString())
                return current;
            else
                throw new Exceptions.Parser.Mismatch(Parser.Lexer.Index, current, attribute);
        }

        public static bool IsLookahead(int k, Tokens t)
        {
            return t.HasFlag(Parser.Lexer.Lookahead(k).Name);
        }

        public static bool IsLookahead(int k, Tokens t, object attribute)
        {
            return IsLookahead(k, t) && 
                (Parser.Lexer.Lookahead(k).Attribute.ToString() == attribute.ToString());
        }
        
    } // class Parser //

} // namespace Simple //
