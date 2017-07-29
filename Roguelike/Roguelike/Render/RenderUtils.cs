using BearLib;
using Roguelike.UI;
using System.Drawing;

namespace Roguelike.Render
{
    public static class RenderUtils
    {
        public static void DrawBox(int x, int y, int width, int height, Color background, Color foreground)
        {
            for (int ix = x; ix < x + width; ix++)
            {
                for (int iy = y; iy < y + height; iy++)
                {
                    Terminal.Color(background);
                    Terminal.Put(ix, iy, UISprites.DialogBackground);

                    if (ix == x && iy == y)
                    {
                        Terminal.Color(foreground);
                        Terminal.Put(ix, iy, UISprites.TopLeftCornerBorder);
                    }
                    else if (ix == x && iy == (y + height) - 1)
                    {
                        Terminal.Color(foreground);
                        Terminal.Put(ix, iy, UISprites.BottomLeftCornerBorder);
                    }
                    else if (ix == (x + width) - 1 && iy == y)
                    {
                        Terminal.Color(foreground);
                        Terminal.Put(ix, iy, UISprites.TopRightCornerBorder);
                    }
                    else if (ix == (x + width) - 1 && iy == (y + height) - 1)
                    {
                        Terminal.Color(foreground);
                        Terminal.Put(ix, iy, UISprites.BottomRightCornerBorder);
                    }
                    else if (ix == x || ix == (x + width) - 1)
                    {
                        Terminal.Color(foreground);
                        Terminal.Put(ix, iy, UISprites.VerticalBorder);
                    }
                    else if (iy == y || iy == (y + height) - 1)
                    {
                        Terminal.Color(foreground);
                        Terminal.Put(ix, iy, UISprites.HorizontalBorder);
                    }
                }
            }
        }

        public static void DrawBox(Rectangle bounds, Color background, Color foreground) => DrawBox(bounds.X, bounds.Y, bounds.Width, bounds.Height, background, foreground);
    }
}
