using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage
{
    internal enum ArgumentType {
        Constant = 0,
        Object = 1,
        Symbol = 2
    }

    internal class Argument
    {
        public int Value;

        public ArgumentType Type;

        public Argument(string input, Dictionary<string, int> object_names, Dictionary<string, int> symbol_names)
        {
            if (object_names.ContainsKey(input))
            {
                Type = ArgumentType.Object;
                Value = object_names[input];
            }
            else if (symbol_names.ContainsKey(input))
            {
                Type = ArgumentType.Symbol;
                Value = symbol_names[input];
            }
            else
            {
                Type = ArgumentType.Constant;
                Value = int.Parse(input);
            }
        }

        public static bool MatchesPattern(Argument[] arguments, ArgumentType[] argumentTypes)
        {
            if (arguments.Length != argumentTypes.Length) { return false; }

            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i].Type != argumentTypes[i]) { return false; }
            }

            return true;
        }
    }
}
