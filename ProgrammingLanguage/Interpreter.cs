using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ProgrammingLanguage
{

    internal class Interpreter
    {
        public static StreamWriter ConsoleWriter = new StreamWriter(Console.OpenStandardOutput(), Encoding.UTF8, 8192);

        string? baseFilePath;

        List<SymbolTable>? symbolTables;

        public List<Tuple<int, int>> tableHierachy = new List<Tuple<int, int>>();

        public int SymbolID = 0;
        // Symbol table 0 is for all variables
        public int SymbolTableID = 0;

        bool force_running = true;
        public bool running = true;

        public Interpreter(string base_file_path)
        {
            if (!File.Exists(base_file_path)) { Console.WriteLine("> Base file not found"); return; }

            baseFilePath = base_file_path;
        }

        public void Process()
        {
            if (baseFilePath is null) { Console.WriteLine("> No base file to process"); return; }

            try
            {
                Console.WriteLine("> Processing started");
                symbolTables = new FileProcessor(baseFilePath).SymbolTables;
                Console.WriteLine("> Processing succeeded");
            }
            catch (ProcessingException e)
            {
                Console.WriteLine("> Processing failed with error:\n" + e.Message);
            }
            
        }

        public static void Print(int message)
        {
            ConsoleWriter.Write(message);
        }

        public static void Print(string message)
        {
            ConsoleWriter.Write(message);
        }

        public static void PrintLine(int message)
        {
            ConsoleWriter.Write(message + "\n");
        }

        public static void PrintLine(string message)
        {
            ConsoleWriter.Write(message + "\n");
        }

        public void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            force_running = false;
        }

        public void PushHierachy()
        {
            tableHierachy.Add(new Tuple<int, int>(SymbolTableID, SymbolID));
        }

        public void Run()
        {
            if (symbolTables is null) { Console.WriteLine("> No symbol table"); return; }

            Stopwatch s = new Stopwatch();
            s.Start();
            Console.WriteLine("> Excecuting");
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);
            while (true)
            {
                while (SymbolID < symbolTables[SymbolTableID].Symbols.Length)
                {
                    // Console.WriteLine(SymbolID);
                    if (!force_running || !running) break;
                    symbolTables[SymbolTableID].Excecute(SymbolID, this);
                }
                if (!force_running || !running) break;

                if (tableHierachy.Count > 0)
                {
                    SymbolID = tableHierachy[tableHierachy.Count - 1].Item2;
                    SymbolTableID = tableHierachy[tableHierachy.Count - 1].Item1;
                    tableHierachy.RemoveAt(tableHierachy.Count - 1);
                }
                else
                {
                    break;
                }
            }
            s.Stop();
            ConsoleWriter.Flush();

            if (force_running) Console.WriteLine("> Program finished excecution");
            else Console.WriteLine("> Program terminated");

            Console.WriteLine($"Time taken - {s.ElapsedMilliseconds}ms");
        }
    }
}
