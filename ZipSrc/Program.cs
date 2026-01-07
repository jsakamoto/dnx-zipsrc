using System.IO.Compression;
using CommandLineSwitchParser;
using Toolbelt.ZipSrc;
using Toolbelt.ZipSrc.Internals;

var options = CommandLineSwitch.Parse<CommandLineOptions>(ref args);

// Error if target directory does not exist
var targetDir = Path.GetFullPath(options.Directory ?? Directory.GetCurrentDirectory());
if (!Directory.Exists(targetDir))
{
    Console.Error.WriteLine($"Error: The specified directory does not exist: \"{targetDir}\"");
    return 1;
}

// Error if target zip directory does not exist
var targetZipPath = options.Name is not null
    ? Path.GetFullPath(options.Name)
    : null;
if (targetZipPath is not null)
{
    if (targetZipPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) is false)
    {
        targetZipPath += ".zip";
    }
    var targetZipDir = Path.GetDirectoryName(targetZipPath);
    if (targetZipDir is not null && !Directory.Exists(targetZipDir))
    {
        Console.Error.WriteLine($"Error: The directory for the specified zip file does not exist: \"{targetZipDir}\"");
        return 1;
    }
}

// Create the zip archive for source files
await SourceZipper.ZipAsync(
    targetDir,
    targetZipPath,
    compressionLevel: CompressionLevel.SmallestSize);

return 0;