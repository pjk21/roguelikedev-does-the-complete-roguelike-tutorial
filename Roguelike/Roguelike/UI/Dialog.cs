using BearLib;
using Roguelike.Input;
using Roguelike.Render;
using System.Drawing;

namespace Roguelike.UI
{
    public abstract class Dialog
    {
        public void Show()
        {
            while (true)
            {
                Terminal.Layer(Renderer.DialogLayer);
                Terminal.Color(Color.White);
                Terminal.BkColor(Color.Black);
                Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

                Draw();

                Terminal.Refresh();

                InputManager.Update();

                if (Update())
                {
                    break;
                }
            }
        }

        protected abstract void Draw();
        protected abstract bool Update();
    }

    public abstract class Dialog<T>
    {
        public T Show()
        {
            T result = default(T);

            while (true)
            {
                Terminal.Layer(Renderer.DialogLayer);
                Terminal.Color(Color.White);
                Terminal.BkColor(Color.Black);
                Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

                Draw();

                Terminal.Refresh();

                InputManager.Update();

                if (Update(out result))
                {
                    break;
                }
            }

            return result;
        }

        protected abstract void Draw();
        protected abstract bool Update(out T result);
    }
}
