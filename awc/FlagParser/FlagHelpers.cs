namespace awc.FlagParser;

internal static class FlagHelpers
{
    /// <summary>
    /// Converts a full lenght flag to its shorthand equivalent e.g. --flag turns into -f
    /// </summary>
    /// <param name="fullFlag">The full flag</param>
    /// <returns>The shorthand version of the flag</returns>
    /// <exception cref="ArgumentException">Thrown if the full flag does not meet the requirements, e.g., does not start with --</exception>
    public static string FullFlagToShortFlag(string fullFlag)
    {
        if (!fullFlag.StartsWith("--")) throw new ArgumentException($"Invalid flag: {fullFlag}, full flags should start with '--', e.g. --flag");
        var firstLetters = fullFlag.Remove(0, 2).Split('-').Select(x => x.Length > 0 ? x[0].ToString() : "");
        return "-"+string.Join("", firstLetters);
    }
}
