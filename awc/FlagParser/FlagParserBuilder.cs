namespace awc.FlagParser;

public class FlagParserBuilder
{
    private readonly Dictionary<string, FlagConfig> _config = new();

    public FlagParserBuilder AddFlag(string fullFlag, FlagKind kind, string? helpText = null)
    {
        var shortFlag = FlagHelpers.FullFlagToShortFlag(fullFlag);
        _config.Add(shortFlag, new FlagConfig(fullFlag, kind, helpText));
        _config.Add(fullFlag, new FlagConfig(fullFlag, kind, helpText));
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
