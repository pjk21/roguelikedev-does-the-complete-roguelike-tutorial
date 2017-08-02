using System.Collections.Generic;

namespace Roguelike.Entities.Components
{
    public class InventoryComponent : Component
    {
        private readonly List<Entity> items = new List<Entity>();

        public Entity[] Items => items.ToArray();

        public void Add(Entity item)
        {
            items.Add(item);

            Program.Game.Entities.Remove(item);
        }

        public void Remove(Entity item, bool drop)
        {
            items.Remove(item);

            if (drop)
            {
                item.X = Entity.X;
                item.Y = Entity.Y;
                Program.Game.Entities.Add(item);
            }
        }
    }
}
