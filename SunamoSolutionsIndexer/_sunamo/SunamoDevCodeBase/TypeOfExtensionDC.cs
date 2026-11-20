namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Type of the file extension.
/// </summary>
public enum TypeOfExtensionDC
{
    /// <summary>
    /// The archive value.
    /// </summary>
    archive,
    /// <summary>
    /// The image value.
    /// </summary>
    image,
    /// <summary>
    /// The source code value.
    /// </summary>
    source_code,
    /// <summary>
    /// The document text value.
    /// </summary>
    documentText,
    /// <summary>
    /// The document binary value.
    /// </summary>
    documentBinary,
    /// <summary>
    /// The database value.
    /// </summary>
    database,

    // I went through whether AllExtension contains all text ones
    /// <summary>
    /// The config text value.
    /// </summary>
    configText,

    // XML, JSON, mdf, ldf, sdf, atd.
    // Can't name data because is difficult search (exists also database)
    /// <summary>
    /// The content text value.
    /// </summary>
    contentText,

    /// <summary>
    /// The content binary value.
    /// </summary>
    contentBinary,

    // I went through whether AllExtension contains all text ones
    // ini, atd.
    /// <summary>
    /// The settings text value.
    /// </summary>
    settingsText,

    // I went through whether AllExtension contains all text ones
    /// <summary>
    /// The visual studio text value.
    /// </summary>
    visual_studioText,

    /// <summary>
    /// The executable value.
    /// </summary>
    executable,
    /// <summary>
    /// The binary value.
    /// </summary>
    binary,

    // for resources it would probably not matter if they were encoded e.g. in b64, but to be safe I sort them all as binary
    // so that I do not damage them
    /// <summary>
    /// The resource value.
    /// </summary>
    resource,

    // I went through whether AllExtension contains all text ones
    // sql, cmd, ps1,
    /// <summary>
    /// The script value.
    /// </summary>
    script,

    /// <summary>
    /// The font value.
    /// </summary>
    font,
    /// <summary>
    /// The multimedia value.
    /// </summary>
    multimedia,
    /// <summary>
    /// The temporary value.
    /// </summary>
    temporary,

    // Is used when extension isn't know
    // For other files, print their description from Windows
    /// <summary>
    /// The other value.
    /// </summary>
    other
}