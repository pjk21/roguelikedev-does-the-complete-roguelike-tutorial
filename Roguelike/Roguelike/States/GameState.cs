using BearLib;
using Roguelike.Render;
using System.Linq;

namespace Roguelike.States
{
    public class GameState : IState
    {
        public IRenderer ActiveRenderer { get; set; } = new SpriteRenderer();

        public void Draw()
        {
            ActiveRenderer.RenderMap(Program.Map);
            ActiveRenderer.RenderEntities(Program.Entities);
        }

        public bool Update(int input)
        {
            var player = Program.Player;
            var map = Program.Map;

            var didPlayerAct = false;

            switch (input)
            {
                case Terminal.TK_ESCAPE:
                case Terminal.TK_CLOSE:
                    return false;

                case Terminal.TK_R when Terminal.Check(Terminal.TK_CONTROL):
                    SwitchRenderer();
                    break;

                case Terminal.TK_LEFT:
                    didPlayerAct = PlayerMoveOrAttack(-1, 0);
                    break;
                case Terminal.TK_RIGHT:
                    didPlayerAct = PlayerMoveOrAttack(1, 0);
                    break;
                case Terminal.TK_UP:
                    didPlayerAct = PlayerMoveOrAttack(0, -1);
                    break;
                case Terminal.TK_DOWN:
                    didPlayerAct = PlayerMoveOrAttack(0, 1);
                    break;
            }

            if (didPlayerAct)
            {
                map.ComputeFov(player.X, player.Y, Entity.PlayerFovRadius, true);

                foreach (var entity in Program.Entities)
                {
                    if (entity != player)
                    {
                        System.Console.WriteLine($"The {entity.Name} growls.");
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
            else if (ActiveRenderer is SpriteRenderer)
            {
                ActiveRenderer = new DebugRenderer();
            }
            else
            {
                ActiveRenderer = new AsciiRenderer();
            }
        }

        private bool PlayerMoveOrAttack(int x, int y)
        {
            var target = Program.Entities.FirstOrDefault(e => e.X == Program.Player.X + x && e.Y == Program.Player.Y + y);

            if (target != null)
            {
                System.Console.WriteLine($"You awkwardly smack the {target.Name}.");
            }
            else
            {
                return Program.Player.Move(x, y);
            }

            return true;
        }
    }
}
