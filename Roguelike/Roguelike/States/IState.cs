namespace Roguelike.States
{
    public interface IState
    {
        void Initialise();
        bool Update();
        void Draw();
    }
}
