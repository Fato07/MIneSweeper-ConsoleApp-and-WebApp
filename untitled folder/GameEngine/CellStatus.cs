namespace GameEngine
{
    public enum CellStatus
    {
        ClosedAndNotAMine,
        ClosedMine,
        OpenMine,
        OpenedAndNotAMine,
        FlaggedMine,
        FlaggedAndNotMine,
    }
}