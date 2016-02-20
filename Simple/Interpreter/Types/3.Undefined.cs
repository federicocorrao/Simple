using System;

namespace Types
{
    public class Undefined : BaseType
    {
        public override Types.Object Itself 
        {
            get { return this; } 
        }

        public override Object Copy() 
        { 
            return new Undefined();
        }

        public override bool True() 
        {
            return false; 
        }

        public override string Serialize() 
        {
            return "(undefined)"; 
        }
    }
}

