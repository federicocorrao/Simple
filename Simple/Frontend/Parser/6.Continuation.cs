using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Continuation
        {
            public static Node.Continuation Parse()
            {
                Node.Continuation n;

                if (IsLookahead(1, Tokens.Operator, Lexemes.ParenOpen))
                {
                    Match(Tokens.Operator, Lexemes.ParenOpen);
                    Node.Sequence sequence = Parser.Sequence.Parse();
                    Match(Tokens.Operator, Lexemes.ParenClose);

                    n = new Node.Call
                    {
                        S = sequence,
                        C = Parser.Continuation.Parse()
                    };
                }
                else if (IsLookahead(1, Tokens.Operator, Lexemes.Dot))
                {
                    Match(Tokens.Operator, Lexemes.Dot);
                    Match(Tokens.Identifier);
                    Token identifier = Parser.Lexer.Current();

                    n = new Node.Access
                    {
                        Id = new Node.Identifier 
                        {
                            id = (string) identifier.Attribute
                        },
                        C = Parser.Continuation.Parse()
                    };
                }
                else if (IsLookahead(1, Tokens.Operator, Lexemes.Assign))
                {
                    Match(Tokens.Operator, Lexemes.Assign);

                    n = new Node.Assign
                    {
                        E = Parser.Expression.Parse()
                    };
                }
                else
                    n = null;

                return n;
            }
        }
    }
}

