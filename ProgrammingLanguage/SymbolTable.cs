using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguage
{
    internal class SymbolTable
    {
        public List<int> UnpackedObjects = new List<int>();
        public int[] Objects;

        public Symbol[] Symbols = new Symbol[0];

        public Dictionary<string, int>? TempObjectNames = new Dictionary<string, int>();
        public Dictionary<string, int>? TempSymbolNames = new Dictionary<string, int>();

        public void Excecute(int symbolID, Interpreter interpreter)
        {
            Symbols[symbolID].Run(interpreter, this);
        } 
    }
}
