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
        int tableAddress = -1;

        public string GetName() => "goto";

        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Symbol })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.ExternalSymbol })) return "Arguments incorrectly formatted";

            symbolAddress = arguments[0].Value;
            tableAddress = arguments[0].Value2;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (tableAddress != -1)
            {
                interpreter.SymbolID++;
                interpreter.PushHierachy();
                interpreter.SymbolTableID = tableAddress;
            }

            interpreter.SymbolID = symbolAddress;
        }
    }
}
