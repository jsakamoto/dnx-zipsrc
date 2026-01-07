namespace Toolbelt.ZipSrc.Test.Internals;

internal class Expectations
{
    public static readonly string[] Case001 = [
        "MyApp.slnx",
        "MyApp.Test/MyApp.Test.csproj",
        "MyApp.Test/UnitTest1.cs",
        "MyApp/.editorconfig",
        "MyApp/MyApp.csproj",
        "MyApp/Pages/_Host.cshtml",
        "MyApp/Pages/App.razor",
        "MyApp/Pages/App.razor.css",
        "MyApp/Pages/App.razor.js",
        "MyApp/Program.cs",
        "MyApp/tsconfig.json",
        "MyApp/wwwroot/css/site.css",
        "MyApp/wwwroot/favicon.ico",
        "MyApp/wwwroot/index.html",
        "MyApp/wwwroot/js/helper.js",
        "MyApp/wwwroot/js/helper.ts",
    ];

    public static readonly string[] Case002 = [
        "WpfApp.sln",
        "WpfApp/App.xaml",
        "WpfApp/App.xaml.cs",
        "WpfApp/Data/.gitignore",
        "WpfApp/Data/sample-data.ldf",
        "WpfApp/Data/sample-data.mdf",
        "WpfApp/package.config",
        "WpfApp/WpfApp.csproj",
    ];

    public static readonly string[] Case003 = [
        ".editorconfig",
        ".env",
        ".gitignore",
        "package-lock.json",
        "package.json",
        "public/favicon.png",
        "public/index.html",
        "src/index.css",
        "src/index.ts",
    ];
}
