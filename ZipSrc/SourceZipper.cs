using System.IO.Compression;
using Toolbelt.Diagnostics;
using Toolbelt.ZipSrc.Internals;

namespace Toolbelt.ZipSrc;

internal class SourceZipper
{
    /// <summary>
    /// Creates a ZIP archive of the specified directory source code, applying .gitignore rules to exclude files
    /// </summary>
    /// <param name="targetDir">The full path to the directory to be zipped. Must not be null or empty.</param>
    /// <param name="targetZipPath">The full path where the resulting ZIP file will be created. If null or not specified, a default path is determined automatically.</param>
    /// <param name="compressionLevel">The compression level to use when creating the ZIP archive. The default is CompressionLevel.SmallestSize.</param>
    public static async ValueTask ZipAsync(string targetDir, string? targetZipPath = default, CompressionLevel compressionLevel = CompressionLevel.SmallestSize)
    {
        // Determine target zip path
        targetZipPath = DetermineTargetZipPath(targetDir, targetZipPath);

        // Prepare ignore lists
        var rootIgnoreList = await GetRootIgnoreList(targetDir);

        // Create zip archive
        using var archive = ZipFile.Open(targetZipPath, ZipArchiveMode.Create);
        var context = new ZipContext
        {
            Archive = archive,
            CompressionLevel = compressionLevel,
            TargetDir = targetDir,
        };
        ZipFolderRecursively(context, targetDir, new Stack<IgnoreList>([rootIgnoreList]));
    }

    /// <summary>
    /// Determines the file path for the target ZIP archive, ensuring that the path does not conflict with an existing file.
    /// </summary>
    /// <remarks>If <paramref name="targetZipPath"/> is null or empty, the method generates a ZIP file name
    /// based on the solution file in <paramref name="targetDir"/>, or the directory name if no solution file is found.
    /// If a file with the chosen name already exists, a numeric suffix is appended to create a unique file
    /// name.</remarks>
    /// <param name="targetDir">The directory containing the files to be archived.</param>
    /// <param name="targetZipPath">The desired file path for the ZIP archive, or null to generate a path based on the solution file or directory name.</param>
    /// <returns>A file path for the ZIP archive that does not conflict with any existing file in the target directory.</returns>
    private static string DetermineTargetZipPath(string targetDir, string? targetZipPath)
    {
        if (string.IsNullOrEmpty(targetZipPath))
        {
            var solutionFile = Directory.GetFiles(targetDir, "*.sln?", SearchOption.TopDirectoryOnly).FirstOrDefault();
            targetZipPath = solutionFile is null
                ? Path.Combine(targetDir, Path.GetFileName(targetDir) + ".zip")
                : Path.Combine(targetDir, Path.ChangeExtension(solutionFile, ".zip"));
        }

        var sufixCounter = 1;
        var targetZipDir = Path.GetDirectoryName(targetZipPath);
        var targetZipName = Path.GetFileNameWithoutExtension(targetZipPath);
        while (File.Exists(targetZipPath))
        {
            sufixCounter++;
            targetZipPath = targetZipDir switch
            {
                null => $"{targetZipName} ({sufixCounter}).zip",
                _ => Path.Combine(targetZipDir, $"{targetZipName} ({sufixCounter}).zip")
            };
        }

        return targetZipPath;
    }

    /// <summary>
    /// Creates an <see cref="IgnoreList"/> for the specified directory, using an existing .gitignore file if present or
    /// generating a default one if not.
    /// </summary>
    /// <param name="targetDir">The path to the target directory for which to obtain or generate the .gitignore file. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains an <see cref="IgnoreList"/> instance for the specified directory.</returns>
    private static async ValueTask<IgnoreList> GetRootIgnoreList(string targetDir)
    {
        var rootIgnorePath = Path.Combine(targetDir, ".gitignore");
        if (File.Exists(rootIgnorePath))
        {
            return IgnoreList.Create(targetDir, rootIgnorePath);
        }
        else
        {
            using var workDir = new WorkDirectory(Path.GetTempPath());
            await XProcess.Start("dotnet", "new gitignore", workDir).WaitForExitAsync();
            return IgnoreList.Create(targetDir, Path.Combine(workDir, ".gitignore"));
        }
    }

    /// <summary>
    /// Adds all files and subdirectories from the specified folder to the target ZIP archive, recursively, while
    /// applying ignore rules from .gitignore files encountered in subdirectories.
    /// </summary>
    /// <param name="context">The ZIP operation context.</param>
    /// <param name="folderPath">The full path of the folder to be zipped, including all nested subdirectories and files.</param>
    /// <param name="ignoreLists">A stack of ignore lists representing .gitignore rules to be applied when determining which files and folders to exclude from the archive.</param>
    private static void ZipFolderRecursively(ZipContext context, string folderPath, Stack<IgnoreList> ignoreLists)
    {
        var files = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var filePath in files)
        {
            if (filePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)) continue;
            if (IsIgnored(ignoreLists, filePath)) continue;
            var entryName = Path.GetRelativePath(context.TargetDir, filePath);
            context.Archive.CreateEntryFromFile(filePath, entryName, context.CompressionLevel);
        }

        var subdirs = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var subdir in subdirs)
        {
            if (new DirectoryInfo(subdir).Attributes.HasFlag(FileAttributes.Hidden)) continue;

            var gitIgnorePath = Path.Combine(subdir, ".gitignore");
            if (File.Exists(gitIgnorePath))
            {
                ignoreLists.Push(IgnoreList.Create(subdir, gitIgnorePath));
                ZipFolderRecursively(context, subdir, ignoreLists);
                ignoreLists.Pop();
            }
            else
            {
                ZipFolderRecursively(context, subdir, ignoreLists);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified file path is denied by any of the provided ignore lists.
    /// </summary>
    /// <param name="ignoreLists">A stack of <see cref="IgnoreList"/> instances to evaluate against the file path. Each list may specify rules for accepting or denying file paths.</param>
    /// <param name="filePath">The file path to check for denial against the ignore lists.</param>
    /// <returns>true if any ignore list explicitly denies the file path; otherwise, false.</returns>
    private static bool IsIgnored(Stack<IgnoreList> ignoreLists, string filePath)
    {
        foreach (var ignore in ignoreLists)
        {
            var state = ignore.GetState(filePath);
            if (state == IgnoreState.Denied) return true;
            if (state == IgnoreState.Accepted) return false;
        }
        return false;
    }
}