using System;

namespace Types
{
    public abstract class Object
    {
        public bool IsIdentifier    { get { return this.GetType() == typeof(Identifier); } }
        public bool IsUndefined     { get { return this.GetType() == typeof(Undefined);  } }
        public bool IsNumber        { get { return this.GetType() == typeof(Number);     } }
        public bool IsClosure       { get { return this.GetType() == typeof(Closure);    } }
        public bool IsList          { get { return this.GetType() == typeof(List);       } }
        public bool IsBaseType      { get { return IsUndefined || IsNumber || IsClosure; } }

        public abstract Types.Object    Itself          { get; }
        public abstract Types.Object    Copy        ()  ;
        public abstract string          Serialize   ()  ;
        public abstract bool            True        ()  ;
        public          bool            False       ()  { return !this.True(); }

        public override string ToString()
        {
            return this.Serialize();
        }

        public static Object Deserialize()
        {
            return new Undefined(); 
        }
    }
}

