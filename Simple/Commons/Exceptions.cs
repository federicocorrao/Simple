using System;

namespace Simple
{
namespace Exceptions {

    abstract class GenericException : Exception
    {
        protected string message;
        public override string Message      { get { return this.message; } }
        public override string ToString()   { return this.Message; }
        public virtual string ToConsole()   { return this.Message; /* String with console colors */ }
    }

    namespace Preprocessor { }
    namespace Lexer {
            class LookaheadOverflow : GenericException
            {
                public LookaheadOverflow(int currentIndex, int k)
                {
                    this.message = String.Format("[Exception.Parser] Lookahead Overflow\n=> in position {1} + {0}",
                                                 k.ToString(),
                                                 currentIndex.ToString());
                }
            }
        }

    namespace Parser
    {
        class Mismatch : GenericException
        {
            public Mismatch(int currentIndex, Token current, Token expected)
            {
                this.message = String.Format("[Exception.Parser] Token Mismatch (T#{0})\n=> {1} was expected instead of {2}",
                                             currentIndex.ToString(),
                                             expected.ToString(),
                                             current.Name.ToString());
            }
            public Mismatch(int currentIndex, Token current, object expected_attribute)
            {
                this.message = String.Format("[Exception.Parser] Attribute Mismatch (T#{0})\n=> {1} was expected instead of {2}",
                                             currentIndex.ToString(),
                                             expected_attribute.ToString(),
                                             current.Attribute.ToString());
            }
        }
        class UnexpectedLookahead : GenericException
        {
            public UnexpectedLookahead(string procedure, Token token, int lookahead)
            {
                    this.message = String.Format("[Exception.Parser] Unexpected Lookahead({0})\n=> {1} in {2}",
                                             lookahead.ToString(),
                                             token.ToString(),
                                             procedure);
            }
        }
        
    }
   
    namespace Interpreter
    {
        class RuntimeError : GenericException
        {
            public RuntimeError(string message)
            {
                this.message = "RuntimeError: " + message;
            }
        }
    }

} // Exceptions //
} // Simple // 
