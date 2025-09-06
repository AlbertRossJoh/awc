namespace awc.Interfaces;

public interface IWordCounter
{
    /// <summary>
    /// Get a list of all words inserted, all words are in lower case for consistency.
    /// </summary>
    /// <returns>A list of words inserted</returns>
    public List<string> GetAllWords();
    
    /// <summary>
    /// Get a list of all words inserted with their corresponding count, all words are in lower case for consistency.
    /// </summary>
    /// <returns>A list of key value pairs containing a word and its respective count</returns>
    public List<KeyValuePair<string, int>> GetAllWordsWithCounts();
    
    /// <summary>
    /// Inserts a single word into the word counter, the word should contain no punctuation or spaces
    /// </summary>
    /// <param name="word">The word to insert</param>
    /// <exception cref="ArgumentException">Thrown if any punctuation or whitespace is detected</exception>
    public IWordCounter Insert(string word);
    
    /// <summary>
    /// Counts all unique words in word counter
    /// </summary>
    /// <returns>The number of unique words</returns>
    public int CountUnique();
    
    /// <summary>
    /// Counts all words inserted into the word counter
    /// </summary>
    /// <returns>The amount of words inserted</returns>
    public int Count();
    
    /// <summary>
    /// Populates the word counter from a stream
    /// </summary>
    /// <param name="streamReader">The stream to read from</param>
    /// <returns>The word counter used</returns>
    public IWordCounter PopulateFromStream(StreamReader streamReader);
    
    /// <summary>
    /// Populates the word counter from a string
    /// </summary>
    /// <param name="str">The string to read from</param>
    /// <returns>The word counter used</returns>
    public IWordCounter PopulateFromString(string str);
}