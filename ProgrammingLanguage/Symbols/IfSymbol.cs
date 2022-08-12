﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class IfSymbol : Symbol
    {
        Argument? Value1;
        Argument? Symbol;

        public string GetName() => "if";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable, EvalType.Symbol }) &&
                !Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Value, EvalType.Symbol })) return "Arguments incorrectly formatted";

            Value1 = arguments[0];
            Symbol = arguments[1];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            if (Argument.EvaluateIntArg(Value1, interpreter) == 1) { Argument.ApplySymbol(Argument.EvaluateSymbolArg(Symbol, interpreter), interpreter); }
            else { interpreter.SymbolID++; }
        }
    }
}
