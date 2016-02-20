using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Simple
{
    public class Lexer
    {
        private static readonly char[] Splitters = new char[] { ' ', '\t', '\n', '\r' };

        private static readonly List<KeyValuePair<Tokens, Regex>> Patterns = new List<KeyValuePair<Tokens, Regex>>()
        {
            new KeyValuePair<Tokens, Regex>(Tokens.Operator, new Regex("^(" + 
                Lexemes.ParenOpen   .GetMarker()   + "|" + 
                Lexemes.ParenClose  .GetMarker()   + "|" +
                Lexemes.BraceOpen   .GetMarker()   + "|" +
                Lexemes.BraceClose  .GetMarker()   + "|" +
                Lexemes.BrackOpen   .GetMarker()   + "|" +
                Lexemes.BrackClose  .GetMarker()   + "|" +
                Lexemes.Dot         .GetMarker()   + "|" +
                Lexemes.Comma       .GetMarker()   + "|" +
                Lexemes.AngOpen     .GetMarker()   + "|" +
                Lexemes.AngClose    .GetMarker()   + "|" +
                Lexemes.Assign      .GetMarker()   + ")$")),
           
            // problema: "done" viene preso con DO-ne
            new KeyValuePair<Tokens, Regex>(Tokens.Keyword, new Regex("^(" +
                Lexemes.If          .GetMarker()   + "|" + 
                Lexemes.Then        .GetMarker()   + "|" +
                Lexemes.Else        .GetMarker()   + "|" +
                Lexemes.While       .GetMarker()   + "|" +
                Lexemes.Do          .GetMarker()   + "|" +
                Lexemes.Async       .GetMarker()   + "|" +
                Lexemes.Loop        .GetMarker()   + "|" +
                Lexemes.Function    .GetMarker()   + "|" +
                Lexemes.Undefined   .GetMarker()   + ")$")),

            new KeyValuePair<Tokens, Regex>(Tokens.Number, new Regex("^[0-9]+$")),

            new KeyValuePair<Tokens, Regex>(Tokens.Identifier, new Regex("^[a-zA-Z\\-_\\+\\/\\*<>&|]+$")),
        };

    // Private: Lexer implementation //

        private List<Token> Tokenize(string input)
        {
            List<Token> output = new List<Token>();

            string[] words = input.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                bool recognized = false;
                foreach (KeyValuePair<Tokens, Regex> kv in Patterns) 
                {
                    if (kv.Value.IsMatch(word))
                    {
                        if (kv.Key == Tokens.Operator || kv.Key == Tokens.Keyword)
                            output.Add(new Token(kv.Key, LexemesExtensionMethods.GetLexeme(word)));
                        else if (kv.Key == Tokens.Identifier)
                            output.Add(new Token(kv.Key, word));
                        else if (kv.Key == Tokens.Number)
                            output.Add(new Token(kv.Key, word));

                        recognized = true;
                        break;
                    }
                }
                if (!recognized)
                    output.Add(new Token(Tokens.Unrecognized, word));
            }

            output.Add(new Token(Tokens.Terminator, string.Empty));
            return output;
        }
   
    // Public: Stream handling methods //

        private List<Token> Stream;
        public Log Log;

        private int index;
        public  int Index { get { return this.index; } }

        public Lexer(string input)
        {
            this.index = -1;
            this.Stream = this.Tokenize(input);
            this.Log = new Log();
        }

        public Token Current()
        {
            return Lookahead(0);
        }

        public Token Lookahead(int k)
        {
            if ((this.index + k >= 0) && (this.index + k < this.Stream.Count))
                return this.Stream[this.index + k];
            else
                throw new Exceptions.Lexer.LookaheadOverflow(this.index, k);
        }

        public Token Next()
        {
            this.index++;
            return Current();
        }
    }

}