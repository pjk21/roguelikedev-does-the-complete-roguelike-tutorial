namespace Roguelike.Render
{
    public class Camera : RogueSharp.Rectangle
    {
        public Camera(int x, int y, int width, int height)
            : base(x, y, width, height)
        {

        }

        public virtual void Follow(Entity entity)
        {
            X = (entity.X - Width / 2).Clamp(0, Program.Map.Width - Width);
            Y = (entity.Y - Height / 2).Clamp(0, Program.Map.Height - Height);
        }
    }
}
