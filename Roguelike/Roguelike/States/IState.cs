namespace Roguelike.States
{
    public interface IState
    {
        bool Update(int input);
        void Draw();
    }
}
