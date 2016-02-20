using System;
using System.Collections.Generic;
using Types;

namespace Simple
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string input = System.IO.File.ReadAllText("Utils/loop.simple");
            ParseAndRun(input);
        }

        private static void ParseAndRun(string input)
        {
            Lexer l = new Lexer(Preprocessor.Scan(input));
            Node.Node tree = Parser.Parse(l);
            Interpreter r = new Interpreter(new Env());
            r.Run(tree);

            string repl = string.Empty;
            do {
                repl = Console.ReadLine();
                r.Run(Parser.Parse(new Lexer(Preprocessor.Scan(repl))));
            }
            while (repl != string.Empty);
        }

    }
}


