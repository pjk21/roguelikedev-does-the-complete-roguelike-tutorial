using System;

namespace Roguelike.Entities.Components
{
    public class BasicMonsterComponent : Component
    {
        public void TakeTurn()
        {
            Console.WriteLine($"The {Entity.Name} growls.");
        }
    }
}
