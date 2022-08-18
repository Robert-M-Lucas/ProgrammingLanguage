using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    [Serializable]
    internal class PrintCSymbol : Symbol
    {
        Argument? printValue;

        public string GetName() => "printc";
        public string GetClose() => "| [Value]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Value })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.ArrayValue })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.ArrayVariable })) return "Arguments incorrectly formatted";

            printValue = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            
            if (printValue.EvalueType == EvalType.Value || printValue.EvalueType == EvalType.Variable) Interpreter.Print(Convert.ToChar(Argument.EvaluateIntArg(printValue, interpreter)).ToString());
            else if (printValue.EvalueType == EvalType.ArrayVariable || printValue.EvalueType == EvalType.ArrayValue)
            {
                int[] arr = Argument.EvaluateArrArg(printValue, interpreter);
                for (int i = 0; i < arr.Length; i++)
                {
                    Interpreter.Print(Convert.ToChar(arr[i]).ToString());
                }
            }
            interpreter.SymbolID += 1;
        }

        public void Serialize(Stream s)
        {
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(s, this);
        }
    }
}
