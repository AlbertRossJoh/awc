namespace awc.Trie;

public class Node(char letter)
{
    public bool IsTerminal { get; set; } = false;
    public char Letter { get; } = letter;
    public int Count { get; set; } = 0;
    private readonly Dictionary<char,Node> _children = new();

    public List<Node> Children => _children.Values.ToList();

    public Node Next(char letter)
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