using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class RandomSymbol : Symbol
    {
        int ObjectIndex;
        Argument? arg1;
        Argument? arg2;
        Random? r;

        public string GetName() => "random";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object })
            && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant, ArgumentType.Constant })
            && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Object, ArgumentType.Constant })
            && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Constant, ArgumentType.Object })
            && !Argument.MatchesPattern(arguments, new ArgumentType[] { ArgumentType.Object, ArgumentType.Object, ArgumentType.Object })) return "Arguments incorrectly formatted";

            ObjectIndex = arguments[0].Value;
           
            if (arguments.Length > 1)
            {
                arg1 = arguments[1];
                arg2 = arguments[2];
            }

            r = new Random();

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            int LowerBound = int.MinValue;
            int UpperBound = int.MaxValue;

            if (arg1 is not null)
            {
                if (arg1.Type == ArgumentType.Constant) LowerBound = arg1.Value;
                else LowerBound = symbolTable.Objects[arg1.Value];

                if (arg2.Type == ArgumentType.Constant) UpperBound = arg2.Value;
                else UpperBound = symbolTable.Objects[arg2.Value];
            }

            symbolTable.Objects[ObjectIndex] = r.Next(LowerBound, UpperBound);
            interpreter.SymbolID++;
        }
    }
}
