namespace Roguelike.UI
{
    public abstract class Dialog
    {
        public abstract void Show();
    }

    public abstract class Dialog<T>
    {
        public abstract T Show();
    }
}
