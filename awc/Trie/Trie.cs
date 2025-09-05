using System.Text;
using awc.Trie.Interfaces;

namespace awc.Trie;

public class Trie
{
    private readonly INode _root = new Node(' ');
    
    public static Trie FromString(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using var stream = new MemoryStream(bytes);
        using var streamReader = new StreamReader(stream);
        return FromStream(streamReader);
    }

    /// <summary>
    /// Inserts a single word into the trie, the word should contain no punctuation or spaces
    /// </summary>
    /// <param name="word">The word to insert</param>
    /// <exception cref="ArgumentException">Thrown if any punctuation or whitespace is detected</exception>
    public void Insert(string word)
    {
        if (ContainsPunctuation(word) || ContainsWhitespace(word)) throw new ArgumentException("word contains punctuation or whitespace, when inserting it should be a single word with no spaces nor punctuation");
        var terminal = word.Aggregate(_root, (node, c) => node.Next(c));
        terminal.IsTerminal = true;
        terminal.Count++;
    }
    
    private static bool ContainsPunctuation(string word) => word.Any(char.IsPunctuation);
    private static bool ContainsWhitespace(string word) => word.Any(char.IsWhiteSpace);
    
    public static Trie FromStream(StreamReader streamReader)
    {
        var trie = new Trie();
        var c = streamReader.Read();
        var word = new StringBuilder();
        while (c != -1)
        {
            var asChar = (char)c;
            
            if (char.IsWhiteSpace(asChar))
            {
                if (word.Length > 0)
                {
                    trie.Insert(word.ToString());
                    word.Clear();
                }
                c = streamReader.Read();
                continue;
            }
            
            if (!char.IsPunctuation(asChar)) word.Append(char.ToLower(asChar));
            
            c = streamReader.Read();
        }
        
        // Word might be in buffer if there is no space before EOF
        if (word.Length <= 0) return trie;
        
        trie.Insert(word.ToString());
        return trie;
    }

    public List<string> GetAllWords()
    {
        var words = new List<string>();
        Collect(_root, new StringBuilder(), words, (builder, _) => builder.ToString());
        return words;
    }
    
    public List<KeyValuePair<string, int>> GetAllWordsWithCounts()
    {
        var words = new List<KeyValuePair<string, int>>();
        Collect(_root, new StringBuilder(), words, (builder, node) => new KeyValuePair<string, int>(builder.ToString(), node.Count));
        return words;
    }
    
    private static void Collect<T>(INode node, StringBuilder currentWord, List<T> acc, Func<StringBuilder, INode, T> f)
    {
        if (node.IsTerminal)
        {
            acc.Add(f(currentWord, node));
        }

        foreach (var child in node.Children)
        {
            Collect(child, currentWord.Append(child.Letter), acc, f);
            currentWord.Remove(currentWord.Length - 1, 1);
        }
    }

    public int CountUnique()
    {
        return CountUnique(_root);
    }
    
    public int Count()
    {
        return Count(_root);
    }

    private static int CountUnique(INode node)
    {
        return (node.IsTerminal ? 1 : 0) + node.Children.Sum(CountUnique);
    }
    
    private static int Count(INode node)
    {
        return node.Count + node.Children.Sum(Count);
    }
}