using System;
using System.Collections.Generic;

namespace Simple
{
    public static class BuiltIns
    {
        public static Dictionary<string, Func<Types.Object, Types.Object>> Methods = 
                  new Dictionary<string, Func<Types.Object, Types.Object>>
        {
            { "eq", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
                {
                    Types.Object first = (l as Types.List).First.Value.Itself;
                    Types.Object second = (l as Types.List).First.Next.Value.Itself;

                    if(first.IsNumber && second.IsNumber)
                    {
                        return new Types.Number 
                        {
                            Value = ((first as Types.Number).Value == (second as Types.Number).Value) ? 1 : 0
                        };
                    }
                    return new Types.Undefined();
                }
            )},
            { "neq", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
            {
                    Types.Object first = (l as Types.List).First.Value.Itself;
                    Types.Object second = (l as Types.List).First.Next.Value.Itself;

                    if(first.IsNumber && second.IsNumber)
                    {
                        return new Types.Number 
                        {
                            Value = ((first as Types.Number).Value != (second as Types.Number).Value) ? 1 : 0
                        };
                    }
                    return new Types.Undefined();
                }
            )},
            { "sub", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
                {
                    Types.Object first = (l as Types.List).First.Value.Itself;
                    Types.Object second = (l as Types.List).First.Next.Value.Itself;

                    if(first.IsNumber && second.IsNumber)
                    {
                        return new Types.Number 
                        {
                            Value = (first as Types.Number).Value - (second as Types.Number).Value
                        };
                    }
                    return new Types.Undefined();
                }
            )},
            { "sum", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
                {
                    Types.Object first = (l as Types.List).First.Value.Itself;
                    Types.Object second = (l as Types.List).First.Next.Value.Itself;

                    if(first.IsNumber && second.IsNumber)
                    {
                        return new Types.Number 
                        {
                            Value = (first as Types.Number).Value + (second as Types.Number).Value
                        };
                    }
                    return new Types.Undefined();
                }
            )},
            { "mul", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
                                                              {
                    Types.Object first = (l as Types.List).First.Value.Itself;
                    Types.Object second = (l as Types.List).First.Next.Value.Itself;

                    if(first.IsNumber && second.IsNumber)
                    {
                        return new Types.Number 
                        {
                            Value = (first as Types.Number).Value * (second as Types.Number).Value
                        };
                    }
                    return new Types.Undefined();
                }
            )},
            { "print", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
                {
                    Console.WriteLine(l.Serialize());
                    return new Types.Undefined();
                }
            )},
            { "sleep", new Func<Types.Object, Types.Object>(delegate(Types.Object l)
                {
                    if(l.Itself.IsNumber)
                        System.Threading.Thread.Sleep((l.Itself as Types.Number).Value * 1000);
                    return new Types.Undefined();
                }
            )},
        };
    }
}

