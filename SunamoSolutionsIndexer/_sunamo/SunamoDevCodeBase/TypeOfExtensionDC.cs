namespace SunamoSolutionsIndexer._sunamo.SunamoDevCodeBase;

/// <summary>
/// Type of the file extension.
/// </summary>
public enum TypeOfExtensionDC
{
    archive,
    image,
    source_code,
    documentText,
    documentBinary,
    database,

    // I went through whether AllExtension contains all text ones
    configText,

    // XML, JSON, mdf, ldf, sdf, atd.
    // Can't name data because is difficult search (exists also database)
    contentText,

    contentBinary,

    // I went through whether AllExtension contains all text ones
    // ini, atd.
    settingsText,

    // I went through whether AllExtension contains all text ones
    visual_studioText,

    executable,
    binary,

    // for resources it would probably not matter if they were encoded e.g. in b64, but to be safe I sort them all as binary
    // so that I do not damage them
    resource,

    // I went through whether AllExtension contains all text ones
    // sql, cmd, ps1,
    script,

    font,
    multimedia,
    temporary,

    // Is used when extension isn't know
    // For other files, print their description from Windows
    other
}