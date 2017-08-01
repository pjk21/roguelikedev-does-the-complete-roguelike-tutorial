using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.UI;
using Roguelike.World.MapGeneration;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Roguelike.States
{
    public class GameState : IState
    {
        public static Camera Camera { get; } = new Camera(0, 0, Program.MapDisplayWidth, Program.MapDisplayHeight);

        public IRenderer ActiveRenderer { get; set; } = new SpriteRenderer();

        private Queue<Entity> entityActQueue = new Queue<Entity>();

        public void Initialize()
        {
            Program.Entities.Clear();

            Program.Player = new Entity("Player", 25, 23, '@', Colours.Player, true)
            {
                SpriteIndex = EntitySprites.Player,
                RenderLayer = Renderer.ActorLayer
            };

            Program.Player.AddComponent(new FighterComponent { MaximumHealth = 30, CurrentHealth = 30, Power = 5, Defense = 2, DeathFunction = DeathFunctions.PlayerDeath });
            Program.Player.AddComponent(new PlayerInputComponent());
            Program.Player.AddComponent(new InventoryComponent());

            Program.Entities.Add(Program.Player);

            Program.Map = new BspMapGenerator().Generate(80, 50);
            Program.Map.ComputeFov(Program.Player.X, Program.Player.Y, Entity.PlayerFovRadius, true);

            foreach (var cell in Program.Map.GetAllCells())
            {
                Program.Map.PathfindingMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable);
            }

            Camera.Follow(Program.Player);

            MessageLog.Add("Welcome to the dungeon, punk.", Color.LightSteelBlue);
        }

        public void Draw()
        {
            ActiveRenderer.RenderMap(Program.Map, Camera);
            ActiveRenderer.RenderEntities(Program.Entities, Camera);
            ActiveRenderer.RenderUI(Camera);
        }

        public bool Update()
        {
            if (InputManager.CheckAction(InputAction.CycleRenderer))
            {
                SwitchRenderer();
            }
            else if (InputManager.CheckAction(InputAction.ToggleDebugMode))
            {
                Program.IsDebugModeEnabled = !Program.IsDebugModeEnabled;
            }

            if (Program.Player.GetComponent<FighterComponent>().CurrentHealth > 0)
            {
                if (InputManager.CheckAction(InputAction.Quit))
                {
                    Program.ChangeState(new MainMenuState());
                }

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

            if (entity == Program.Player)
            {
                Program.Map.ComputeFov(entity.X, entity.Y, Entity.PlayerFovRadius, true);
                Camera.Follow(entity);
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
