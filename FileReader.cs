using System;
using System.IO;

public static class FileReader
{
    public static void PrintFileContents()
    {
        const string path = @"c:\temp\data.txt";
        try
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"File not found: {path}");
                return;
            }

            string content = File.ReadAllText(path);
            Console.WriteLine(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }
}
