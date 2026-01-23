using System.IO.Compression;

namespace Toolbelt.ZipSrc.Internals;

/// <summary>
/// Represents the context for a zip archive operation, containing the archive, compression settings, and target directory.
/// </summary>
internal class ZipContext
{
    /// <summary>
    /// Gets the <see cref="ZipArchive"/> instance used for writing zip entries.
    /// </summary>
    public required ZipArchive Archive { get; init; }

    /// <summary>
    /// Gets the compression level to use when adding entries to the archive.
    /// </summary>
    public CompressionLevel CompressionLevel { get; init; }

    /// <summary>
    /// Gets the target directory path for the zip operation.
    /// </summary>
    public required string TargetDir { get; init; }

    public Action<LogLevel, string>? OutputLog { get; init; }
}