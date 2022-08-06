using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class GreaterSymbol : Symbol
    {
        int ObjectIndex;
        Argument? CompareTo;
        int SymbolIndex;

        public string GetName() => "greater";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Object, ArgumentType.Symbol }) &&
                !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant, ArgumentType.Symbol })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
            CompareTo = arguments[1];
            SymbolIndex = arguments[2].Value;

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (CompareTo is not null)
            {
                if ((CompareTo.Type == ArgumentType.Constant && symbolTable.UnpackedObjects[ObjectIndex] > CompareTo.Value)
                    || (CompareTo.Type == ArgumentType.Object && symbolTable.UnpackedObjects[ObjectIndex] > symbolTable.UnpackedObjects[CompareTo.Value])) { interpreter.SymbolID = SymbolIndex; }
                else { interpreter.SymbolID++; }
            }
        }
    }
}
