using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class LessSymbol : Symbol
    {
        int ObjectIndex;
        Argument? CompareTo;
        int SymbolIndex;

        public string GetName() => "less";
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
                if (symbolTable.Objects[ObjectIndex] < (int)(Argument.EvaluateArg(CompareTo, interpreter, symbolTable)?.Int)) { interpreter.SymbolID = SymbolIndex; }
                else { interpreter.SymbolID++; }
            }
        }
    }
}
