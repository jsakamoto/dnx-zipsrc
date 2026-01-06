using System.IO.Compression;

namespace Toolbelt.ZipSrc;

internal class SourceZipper
{
    public static void Zip(string sourceDir, string? targetZipPath = default, CompressionLevel compressionLevel = CompressionLevel.SmallestSize)
    {
        // Determine target zip path
        if (string.IsNullOrEmpty(targetZipPath))
        {
            var solutionFile = Directory.GetFiles(sourceDir, "*.sln?", SearchOption.TopDirectoryOnly).FirstOrDefault();
            targetZipPath = solutionFile is null
                ? Path.Combine(sourceDir, Path.GetFileName(sourceDir) + ".zip")
                : Path.Combine(sourceDir, Path.ChangeExtension(solutionFile, ".zip"));
        }

        using var archive = ZipFile.Open(targetZipPath, ZipArchiveMode.Create);
        var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories)
            .Where(filePath => !filePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));
        foreach (var filePath in files)
        {
            var entryName = Path.GetRelativePath(sourceDir, filePath);
            archive.CreateEntryFromFile(filePath, entryName, compressionLevel);
        }
    }
}
