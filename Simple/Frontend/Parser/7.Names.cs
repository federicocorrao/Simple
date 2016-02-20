using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Names
        {
            public static Node.Names Parse()
            {
                Node.Names n;

                if (IsLookahead(1, Tokens.Identifier))
                {
                    Match(Tokens.Identifier);
                    Token identifier = Parser.Lexer.Current();

                    if (IsLookahead(1, Tokens.Operator, Lexemes.Comma))
                        Match(Tokens.Operator, Lexemes.Comma);

                    n = new Node.Names
                    {
                        Id = new Node.Identifier
                        { 
                            id = (string)identifier.Attribute
                        },
                        N = Parser.Names.Parse()
                    };
                }
                else if (IsLookahead(1, Tokens.Operator, Lexemes.ParenClose))
                {
                    n = null;
                }
                else
                    throw new Exceptions.Parser.UnexpectedLookahead("Parse(Names)", Lexer.Lookahead(1), 1);

                return n;
            }
        }
    }
}

