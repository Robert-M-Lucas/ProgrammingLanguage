using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    [Serializable]
    internal class ExitSymbol : Symbol
    {
        public string GetName() => "exit";
        public string GetClose() => string.Empty;
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

        public void Serialize(Stream s)
        {
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(s, this);
        }
    }
}
