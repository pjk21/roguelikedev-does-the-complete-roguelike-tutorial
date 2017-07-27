using BearLib;
using Roguelike.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Roguelike.UI
{
    public static class MessageLog
    {
        public static readonly Queue<Message> messages = new Queue<Message>();

        public static int MaximumPanelLines { get; } = 6;

        public static void Add(string text) => Add(text, Color.White);

        public static void Add(string text, Color colour)
        {
            foreach (var line in WrapText(text))
            {
                var message = new Message(line, colour);
                messages.Enqueue(message);
            }
        }

        public static void Clear()
        {
            messages.Clear();
        }

        public static void Render()
        {
            Terminal.Color(Color.White);
            Terminal.Layer(Renderer.UILayer);
            int yOffset = 0;

            for (int x = 0; x < Program.MapDisplayWidth; x++)
            {
                Terminal.Put(x, Program.MapDisplayHeight, 0x2500);
            }

            Terminal.Put(Program.MapDisplayWidth, Program.MapDisplayHeight, 0x2524);

            foreach (var message in messages.Skip(Math.Max(0, messages.Count - MaximumPanelLines)))
            {
                Terminal.Print(1, Program.MapDisplayHeight + (yOffset++) + 1, $"[color={message.Colour.ToArgb()}]{message.Text}");
            }
        }

        private static IEnumerable<string> WrapText(string line)
        {
            var maximumWidth = Program.MapDisplayWidth;
            var split = line.Split(' ');

            var sb = new StringBuilder();

            foreach (var word in split)
            {
                if (sb.Length + word.Length >= maximumWidth)
                {
                    yield return sb.ToString().Trim();
                    sb.Clear();
                }

                sb.Append(word + " ");
            }

            yield return sb.ToString().Trim();
        }

        public class Message
        {
            public string Text { get; }
            public Color Colour { get; }

            public Message(string text, Color colour)
            {
                Text = text;
                Colour = colour;
            }
        }
    }
}
