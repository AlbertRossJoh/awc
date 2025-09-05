namespace awc.FlagParser;

public record FlagConfig(string Flag, FlagKind Kind, string? HelpText = null);
