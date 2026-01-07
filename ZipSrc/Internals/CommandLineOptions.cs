namespace Toolbelt.ZipSrc.Internals;

/// <summary>
/// Represents the set of command-line options for a zip operation.
/// </summary>
internal class CommandLineOptions
{
    /// <summary>
    /// The target directory path to be zipped. Optional.
    /// </summary>
    public string? Directory { get; set; }

    /// <summary>
    /// The target zip file path. Optional.
    /// </summary>
    public string? Name { get; set; }
}
