namespace awc.FlagParser;

public class FlagParser(Dictionary<string, FlagConfig> flagConfigs)
{
    private readonly Dictionary<string, string?> _arguments = new();

    public FlagParser ParseArgs(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (!flagConfigs.TryGetValue(arg, out var config))
            {
                throw new ArgumentException($"Unknown flag kind: {arg}, valid flags are: [{string.Join(", ", flagConfigs.Keys)}]");
            }

            switch (config.Kind)
            {
                case FlagKind.Value when i + 1 >= args.Length:
                    throw new ArgumentException($"Flag {arg} expected to have a value");
                case FlagKind.Mode:
                    if (arg == "--help" || arg == "-h")
                    {
                        PrintHelp();
                        Environment.Exit(0);
                    }
                    _arguments[arg] = arg;
                    continue;
                case FlagKind.Value:
                    var potentialValue = args[i + 1];
                    if (!string.IsNullOrEmpty(potentialValue) && !potentialValue.StartsWith('-'))
                        _arguments.Add(arg, potentialValue);
                    break;
                    
            }
            i++;
        }    
        
        return this;
    }

    public string? GetValueByFullFlag(string flag)
    {
        var shortFlag = FlagHelpers.FullFlagToShortFlag(flag);
        return _arguments.GetValueOrDefault(shortFlag) ?? _arguments.GetValueOrDefault(flag);
    }

    public bool FlagExists(string flag)
    {
        var shortFlag = FlagHelpers.FullFlagToShortFlag(flag);
        return _arguments.ContainsKey(shortFlag) || _arguments.ContainsKey(flag);
    }

    private void PrintHelp()
    {
        foreach (var (_, config) in flagConfigs.DistinctBy(x => x.Value.Flag))
        {
            var shortFlag = FlagHelpers.FullFlagToShortFlag(config.Flag);
            if (config.Flag != "--help") Console.WriteLine($"{config.Flag}, {shortFlag} : {config.HelpText}");
        }
    }
}
