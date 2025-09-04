// See https://aka.ms/new-console-template for more information

using awc.Trie;

class Program
{
    static void Main(string[] args)
    {
        string? filePath = "../../../data.txt";
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] != "-f" && args[i] != "--file") continue;
            
            if (i + 1 < args.Length)
            {
                filePath = args[i + 1];
                i++; 
            }
            else
            {
                Console.WriteLine("Error: The -f or --file flag requires a file path.");
                return;
            }
        }
        
        if (!string.IsNullOrEmpty(filePath))
        {
            ReadFromFile(filePath);
        }
        else
        {
            ReadFromStdin();
        }
    }

    private static void ReadFromFile(string filePath)
    {
        try
        {
            using var reader = new StreamReader(filePath);
            var trie = Trie.FromStream(reader);
            //PrintTrieWords(trie);
            Console.WriteLine($"WC is: {trie.Count()}");
            Console.WriteLine($"UWC is: {trie.CountUnique()}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The file '{filePath}' was not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
        }
    }

    private static void ReadFromStdin()
    {
        using var stdIn = Console.OpenStandardInput();
        using var reader = new StreamReader(stdIn);
        var trie = Trie.FromStream(reader);
        PrintTrieWords(trie);
    }

    private static void PrintTrieWords(Trie trie, bool withCount = true)
    {
        Console.WriteLine($"[{string.Join(", ", trie.GetAllWordsWithCounts())}]");
    }
}