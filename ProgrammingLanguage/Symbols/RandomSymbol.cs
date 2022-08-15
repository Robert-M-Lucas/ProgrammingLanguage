using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class RandomSymbol : Symbol
    {
        Argument? Object;
        Argument? arg1;
        Argument? arg2;
        Random? r;

        public string GetName() => "random";
        public string GetClose() => "| [Variable] | [Opt Min] | [Opt Max]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable })
            && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Value, EvalType.Value })
            && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Variable, EvalType.Value })
            && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Value, EvalType.Variable })
            && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Variable, EvalType.Variable })) return "Arguments incorrectly formatted";

            Object = arguments[0];
           
            if (arguments.Length > 1)
            {
                arg1 = arguments[1];
                arg2 = arguments[2];
            }

            r = new Random();

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            int LowerBound = int.MinValue;
            int UpperBound = int.MaxValue;

            if (arg1 is not null)
            {
                LowerBound = Argument.EvaluateIntArg(arg1, interpreter);
                UpperBound = Argument.EvaluateIntArg(arg2, interpreter);
            }

            interpreter.CurrentSymbolTable.Objects[Argument.EvaluateObjectArg(Object, interpreter)] = r.Next(LowerBound, UpperBound);
            interpreter.SymbolID++;
        }
    }
}
