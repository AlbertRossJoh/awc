using awc.WordCounter;
using awc.WordCounter.Dictionary;
using awc.WordCounter.Trie;

namespace AwcTests;
using Xunit;

public class WordCounterTests
{
    private readonly string _sentence = "Go do that thing that you do so well!";

    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_Count_CorrectTotalCount(IWordCounter wordCounter)
    {
        // Arrange
        wordCounter.PopulateFromString(_sentence);
        
        // Act
        var count = wordCounter.Count();
        
        // Assert
        Assert.Equal(9, count);
    }
    
    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_CountUnique_CorrectCount(IWordCounter wordCounter)
    {
        // Arrange
        wordCounter.PopulateFromString(_sentence);
        
        // Act
        var count = wordCounter.CountUnique();
        
        // Assert
        Assert.Equal(7, count);
    }

    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_AllWords_AllWordsAccountedFor(IWordCounter wordCounter)
    {
        // Arrange
        wordCounter.PopulateFromString(_sentence);
        
        // Act
        var words = wordCounter.GetAllWords();
        
        // Assert
        Assert.Equal(7, words.Count);       
        Assert.Distinct(words);
        Assert.Equal(["go", "do", "that", "thing", "you", "so", "well"], words);
    }

    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_AllWordsWithCounts_AllWordsAccountedForAndCorrectCounts(IWordCounter wordCounter)
    {
        // Arrange
        wordCounter.PopulateFromString(_sentence);
        
        // Act
        var words = wordCounter.GetAllWordsWithCounts();
        
        // Assert
        Assert.Equal(7, words.Count);       
        Assert.Distinct(words);
        Assert.Equal([
            new KeyValuePair<string, int>("go", 1), 
            new KeyValuePair<string, int>("do", 2), 
            new KeyValuePair<string, int>("that", 2), 
            new KeyValuePair<string, int>("thing", 1), 
            new KeyValuePair<string, int>("you",1), 
            new KeyValuePair<string, int>("so",1), 
            new KeyValuePair<string, int>("well",1)], words);       
    }
    
    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_InsertSingle_WordInsertedOnce(IWordCounter wordCounter)
    {
        // Arrange
        wordCounter.Insert("hello");
        
        // Act
        var words = wordCounter.GetAllWordsWithCounts();
        
        // Assert
        Assert.Single(words);       
        Assert.Equal([new KeyValuePair<string, int>("hello", 1)], words);       
    }
    
    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_InsertSingleWithWhitespace_Throws(IWordCounter wordCounter)
    {
        // Assert
        Assert.Throws<ArgumentException>(() => wordCounter.Insert("hello you"));
    }
    
    [Theory]
    [MemberData(nameof(WordCounters))]
    public void WordCounter_InsertSingleWithPunctuation_Throws(IWordCounter wordCounter)
    {
        // Assert
        Assert.Throws<ArgumentException>(() => wordCounter.Insert("hello.you"));
    }

    public static IEnumerable<object[]> WordCounters()
    {
        yield return [new TrieWordCounter()];
        yield return [new DictionaryWordCounter()];
    }
}
