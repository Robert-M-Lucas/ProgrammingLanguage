using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    [Serializable]
    internal class WaitSymbol : Symbol
    {
        Argument argument;

        public string GetName() => "wait";
        public string GetClose() => "| [Time ms]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Value })
                && !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable })) return "Arguments incorrectly formatted";

            argument = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            Thread.Sleep(Argument.EvaluateIntArg(argument, interpreter));
            interpreter.SymbolID += 1;
        }

        public void Serialize(Stream s)
        {
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(s, this);
        }
    }
}
