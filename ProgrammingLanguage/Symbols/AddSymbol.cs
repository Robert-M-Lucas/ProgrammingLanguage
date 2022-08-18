using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    [Serializable]
    internal class AddSymbol : Symbol
    {
        Argument? Object;
        Argument? Modifier;

        public string GetName() => "add";
        public string GetClose() => "| [Variable] | [Value]";

        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Value })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Variable })) return "Arguments incorrectly formatted";

            Object = arguments[0];
            Modifier = arguments[1];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            interpreter.CurrentSymbolTable.Objects[Argument.EvaluateObjectArg(Object, interpreter)] += (int)(Argument.EvaluateIntArg(Modifier, interpreter));
            interpreter.SymbolID++;
        }

        public void Serialize(Stream s)
        {
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(s, this);
        }
    }
}
