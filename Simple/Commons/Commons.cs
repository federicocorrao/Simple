using System;
using System.ComponentModel;
using System.Reflection;

namespace Simple
{
// Abstract Syntax Tree Node Definitions //

    namespace Node
    {
        public abstract     class Node                           { }
        public              class Program        : Node          { public Expression E;  }

        public abstract     class Expression     : Node          { }
        public              class IfThen         : Expression    { public Expression Test, If; }
        public              class IfThenElse     : Expression    { public Expression Test, If, Else; }
        public              class WhileLoop      : Expression    { public Expression Test, Loop; }
        public              class LoopWhile      : Expression    { public Expression Test, Loop; }
        public              class Do             : Expression    { public Expression E; }
        public              class Async          : Expression    { public Expression E; }
        public              class Structured     : Expression    { public Value V; public Continuation C; }

        public abstract     class Continuation   : Node          { }
        public              class Call           : Continuation  { public Sequence S; public Continuation C; }
        public              class Access         : Continuation  { public Identifier Id; public Continuation C; }
        public              class Assign         : Continuation  { public Expression E; }

        public abstract     class Value          : Expression    { }
        public              class Number         : Value         { public string n; }
        public              class Identifier     : Value         { public string id; }
        public              class Reference      : Value         { public string id; }
        public              class Function       : Value         { public Names N; public Expression E; }
        public              class Undefined      : Value         { }
        public              class Wrapped        : Value         { public Expression E; }
        public              class Block          : Value         { public Sequence S; public Identifier Label; }

        public              class Sequence       : Node          { public Expression E; public Sequence S; }
        public              class Names          : Node          { public Identifier Id; public Names N; }
    }

// Token Definitions //

    [Flags]
    public enum Tokens
    {
        Unrecognized    = 1,
        Indentation     = 2,
        Terminator      = 4, 
        Keyword         = 8, 
        Identifier      = 16,
        Operator        = 32,
        Number          = 64, 
    }

    public class Token
    {
        public readonly Tokens Name;
        public readonly object Attribute;

        public Token(Tokens name, object Attribute)
        {
            this.Name = name;
            this.Attribute = Attribute;
        }

        public override string ToString()
        {
            return string.Format("[Token ({0}, [{2}]{1})]",
                                 this.Name.ToString(),
                                 this.Attribute.ToString(),
                                 this.Attribute.GetType().ToString());
        }
    }

// Lexemes //

    public enum Lexemes
    {
        [DescriptionAttribute("§parenopen§" )]  ParenOpen,
        [DescriptionAttribute("§parenclose§")]  ParenClose,
        [DescriptionAttribute("§braceopen§" )]  BraceOpen,
        [DescriptionAttribute("§braceclose§")]  BraceClose,
        [DescriptionAttribute("§brackopen§" )]  BrackOpen,
        [DescriptionAttribute("§brackclose§")]  BrackClose,
        [DescriptionAttribute("§angopen§"   )]  AngOpen,
        [DescriptionAttribute("§angclose§"  )]  AngClose,
        [DescriptionAttribute("§assign§"    )]  Assign,
        [DescriptionAttribute("§if§"        )]  If,
        [DescriptionAttribute("§then§"      )]  Then, 
        [DescriptionAttribute("§else§"      )]  Else, 
        [DescriptionAttribute("§while§"     )]  While,
        [DescriptionAttribute("§loop§"      )]  Loop,
        [DescriptionAttribute("§do§"        )]  Do, 
        [DescriptionAttribute("§undefined§" )]  Undefined,
        [DescriptionAttribute("§function§"  )]  Function,
        [DescriptionAttribute("§dot§"       )]  Dot,
        [DescriptionAttribute("§comma§"     )]  Comma,
        [DescriptionAttribute("§async§"     )]  Async,
    }

    public static class LexemesExtensionMethods
    { 
        public static string GetMarker(this Lexemes value) 
        { 
            FieldInfo fi = value.GetType().GetField(value.ToString()); 
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false); 
            return attributes[0].Description;            
        }

        public static Lexemes GetLexeme(string value)
        {
            foreach (Lexemes l in Enum.GetValues(typeof(Lexemes)))
            {
                if (GetMarker(l) == value)
                    return l;
            }
            throw new Exception("Unrecognized Lexeme value: " + value);
        }
    }

}
