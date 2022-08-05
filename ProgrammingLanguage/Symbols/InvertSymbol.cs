using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class InvertSymbol : Symbol
    {
        int ObjectIndex;

        public string GetName() => "invert";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (symbolTable.UnpackedObjects[ObjectIndex] == 0) { symbolTable.UnpackedObjects[ObjectIndex] = 1; }
            else { symbolTable.UnpackedObjects[ObjectIndex] = 0; }

            interpreter.SymbolID += 1;
        }
    }
}
