using System;

namespace Types
{
    public class List : Types.Object
    {
        public class ListElement
        {
            public Types.Object Value;
            public ListElement  Next;
            public ListElement  Prev;

            public ListElement Copy(ListElement prev)
            {
                ListElement e = new ListElement 
                { 
                    Value = Value.Copy(), 
                    Prev = prev, 
                };
                e.Next = (this.Next != null) ? this.Next.Copy(e) : null;
                return e;
            }

            public string Serialize()
            {
                string next = (this.Next == null) ? "\b" : this.Next.Serialize();
                return string.Format("{0},{1}", Value.Serialize(), next);
            }
        }

    //////

        public  ListElement  First;     // public per parametri
        private ListElement  Last;
        private int          length;

        public int  Length      { get { return length; } }
        public bool IsEmpty     { get { return this.length == 0; } }
        public bool IsSingleton { get { return this.length == 1; } }
        public bool IsPipe      { get { return this.IsSingleton && this.First.Value.IsList; } }

    //////

        public override Types.Object Itself
        {
            get {
                if (this.IsEmpty)
                    return new Types.Undefined();
                if (this.IsSingleton)
                    return this.First.Value.Itself;
                else
                    return this;
            } 
        }

        public override Object Copy()
        {
            List c = new List 
            {
                First = this.First.Copy(this.First),
                length = this.length
            };
            c.Last = c.FindLast();
            return c;
        }

        public override bool True()
        {
            ListElement pointer = this.First;
            bool istrue = true;
            while (pointer != null)
            {
                istrue = istrue && pointer.Value.True();
                if (istrue)
                    break;
                pointer = pointer.Next;
            }
            return istrue;
        }

        public override string Serialize()
        {
            if (this.IsEmpty)
                return string.Format("(undefined)",  length);
            if (this.IsSingleton)
                return this.Itself.Serialize();
            else
                return string.Format("(list {0})<{1}>", length, this.First.Serialize());
        }

    //////

        public List() 
        {
            Clear();
        }

        public void Clear()
        {
            this.First = null;
            this.Last = null;
            this.length = 0;
        }

        public void AppendElement(Types.Object o)
        {
            o = o.Itself;

            if (o.IsUndefined)
                return;

            if (this.IsEmpty)
            {
                this.First = new ListElement { Value = o, Prev = this.First };
                this.Last = this.First;
            }
            else
            {
                this.Last.Next = new ListElement { Value = o, Prev = this.Last };
                this.Last = this.Last.Next;
            }
            this.length++;
        }

        public void Concatenate(Types.Object o)
        {
            if (o.IsBaseType)
            {
                this.AppendElement(o);
                return;
            }
            ListElement pointer = (o as List).First;
            while (pointer != null)
            {
                this.AppendElement(pointer.Value);
                pointer = pointer.Next;
            }
        }

        private ListElement FindLast()
        {
            ListElement pointer = this.First;
            while (pointer.Next != null)
                pointer = pointer.Next;
            return pointer;
        }

        public Types.Object At(int index)
        {
            if (index >= this.length || index < 0)
                return new Undefined();
            ListElement pointer = this.First;
            for (int i = 0; i < index; i++)
                pointer = pointer.Next;
            return pointer.Value;
        }
    } 

}
