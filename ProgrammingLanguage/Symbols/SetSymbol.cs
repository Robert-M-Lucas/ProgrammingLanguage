using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class SetSymbol : Symbol
    {
        Argument? Object;
        Argument? Value;

        public string GetName() => "set";
        public string GetClose() => "| [Variable] | [Value]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Value })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Variable })) return "Arguments incorrectly formatted";

            Object = arguments[0];
            Value = arguments[1];
            

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            /*
            Console.WriteLine("*" + Argument.EvaluateObjectArg(Object, interpreter));
            Console.WriteLine("*" + Argument.EvaluateIntArg(Value, interpreter));
            */
            interpreter.CurrentSymbolTable.Objects[Argument.EvaluateObjectArg(Object, interpreter)] = Argument.EvaluateIntArg(Value, interpreter);
            interpreter.SymbolID += 1;
        }
    }
}
