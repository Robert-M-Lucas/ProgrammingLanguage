using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    [Serializable]
    internal class InvertSymbol : Symbol
    {
        Argument Object;

        public string GetName() => "invert";
        public string GetClose() => "| [Variable]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable })) return "Arguments incorrectly formatted";

            Object = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            int obj_index = Argument.EvaluateObjectArg(Object, interpreter);
            if (interpreter.CurrentSymbolTable.Objects[obj_index] == 0) { interpreter.CurrentSymbolTable.Objects[obj_index] = 1; }
            else { interpreter.CurrentSymbolTable.Objects[obj_index] = 0; }

            interpreter.SymbolID += 1;
        }

        public void Serialize(Stream s)
        {
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(s, this);
        }
    }
}
