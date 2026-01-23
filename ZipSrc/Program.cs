using System.IO.Compression;
using CommandLineSwitchParser;
using Toolbelt.ZipSrc;
using Toolbelt.ZipSrc.Internals;

if (!CommandLineSwitch.TryParse<CommandLineOptions>(ref args, out var options, out var error))
{
    Console.Error.WriteLine($"Error parsing command-line arguments: {error}");
    return 1;
}
if (options.Help) return PrintHelp();
if (options.Version) return PrintVersion();

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
    compressionLevel: CompressionLevel.SmallestSize,
    (LogLevel level, string log) => OutputLogToConsole(options, level, log));

return 0;

static int PrintVersion()
{
    var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown";
    Console.WriteLine($"zipsrc version {version}");
    return 0;
}

static int PrintHelp()
{
    Console.WriteLine("Usage: zipsrc [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  -d, --directory <path>   Specifies the target directory to be zipped. If not provided, the current directory is used.");
    Console.WriteLine("  -n, --name <zipfile>     Specifies the name of the output zip file. If not provided, a default name is generated.");
    Console.WriteLine("  -h, --help               Displays this help information.");
    Console.WriteLine("  -v, --version            Displays the version information.");
    Console.WriteLine("  -l, --loglevel <level>   Sets the logging level (quiet, normal, detailed). Default is Normal.");
    return 0;
}

static void OutputLogToConsole(CommandLineOptions options, LogLevel level, string log)
{
    if (level <= options.LogLevel) Console.WriteLine(log);
}
