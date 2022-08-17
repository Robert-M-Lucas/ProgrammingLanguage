namespace ProgrammingLanguage
{
    internal enum EvalType
    {
        Value = 0,
        Variable = 1,
        ArrayVariable = 2,
        ArrayValue = 3,
        Symbol = 4
    }

    internal enum ArgType {
        Constant = 0,
        Object = 1,
        Symbol = 2,
        ExternalSymbol = 3,
        Array = 4,
        ArrayConstant = 5,
        VariableArrayReference = 6
    }

    /*
    struct IntOrArr
    {
        public int Int = 0;
        public int[]? Arr = null;
        public bool IsInt = true;

        public IntOrArr(int integer)
        {
            Int = integer;
            IsInt = true;
        }

        public IntOrArr(int[] array)
        {
            Arr = array;
            IsInt = false;
        }
    }
    */

    internal struct SymbolRef
    {
        public int TableID;
        public int SymbolID;
        public bool Push;

        public SymbolRef (int table_id, int symbol_id, bool push)
        {
            TableID = table_id;
            SymbolID = symbol_id;
            Push = push;
        }
    }

    internal class Argument
    {
        public int Value = -1;
        public int Value2 = -1;
        public int Value3 = -1;
        public int[]? ValueArr;

        public ArgType ArgumentType;
        public EvalType EvalueType;

        public Argument(string input, List<SymbolTable>? symbolTables = null, int current_table = 0, Dictionary<string, int>? file_names = null)
        {
            if (input == string.Empty)
            {
                ArgumentType = ArgType.Constant;
                EvalueType = EvalType.Value;
                Value = 0;
                return;
            }
            if (input[0] == '@')
            {
                symbolTables = null;
                current_table = 0;
                file_names = null;
                input = input.Substring(1);
                string temp_input = "";
                bool escaped = false;
                foreach (char c in input)
                {
                    if (c == '\\' && !escaped) escaped = true;
                    else
                    {
                        if (escaped)
                        {
                            switch (c)
                            {
                                case '\\':
                                    temp_input += "\\";
                                    break;
                                case '_':
                                    temp_input += " ";
                                    break;
                                case 'p':
                                    temp_input += "|";
                                    break;
                                case 's':
                                    temp_input += ";";
                                    break;
                                case 'n':
                                    temp_input += "\n";
                                    break;
                                default:
                                    temp_input += "\\" + c;
                                    break;
                            }
                        }
                        else
                        {
                            temp_input += c;
                        }
                        escaped = false;
                    }

                }
                input = temp_input;
            }

            if (symbolTables is not null && symbolTables[current_table].TempObjectNames.ContainsKey(input))
            {
                ArgumentType = ArgType.Object;
                EvalueType = EvalType.Variable;
                Value = symbolTables[current_table].TempObjectNames[input];
            }
            else if (symbolTables is not null && symbolTables[current_table].TempSymbolNames.ContainsKey(input))
            {
                ArgumentType = ArgType.Symbol;
                EvalueType = EvalType.Symbol;
                Value = symbolTables[current_table].TempSymbolNames[input];
            }
            else if (symbolTables is not null && symbolTables[current_table].TempArrayNames.ContainsKey(input))
            {
                ArgumentType = ArgType.Array;
                EvalueType = EvalType.ArrayVariable;
                var value_tuple = symbolTables[current_table].TempArrayNames[input];
                Value = value_tuple.Item1;
                Value2 = value_tuple.Item2;
            }
            else if (symbolTables is not null && file_names is not null && input.Contains('.'))
            {
                string[] path = input.Split('.');

                if (symbolTables[current_table].TempArrayNames.ContainsKey(path[0]))
                {
                    if (symbolTables[current_table].TempObjectNames.ContainsKey(path[1]))
                    {
                        ArgumentType = ArgType.VariableArrayReference;
                        EvalueType = EvalType.Variable;
                        Value = symbolTables[current_table].TempArrayNames[path[0]].Item1;
                        Value2 = symbolTables[current_table].TempObjectNames[path[1]];
                    }
                    else if (path[1] == "length") {
                        ArgumentType = ArgType.Constant;
                        EvalueType = EvalType.Value;
                        Value = symbolTables[current_table].TempArrayNames[path[0]].Item2;
                    }
                    else
                    {
                        ArgumentType = ArgType.Object;
                        EvalueType = EvalType.Variable;
                        Value = symbolTables[current_table].TempArrayNames[path[0]].Item1 + int.Parse(path[1]);
                    }
                }
                else
                {
                    try
                    {
                        Value2 = file_names[path[0]];
                    }
                    catch (KeyNotFoundException)
                    {
                        try
                        {
                            Value = symbolTables[current_table].TempSymbolNames[path[0]];
                            if (path[1] == "push") Value3 = 1;
                            ArgumentType = ArgType.Symbol;
                            EvalueType = EvalType.Symbol;
                            return;
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ProcessingException($"File or symbol {path[0]} not found");
                        }
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

                    if (path.Length == 3 && path[2] == "push") Value3 = 1;

                    ArgumentType = ArgType.ExternalSymbol;
                    EvalueType = EvalType.Symbol;
                } 
            }
            else
            {
                if (!int.TryParse(input, out Value))
                {
                    if (input.Length == 1)
                    {
                        Value = input[0];
                        ArgumentType = ArgType.Constant;
                        EvalueType = EvalType.Value;
                    }
                    else
                    {
                        ArgumentType = ArgType.ArrayConstant;
                        EvalueType = EvalType.ArrayValue;
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
                    ArgumentType = ArgType.Constant;
                    EvalueType = EvalType.Value;
                }
            }
        }

        public static bool MatchesArgPattern(Argument[] arguments, ArgType[] argumentTypes)
        {
            if (arguments.Length != argumentTypes.Length) { return false; }

            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i].ArgumentType != argumentTypes[i]) { return false; }
            }

            return true;
        }

        public static bool MatchesEvalPattern(Argument[] arguments, EvalType[] evalTypes)
        {
            if (arguments.Length != evalTypes.Length) { return false; }

            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i].EvalueType != evalTypes[i]) { return false; }
            }

            return true;
        }

        public static int EvaluateIntArg(Argument arg, Interpreter interpreter)
        {
            switch (arg.ArgumentType)
            {
                case ArgType.Constant:
                    return arg.Value;
                case ArgType.Object:
                    return interpreter.CurrentSymbolTable.Objects[arg.Value];
                case ArgType.VariableArrayReference:
                    return interpreter.CurrentSymbolTable.Objects[arg.Value + interpreter.CurrentSymbolTable.Objects[arg.Value2]];
                default:
                    throw new FormatException($"Arg {arg.ArgumentType} can't be treated as int");
            }
        }

        public static int EvaluateObjectArg(Argument arg, Interpreter interpreter)
        {
            switch (arg.ArgumentType)
            {
                case ArgType.Object:
                    return arg.Value;
                case ArgType.VariableArrayReference:
                    return arg.Value + interpreter.CurrentSymbolTable.Objects[arg.Value2];
                default:
                    throw new FormatException($"Arg {arg.ArgumentType} can't be treated as object");
            }
        }

        public static int[] EvaluateArrArg(Argument arg, Interpreter interpreter)
        {
            switch (arg.ArgumentType)
            {
                case ArgType.Array:
                    return interpreter.CurrentSymbolTable.Objects[arg.Value..(arg.Value+arg.Value2)];
                case ArgType.ArrayConstant:
                    if (arg.ValueArr is null) throw new NullReferenceException();
                    return arg.ValueArr[..];
                default:
                    throw new FormatException($"Arg {arg.ArgumentType} can't be treated as arr");
            }
        }

        public static Tuple<int, int> EvaluateArrRefArg(Argument arg, Interpreter interpreter)
        {
            switch (arg.ArgumentType)
            {
                case ArgType.Array:
                    return new Tuple<int, int>(arg.Value, arg.Value2);
                default:
                    throw new FormatException($"Arg {arg.ArgumentType} can't be treated as arr reference");
            }
        }
        public static SymbolRef EvaluateSymbolArg(Argument arg, Interpreter interpreter)
        {
            switch (arg.ArgumentType)
            {
                case ArgType.Symbol:
                    return new SymbolRef(interpreter.SymbolTableID, arg.Value, arg.Value3 == 1);
                case ArgType.ExternalSymbol:
                    return new SymbolRef(arg.Value2, arg.Value, arg.Value3 == 1);
                default:
                    throw new FormatException($"Arg {arg.ArgumentType} can't be treated as symbol");
            }
        }

        public static void ApplySymbol(SymbolRef symbol, Interpreter interpreter)
        {
            if (symbol.Push) interpreter.PushHierachy();
            interpreter.SymbolTableID = symbol.TableID;
            interpreter.SymbolID = symbol.SymbolID;
        }
    }
}
