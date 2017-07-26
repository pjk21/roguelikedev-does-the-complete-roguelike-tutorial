namespace Roguelike.Input
{
    public enum InputAction
    {
        None,

        Quit,
        CycleRenderer,
        ToggleDebugMode,

        MoveNorth,
        MoveSouth,
        MoveEast,
        MoveWest,
        MoveNorthEast,
        MoveNorthWest,
        MoveSouthEast,
        MoveSouthWest,

        Rest,
        Take,

        ShowInventory,
        UseItem,

        ClickMove
    }
}
