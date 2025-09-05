namespace awc.FlagParser;

internal static class FlagHelpers
{
    public static string FullFlagToShortFlag(string fullFlag)
    {
        if (!fullFlag.StartsWith("--")) throw new ArgumentException($"Invalid flag: {fullFlag}, full flags should start with '--', e.g. --flag");
        var firstLetters = fullFlag.Remove(0, 2).Split('-').Select(x => x.Length > 0 ? x[0].ToString() : "");
        return "-"+string.Join("", firstLetters);
    }
}
