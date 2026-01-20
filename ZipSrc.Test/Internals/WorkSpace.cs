namespace Toolbelt.ZipSrc.Test.Internals;

internal class WorkSpace : IDisposable
{
    private readonly WorkDirectory _workDirectory;

    private readonly string _caseDir;

    private WorkSpace(string caseDir)
    {
        this._caseDir = caseDir;
        var testProjDir = FileIO.FindContainerDirToAncestor("*.csproj");
        this._workDirectory = WorkDirectory.CreateCopyFrom(Path.Combine(testProjDir, "Fixtures"), _ => true);

        foreach (var srcGitDir in Directory.GetDirectories(this._workDirectory, "git", SearchOption.AllDirectories))
        {
            var targetGitDir = Path.Combine(Path.GetDirectoryName(srcGitDir) ?? ".", ".git");
            Directory.Move(srcGitDir, targetGitDir);
            new DirectoryInfo(targetGitDir).Attributes |= FileAttributes.Hidden;
        }
    }

    public static WorkSpace Create(string caseDir) => new WorkSpace(caseDir);

    public static implicit operator string(WorkSpace workSpace) => Path.Combine(workSpace._workDirectory.Path, workSpace._caseDir);

    public override string ToString() => Path.Combine(this._workDirectory.Path, this._caseDir);

    public void Dispose()
    {
        this._workDirectory.Dispose();
    }
}
