using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Sequence
        {
            public static Node.Sequence Parse()
            {
                Node.Sequence n;

                if (IsLookahead(1, Tokens.Operator, Lexemes.BraceClose) ||
                    IsLookahead(1, Tokens.Operator, Lexemes.ParenClose))
                {
                    n = null;
                }
                else
                {
                    Node.Expression e = Parser.Expression.Parse();

                    if (IsLookahead(1, Tokens.Operator, Lexemes.Comma))
                        Match(Tokens.Operator, Lexemes.Comma);

                    n = new Node.Sequence
                    {
                        E = e,
                        S = Parser.Sequence.Parse()
                    };
                }

                return n;
            }
        }
    }
}

