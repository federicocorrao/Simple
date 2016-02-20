using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Block
        {
            public static Node.Block Parse()
            {
                Node.Block n;

                Match(Tokens.Operator, Lexemes.BraceOpen);

                n = new Node.Block
                {
                    S = Parser.Sequence.Parse(),
                    Label = null
                };

                Match(Tokens.Operator, Lexemes.BraceClose);

                return n;
            }
        }
    }
}

