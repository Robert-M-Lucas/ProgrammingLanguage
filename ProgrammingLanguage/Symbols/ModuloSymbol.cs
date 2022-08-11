using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class ModuloSymbol : Symbol
    {
        int ObjectIndex;
        Argument? CompareTo;

        public string GetName() => "modulo";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Object }) &&
                !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
            CompareTo = arguments[1];

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (CompareTo is not null)
            {
                if (CompareTo.Type == ArgumentType.Constant) { symbolTable.Objects[ObjectIndex] %= CompareTo.Value; }
                else if (CompareTo.Type == ArgumentType.Object) { symbolTable.Objects[ObjectIndex] %= symbolTable.Objects[CompareTo.Value]; }
            }
            interpreter.SymbolID++;
        }
    }
}
