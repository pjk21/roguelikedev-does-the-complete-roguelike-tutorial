namespace Roguelike.Input
{
    public enum InputAction
    {
        None,

        Quit,
        CycleRenderer,
        ToggleDebugMode,

        MenuUp,
        MenuDown,
        MenuLeft,
        MenuRight,
        MenuSelect,
        MenuCancel,

        MoveNorth,
        MoveSouth,
        MoveEast,
        MoveWest,
        MoveNorthEast,
        MoveNorthWest,
        MoveSouthEast,
        MoveSouthWest,
        MouseMove,

        Rest,
        Take,

        ShowInventory,
        UseItem,
        DropItem,

        LeftClick
    }
}
