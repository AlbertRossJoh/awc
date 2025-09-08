using System.Text;
using awc.Interfaces;

namespace awc;

public class DictionaryWordCounter: WordCounterBase
{
    private readonly Dictionary<string, int> _wordCount = new ();

    public override IWordCounter Insert(string word)
    {
        if (ContainsPunctuation(word) || ContainsWhitespace(word)) throw new ArgumentException("word contains punctuation or whitespace, when inserting it should be a single word with no spaces nor punctuation");
        
        if (!_wordCount.TryAdd(word, 1))
        {
            _wordCount[word]++;
        }

        return this;
    }
    
    public override List<string> GetAllWords()
    {
        return _wordCount.Keys.ToList();
    }

    public override List<KeyValuePair<string, int>> GetAllWordsWithCounts()
    {
        return _wordCount.ToList();
    }

    public override int CountUnique()
    {
        return _wordCount.Count;
    }

    public override int Count()
    {
        return _wordCount.Values.Sum();
    }
}