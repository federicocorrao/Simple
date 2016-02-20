using System;

namespace Types
{
    public class Identifier : Types.Object
    { 
        public string Name;
        public Types.Object Value;
        public Types.Closure Closure;

        public override Types.Object Itself 
        {
            get { return Value.IsIdentifier ? Value.Itself : Value; } 
        }

        public override bool True() 
        {
            return this.Itself.True();
        }

        public override Object Copy()
        {
            return new Identifier 
            {
                Name = this.Name, 
                Value = this.Value.Copy() 
            }; 
        }

        public override string Serialize() 
        {
            return string.Format("{0}", Value.Serialize()); 
        }
    }
}

