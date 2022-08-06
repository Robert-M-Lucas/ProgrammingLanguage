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
        Symbol = 2,
        ExternalSymbol = 3,
    }

    internal class Argument
    {
        public int Value;
        public int Value2 = -1;

        public ArgumentType Type;

        public Argument(string input, List<SymbolTable>? symbolTables, int current_table, Dictionary<string, int>? file_names)
        {
            if (symbolTables is not null && symbolTables[current_table].TempObjectNames.ContainsKey(input))
            {
                Type = ArgumentType.Object;
                Value = symbolTables[current_table].TempObjectNames[input];
            }
            else if (symbolTables is not null && symbolTables[current_table].TempSymbolNames.ContainsKey(input))
            {
                Type = ArgumentType.Symbol;
                Value = symbolTables[current_table].TempSymbolNames[input];
            }
            else if (symbolTables is not null && file_names is not null && input.Contains('.'))
            {
                string[] path = input.Split('.');
                try
                {
                    Value2 = file_names[path[0]];
                }
                catch (KeyNotFoundException)
                {
                    throw new ProcessingException($"File {path[0]} not found");
                }
                
                if (path[1] == "start")
                {
                    Value = 0;
                }
                else
                {
                    try
                    {
                        Value = symbolTables[Value2].TempSymbolNames[path[1]];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ProcessingException($"Tag {path[1]} not found");
                    }
                }
                Type = ArgumentType.ExternalSymbol;
            }
            else
            {
                Type = ArgumentType.Constant;

                if (!int.TryParse(input, out Value))
                {
                    if (input.Length == 1)
                    {
                        Value = input[0];
                    }
                }
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
