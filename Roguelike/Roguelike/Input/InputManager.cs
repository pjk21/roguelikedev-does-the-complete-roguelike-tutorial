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
            if (Terminal.HasInput())
            {
                lastInput = Terminal.Read();
                LastCommand = inputMap.FirstOrDefault(kvp => kvp.Value.Any(map => map.Check(lastInput))).Key;
            }
            else
            {
                lastInput = Terminal.TK_INPUT_NONE;
                LastCommand = InputAction.None;
            }

            MousePosition.X = Terminal.State(Terminal.TK_MOUSE_X);
            MousePosition.Y = Terminal.State(Terminal.TK_MOUSE_Y);
        }

        public static InputAction AwaitInput()
        {
            var input = Terminal.Read();
            return inputMap.FirstOrDefault(kvp => kvp.Value.Any(map => map.Check(input))).Key;
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
            map.Add(InputAction.MoveNorthEast, new List<KeyPress> { new KeyPress(Terminal.TK_KP_9) });
            map.Add(InputAction.MoveNorthWest, new List<KeyPress> { new KeyPress(Terminal.TK_KP_7) });
            map.Add(InputAction.MoveSouthEast, new List<KeyPress> { new KeyPress(Terminal.TK_KP_3) });
            map.Add(InputAction.MoveSouthWest, new List<KeyPress> { new KeyPress(Terminal.TK_KP_1) });

            map.Add(InputAction.Rest, new List<KeyPress> { new KeyPress(Terminal.TK_R) });
            map.Add(InputAction.Take, new List<KeyPress> { new KeyPress(Terminal.TK_G) });

            map.Add(InputAction.ShowInventory, new List<KeyPress> { new KeyPress(Terminal.TK_I) });
            map.Add(InputAction.UseItem, new List<KeyPress> { new KeyPress(Terminal.TK_U) });
            map.Add(InputAction.DropItem, new List<KeyPress> { new KeyPress(Terminal.TK_D) });

            map.Add(InputAction.ClickMove, new List<KeyPress> { new KeyPress(Terminal.TK_MOUSE_LEFT) });

            return map;
        }
    }
}
