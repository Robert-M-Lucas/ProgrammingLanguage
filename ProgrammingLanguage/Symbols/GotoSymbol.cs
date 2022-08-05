using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class GotoSymbol : Symbol
    {
        int symbolAddress;

        public string GetName() => "goto";

        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Symbol })) return "Arguments incorrectly formatted";

            symbolAddress = arguments[0].Value;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            interpreter.SymbolID = symbolAddress;
        }
    }
}
