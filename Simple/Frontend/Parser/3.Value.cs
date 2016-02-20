using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Value
        {
            public static Node.Value Parse()
            {
                Node.Value n;

                if (IsLookahead(1, Tokens.Number))
                {
                    Match(Tokens.Number);

                    n = new Node.Number
                    {
                        n = (string)Parser.Lexer.Current().Attribute
                    };
                }
                else if (IsLookahead(1, Tokens.Identifier))
                {
                    Match(Tokens.Identifier);
                    Token identifier = Parser.Lexer.Current();

                    if (IsLookahead(1, Tokens.Operator, Lexemes.Dot) &&
                        IsLookahead(2, Tokens.Identifier, "reference"))
                    {
                        Match(Tokens.Operator, Lexemes.Dot);
                        Match(Tokens.Identifier);
                        n = new Node.Reference { id =(string) identifier.Attribute };
                    }
                    else
                        n = new Node.Identifier
                        {
                            id = (string)identifier.Attribute
                        };
                }
                else if (IsLookahead(1, Tokens.Keyword, Lexemes.Function))
                {
                    Match(Tokens.Keyword, Lexemes.Function);
                    Match(Tokens.Operator, Lexemes.ParenOpen);
                    Node.Names pl = Parser.Names.Parse();
                    Match(Tokens.Operator, Lexemes.ParenClose);
                    Node.Expression body = Parser.Expression.Parse();

                    n = new Node.Function
                    {
                        N = pl,
                        E = body
                    };
                }
                else if (IsLookahead(1, Tokens.Operator, Lexemes.BrackOpen))
                {
                    Match(Tokens.Operator, Lexemes.BrackOpen);

                    n = new Node.Wrapped
                    { 
                        E = Parser.Expression.Parse()
                    };

                    Match(Tokens.Operator, Lexemes.BrackClose);
                }
                else if (IsLookahead(1, Tokens.Operator, Lexemes.BraceOpen))
                {
                    n = Parser.Block.Parse();
                }
                else if (IsLookahead(1, Tokens.Keyword, Lexemes.Undefined))
                {
                    Match(Tokens.Keyword, Lexemes.Undefined);
                    n = new Node.Undefined { };
                }
                else
                    throw new Exceptions.Parser.UnexpectedLookahead("Parse(Value)", Lexer.Lookahead(1), 1);

                return n;
            }
        }
    }
}

