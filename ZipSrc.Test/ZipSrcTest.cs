using System.IO.Compression;
using Toolbelt.ZipSrc.Test.Internals;

namespace Toolbelt.ZipSrc.Test;

[Parallelizable(ParallelScope.Children)]
public class ZipSrcTest
{
    /* 
    |               | Case 001    |
    |---------------|-------------|
    | Project Type  | Blazor      |
    | Solution File | MyApp.slnx  |
    | Package       | Package Ref |
    | .gitignore    | (none)      |
    */
    [Test]
    public async Task ZipSrc_Case001_TestAsync()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case001");

        // When
        await SourceZipper.ZipAsync(workSpace);

        // Then
        var zipPath = Path.Combine(workSpace, "MyApp.zip");
        File.Exists(zipPath).IsTrue();

        using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);
        var entries = archive.Entries.Select(e => e.FullName).Select(PathUtil.Normalize).Order();
        entries.Is(Expectations.Case001);
    }

    /* 
    |               | Case 002       |
    |---------------|----------------|
    | Project Type  | WPF            |
    | Solution File | WpfApp.sln     |
    | Package       | package.config |
    | .gitignore    | in subdir      |
    */
    [Test]
    public async Task ZipSrc_Case002_TestAsync()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case002");

        // When
        await SourceZipper.ZipAsync(workSpace);

        // Then
        var zipPath = Path.Combine(workSpace, "WpfApp.zip");
        File.Exists(zipPath).IsTrue();

        using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);
        var entries = archive.Entries.Select(e => e.FullName).Select(PathUtil.Normalize).Order();
        entries.Is(Expectations.Case002);
    }

    /* 
    |               | Case 003   |
    |---------------|------------|
    | Project Type  | TypeScript |
    | Solution File | (none)     |
    | Package       | npm        |
    | .gitignore    | in root    |
    */
    [Test]
    public async Task ZipSrc_Case003_TestAsync()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case003");

        // When
        await SourceZipper.ZipAsync(workSpace);

        // Then
        var zipPath = Path.Combine(workSpace, "Case003.zip");
        File.Exists(zipPath).IsTrue();

        using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);
        var entries = archive.Entries.Select(e => e.FullName).Select(PathUtil.Normalize).Order();
        entries.Is(Expectations.Case003);
    }

    [Test]
    public async Task ZipSrc_Repeatedly_TestAsync()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case001");

        // When: Execute zipping multiple times
        await SourceZipper.ZipAsync(workSpace);
        await SourceZipper.ZipAsync(workSpace);
        await SourceZipper.ZipAsync(workSpace);

        // Then
        var zipPathList = new List<string> {
            Path.Combine(workSpace, "MyApp.zip"),
            Path.Combine(workSpace, "MyApp (2).zip"),
            Path.Combine(workSpace, "MyApp (3).zip"),
        };
        zipPathList.ForEach(zipPath =>
        {
            File.Exists(zipPath).IsTrue();

            using var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);
            var entries = archive.Entries.Select(e => e.FullName).Select(PathUtil.Normalize).Order();
            entries.Is(Expectations.Case001);
        });
    }
}
