﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class AbsSymbol : Symbol
    {
        Argument? Object;

        public string GetName() => "abs";
        public string GetClose() => "| [Variable]";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesEvalPattern(arguments, new EvalType[] { EvalType.Variable})) return "Arguments incorrectly formatted";

            Object = arguments[0];

            return null;
        }

        public void Run(Interpreter interpreter)
        {
            int obj_index = Argument.EvaluateObjectArg(Object, interpreter);
            if (interpreter.CurrentSymbolTable.Objects[obj_index] < 0) { interpreter.CurrentSymbolTable.Objects[obj_index] *= -1; }
            
            interpreter.SymbolID++;
        }
    }
}