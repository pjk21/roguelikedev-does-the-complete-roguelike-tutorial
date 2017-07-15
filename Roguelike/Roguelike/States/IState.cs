namespace Roguelike.States
{
    public interface IState
    {
        void Initialize();
        bool Update();
        void Draw();
    }
}
