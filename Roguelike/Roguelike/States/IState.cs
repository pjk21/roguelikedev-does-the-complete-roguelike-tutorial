namespace Roguelike.States
{
    public interface IState
    {
        bool Update();
        void Draw();
    }
}
