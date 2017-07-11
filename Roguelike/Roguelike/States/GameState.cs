using BearLib;
using Roguelike.Render;

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

            var computeFov = false;

            switch (input)
            {
                case Terminal.TK_ESCAPE:
                case Terminal.TK_CLOSE:
                    return false;

                case Terminal.TK_R when Terminal.Check(Terminal.TK_CONTROL):
                    SwitchRenderer();
                    break;

                case Terminal.TK_LEFT:
                    player.Move(-1, 0);
                    computeFov = true;
                    break;
                case Terminal.TK_RIGHT:
                    player.Move(1, 0);
                    computeFov = true;
                    break;
                case Terminal.TK_UP:
                    player.Move(0, -1);
                    computeFov = true;
                    break;
                case Terminal.TK_DOWN:
                    player.Move(0, 1);
                    computeFov = true;
                    break;
            }

            if (computeFov)
            {
                map.ComputeFov(player.X, player.Y, Entity.PlayerFovRadius, true);
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
    }
}
