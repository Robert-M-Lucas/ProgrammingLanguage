using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class LessSymbol : Symbol
    {
        Argument? Value1;
        Argument? Value2;
        Argument? Symbol;

        public string GetName() => "less";
        public string GetClose() => "| [Value] | [Value] | [Goto]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Variable, EvalType.Symbol }) &&
                !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Value, EvalType.Symbol }) &&
                !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Value, EvalType.Variable, EvalType.Symbol }) &&
                !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Value, EvalType.Value, EvalType.Symbol })) return "Arguments incorrectly formatted";

            Value1 = arguments[0];
            Value2 = arguments[1];
            Symbol = arguments[2];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            if (Argument.EvaluateIntArg(Value1, interpreter) < Argument.EvaluateIntArg(Value2, interpreter)) { Argument.ApplySymbol(Argument.EvaluateSymbolArg(Symbol, interpreter), interpreter); }
            else { interpreter.SymbolID++; }
        }
    }
}
