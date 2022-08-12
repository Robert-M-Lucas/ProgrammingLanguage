using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class GotoSymbol : Symbol
    {
        Argument Symbol;

        public string GetName() => "goto";

        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Symbol })) return "Arguments incorrectly formatted";

            Symbol = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            Argument.ApplySymbol(Argument.EvaluateSymbolArg(Symbol, interpreter), interpreter);
        }
    }
}
