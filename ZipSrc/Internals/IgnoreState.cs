namespace Toolbelt.ZipSrc.Internals;

/// <summary>
/// Represents the state of a file or directory regarding ignore rules.
/// </summary>
internal enum IgnoreState
{
    /// <summary>
    /// No ignore rule applies to the item.
    /// </summary>
    None = 0,

    /// <summary>
    /// The item is explicitly denied (excluded) by an ignore rule.
    /// </summary>
    Denied = 1,

    /// <summary>
    /// The item is explicitly accepted (included) by an ignore rule, overriding any previous deny rules.
    /// </summary>
    Accepted = 2
}
