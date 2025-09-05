namespace awc.Trie.Interfaces;

internal interface INode
{
    internal bool IsTerminal { get; set; }
    internal char Letter { get; }
    internal int Count { get; set; }
    internal INode Next(char letter);
    internal List<INode> Children { get; }
}
