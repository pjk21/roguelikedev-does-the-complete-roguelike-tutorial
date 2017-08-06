using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Roguelike.States
{
    public class GameState : IState
    {
        public IRenderer ActiveRenderer { get; set; } = new SpriteRenderer();

        private Queue<Entity> entityActQueue = new Queue<Entity>();

        public void Initialize()
        {
            Program.Game.Camera.Follow(Program.Game.Player);

            MessageLog.Add("Welcome to the dungeon, punk.", Color.LightSteelBlue);
        }

        public void Draw()
        {
            ActiveRenderer.RenderMap(Program.Game.Map, Program.Game.Camera);
            ActiveRenderer.RenderEntities(Program.Game.Entities, Program.Game.Camera);
            ActiveRenderer.RenderUI(Program.Game.Camera);
        }

        public bool Update()
        {
            if (InputManager.CheckAction(InputAction.CycleRenderer))
            {
                SwitchRenderer();
            }
            else if (InputManager.CheckAction(InputAction.ToggleDebugMode))
            {
                Program.Game.IsDebugModeEnabled = !Program.Game.IsDebugModeEnabled;
            }

            if (Program.Game.Player.GetComponent<FighterComponent>().CurrentHealth > 0)
            {
                if (InputManager.CheckAction(InputAction.Quit))
                {
                    Program.Game.Save();
                    Program.ChangeState(new MainMenuState());
                }

                if (InputManager.CheckAction(InputAction.Save))
                {
                    Program.Game.Save();
                }

                if (entityActQueue.Count == 0)
                {
                    entityActQueue = new Queue<Entity>(Program.Game.Entities.Where(e => e.HasComponent<ActorComponent>()));
                }

                while (entityActQueue.Count > 0)
                {
                    var currentEntity = entityActQueue.Peek();

                    if (!ProcessEntity(currentEntity))
                    {
                        return true;
                    }
                    else
                    {
                        entityActQueue.Dequeue();
                    }
                }
            }
            else
            {
                if (InputManager.AnyKeyPress())
                {
                    Program.ChangeState(new GameOverState());
                }
            }

            return true;
        }

        private bool ProcessEntity(Entity entity)
        {
            if (!entity.HasComponent<ActorComponent>())
            {
                return true;
            }

            var command = entity.GetComponent<ActorComponent>().GetCommand();

            if (command == null)
            {
                return false;
            }

            while (true)
            {
                var result = command.Execute(entity);

                if (result.Result)
                {
                    break;
                }

                if (!result.Result && result.Alternative == null)
                {
                    return false;
                }

                command = result.Alternative;
            }

            if (entity == Program.Game.Player)
            {
                Program.Game.Map.FovMap.ComputeFov(entity.X, entity.Y, Entity.PlayerFovRadius, true);
                Program.Game.Camera.Follow(entity);
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
    }
}
