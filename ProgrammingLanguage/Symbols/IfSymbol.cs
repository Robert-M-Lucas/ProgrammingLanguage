using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class IfSymbol : Symbol
    {
        int ObjectIndex;
        int SymbolIndex;

        public string GetName() => "if";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Symbol })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
            SymbolIndex = arguments[1].Value;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (symbolTable.UnpackedObjects[ObjectIndex] == 1) { interpreter.SymbolID = SymbolIndex; }
            else { interpreter.SymbolID++; }
        }
    }
}
