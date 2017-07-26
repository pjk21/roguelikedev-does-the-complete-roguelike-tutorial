using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Roguelike.Entities
{
    public class Entity
    {
        public const int PlayerFovRadius = 10;

        private readonly Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public string Name { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public char Character { get; set; }
        public Color Colour { get; set; } = Color.White;
        public int RenderLayer { get; set; }

        public int? SpriteIndex { get; set; }
        public Color SpriteTint { get; set; } = Color.White;

        public bool IsSolid { get; set; } = false;

        public Entity(string name, int x, int y, char character, Color colour, bool solid = false)
        {
            Name = name;

            X = x;
            Y = y;

            Character = character;
            Colour = colour;

            IsSolid = solid;
        }

        public void AddComponent<T>(T component) where T : Component
        {
            component.Entity = this;
            components[typeof(T)] = component;
        }

        public void RemoveComponent<T>() where T : Component
        {
            components.Remove(typeof(T));
        }

        public bool HasComponent<T>() where T : Component
        {
            if (components.ContainsKey(typeof(T)))
            {
                return true;
            }

            return components.Keys.Any(t => typeof(T).IsAssignableFrom(t));
        }

        public T GetComponent<T>() where T : Component
        {
            if (components.TryGetValue(typeof(T), out Component component))
            {
                return (T)component;
            }
            else
            {
                var derived = components.Keys.FirstOrDefault(t => typeof(T).IsAssignableFrom(t));

                if (derived != null)
                {
                    return (T)components[derived];
                }
            }

            return null;
        }

        public bool Move(int x, int y)
        {
            if (Program.Map.CanEnter(X + x, Y + y))
            {
                X += x;
                Y += y;

                return true;
            }

            return false;
        }

        public float DistanceTo(Entity other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
