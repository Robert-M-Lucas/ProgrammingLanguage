using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class SetSymbol : Symbol
    {
        int ObjectIndex;
        int Constant;

        public string GetName() => "set";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
            Constant = arguments[1].Value;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            symbolTable.Objects[ObjectIndex] = Constant;
            interpreter.SymbolID += 1;
        }
    }
}
