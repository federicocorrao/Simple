using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Expression
        {
            public static Node.Expression Parse()
            {
                Node.Expression n;

                if (IsLookahead(1, Tokens.Keyword, Lexemes.If))
                {
                    Match(Tokens.Keyword, Lexemes.If);
                    Node.Expression test = Parser.Expression.Parse();
                    Match(Tokens.Keyword, Lexemes.Then);
                    Node.Expression expIf = Parser.Expression.Parse();

                    if (IsLookahead(1, Tokens.Keyword, Lexemes.Else))
                    {
                        Match(Tokens.Keyword, Lexemes.Else);
                        Node.Expression expElse = Parser.Expression.Parse();

                        n = new Node.IfThenElse
                        {
                            Test = test,
                            If = expIf,
                            Else = expElse 
                        };
                    }
                    else
                        n = new Node.IfThen
                        {
                            Test = test,
                            If = expIf
                        };
                }
                else if (IsLookahead(1, Tokens.Keyword, Lexemes.While))
                {
                    Match(Tokens.Keyword, Lexemes.While);
                    Node.Expression test = Parser.Expression.Parse();
                    Match(Tokens.Keyword, Lexemes.Loop);
                    Node.Expression exp = Parser.Expression.Parse();

                    n = new Node.WhileLoop
                    {
                        Test = test,
                        Loop = exp
                    };
                }
                else if (IsLookahead(1, Tokens.Keyword, Lexemes.Loop))
                {
                    Match(Tokens.Keyword, Lexemes.Loop);
                    Node.Expression exp = Parser.Expression.Parse();
                    Match(Tokens.Keyword, Lexemes.While);
                    Node.Expression test = Parser.Expression.Parse();

                    n = new Node.LoopWhile
                    {
                        Test = test,
                        Loop = exp
                    };
                }
                else if(IsLookahead(1, Tokens.Keyword, Lexemes.Do))
                {
                    Match(Tokens.Keyword, Lexemes.Do);

                    n = new Node.Do 
                    {
                        E = Parser.Expression.Parse()
                    };
                }
                else if(IsLookahead(1, Tokens.Keyword, Lexemes.Async))
                {
                    Match(Tokens.Keyword, Lexemes.Async);

                    n = new Node.Async 
                    {
                        E = Parser.Expression.Parse()
                    };
                }
                else if(IsLookahead(1, Tokens.Identifier                    ) ||
                        IsLookahead(1, Tokens.Number                        ) ||
                        IsLookahead(1, Tokens.Keyword, Lexemes.Function     ) ||
                        IsLookahead(1, Tokens.Operator, Lexemes.BrackOpen   ) ||
                        IsLookahead(1, Tokens.Operator, Lexemes.BraceOpen   ) ||
                        IsLookahead(1, Tokens.Keyword, Lexemes.Undefined    ))
                {
                    n = new Node.Structured
                    {
                        V = Parser.Value.Parse(),
                        C = Parser.Continuation.Parse()
                    };
                }
                else
                    throw new Exceptions.Parser.UnexpectedLookahead("Parse(Expression)", Lexer.Lookahead(1), 1);

                return n;
            }
        }
    }
}

/*else if (IsLookahead(1, Tokens.Identifier))
                {
                    if (IsLookahead(2, Tokens.Operator, Lexemes.Assign))
                    {
                        Match(Tokens.Identifier);
                        Token identifier = Parser.Lexer.Current();
                        Match(Tokens.Operator, Lexemes.Assign);

                        if(IsLookahead(1, Tokens.Operator, Lexemes.BraceOpen))
                        {
                            n = new Node.AssignBlock
                            {
                                Id = new Node.Identifier { Id = (string)identifier.Attribute },
                                Commands = Parser.Block.Parse().Commands
                            };
                        }
                        else
                            n = new Node.Assign
                            {
                                LValue = new Node.Identifier { Id = (string)identifier.Attribute },
                                RValue = Parser.Expression.Parse()
                            };
                    }
                    else
                        n = new Node.Uncaught
                        {
                            Exp = Parser.Expression.Parse()
                        };
                }
                else if (IsLookahead(1, Tokens.Operator, Lexemes.BraceOpen))
                {
                    n = new Node.FreeBlock
                    { 
                        Commands = Parser.Block.Parse().Commands
                    };
                }
                else if (IsLookahead(1, Tokens.Number                           ) ||
                         IsLookahead(1, Tokens.Keyword,  Lexemes.Function       ) ||
                         IsLookahead(1, Tokens.Operator, Lexemes.BrackOpen      ))
                {
                    n = new Node.Uncaught 
                    { 
                        Exp = Parser.Expression.Parse() 
                    };
                }*/
