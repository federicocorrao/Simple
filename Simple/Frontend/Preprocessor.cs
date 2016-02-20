using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Simple
{
    /* Preprocessor
     * Replaces critical lexemes with whitespace-wrapped markers.
     * This enables tokenization based on String.Split(whitespace).
     */
	public static class Preprocessor
	{
		private static readonly List<KeyValuePair<string, string>> ScanMappings = new List<KeyValuePair<string, string>>()
		{
            // In inverse lexicographic order (e.g. == before =)
            new KeyValuePair<string, string>("("        , " " + Lexemes.ParenOpen  .GetMarker() + " "),
            new KeyValuePair<string, string>(")"        , " " + Lexemes.ParenClose .GetMarker() + " "),
            new KeyValuePair<string, string>("{"        , " " + Lexemes.BraceOpen  .GetMarker() + " "),
            new KeyValuePair<string, string>("}"        , " " + Lexemes.BraceClose .GetMarker() + " "),
            new KeyValuePair<string, string>("["        , " " + Lexemes.BrackOpen  .GetMarker() + " "),
            new KeyValuePair<string, string>("]"        , " " + Lexemes.BrackClose .GetMarker() + " "),
            new KeyValuePair<string, string>("<"        , " " + Lexemes.AngOpen    .GetMarker() + " "),
            new KeyValuePair<string, string>(">"        , " " + Lexemes.AngClose   .GetMarker() + " "),
            new KeyValuePair<string, string>("="        , " " + Lexemes.Assign     .GetMarker() + " "),
            new KeyValuePair<string, string>("if"       , " " + Lexemes.If         .GetMarker() + " "),
            new KeyValuePair<string, string>("then"     , " " + Lexemes.Then       .GetMarker() + " "),
            new KeyValuePair<string, string>("else"     , " " + Lexemes.Else       .GetMarker() + " "),
            new KeyValuePair<string, string>("while"    , " " + Lexemes.While      .GetMarker() + " "),
            new KeyValuePair<string, string>("loop"     , " " + Lexemes.Loop       .GetMarker() + " "),
            new KeyValuePair<string, string>("do"       , " " + Lexemes.Do         .GetMarker() + " "),
            new KeyValuePair<string, string>("async"    , " " + Lexemes.Async      .GetMarker() + " "),
            new KeyValuePair<string, string>("undefined", " " + Lexemes.Undefined  .GetMarker() + " "),
            new KeyValuePair<string, string>("function" , " " + Lexemes.Function   .GetMarker() + " "),
            new KeyValuePair<string, string>("."        , " " + Lexemes.Dot        .GetMarker() + " "),
            new KeyValuePair<string, string>(","        , " " + Lexemes.Comma      .GetMarker() + " "),
        };

        private static readonly Regex CommentMatcher = new Regex("#(.)*$", RegexOptions.Compiled | RegexOptions.Multiline);

// //

		public static string Scan(string input)
		{
            string output = input;

            // single-line comments  
            while(true) {
                Match m = CommentMatcher.Match(output);
                if (m.Success) output = output.Remove(m.Index, m.Length);
                else break;
            }

            foreach(KeyValuePair<string, string> mapping in ScanMappings)
                output = output.Replace(mapping.Key, mapping.Value);

			return output;
		}
	
    }
}
