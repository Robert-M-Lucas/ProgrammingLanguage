using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgrammingLanguage
{
    internal class ProcessingException : Exception
    {
        public ProcessingException() { }

        public ProcessingException(string? message) : base(message) { }

        public ProcessingException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    internal class FileProcessor
    {
        public List<SymbolTable> SymbolTables = new List<SymbolTable>();
        Dictionary<string, int> FileNames = new Dictionary<string, int>();
        List<string> pastFiles = new List<string>();
        List<string> pastFileNames = new List<string>();
        Dictionary<string, Type> SymbolNames = new Dictionary<string, Type>();

        public FileProcessor(string base_file_path)
        {
            BuildSymbolDictionary();
            ProcessFile(base_file_path, "base");
        }

        public void BuildSymbolDictionary()
        {
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(Symbol))))
            {
                Symbol symbol = (Symbol)(Activator.CreateInstance(mytype) ?? throw new NullReferenceException());
                SymbolNames[symbol.GetName()] = symbol.GetType();
            }
        }

        public void ProcessFile(string file_path, string calling_path)
        {
            if (Path.GetDirectoryName(file_path) == string.Empty) { file_path = Path.Join(Path.GetDirectoryName(calling_path), file_path); }
            string file_name = Path.GetFileNameWithoutExtension(file_path);

            if (pastFiles.Contains(file_path))
            {
                throw new ProcessingException($"Circular import in '{calling_path}' trying to import '{file_path}'");
            }
            else if (pastFileNames.Contains(file_name))
            {
                throw new ProcessingException($"Error importing '{file_path}' - name already used");
            }

            pastFiles.Add(file_path);
            pastFileNames.Add(file_name);

            SymbolTables.Add(new SymbolTable());
            int current_symbol_table = SymbolTables.Count - 1;

            FileNames.Add(file_name, current_symbol_table); // -1 index denotes table name

            string file_string;

            try
            {
                file_string = File.ReadAllText(file_path);
                file_string = file_string.Replace("\n", "");
                file_string = file_string.Replace("\r", "");
                file_string = file_string.Replace(" ", "");
                file_string = file_string.ToLower();
            }
            catch (IOException e)
            {
                throw new ProcessingException($"Error reading '{file_path}' - {e.Message}");
            }

            string[] file_split = file_string.Split(';');
            if (file_split.Length > 0 && file_split[file_split.Length - 1] == string.Empty)
            {
                file_split = file_split.SkipLast(1).ToArray();
            }

            Dictionary<string, int> object_names = new Dictionary<string, int>();
            Dictionary<string, int> symbol_names = new Dictionary<string, int>();

            List<int> completed_lines = new List<int>();

            for (int pass = -1; pass <= 3; pass++) {
                int line_no = -1;
                int meta_lines_passed = 0;

                while (line_no < file_split.Length - 1) 
                {
                    line_no++;

                    if (completed_lines.Contains(line_no)) { meta_lines_passed++; continue; }
                    string[] line = file_split[line_no].Split('|');
                    string command = line[0];

                    // Meta count phase
                    
                    if (pass == -1)
                    {
                        if (command == "import" || command == "let" || command == "tag") { meta_lines_passed += 1; }
                    }
                    // Import phase
                    else if (pass == 0)
                    {
                        if (command == "import")
                        {
                            if (Path.GetDirectoryName(line[1]) == string.Empty)
                            {
                                line[1] = Path.Join(Path.GetDirectoryName(file_path), line[1]);
                            }
                            ProcessFile(line[1], file_path);
                            completed_lines.Add(line_no);
                        }
                        else { break; }
                    }
                    // Variable declaration phase
                    else if (pass == 1)
                    {
                        if (command == "import") throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Import commands must be at the top of the file");
                        else if (command == "let") 
                        {
                            if (symbol_names.ContainsKey(line[1]) || object_names.ContainsKey(line[1]))
                            {
                                throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Name '{line[1]}' already exists");
                            }
                            object_names[line[1]] = SymbolTables[current_symbol_table].Objects.Count;
                            try { SymbolTables[current_symbol_table].Objects.Add(int.Parse(line[2])); } 
                            catch (FormatException) { throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Variable value incorrectly formatted");  }
                            
                            completed_lines.Add(line_no);
                        }
                        else {
                            break;
                        }
                    }
                    // Tag phase
                    else if (pass == 2)
                    {
                        if (command == "import") throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Import commands must be at the top of the file");
                        else if (command == "let") throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1} - Variable declatarions must be at the top of the file below imports]");
                        else if (command == "tag")
                        {
                            if (symbol_names.ContainsKey(line[1]) || object_names.ContainsKey(line[1]))
                            {
                                throw new ProcessingException($"Error processing '{file_path}' - Name '{line[1]}' already exists [{line_no+1}]");
                            }
                            symbol_names[line[1]] = line_no - meta_lines_passed;
                            meta_lines_passed++;
                            completed_lines.Add(line_no);
                        }
                    }
                    // Generic phase
                    else
                    {
                        if (command == "import") throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Import commands must be at the top of the file");
                        else if (command == "let") throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Variable declatarions must be at the top of the file below imports");
                        else 
                        {
                            if (!SymbolNames.ContainsKey(command)) throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] - Symbol '{command}' not found");

                            Symbol symbol = (Symbol)(Activator.CreateInstance(SymbolNames[command]) ?? throw new NullReferenceException());
                            Argument[] arguments = new Argument[line.Length-1];

                            for (int i = 0; i < arguments.Length; i++)
                            {
                                arguments[i] = new Argument(line[i+1], object_names, symbol_names);
                            }
                            
                            string? error = symbol.Build(arguments);
                            if (error is not null) throw new ProcessingException($"Error processing '{file_path}' [{line_no + 1}] '{command}' - Error processing symbol: {error}");
                            SymbolTables[current_symbol_table].Symbols[line_no-meta_lines_passed] = symbol;
                        }
                    }
                }



                if (pass == -1)
                {
                    SymbolTables[current_symbol_table].Symbols = new Symbol[file_split.Length - meta_lines_passed];
                }
            }
        }
    }
}
