using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class InputSymbol : Symbol
    {
        int ObjectIndex;

        public string GetName() => "input";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            symbolTable.Objects[ObjectIndex] = new Argument(Console.ReadLine()??Convert.ToChar(0).ToString(), null, 0, null).Value;
            interpreter.SymbolID += 1;
        }
    }
}
