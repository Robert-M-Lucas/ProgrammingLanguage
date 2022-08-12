using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class SetArrSymbol : Symbol
    {
        int ArrayIndex = 0;
        Argument? Arg1;
        Argument? Arg2;

        public string GetName() => "setarr";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.Array })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.ArrayConstant })
                 && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.Constant, ArgumentType.Constant })
                 && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.Constant, ArgumentType.Object })
                 && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.Object, ArgumentType.Constant })
                 && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.Object, ArgumentType.Object })) return "Arguments incorrectly formatted";

            ArrayIndex = arguments[0].Value;
            Arg1 = arguments[1];
            if (arguments.Length > 2)
            {
                Arg2 = arguments[2];
            }

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (Arg1.Type == ArgumentType.ArrayConstant)
            {
                for (int i = 0; i < Arg1.ValueArr.Length; i++)
                {
                    symbolTable.Objects[ArrayIndex+i] = Arg1.ValueArr[i];
                }
            }
            else if (Arg1.Type == ArgumentType.ArrayConstant)
            {
                for (int i = 0; i < Arg1.ValueArr.Length; i++)
                {
                    symbolTable.Objects[ArrayIndex + i] = Arg1.ValueArr[i];
                }
            }
            else
            {
                int index;
                int value;

                if (Arg1.Type == ArgumentType.Constant)
                {
                    index = Arg1.Value;
                }
                else
                {
                    index = symbolTable.Objects[Arg1.Value];
                }

                if (Arg2.Type == ArgumentType.Constant)
                {
                    value = Arg2.Value;
                }
                else
                {
                    value = symbolTable.Objects[Arg2.Value];
                }
                symbolTable.Objects[ArrayIndex +  index] = value;
            }
            interpreter.SymbolID += 1;
        }
    }
}
