using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class PrintSymbol : Symbol
    {
        Argument printValue;
        public string GetName() => "print";
        public string? Build(Argument[] arguments)
        {
            if (arguments.Length != 1 || arguments[0].Type != ArgumentType.Constant && arguments[0].Type != ArgumentType.Object) return "Arguments incorrectly formatted";


            printValue = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            if (printValue.Type == ArgumentType.Constant) Console.WriteLine(printValue.Value);
            else Console.WriteLine(symbolTable.Objects[printValue.Value]);
            interpreter.SymbolID += 1;
        }
    }
}
