using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using GitignoreParserNet;

namespace Toolbelt.ZipSrc.Internals;

/// <summary>
/// Represents a list of file patterns to ignore based on gitignore syntax.
/// </summary>
internal class IgnoreList
{
    private GitignoreParser _parser;

    private string _basePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="IgnoreList"/> class.
    /// </summary>
    /// <param name="basePath">The base path for resolving relative file paths.</param>
    /// <param name="parser">The gitignore parser instance.</param>
    public IgnoreList(string basePath, GitignoreParser parser)
    {
        this._basePath = basePath;
        this._parser = parser;
    }

    /// <summary>
    /// Creates a new <see cref="IgnoreList"/> instance from an ignore file.
    /// </summary>
    /// <param name="basePath">The base path for resolving relative file paths.</param>
    /// <param name="ignoreFilePath">The path to the ignore file containing patterns.</param>
    /// <returns>A new <see cref="IgnoreList"/> instance.</returns>
    public static IgnoreList Create(string basePath, string ignoreFilePath)
    {
        var parser = new GitignoreParser(File.ReadAllText(ignoreFilePath));
        return new IgnoreList(basePath, parser);
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "Negatives")]
    extern public static ref (Regex Merged, Regex[] Individual) GetNegatives(GitignoreParser parser);

    /// <summary>
    /// Gets the ignore state for the specified file path.
    /// </summary>
    /// <param name="filePath">The absolute or relative file path to check.</param>
    /// <returns>
    /// An <see cref="IgnoreState"/> value indicating whether the file is denied, accepted, or has no explicit state.
    /// </returns>
    public IgnoreState GetState(string filePath)
    {
        var relativePath = Path.GetRelativePath(this._basePath, filePath).Replace(Path.DirectorySeparatorChar, '/');
        var negatives = GetNegatives(this._parser);
        return
            this._parser.Denies(relativePath) ? IgnoreState.Denied :
            negatives.Merged.IsMatch("/" + relativePath.TrimStart('/')) ? IgnoreState.Accepted :
            IgnoreState.None;
    }
}
