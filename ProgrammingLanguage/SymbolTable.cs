using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage
{
    internal class SymbolTable
    {
        public List<int> Objects = new List<int>();

        public Symbol[] Symbols = new Symbol[0];

        public void Excecute(int symbolID, Interpreter interpreter)
        {
            Symbols[symbolID].Run(interpreter, this);
        } 
    }
}
