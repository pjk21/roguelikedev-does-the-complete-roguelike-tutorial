using BearLib;
using Newtonsoft.Json;
using Roguelike.Render;
using RogueSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Roguelike.Input
{
    public static class InputManager
    {
        private const string KeyMapFile = "keys.json";

        private static Dictionary<InputAction, List<KeyPress>> inputMap = new Dictionary<InputAction, List<KeyPress>>();

        private static int lastInput;

        public static InputAction LastCommand { get; private set; }

        public static Point MousePosition { get; private set; } = new Point();

        static InputManager()
        {
#if DEBUG
            inputMap = GetDefaultInputMap();
#else
            if (!File.Exists(KeyMapFile))
            {
                inputMap = GetDefaultInputMap();
                SaveKeyMap(KeyMapFile);
            }
            else
            {
                inputMap = LoadKeyMap(KeyMapFile);
            }
#endif
        }

        public static Point GetMouseWorldPosition(Camera camera)
        {
            int x = MousePosition.X + camera.X;
            int y = MousePosition.Y + camera.Y;

            return new Point(x, y);
        }

        public static void Update()
        {
            lastInput = Terminal.Read();
            LastCommand = inputMap.FirstOrDefault(kvp => kvp.Value.Any(map => map.Check(lastInput))).Key;

            MousePosition.X = Terminal.State(Terminal.TK_MOUSE_X);
            MousePosition.Y = Terminal.State(Terminal.TK_MOUSE_Y);
        }

        public static void SaveKeyMap(string file)
        {
            var json = JsonConvert.SerializeObject(inputMap, Formatting.Indented);
            File.WriteAllText(file, json);
        }

        public static Dictionary<InputAction, List<KeyPress>> LoadKeyMap(string file)
        {
            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Dictionary<InputAction, List<KeyPress>>>(json);
        }

        private static Dictionary<InputAction, List<KeyPress>> GetDefaultInputMap()
        {
            var map = new Dictionary<InputAction, List<KeyPress>>();

            map.Add(InputAction.Quit, new List<KeyPress> { new KeyPress(Terminal.TK_ESCAPE), new KeyPress(Terminal.TK_CLOSE) });
            map.Add(InputAction.CycleRenderer, new List<KeyPress> { new KeyPress(Terminal.TK_F2) });
            map.Add(InputAction.ToggleDebugMode, new List<KeyPress> { new KeyPress(Terminal.TK_F3) });

            map.Add(InputAction.MoveEast, new List<KeyPress> { new KeyPress(Terminal.TK_RIGHT), new KeyPress(Terminal.TK_KP_6) });
            map.Add(InputAction.MoveWest, new List<KeyPress> { new KeyPress(Terminal.TK_LEFT), new KeyPress(Terminal.TK_KP_4) });
            map.Add(InputAction.MoveNorth, new List<KeyPress> { new KeyPress(Terminal.TK_UP), new KeyPress(Terminal.TK_KP_8) });
            map.Add(InputAction.MoveSouth, new List<KeyPress> { new KeyPress(Terminal.TK_DOWN), new KeyPress(Terminal.TK_KP_2) });
            map.Add(InputAction.Rest, new List<KeyPress> { new KeyPress(Terminal.TK_SPACE) });

            return map;
        }
    }
}
