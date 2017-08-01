using BearLib;
using System;
using System.Drawing;

namespace Roguelike.UI
{
    public class MenuItem
    {
        public static Color DefaultColour { get; } = Color.White;
        public static Color HighlightedColour { get; } = Color.Yellow;
        public static Color DisabledColour { get; } = Color.Silver;

        public string Text { get; set; }
        public Action OnSelected { get; set; }
        public bool IsEnabled { get; set; } = true;

        public void Select()
        {
            OnSelected?.Invoke();
        }

        public void Draw(int x, int y, bool highlighted)
        {
            if (IsEnabled)
            {
                if (highlighted)
                {
                    Terminal.Color(HighlightedColour);
                }
                else
                {
                    Terminal.Color(DefaultColour);
                }
            }
            else
            {
                Terminal.Color(DisabledColour);
            }

            Terminal.Print(x, y, Text);
        }
    }
}
