namespace Toolbelt.ZipSrc.Test.Internals;

/// <summary>
/// Provides utility methods and data structures for managing expected file paths in various scenarios.
/// </summary>
internal static class Expectations
{
    /// <summary>
    /// Represents a file system path that is intended for use in a specific operating system context.
    /// </summary>
    /// <param name="LinuxOnly">Indicates whether the path exists only on Linux systems.</param>
    /// <param name="Path">The file system path should exist.</param>
    public record ExpectedPath(bool LinuxOnly, string Path);

    /// <summary>
    /// Converts a collection of ExpectedPath objects to their corresponding paths, filtering based on the operating system.
    /// </summary>
    public static IEnumerable<string> ToPaths(this IEnumerable<ExpectedPath> expectedPaths)
        => expectedPaths
            .Where(ep => OperatingSystem.IsWindows() ? !ep.LinuxOnly : true)
            .Select(ep => ep.Path);

    public static readonly ExpectedPath[] Case001 = [
        new(false, "MyApp.slnx"),
        new(false, "MyApp.Test/MyApp.Test.csproj"),
        new(false, "MyApp.Test/UnitTest1.cs"),
        new(false, "MyApp/.editorconfig"),
        new(false, "MyApp/MyApp.csproj"),
        new(false, "MyApp/Native/native.obj"),
        new(false, "MyApp/Pages/_Host.cshtml"),
        new(false, "MyApp/Pages/App.razor"),
        new(false, "MyApp/Pages/App.razor.css"),
        new(false, "MyApp/Pages/App.razor.js"),
        new(false, "MyApp/Program.cs"),
        new(false, "MyApp/tsconfig.json"),
        new(false, "MyApp/wwwroot/css/site.css"),
        new(false, "MyApp/wwwroot/favicon.ico"),
        new(false, "MyApp/wwwroot/index.html"),
        new(false, "MyApp/wwwroot/js/helper.js"),
        new(false, "MyApp/wwwroot/js/helper.ts"),
        new(true, "shell.ini"),
    ];

    public static readonly ExpectedPath[] Case002 = [
        new(false, "WpfApp.sln"),
        new(false, "WpfApp/App.xaml"),
        new(false, "WpfApp/App.xaml.cs"),
        new(false, "WpfApp/Data/.gitignore"),
        new(false, "WpfApp/Data/sample-data.ldf"),
        new(false, "WpfApp/Data/sample-data.mdf"),
        new(false, "WpfApp/package.config"),
        new(true, "WpfApp/shell.ini"),
        new(false, "WpfApp/WpfApp.csproj"),
    ];

    public static readonly ExpectedPath[] Case003 = [
        new(false, ".editorconfig"),
        new(false, ".env"),
        new(false, ".gitignore"),
        new(false, "package-lock.json"),
        new(false, "package.json"),
        new(false, "public/data/sample-data.zip"),
        new(false, "public/favicon.png"),
        new(false, "public/index.html"),
        new(false, "shell.ini"),
        new(false, "src/index.css"),
        new(false, "src/index.ts"),
    ];
}
