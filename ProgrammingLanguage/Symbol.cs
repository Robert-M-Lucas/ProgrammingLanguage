﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage
{
    internal interface Symbol
    {
        public string GetName();

        public string? Build(Argument[] arguments);

        public void Run(Interpreter interpreter);
    }
}
