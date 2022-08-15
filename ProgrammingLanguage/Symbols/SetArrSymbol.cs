using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class SetArrSymbol : Symbol
    {
        Argument? Array;
        Argument? Arg1;

        public string GetName() => "setarr";
        public string GetClose() => "| [Array Variable] | [Array Value]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.ArrayVariable, EvalType.ArrayValue })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.ArrayVariable, EvalType.ArrayVariable })
                 && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.ArrayVariable, EvalType.Variable })
                 && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.ArrayVariable, EvalType.Value })) return "Arguments incorrectly formatted";

            Array = arguments[0];
            Arg1 = arguments[1];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            Tuple<int, int> array_ref = Argument.EvaluateArrRefArg(Array, interpreter);

            int[]? new_arr = null;
            int? new_val = null;
            if (Arg1.EvalueType == EvalType.ArrayVariable || Arg1.EvalueType == EvalType.ArrayValue) 
            { 
                new_arr = Argument.EvaluateArrArg(Arg1, interpreter);
                if (new_arr.Length != array_ref.Item2) throw new ArgumentException("Array length mismatch");
            }
            else { new_val = Argument.EvaluateIntArg(Arg1, interpreter); }

            for (int i = array_ref.Item1; i < array_ref.Item1 + array_ref.Item2; i++)
            {
                if (new_arr is not null)
                {
                    interpreter.CurrentSymbolTable.Objects[i] = new_arr[i - array_ref.Item1];
                }
                else
                {
                    if (new_val is null) throw new NullReferenceException();
                    interpreter.CurrentSymbolTable.Objects[i] = (int)new_val;
                }
            }

            interpreter.SymbolID += 1;
        }
    }
}
