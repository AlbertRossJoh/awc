using System.Text;

namespace awc.Trie;

public class Trie
{
    private readonly Node _root = new(' ');

    public static Trie FromString(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using var stream = new MemoryStream(bytes);
        using var streamReader = new StreamReader(stream);
        return FromStream(streamReader);
    }

    public static Trie FromStream(StreamReader streamReader)
    {
        var trie = new Trie();
        var c = streamReader.Read();
        var current = trie._root;
        while (c != -1)
        {
            
            if (char.IsWhiteSpace((char)c))
            {
                if (char.IsLetter(current.Letter))
                {
                    current.IsTerminal = true;
                    current.Count++;
                }
                current = trie._root;
                c = streamReader.Read();
                continue;
            }
            
            if (!char.IsPunctuation((char)c))
            {
                c = char.ToLower((char)c);
                current = current.Next((char)c);
            }
            
            c = streamReader.Read();
        }
        current.IsTerminal = true;
        current.Count++;
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
    
    private static void Collect<T>(Node node, StringBuilder currentWord, List<T> acc, Func<StringBuilder, Node, T> f)
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

    // rewrite does not take int account nested words
    private static int CountUnique(Node node)
    {
        return node.IsTerminal ? 1 : node.Children.Sum(CountUnique);
    }
    
    private static int Count(Node node)
    {
        return node.IsTerminal ? node.Count : node.Children.Sum(CountUnique);
    }
}