using BearLib;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Input
{
    public static class InputManager
    {
        private static readonly Dictionary<Command, List<KeyPress>> inputMap = new Dictionary<Command, List<KeyPress>>();

        private static int lastInput;

        public static Command LastCommand { get; private set; }

        static InputManager()
        {
            inputMap = GetDefaultInputMap();
        }

        public static void Update()
        {
            lastInput = Terminal.Read();
            LastCommand = inputMap.FirstOrDefault(kvp => kvp.Value.Any(map => map.Check(lastInput))).Key;
        }

        private static Dictionary<Command, List<KeyPress>> GetDefaultInputMap()
        {
            var map = new Dictionary<Command, List<KeyPress>>();

            map.Add(Command.Quit, new List<KeyPress> { new KeyPress(Terminal.TK_ESCAPE), new KeyPress(Terminal.TK_CLOSE) });
            map.Add(Command.CycleRenderer, new List<KeyPress> { new KeyPress(Terminal.TK_F9) });

            map.Add(Command.MoveEast, new List<KeyPress> { new KeyPress(Terminal.TK_LEFT), new KeyPress(Terminal.TK_KP_4) });
            map.Add(Command.MoveWest, new List<KeyPress> { new KeyPress(Terminal.TK_RIGHT), new KeyPress(Terminal.TK_KP_6) });
            map.Add(Command.MoveNorth, new List<KeyPress> { new KeyPress(Terminal.TK_UP), new KeyPress(Terminal.TK_KP_8) });
            map.Add(Command.MoveSouth, new List<KeyPress> { new KeyPress(Terminal.TK_DOWN), new KeyPress(Terminal.TK_KP_2) });

            return map;
        }
    }
}
