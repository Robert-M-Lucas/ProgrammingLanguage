using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ProgrammingLanguage.Generator
{

    internal static class Generate
    {
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        public static void Gen()
        {
            Console.Write("Enter path to extension base folder: ");
            string path = Console.ReadLine()??"";
            Console.WriteLine("Creating JSON...");

            List<string> kword_list = new List<string> { "import", "let", "arr", "tag" };
            List<string> extra_kword_list = new List<string> { "push", "start" };
            List<string> kword_close_list = new List<string> { "| [File Name]", "| [Var Name] | [Value]", "| [Arr Name] | [Value]", "| [Tag Name]" };
            List<string> symbol_list = new List<string> {  };
			List<string> symbol_close_list = new List<string> { };

            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                 .Where(mytype => mytype.GetInterfaces().Contains(typeof(Symbol))))
            {
                Symbol symbol = (Symbol)(Activator.CreateInstance(mytype) ?? throw new NullReferenceException());
                symbol_list.Add(symbol.GetName());
				symbol_close_list.Add(symbol.GetClose());
            }

            string tmLanguageString = @"{
	""$schema"": ""https://raw.githubusercontent.com/martinring/tmlanguage/master/tmlanguage.json"",
	""name"": ""RLC"",
	""patterns"": [
		{
			""include"": ""#keywords""
		},
		{
			""include"": ""#functions""
		},
{
			""include"": ""#comments""
		}
	],
	""repository"": {
		""keywords"": {
			""patterns"": [{
				""name"": ""keyword.control.rlc"",
				""match"": ""\\b(" + string.Join('|', kword_list) + '|' + string.Join('|', extra_kword_list) + @")\\b""
			}]
		},
		""functions"": {
			""patterns"": [{
				""name"": ""support.function"",
				""match"": ""\\b(" + string.Join('|', symbol_list) + @")\\b""
			}]
		},
        ""comments"": {
            ""patterns"": [{
                ""name"": ""comment.line.number-sign.xpp"",
                ""match"": "";;.*""
            }]
        }
	},
	""scopeName"": ""source.rlc""
}";
            File.WriteAllText(Path.Join(path, "syntaxes\\rlc.tmLanguage.json"), tmLanguageString);

			string lang_config_string = @"{
    ""comments"": {
		""lineComment"": "";;"",
	},
    ""autoClosingPairs"": [";

            /*
			for (int i = 0; i < kword_list.Count; i++)
			{
				lang_config_string += "\n\t\t{ \"open\":\"" + kword_list[i] + " \", \"close\": \"" + kword_close_list[i] + ";\" },";  
			}
            for (int i = 0; i < symbol_list.Count; i++)
            {
                lang_config_string += "\n\t\t{ \"open\":\"" + symbol_list[i] + " \", \"close\": \"" + symbol_close_list[i] + ";\" },";
            }
            */

            lang_config_string += "\n\t]\n}";

            File.WriteAllText(Path.Join(path, "language-configuration.json"), lang_config_string);

			Console.Write("Enter windows user: ");

			string user = Console.ReadLine()??"";

            Console.WriteLine("Installing extension...");

            string extension_path = $"C:\\Users\\{user}\\.vscode\\extensions";

			if (Directory.Exists(extension_path + "\\rlc-lang"))
			{
                Console.WriteLine(extension_path + "\\rlc-lang");
				Directory.Delete(extension_path + "\\rlc-lang", true);
			}

            CopyDirectory(path, extension_path + "\\rlc-lang", true);
        }
    }
}
