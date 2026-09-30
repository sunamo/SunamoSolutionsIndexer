namespace SunamoSolutionsIndexer._sunamo;

/// <summary>
/// Async file helpers that forward to File.*Async on modern runtimes and provide an equivalent implementation for net48/netstandard2.0.
/// </summary>
internal static class FileAsync
{
    /// <summary>
    /// Reads the whole text file.
    /// </summary>
    /// <param name="path">Path of the file.</param>
    /// <returns>Content of the file.</returns>
    internal static async Task<string> ReadAllTextAsync(string path)
    {
#if NET5_0_OR_GREATER
        return await File.ReadAllTextAsync(path);
#else
        using var reader = new StreamReader(path);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
#endif
    }

    /// <summary>
    /// Writes the text to the file, replacing existing content.
    /// </summary>
    /// <param name="path">Path of the file.</param>
    /// <param name="contents">Text to write.</param>
    internal static async Task WriteAllTextAsync(string path, string? contents)
    {
#if NET5_0_OR_GREATER
        await File.WriteAllTextAsync(path, contents);
#else
        using var writer = new StreamWriter(path, false);
        await writer.WriteAsync(contents).ConfigureAwait(false);
#endif
    }
}
