﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage.Symbols
{
    internal class SkipSymbol : Symbol
    {
        public string GetName() => "skip";
        public string? Build(Argument[] arguments)
        {
            if (!Argument.MatchesPattern(arguments, new ArgumentType[] { })) return "Arguments incorrectly formatted";

            return null;
        }

        public void Run(Interpreter interpreter, SymbolTable symbolTable)
        {
            interpreter.SymbolID += 1;
        }
    }
}
