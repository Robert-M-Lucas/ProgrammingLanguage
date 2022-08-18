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
        public int[] Objects = new int[0];

        public Symbol[] Symbols = new Symbol[0];

        public Dictionary<string, int>? TempObjectNames;
        public Dictionary<string, Tuple<int, int>>? TempArrayNames;
        public Dictionary<string, int>? TempSymbolNames;

        public SymbolTable() { InitializeTemps(); }

        public SymbolTable(bool createTemps)
        {
            if (createTemps) InitializeTemps();
        }

        public void InitializeTemps()
        {
            TempObjectNames = new Dictionary<string, int>();
            TempArrayNames = new Dictionary<string, Tuple<int, int>>();
            TempSymbolNames = new Dictionary<string, int>();
        }

        public bool ContainsName(string name)
        {
            return (TempArrayNames?.ContainsKey(name) ?? false) || (TempObjectNames?.ContainsKey(name) ?? false) || (TempSymbolNames?.ContainsKey(name) ?? false);
        }

        public void Excecute(int symbolID, Interpreter interpreter)
        {
            Symbols[symbolID].Run(interpreter);
        } 
    }
}
