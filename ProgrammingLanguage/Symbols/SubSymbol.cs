using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class SubSymbol : Symbol
    {
        Argument? Object;
        Argument? Modifier;

        public string GetName() => "sub";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Value })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Variable })) return "Arguments incorrectly formatted";

            Object = arguments[0];
            Modifier = arguments[1];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            interpreter.CurrentSymbolTable.Objects[Argument.EvaluateObjectArg(Object, interpreter)] -= (int)(Argument.EvaluateIntArg(Modifier, interpreter));
            interpreter.SymbolID++;
        }
    }
}
