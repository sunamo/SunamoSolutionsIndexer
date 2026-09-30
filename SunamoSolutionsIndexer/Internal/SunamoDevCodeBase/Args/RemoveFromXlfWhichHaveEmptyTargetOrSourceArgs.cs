namespace SunamoDevCode.Args;

internal class RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs
{
    public static RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs Default { get; set; } = new RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs();

    public bool RemoveWholeTransUnit { get; set; } = true;

    public bool Save { get; set; } = true;
}
