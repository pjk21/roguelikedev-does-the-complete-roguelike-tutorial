namespace Roguelike.Entities.Components
{
    public class FighterComponent : Component
    {
        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Power { get; set; }
        public int Defense { get; set; }
    }
}
