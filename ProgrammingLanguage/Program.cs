using ProgrammingLanguage;
using ProgrammingLanguage.Generator;

class Program
{
    public static void Main(string[] args)
    {
        Interpreter interpreter;

        if (args.Length > 0) { 
            if (args[0] == "gen")
            {
                Generate.Gen();
                return;
            }
            interpreter = new Interpreter(args[0]); 
        }
        else { interpreter = new Interpreter(@"code.rlc"); }
        interpreter.Process();
        interpreter.Run();
    }
}