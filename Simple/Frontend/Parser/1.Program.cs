using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Program
        {
            public static Node.Program Parse()
            {
                Node.Program n;

                if (IsLookahead(1, Tokens.Terminator))
                {
                    n = null;  // empty program
                }
                else
                    n = new Node.Program
                    { 
                        E = Parser.Expression.Parse()
                    };

                return n;
            }
        }
    }
}
