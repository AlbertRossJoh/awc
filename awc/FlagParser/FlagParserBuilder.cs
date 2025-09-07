namespace awc.FlagParser;

public class FlagParserBuilder
{
    private readonly Dictionary<string, FlagConfig> _config = new();

    public FlagParserBuilder AddFlag(string fullFlag, FlagKind kind, string? usage = null)
    {
        var shortFlag = FlagHelpers.FullFlagToShortFlag(fullFlag);
        _config.Add(shortFlag, new FlagConfig(fullFlag, kind, usage));
        _config.Add(fullFlag, new FlagConfig(fullFlag, kind, usage));
        return this;
    }

    private void AddHelpFlag()
    {
        _config.Add("-h", new FlagConfig("--help", FlagKind.Mode));
        _config.Add("--help", new FlagConfig("--help", FlagKind.Mode));
    }

    public FlagParser Build()
    {
        AddHelpFlag();
        return new FlagParser(_config);
    }
}
