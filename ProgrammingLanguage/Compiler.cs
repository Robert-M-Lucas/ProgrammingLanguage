using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using File = System.IO.File;
using System.Diagnostics;

namespace ProgrammingLanguage
{
    internal static class Compiler
    {
        public static Stream Append(this Stream destination, Stream source)
        {
            destination.Position = destination.Length;
            source.CopyTo(destination);

            return destination;
        }

        public static void Compile(string codePath)
        {
            Console.WriteLine("> Compile Mode");
            Console.WriteLine("> Processing");
            FileProcessor fileProcessor = new FileProcessor(codePath);

            string folder_dir = (Path.GetDirectoryName(codePath) ?? throw new NullReferenceException()) + "\\Compiled"; ;
            Console.WriteLine($"> Creating compiled directory at [{folder_dir}]");

            Directory.CreateDirectory($"{folder_dir}");

            for (int tableID = 0; tableID < fileProcessor.SymbolTables.Count; tableID++)
            {
                string file_path = folder_dir + $"\\{tableID}.rlctable";
                Console.WriteLine($"> Writing to [{file_path}]");
                FileStream f = File.OpenWrite(file_path);
                f.Write(BitConverter.GetBytes(fileProcessor.SymbolTables[tableID].Objects.Length)); // Length: 4
                foreach (Symbol s in fileProcessor.SymbolTables[tableID].Symbols)
                {
                    MemoryStream symbolStream = new MemoryStream();
                    s.Serialize(symbolStream);
                    f.Write(BitConverter.GetBytes(symbolStream.Length));
                    f.Write(symbolStream.ToArray());
                }
                f.Close();
            }
            Console.WriteLine("> Compiling done");
        }

        public static List<SymbolTable> Decompile(string compiledPath)
        {
            Console.WriteLine("> Decompiling started");
            Stopwatch s = new Stopwatch();
            s.Start();

            List<SymbolTable> symbolTables = new List<SymbolTable>();
            string[] fileArray = Directory.GetFiles(compiledPath);

            foreach (string file in fileArray)
            {
                byte[] data = File.ReadAllBytes(file);
                SymbolTable symbolTable = new SymbolTable(false);
                List<Symbol> symbols = new List<Symbol>();
                symbolTable.Objects = new int[BitConverter.ToInt32(data[..4])];

                int cursor = 4;
                while (cursor < data.Length)
                {
                    int length = BitConverter.ToInt32(data[cursor..(cursor+4)]);
                    cursor += 4;
                    BinaryFormatter bf = new BinaryFormatter();
#pragma warning disable SYSLIB0011
                    symbols.Add((Symbol)bf.Deserialize(new MemoryStream(data[cursor..(cursor+length)])));
                    cursor += length;
                }

                symbolTable.Symbols = symbols.ToArray();
                symbolTables.Add(symbolTable);
            }

            s.Stop();
            Console.WriteLine($"> Decompilation succeeded in {s.ElapsedMilliseconds}ms");

            return symbolTables;
        }
    }
}
