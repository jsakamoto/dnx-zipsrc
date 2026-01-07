namespace Toolbelt.ZipSrc.Test.Internals;

internal class PathUtil
{
    public static string Normalize(string path) => path.Replace('\\', '/');
}
