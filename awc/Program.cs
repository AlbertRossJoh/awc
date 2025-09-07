// See https://aka.ms/new-console-template for more information

using awc;
using awc.FlagParser;
using awc.Interfaces;
using awc.Trie;

internal static class Program
{
    private const string FileFlag = "--file";
    private const string UniqueCountFlag = "--unique-count";
    private const string WordCountFlag = "--word-count";
    private const string UniqueWords = "--unique-words";
    private const string UseTrie = "--trie";
    
    private static readonly FlagParser Parser = 
        new FlagParserBuilder()
            .AddFlag(FileFlag, FlagKind.Value, "The file which you want to read")
            .AddFlag(UniqueCountFlag, FlagKind.Mode, "Toggle if you want the count of unique words")
            .AddFlag(WordCountFlag, FlagKind.Mode, "Toggle if you want the count of words")
            .AddFlag(UniqueWords, FlagKind.Mode, "Toggle if you want all the unique words, can be combined with -wc or -uc for counts of each word")
            .AddFlag(UseTrie, FlagKind.Mode, "Toggle if you want to use the trie implementation, this is useful if ")
            .Build();

    private static void Main(string[] args)
    {
        Parser.ParseArgs(args);

        var filePath = Parser.GetValueByFullFlag(FileFlag);

        IWordCounter wordCounter = new DictionaryWordCounter();
        
        if (Parser.FlagExists(UseTrie)) wordCounter = new TrieWordCounter();
        
        if (!string.IsNullOrEmpty(filePath))
        {
            wordCounter = ReadFromFile(wordCounter, filePath);
        }
        else
        {
            wordCounter = ReadFromStdin(wordCounter);
        }

        if (Parser.FlagExists(UniqueWords) && (Parser.FlagExists(WordCountFlag) || Parser.FlagExists(UniqueCountFlag)))
        {
            var wordsWithCount = wordCounter.GetAllWordsWithCounts();
            foreach (var wordCount in wordsWithCount)
            {
                Console.WriteLine($"{wordCount.Value}: {wordCount.Key}");
            }
        }
        else if (Parser.FlagExists(UniqueWords))
        {
            var uniqueWords = wordCounter.GetAllWords();
            foreach (var word in uniqueWords)
            {
                Console.WriteLine(word);
            }
        }
        else if (Parser.FlagExists(WordCountFlag))
        {
            Console.WriteLine(wordCounter.Count());
        }
        else if (Parser.FlagExists(UniqueCountFlag))
        {
            Console.WriteLine(wordCounter.CountUnique());
        }
    }

    private static IWordCounter ReadFromFile(IWordCounter wordCounter, string filePath)
    {
        using var reader = new StreamReader(filePath);
        return wordCounter.PopulateFromStream(reader);
    }

    private static IWordCounter ReadFromStdin(IWordCounter wordCounter)
    {
        using var stdIn = Console.OpenStandardInput();
        using var reader = new StreamReader(stdIn);
        return wordCounter.PopulateFromStream(reader);
    }
}