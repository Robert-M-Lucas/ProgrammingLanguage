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
        int Object2Index = -1;
        int Constant;

        public string GetName() => "set";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Object })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
            if (arguments[1].Type == ArgumentType.Object)
            {
                Object2Index = arguments[1].Value;
            }
            else
            {
                Object2Index = -1;
                Constant = arguments[1].Value;
            }
            

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (Object2Index == -1)
            {
                symbolTable.Objects[ObjectIndex] = Constant;
            }
            else
            {
                symbolTable.Objects[ObjectIndex] = symbolTable.Objects[Object2Index];
            }
            interpreter.SymbolID += 1;
        }
    }
}
