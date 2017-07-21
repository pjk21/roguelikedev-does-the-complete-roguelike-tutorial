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
        private Camera camera = new Camera(0, 0, Program.MapDisplayWidth, Program.MapDisplayHeight);

        public IRenderer ActiveRenderer { get; set; } = new SpriteRenderer();

        private Queue<Entity> entityActQueue = new Queue<Entity>();

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
            switch (InputManager.LastCommand)
            {
                case InputAction.Quit:
                    return false;

                case InputAction.CycleRenderer:
                    SwitchRenderer();
                    break;
                case InputAction.ToggleDebugMode:
                    Program.IsDebugModeEnabled = !Program.IsDebugModeEnabled;
                    break;
            }

            if (Program.Player.GetComponent<FighterComponent>().CurrentHealth > 0)
            {
                if (entityActQueue.Count == 0)
                {
                    entityActQueue = new Queue<Entity>(Program.Entities.Where(e => e.HasComponent<ActorComponent>()));
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
                if (InputManager.LastCommand != InputAction.None)
                {
                    Program.CurrentState = new GameOverState();
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

            if (entity == Program.Player)
            {
                Program.Map.ComputeFov(entity.X, entity.Y, Entity.PlayerFovRadius, true);
                camera.Follow(entity);
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
