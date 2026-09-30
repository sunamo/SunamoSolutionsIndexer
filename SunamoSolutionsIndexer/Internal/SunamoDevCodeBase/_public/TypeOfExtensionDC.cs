namespace SunamoSolutionsIndexer.Internal._public;

public enum TypeOfExtensionDC
{
    archive,
    image,
    source_code,
    documentText,
    documentBinary,
    database,

    // prošel jsem zda v AllExtension jsou všechny textové
    configText,

    // XML, JSON, mdf, ldf, sdf, atd.
    // Can't name data because is difficult search (exists also database)
    contentText,

    contentBinary,

    // prošel jsem zda v AllExtension jsou všechny textové
    // ini, atd.
    settingsText,

    // prošel jsem zda v AllExtension jsou všechny textové
    visual_studioText,

    executable,
    binary,

    // u resourců by to asi tak nevadilo kdyby byly zakódovany třeba ve b64 ale pro jistotu je všechny řadím do binárních
    // ať je nepoškodím
    resource,

    // prošel jsem zda v AllExtension jsou všechny textové
    // sql, cmd, ps1,
    script,

    font,
    multimedia,
    temporary,

    // Is used when extension isn't know
    // U ostatních souborů vypsat jejich popis z windows
    other
}
