using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Utils
{
    public class WeightedPool<T>
    {
        private readonly Dictionary<T, int> choices = new Dictionary<T, int>();

        public void Add(T item, int weight)
        {
            if (weight > 0)
            {
                choices[item] = weight;
            }
        }

        public void Remove(T item)
        {
            choices.Remove(item);
        }

        public T Pick()
        {
            var sum = choices.Values.Sum();
            var randomValue = Program.Game.Random.Next(sum);
            int total = 0;

            foreach (var weightedChoice in choices)
            {
                total += weightedChoice.Value;

                if (randomValue <= total)
                {
                    return weightedChoice.Key;
                }
            }

            throw new System.Exception("Could not return an item from the pool.");
        }
    }
}
