using System;
using System.Collections.Generic;

namespace Simple
{
    public class Interpreter
    {
        public Env Environment;

        public Interpreter(Env e)
        {
            this.Environment = e;
        }

        public Types.Object Run(Node.Node node)
        {
            System.Diagnostics.Stopwatch t = new System.Diagnostics.Stopwatch();
            t.Start();

            Types.Object e = Evaluate(node);

            t.Stop();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Returned: " + e.ToString() + " in "+ t.ElapsedMilliseconds);
            Console.ResetColor();
            return e;
        }

        private Types.Object Evaluate(Node.Node node)
        {
            if (node == null) // empty program
                return new Types.Undefined();    

            Type t = node.GetType();

            if      (t == typeof(Node.Program))         return EvaluateProgram      (node as Node.Program);
            else if (t == typeof(Node.IfThen))          return EvaluateIfThen       (node as Node.IfThen);
            else if (t == typeof(Node.IfThenElse))      return EvaluateIfThenElse   (node as Node.IfThenElse);
            else if (t == typeof(Node.LoopWhile))       return EvaluateLoopWhile    (node as Node.LoopWhile);
            else if (t == typeof(Node.WhileLoop))       return EvaluateWhileLoop    (node as Node.WhileLoop);
            else if (t == typeof(Node.Do))              return EvaluateDo           (node as Node.Do);
            else if (t == typeof(Node.Async))           return EvaluateAsync        (node as Node.Async);
            else if (t == typeof(Node.Structured))      return EvaluateStructured   (node as Node.Structured);
            else if (t == typeof(Node.Assign))          return EvaluateAssign       (node as Node.Assign);
            else if (t == typeof(Node.Access))          return NYI(node);
            else if (t == typeof(Node.Call))            return EvaluateCall         (node as Node.Call);
            else if (t == typeof(Node.Wrapped))         return EvaluateWrapped      (node as Node.Wrapped);
            else if (t == typeof(Node.Number))          return EvaluateNumber       (node as Node.Number);
            else if (t == typeof(Node.Identifier))      return EvaluateIdentifier   (node as Node.Identifier);
            else if (t == typeof(Node.Reference))       return EvaluateReference    (node as Node.Reference);
            else if (t == typeof(Node.Undefined))       return EvaluateUndefined    (node as Node.Undefined);
            else if (t == typeof(Node.Function))        return EvaluateClosure      (node as Node.Function);
            else if (t == typeof(Node.Block))           return EvaluateBlock        (node as Node.Block);
            else if (t == typeof(Node.Sequence))        return EvaluateSequence     (node as Node.Sequence);
            else if (t == typeof(Node.Names))           return NYI(node);
            else                                        return NYI(node);
        }
    
    //////

        private Types.Object NYI(Node.Node node)
        {
            throw new NotImplementedException();
        }

        private Types.Object EvaluateProgram(Node.Program node)
        {
            return Evaluate(node.E);
        }

        private Types.Object EvaluateIfThen(Node.IfThen node)
        {
            if (Evaluate(node.Test).True())
                return Evaluate(node.If);
            else
                return new Types.Undefined();
        }

        private Types.Object EvaluateIfThenElse(Node.IfThenElse node)
        {
            if(Evaluate(node.Test).True())
                return Evaluate(node.If);
            else
                return Evaluate(node.Else);
        }

        private Types.Object EvaluateWhileLoop(Node.WhileLoop node)
        {
            Types.List list = new Types.List();

            while (Evaluate(node.Test).True())
                list.Concatenate(Evaluate(node.Loop));
            return list;
        }

        private Types.Object EvaluateLoopWhile(Node.LoopWhile node)
        {
            Types.List list = new Types.List();

            list.AppendElement(Evaluate(node.Loop));
            while (Evaluate(node.Test).True())
                list.Concatenate(Evaluate(node.Loop));
            return list;
        }

        private Types.Object EvaluateDo(Node.Do node)
        {
            Evaluate(node.E);
            return new Types.Undefined();
        }

        private Types.Object EvaluateAsync(Node.Async node)
        {
            Interpreter i = new Interpreter(this.Environment.Copy());
            System.Threading.ThreadStart s = new System.Threading.ThreadStart(delegate()
            {
                i.Evaluate(node.E); // problema: le primitive stesse del linguaggio dovrebbero essere thread-safe
            });
            System.Threading.Thread t = new System.Threading.Thread(s);
            t.Start();
            return new Types.Undefined();
        }

