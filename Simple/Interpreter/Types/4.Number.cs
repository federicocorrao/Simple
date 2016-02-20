using System;

namespace Types
{
    public class Number : BaseType 
    {
        public int Value { get; set; }

        public override Types.Object Itself 
        { 
            get { return this; } 
        }

        public override Object Copy()
        {
            return new Number { Value = this.Value };
        }

        public override bool True() 
        {
            return (Value != 0);
        }

        public override string Serialize() 
        {
            return string.Format("(number: {0})", Value); 
        }
    }
}
