using BearLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Roguelike.Input
{
    public static class InputManager
    {
        private const string KeyMapFile = "keys.json";

        private static Dictionary<Command, List<KeyPress>> inputMap = new Dictionary<Command, List<KeyPress>>();

        private static int lastInput;

        public static Command LastCommand { get; private set; }

        static InputManager()
        {
            if (!File.Exists(KeyMapFile))
            {
                inputMap = GetDefaultInputMap();
                SaveKeyMap(KeyMapFile);
            }
            else
            {
                inputMap = LoadKeyMap(KeyMapFile);
            }
        }

        public static void Update()
        {
            lastInput = Terminal.Read();
            LastCommand = inputMap.FirstOrDefault(kvp => kvp.Value.Any(map => map.Check(lastInput))).Key;
        }

        public static void SaveKeyMap(string file)
        {
            var json = JsonConvert.SerializeObject(inputMap, Formatting.Indented);
            File.WriteAllText(file, json);
        }

        public static Dictionary<Command, List<KeyPress>> LoadKeyMap(string file)
        {
            var json = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Dictionary<Command, List<KeyPress>>>(json);
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
