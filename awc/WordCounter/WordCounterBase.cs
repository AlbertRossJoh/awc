using System.Text;

namespace awc.WordCounter;

public abstract class WordCounterBase: IWordCounter
{
    public IWordCounter PopulateFromString(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        using var stream = new MemoryStream(bytes);
        using var streamReader = new StreamReader(stream);
        return PopulateFromStream(streamReader);
    }

    public abstract IWordCounter Insert(string word);
    
    protected static bool ContainsPunctuation(string word) => word.Any(char.IsPunctuation);
    protected static bool ContainsWhitespace(string word) => word.Any(char.IsWhiteSpace);
    
    public IWordCounter PopulateFromStream(StreamReader streamReader)
    {
        var c = streamReader.Read();
        var word = new StringBuilder();
        while (c != -1)
        {
            var asChar = (char)c;
           
            if (char.IsWhiteSpace(asChar))
            {
                if (word.Length > 0)
                {
                    Insert(word.ToString());
                    word.Clear();
                }
                c = streamReader.Read();
                continue;
            }
            
            if (!char.IsPunctuation(asChar)) word.Append(char.ToLower(asChar));
            
            c = streamReader.Read();
        }
        
        // Word might be in buffer if there is no space before EOF
        if (word.Length <= 0) return this;
        
        Insert(word.ToString());
        return this;
    }

    public abstract List<string> GetAllWords();

    public abstract List<KeyValuePair<string, int>> GetAllWordsWithCounts();

    public abstract int CountUnique();

    public abstract int Count();
}