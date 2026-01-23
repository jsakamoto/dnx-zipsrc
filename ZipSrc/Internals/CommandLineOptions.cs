namespace Toolbelt.ZipSrc.Internals;

/// <summary>
/// Represents the set of command-line options for a zip operation.
/// </summary>
internal class CommandLineOptions
{
    /// <summary>
    /// Displays the version information of the tool.
    /// </summary>
    public bool Version { get; set; }

    /// <summary>
    /// Displays help information about the command-line options.
    /// </summary>
    public bool Help { get; set; }

    /// <summary>
    /// The target directory path to be zipped. Optional.
    /// </summary>
    public string? Directory { get; set; }

    /// <summary>
    /// The target zip file path. Optional.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The logging level for the operation.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Normal;
}
