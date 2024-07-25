namespace orchestrator.api.Extensions;

public static class StaticMethod
{
    public static void PrintMessage(string message)
    {
        Console.WriteLine(string.Empty);
        Console.WriteLine("====================");
        Console.WriteLine(message);
        Console.WriteLine("====================");
        Console.WriteLine(string.Empty);
    }
}