    //////

        private Types.Object EvaluateStructured(Node.Structured node)
        {
            if      (node.C == null)                        return Evaluate(node.V);
            Type    compoundType = node.C.GetType();
            if      (compoundType == typeof(Node.Assign))   return StructuredDoAssign(node);
            else if (compoundType == typeof(Node.Call))     return StructuredDoCall(node);
            else
                return NYI(node);
        }

        private Types.Object StructuredDoAssign(Node.Structured node)
        {
            Types.Object lvalue = Evaluate(node.V);
            Types.Object rvalue = Evaluate(node.C).Itself;

            if (lvalue.IsIdentifier)
                this.Environment.SetLocal(lvalue as Types.Identifier, rvalue);
            else
                throw new Exception("Left-hand side is not a valid L-value");
            return rvalue;
        }

        private Types.Object StructuredDoCall(Node.Structured node)
        {
            Types.Object target     = Evaluate(node.V);
            Types.Object parameters = Evaluate(node.C);

            if (target.IsIdentifier)
            {
                if (target.Itself.IsClosure)
                    return CallClosure(target.Itself as Types.Closure, parameters);
                else if(target.Itself.IsList)
                {
                    return (target.Itself as Types.List).At(((parameters as Types.List).First.Value as Types.Number).Value-1);
                }
                else
                {
                    if (BuiltIns.Methods.ContainsKey((target as Types.Identifier).Name))
                        return BuiltIns.Methods[(target as Types.Identifier).Name](parameters);
                    else
                        return new Types.Undefined();
                }
            }
            else if (target.Itself.IsClosure)
            {
                return CallClosure(target.Itself as Types.Closure, parameters);
            }
            else if(target.Itself.IsList)
            {
                return (target.Itself as Types.List).At(((parameters as Types.List).First.Value as Types.Number).Value-1);
            }
            else
                return new Types.Undefined();
        }

        private Types.Object EvaluateAssign(Node.Assign node)
        {
            return Evaluate(node.E);
        }

        private Types.Object EvaluateCall(Node.Call node)
        {
            return Evaluate(node.S);
        }

    ////// 

        private Types.Object EvaluateWrapped(Node.Wrapped node)
        {
            return Evaluate(node.E);
        }

        private Types.Object EvaluateNumber(Node.Number node)
        {
            return new Types.Number { Value = Convert.ToInt32(node.n) };
        }

        private Types.Object EvaluateIdentifier(Node.Identifier node)
        {
            return this.Environment.GetLocal(node.id).Copy();
        }

        private Types.Object EvaluateReference(Node.Reference node)
        {
            return new Types.Reference { Pointer = this.Environment.GetLocal(node.id) };
        }

        private Types.Object EvaluateUndefined(Node.Undefined node)
        {
            return new Types.Undefined();
        } 

        private Types.Object EvaluateBlock(Node.Block node)
        {
            return Evaluate(node.S).Itself;
        }

        private Types.List EvaluateSequence(Node.Sequence node)
        {
            Types.List sequence = new Types.List();
            Node.Sequence next = node;
            while(next != null)
            {
                sequence.AppendElement(Evaluate(next.E));
                next = next.S;
            }
            return sequence;
        }
    
        private Types.Object EvaluateClosure(Node.Function node)
        {
            List<string> parameters = new List<string>();
            for (Node.Names next = node.N; next != null; next = next.N)
                parameters.Add(next.Id.id); 

            Types.Closure closure = new Types.Closure
            { 
                Body = node.E, 
                Parameters = parameters,
                Environment = new Env(this.Environment),
            };
            return closure;
        }

    //////

        private Types.Object CallClosure(Types.Closure closure, Types.Object parameters)
        {
            if (!parameters.Itself.IsUndefined)
            {
                Types.List.ListElement pointer = (parameters as Types.List).First;

                for (int i = 0; pointer != null; pointer = pointer.Next, i++)
                    closure.Environment.SetLocal(new Types.Identifier { Name = closure.Parameters[i] }, pointer.Value.Itself);
            }
            return new Interpreter(closure.Environment.Copy()).Evaluate(closure.Body);
        }

    }
}

