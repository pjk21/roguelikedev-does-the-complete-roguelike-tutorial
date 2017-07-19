﻿using System;
using System.Collections.Generic;
using System.Drawing;

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
        public int LayerOffset { get; set; }

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

        public Entity AddComponent<T>(T component) where T : Component
        {
            component.Entity = this;
            components[typeof(T)] = component;

            return this;
        }

        public void RemoveComponent<T>() where T : Component
        {
            components.Remove(typeof(T));
        }

        public bool HasComponent<T>() where T : Component
        {
            return components.ContainsKey(typeof(T));
        }

        public T GetComponent<T>() where T : Component
        {
            if (components.TryGetValue(typeof(T), out Component component))
            {
                return (T)component;
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