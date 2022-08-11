using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class PrintCSymbol : Symbol
    {
        Argument printValue;

        bool new_line = true;

        public string GetName() => "printc";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Constant })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.ArrayConstant })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Constant, ArgumentType.Constant })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Array, ArgumentType.Constant })
                && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.ArrayConstant, ArgumentType.Constant })) return "Arguments incorrectly formatted";


            printValue = arguments[0];

            if (arguments.Length > 1)
            {
                if (arguments[1].Value == 1) new_line = false;
                else return "Argument 2 must be 1";
            }

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (new_line)
            {
                if (printValue.Type == ArgumentType.Constant) Interpreter.PrintLine(Convert.ToChar(printValue.Value).ToString());
                else if (printValue.Type == ArgumentType.ArrayConstant)
                {
                    for (int i = 0; i < printValue.ValueArr.Length; i++)
                    {
                        if (!(i == printValue.ValueArr.Length-1))
                        {
                            Interpreter.Print(Convert.ToChar(printValue.ValueArr[i]).ToString());
                        }
                        else
                        {
                            Interpreter.PrintLine(Convert.ToChar(printValue.ValueArr[i]).ToString());
                        }
                    }
                }
                else if (printValue.Type == ArgumentType.Array)
                {
                    for (int i = 0; i < printValue.Value2; i++)
                    {
                        if (!(i == printValue.Value2 - 1))
                        {
                            Interpreter.Print(Convert.ToChar(symbolTable.Objects[printValue.Value + i]).ToString());
                        }
                        else
                        {
                            Interpreter.PrintLine(Convert.ToChar(symbolTable.Objects[printValue.Value + i]).ToString());
                        }
                    }
                }
                else Interpreter.PrintLine(Convert.ToChar(symbolTable.Objects[printValue.Value]).ToString());
            }
            else
            {
                if (printValue.Type == ArgumentType.Constant) Interpreter.Print(Convert.ToChar(printValue.Value).ToString());
                else if (printValue.Type == ArgumentType.ArrayConstant)
                {
                    for (int i = 0; i < printValue.ValueArr.Length; i++)
                    {
                        Interpreter.Print(Convert.ToChar(printValue.ValueArr[i]).ToString());
                    }
                }
                else if (printValue.Type == ArgumentType.Array)
                {
                    for (int i = 0; i < printValue.Value2; i++)
                    {
                        Interpreter.Print(Convert.ToChar(symbolTable.Objects[printValue.Value + i]).ToString());
                    }
                }
                else Interpreter.Print(Convert.ToChar(symbolTable.Objects[printValue.Value]).ToString());
            }
            interpreter.SymbolID += 1;
        }
    }
}
