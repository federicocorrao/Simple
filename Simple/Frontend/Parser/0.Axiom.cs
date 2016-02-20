using System;

namespace Simple
{
    public static partial class Parser
    {
        public static class Axiom
        {
            public static Node.Program Parse()
            {
                Node.Program program = Parser.Program.Parse();
                Match(Tokens.Terminator);
                return program;
            }
        }
    }
}

