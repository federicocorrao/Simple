using System;

namespace Types
{
    public class Reference : BaseType
    {
        public Types.Object Pointer;

        public override Object Itself
        {
            get { return Pointer; }
        }

        public override Object Copy()
        {
            return new Reference
            {
                Pointer = this.Pointer
            };
        }

        public override bool True()
        {
            return this.Pointer.True();
        }

        public override string Serialize()
        {
            return string.Format("(reference: {0})", this.Pointer.Serialize());
        }
    }
}

