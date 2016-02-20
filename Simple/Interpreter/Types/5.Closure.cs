using System;
using System.Collections.Generic;

namespace Types
{
    public class Closure : BaseType
    {
        public Simple.Node.Expression   Body;
        public Simple.Env               Environment;
        public List<string>             Parameters;

        public override Types.Object Itself 
        {
            get { return this; } 
        }

        public override bool True() 
        {
            return true;
        }

        public override Object Copy() 
        {
            return new Closure 
            {
                Body = this.Body, 
                Environment = this.Environment, 
                Parameters = this.Parameters 
            }; 
        }

        public override string Serialize() 
        {
            return "(closure)"; 
        }

        //////

        public Types.Object New()
        {
            return new Undefined();
        }
    }
}

