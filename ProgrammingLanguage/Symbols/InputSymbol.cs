using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class InputSymbol : Symbol
    {
        Argument Object;

        public string GetName() => "input";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable })) return "Arguments incorrectly formatted";

            Object = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            interpreter.CurrentSymbolTable.Objects[Argument.EvaluateObjectArg(Object, interpreter)] = new Argument(Console.ReadLine()??"0", null, 0, null).Value;
            interpreter.SymbolID += 1;
        }
    }
}
