using awc.WordCounter.Trie.Interfaces;

namespace awc.WordCounter.Trie;

internal class Node(char letter) : INode
{
    public bool IsTerminal { get; set; } = false;
    public char Letter { get; } = letter;
    public int Count { get; set; } = 0;
    private readonly Dictionary<char,INode> _children = new();

    public List<INode> Children => _children.Values.ToList();

    public INode Next(char letter)
    {
        if (_children.TryGetValue(letter, out var next))
        {
            return next;
        }
        var node = new Node(letter);
        _children[letter] = node;
        return node;
    }
}