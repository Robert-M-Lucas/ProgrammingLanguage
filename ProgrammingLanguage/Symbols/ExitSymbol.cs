using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class ExitSymbol : Symbol
    {
        public string GetName() => "exit";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesArgPattern(arguments, new ArgType[] { })) return "Arguments incorrectly formatted";

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            interpreter.running = false;
            interpreter.SymbolID += 1;
        }
    }
}
