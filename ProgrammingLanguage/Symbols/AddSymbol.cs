using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class AddSymbol : Symbol
    {
        int ObjectIndex;
        Argument? CompareTo;

        public string GetName() => "add";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Object }) &&
                !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant }) &&
                !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.VariableArrayReference })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
            CompareTo = arguments[1];

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            symbolTable.Objects[ObjectIndex] += (int)(Argument.EvaluateArg(CompareTo, interpreter, symbolTable)?.Int);
            interpreter.SymbolID++;
        }
    }
}
