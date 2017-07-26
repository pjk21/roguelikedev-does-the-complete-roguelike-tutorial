﻿using BearLib;
using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.UI;
using Roguelike.World;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Roguelike.Render
{
    public abstract class Renderer : IRenderer
    {
        public const int MapLayer = 0;
        public const int CorpseLayer = 8;
        public const int ItemLayer = 9;
        public const int ActorLayer = 10;
        public const int UILayer = 20;
        public const int DialogLayer = 25;

        public abstract void RenderEntities(IEnumerable<Entity> entities, Camera camera);

        public void RenderMap(Map map, Camera camera)
        {
            Terminal.Layer(MapLayer);

            for (int x = camera.Left; x < camera.Right; x++)
            {
                for (int y = camera.Top; y < camera.Bottom; y++)
                {
                    RenderTile(map, x, y, camera);
                }
            }
        }

        protected abstract void RenderTile(Map map, int x, int y, Camera camera);

        public virtual void RenderUI(Camera camera)
        {
            Terminal.Color(Color.White);
            Terminal.Layer(UILayer);

            for (int y = 0; y < Program.ScreenHeight; y++)
            {
                Terminal.Put(Program.MapDisplayWidth, y, 0x2502);
            }

            int x = Program.MapDisplayWidth + 1;
            var playerFighter = Program.Player.GetComponent<FighterComponent>();

            Terminal.Color(Color.Gold);
            Terminal.Print(x, 1, $"Player");

            Terminal.Color(Color.White);
            Terminal.Print(x + 1, 2, $"HP");
            Terminal.Print(Program.ScreenWidth - $"{playerFighter.CurrentHealth}/{playerFighter.MaximumHealth}".Length - 1, 2, $"{playerFighter.CurrentHealth}/{playerFighter.MaximumHealth}");
            Terminal.Print(x + 1, 3, $"POW");
            Terminal.Print(Program.ScreenWidth - playerFighter.Power.ToString().Length - 1, 3, playerFighter.Power.ToString());
            Terminal.Print(x + 1, 4, $"DEF");
            Terminal.Print(Program.ScreenWidth - playerFighter.Defense.ToString().Length - 1, 4, playerFighter.Defense.ToString());

            var mouse = InputManager.GetMouseWorldPosition(camera);

            if (Program.Map.IsInFov(mouse.X, mouse.Y))
            {
                var entitiesUnderMouse = Program.Entities
                    .Where(e => e.X == mouse.X && e.Y == mouse.Y)
                    .Take(5);

                int entityUnderMouseY = 6;
                foreach (var entity in entitiesUnderMouse)
                {
                    Terminal.Print(x, entityUnderMouseY++, entity.Name);
                }
            }

            MessageLog.Render();
        }
    }
}
