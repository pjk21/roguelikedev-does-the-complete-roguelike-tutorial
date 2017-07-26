using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roguelike.Entities.Components
{
    public class InventoryComponent : Component
    {
        private readonly List<Entity> items = new List<Entity>();

        public Entity[] Items => items.ToArray();

        public void Add(Entity item)
        {
            items.Add(item);

            Program.Entities.Remove(item);
        }

        public void Remove(Entity item)
        {
            items.Remove(item);

            item.X = Entity.X;
            item.Y = Entity.Y;
            Program.Entities.Add(item);
        }
    }
}
