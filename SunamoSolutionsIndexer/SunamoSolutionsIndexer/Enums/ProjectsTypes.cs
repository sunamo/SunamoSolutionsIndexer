namespace SunamoSolutionsIndexer.Enums;

/// <summary>
/// Defines the types of projects based on their platform, technology, or organizational category.
/// </summary>
public enum ProjectsTypes
{
    /// <summary>
    /// Projects targeting only Windows with .NET Core.
    /// </summary>
    OnlyWindowsCore,
    /// <summary>
    /// Projects that do not target .NET Core.
    /// </summary>
    NotCore_Projects,
    /// <summary>
    /// Projects forked from GitHub repositories.
    /// </summary>
    ForkedOnGitHub,

    /// <summary>
    /// Azure cloud service projects.
    /// </summary>
    Azure,
    /// <summary>
    /// Jenkins continuous integration projects.
    /// </summary>
    Jenkins,

    /// <summary>
    /// Bobril framework projects.
    /// </summary>
    Bobril,
    /// <summary>
    /// C language projects.
    /// </summary>
    C,
    /// <summary>
    /// C++ language projects.
    /// </summary>
    Cpp,
    /// <summary>
    /// Chrome extension or Chrome-related projects.
    /// </summary>
    Chrome,
    /// <summary>
    /// Visual Studio Code extension projects.
    /// </summary>
    Code,
    /// <summary>
    /// CSS stylesheet projects.
    /// </summary>
    Css,
    /// <summary>
    /// Dart language projects.
    /// </summary>
    Dart,
    /// <summary>
    /// Docker container projects.
    /// </summary>
    Docker,
    /// <summary>
    /// Exercism coding exercise projects.
    /// </summary>
    Exercism,
    /// <summary>
    /// Go language projects.
    /// </summary>
    Go,
    /// <summary>
    /// Haskell language projects.
    /// </summary>
    Haskell,
    /// <summary>
    /// Java language projects.
    /// </summary>
    Java,
    /// <summary>
    /// JavaScript language projects.
    /// </summary>
    JavaScript,
    /// <summary>
    /// Mono framework projects.
    /// </summary>
    Mono,
    /// <summary>
    /// Node.js projects.
    /// </summary>
    Node,
    /// <summary>
    /// Projects that are no longer in use.
    /// </summary>
    NotUsed,
    /// <summary>
    /// Projects that do not target .NET Core.
    /// </summary>
    NotCore,
    /// <summary>
    /// Web projects that do not target .NET Core.
    /// </summary>
    NotCoreWeb,
    /// <summary>
    /// PHP language projects.
    /// </summary>
    PHP,
    /// <summary>
    /// C# language projects.
    /// </summary>
    Cs,
    /// <summary>
    /// Python language projects.
    /// </summary>
    Python,
    /// <summary>
    /// Advanced React framework projects.
    /// </summary>
    ReactAdvanced,
    /// <summary>
    /// React Native mobile framework projects.
    /// </summary>
    ReactNative,
    /// <summary>
    /// React framework projects.
    /// </summary>
    React,
    /// <summary>
    /// Redux state management projects.
    /// </summary>
    Redux,
    /// <summary>
    /// Ruby language projects.
    /// </summary>
    Ruby,
    /// <summary>
    /// Script-based projects.
    /// </summary>
    Scripts,
    /// <summary>
    /// SQL database projects.
    /// </summary>
    Sql,
    /// <summary>
    /// Telmax company projects.
    /// </summary>
    Telmax,
    /// <summary>
    /// Trask company projects.
    /// </summary>
    Trask,
    /// <summary>
    /// TypeScript language projects.
    /// </summary>
    TypeScript,
    /// <summary>
    /// Vue.js framework projects.
    /// </summary>
    Vue,
    /// <summary>
    /// Xamarin mobile framework projects.
    /// </summary>
    Xamarin,

    /// <summary>
    /// Projects forked from GitHub (alternative naming).
    /// </summary>
    ForkedGithub,
    /// <summary>
    /// Projects not owned by the current user.
    /// </summary>
    NotMine,
    /// <summary>
    /// Old Novanta company projects.
    /// </summary>
    NovantaOld,
    /// <summary>
    /// Novanta company projects.
    /// </summary>
    Novanta,

    /// <summary>
    /// No specific project type assigned.
    /// </summary>
    None,
    /// <summary>
    /// Unknown project type when Projects folder is not paired.
    /// </summary>
    Unknown
}