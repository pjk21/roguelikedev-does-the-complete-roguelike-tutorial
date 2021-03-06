﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Roguelike.Entities
{
    [Serializable]
    public class Entity
    {
        public const int PlayerFovRadius = 10;
        public delegate Entity EntityFactoryDelegate(int x, int y, int dungeonLevel);

        private readonly Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public string Name { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public char Character { get; set; }
        public Color Colour { get; set; } = Color.White;
        public int RenderLayer { get; set; }
        public bool IsAlwaysVisible { get; set; }

        public int? SpriteIndex { get; set; }
        public Color SpriteTint { get; set; } = Color.White;

        public bool IsSolid { get; set; } = false;

        public HashSet<string> Tags { get; } = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public event Action TurnEnd;

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
            components[component.GetType()] = component;

            component.OnAdded();
        }

        public void RemoveComponent<T>() where T : Component
        {
            var component = GetComponent<T>();

            if (component != null)
            {
                RemoveComponent(component);
            }
        }

        public void RemoveComponent(Component component)
        {
            if (components.ContainsValue(component))
            {
                components.Remove(component.GetType());
                component.OnRemoved();
            }
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

        public void EndTurn()
        {
            TurnEnd?.Invoke();
        }

        public float DistanceTo(Entity other)
        {
            return DistanceTo(other.X, other.Y);
        }

        public float DistanceTo(int x, int y)
        {
            var dx = x - X;
            var dy = y - Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
