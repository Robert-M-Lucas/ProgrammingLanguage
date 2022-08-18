using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    [Serializable]
    internal class GotoSymbol : Symbol
    {
        Argument Symbol;

        public string GetName() => "goto";
        public string GetClose() => "| [Goto]";

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

        public void Serialize(Stream s)
        {
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(s, this);
        }
    }
}
