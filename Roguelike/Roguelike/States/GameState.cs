﻿using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.UI;
using System.Drawing;
using System.Linq;

namespace Roguelike.States
{
    public class GameState : IState
    {
        private Camera camera = new Camera(0, 0, Program.MapDisplayWidth, Program.MapDisplayHeight);

        public IRenderer ActiveRenderer { get; set; } = new SpriteRenderer();

        public void Initialize()
        {
            camera.Follow(Program.Player);

            MessageLog.Add("Welcome to the dungeon, punk.", Color.LightSteelBlue);
        }

        public void Draw()
        {
            ActiveRenderer.RenderMap(Program.Map, camera);
            ActiveRenderer.RenderEntities(Program.Entities, camera);
            ActiveRenderer.RenderUI(camera);
        }

        public bool Update()
        {
            var player = Program.Player;
            var map = Program.Map;

            var didPlayerAct = false;

            if (Program.Player.GetComponent<FighterComponent>().CurrentHealth > 0)
            {
                switch (InputManager.LastCommand)
                {
                    case Command.Quit:
                        return false;

                    case Command.CycleRenderer:
                        SwitchRenderer();
                        break;
                    case Command.ToggleDebugMode:
                        Program.IsDebugModeEnabled = !Program.IsDebugModeEnabled;
                        break;

                    case Command.MoveEast:
                        didPlayerAct = PlayerMoveOrAttack(-1, 0);
                        break;
                    case Command.MoveWest:
                        didPlayerAct = PlayerMoveOrAttack(1, 0);
                        break;
                    case Command.MoveNorth:
                        didPlayerAct = PlayerMoveOrAttack(0, -1);
                        break;
                    case Command.MoveSouth:
                        didPlayerAct = PlayerMoveOrAttack(0, 1);
                        break;
                }
            }
            else
            {
                if (InputManager.LastCommand != Command.None)
                {
                    Program.CurrentState = new GameOverState();
                }
            }

            if (didPlayerAct)
            {
                map.ComputeFov(player.X, player.Y, Entity.PlayerFovRadius, true);
                camera.Follow(player);

                foreach (var entity in Program.Entities)
                {
                    if (entity != player)
                    {
                        entity.GetComponent<BasicMonsterComponent>()?.TakeTurn();
                    }
                }
            }

            return true;
        }

        private void SwitchRenderer()
        {
            if (ActiveRenderer is AsciiRenderer)
            {
                ActiveRenderer = new SpriteRenderer();
            }
            else
            {
                ActiveRenderer = new AsciiRenderer();
            }
        }

        private bool PlayerMoveOrAttack(int x, int y)
        {
            var target = Program.Entities.FirstOrDefault(e => e.HasComponent<FighterComponent>() && e.X == Program.Player.X + x && e.Y == Program.Player.Y + y);

            if (target != null)
            {
                Program.Player.GetComponent<FighterComponent>().Attack(target);
            }
            else
            {
                return Program.Player.Move(x, y);
            }

            return true;
        }
    }
}
