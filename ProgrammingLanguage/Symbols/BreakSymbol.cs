using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class BreakSymbol : Symbol
    {
        public string GetName() => "break";
        public string GetClose() => string.Empty;
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesArgPattern(arguments, new ArgType[] { })) return "Arguments incorrectly formatted";

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            interpreter.DropHierachy();
        }
    }
}
