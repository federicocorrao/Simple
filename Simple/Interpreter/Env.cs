using System;
using System.Collections.Generic;

namespace Simple
{
    public class Env
    {
        public Env Parent;

        public Dictionary<string, Types.Identifier> Locals = new Dictionary<string, Types.Identifier>
        {
        };

        public Env()            { this.Parent = null;   }
        public Env(Env parent)  { this.Parent = parent; }

    //////

        public Types.Identifier GetLocal(string identifier)
        {
            if (Locals.ContainsKey(identifier))
                return Locals[identifier];
            else
                if (Parent != null)
                    return Parent.GetLocal(identifier);
                else
                    return new Types.Identifier { Name = identifier, Value = new Types.Undefined(), Closure = null };
        }

        public void SetLocal(Types.Identifier identifier, Types.Object value)
        {
            if (!Locals.ContainsKey(identifier.Name))
                Locals.Add(identifier.Name, new Types.Identifier { Name = identifier.Name });
            Locals[identifier.Name].Value = value;
        }

        public Env Copy()
        {
            Env c = new Env();
            c.Parent = this.Parent;
            foreach (KeyValuePair<string, Types.Identifier> p in this.Locals)
                c.Locals.Add(p.Key, p.Value.Copy() as Types.Identifier);
            return c;
        }
    }
}

