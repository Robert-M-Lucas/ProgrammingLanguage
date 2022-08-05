using ProgrammingLanguage;

class Program
{
    public static void Main(string[] args)
    {
        Interpreter interpreter;

        if (args.Length > 0) { interpreter = new Interpreter(args[0]); }
        else { interpreter = new Interpreter(@"C:\Users\rober\Documents\C# projects\ProgrammingLanguage\ProgrammingLanguage\code.rlc"); }
        interpreter.Process();
        interpreter.Run();
    }
}