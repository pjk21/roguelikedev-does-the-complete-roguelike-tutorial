namespace Roguelike.World.MapGeneration
{
    public interface IMapGenerator
    {
        Map Generate(int width, int height);
    }
}
