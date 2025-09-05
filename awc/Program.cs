// See https://aka.ms/new-console-template for more information

using awc.FlagParser;
using awc.Trie;

class Program
{
    private const string FileFlag = "--file";
    private const string UniqueCountFlag = "--unique-count";
    private const string WordCountFlag = "--word-count";
    private const string UniqueWords = "--unique-words";
    
    private static readonly FlagParser Parser = 
        new FlagParserBuilder()
            .AddFlag(FileFlag, FlagKind.Value, "The file which you want to read")
            .AddFlag(UniqueCountFlag, FlagKind.Mode, "Toggle if you want the count of unique words")
            .AddFlag(WordCountFlag, FlagKind.Mode, "Toggle if you want the count of words")
            .AddFlag(UniqueWords, FlagKind.Mode, "Toggle if you want all the unique words, can be combined with -wc for counts of each word")
            .Build();

    private static void Main(string[] args)
    {
        Parser.ParseArgs(args);

        var filePath = Parser.GetValueByFullFlag(FileFlag);

        Trie trie;
        
        if (!string.IsNullOrEmpty(filePath))
        {
            trie = ReadFromFile(filePath);
        }
        else
        {
            trie = ReadFromStdin();
        }

        if (Parser.FlagExists(UniqueWords) && (Parser.FlagExists(WordCountFlag) || Parser.FlagExists(UniqueCountFlag)))
        {
            var wordsWithCount = trie.GetAllWordsWithCounts();
            foreach (var wordCount in wordsWithCount)
            {
                Console.WriteLine($"{wordCount.Value}: {wordCount.Key}");
            }
        }
        else if (Parser.FlagExists(UniqueWords))
        {
            var uniqueWords = trie.GetAllWords();
            foreach (var word in uniqueWords)
            {
                Console.WriteLine(word);
            }
        }
        else if (Parser.FlagExists(WordCountFlag))
        {
            Console.WriteLine(trie.Count());
        }
        else if (Parser.FlagExists(UniqueCountFlag))
        {
            Console.WriteLine(trie.CountUnique());
        }
    }

    private static Trie ReadFromFile(string filePath)
    {
        using var reader = new StreamReader(filePath);
        return Trie.FromStream(reader);
    }

    private static Trie ReadFromStdin()
    {
        using var stdIn = Console.OpenStandardInput();
        using var reader = new StreamReader(stdIn);
        return Trie.FromStream(reader);
    }
}