namespace ProgrammingLanguage
{
    internal enum ArgumentType {
        Constant = 0,
        Object = 1,
        Symbol = 2,
        ExternalSymbol = 3,
        Array = 4,
        ArrayConstant = 5,
    }

    internal class Argument
    {
        public int Value;
        public int Value2 = -1;
        public int[] ValueArr;

        public ArgumentType Type;

        public Argument(string input, List<SymbolTable>? symbolTables = null, int current_table = 0, Dictionary<string, int>? file_names = null)
        {
            if (input[0] == '@')
            {
                symbolTables = null;
                current_table = 0;
                file_names = null;
                input = input.Substring(1);
            }

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
            else if (symbolTables is not null && symbolTables[current_table].TempArrayNames.ContainsKey(input))
            {
                Type = ArgumentType.Array;
                var value_tuple = symbolTables[current_table].TempArrayNames[input];
                Value = value_tuple.Item1;
                Value2 = value_tuple.Item2;
            }
            else if (symbolTables is not null && file_names is not null && input.Contains('.'))
            {
                string[] path = input.Split('.');

                if (symbolTables[current_table].TempArrayNames.ContainsKey(path[0]))
                {
                    Type = ArgumentType.Object;
                    Value = symbolTables[current_table].TempArrayNames[path[0]].Item1 + int.Parse(path[1]);
                }
                else
                {
                    try
                    {
                        Value2 = file_names[path[0]];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ProcessingException($"File {path[0]} not found");
                    }

                    if (path[1] == "start" && !symbolTables[Value2].TempSymbolNames.ContainsKey("start"))
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
            }
            else
            {
                if (!int.TryParse(input, out Value))
                {
                    if (input.Length == 1)
                    {
                        Value = input[0];
                        Type = ArgumentType.Constant;
                    }
                    else
                    {
                        Type = ArgumentType.ArrayConstant;
                        Value = input.Length;
                        ValueArr = new int[input.Length];
                        for (int i = 0; i < input.Length; i++)
                        {
                            ValueArr[i] = input[i];
                        }
                    }
                }
                else
                {
                    Type = ArgumentType.Constant;
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
