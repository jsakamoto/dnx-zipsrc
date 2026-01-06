using Toolbelt.ZipSrc.Test.Internals;

namespace Toolbelt.ZipSrc.Test;

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
    public void ZipSrc_Case001_Test()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case001");

        // When
        SourceZipper.Zip(workSpace);

        // Then
        File.Exists(Path.Combine(workSpace, "MyApp.zip")).IsTrue();

        Assert.Inconclusive("Test not implemented.");
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
    public void ZipSrc_Case002_Test()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case002");

        // When
        SourceZipper.Zip(workSpace);

        // Then
        File.Exists(Path.Combine(workSpace, "WpfApp.zip")).IsTrue();

        Assert.Inconclusive("Test not implemented.");
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
    public void ZipSrc_Case003_Test()
    {
        // Given
        using var workSpace = WorkSpace.Create("Case003");

        // When
        SourceZipper.Zip(workSpace);

        // Then
        File.Exists(Path.Combine(workSpace, "Case003.zip")).IsTrue();

        Assert.Inconclusive("Test not implemented.");
    }
}
