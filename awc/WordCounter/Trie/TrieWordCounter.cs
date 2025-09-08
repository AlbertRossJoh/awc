using System.Text;
using awc.Interfaces;
using awc.Trie;
using awc.Trie.Interfaces;

namespace awc.WordCounter.Trie;

public class TrieWordCounter : WordCounterBase
{
    private readonly INode _root = new Node(' ');

    public override IWordCounter Insert(string word)
    {
        if (ContainsPunctuation(word) || ContainsWhitespace(word)) throw new ArgumentException("word contains punctuation or whitespace, when inserting it should be a single word with no spaces nor punctuation");
        var terminal = word.Aggregate(_root, (node, c) => node.Next(c));
        terminal.IsTerminal = true;
        terminal.Count++;
        return this;
    }

    public override List<string> GetAllWords()
    {
        var words = new List<string>();
        Collect(_root, new StringBuilder(), words, (builder, _) => builder.ToString());
        return words;
    }
    
    public override List<KeyValuePair<string, int>> GetAllWordsWithCounts()
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

    public override int CountUnique()
    {
        return CountUnique(_root);
    }
    
    public override int Count()
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