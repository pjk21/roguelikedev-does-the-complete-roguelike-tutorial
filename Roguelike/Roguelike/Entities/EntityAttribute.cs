using System;

namespace Roguelike.Entities
{
    [Serializable]
    public class EntityAttribute
    {
        public int Base { get; set; }
        public int Modifier { get; set; }

        public int Value => Base + Modifier;
    }
}
