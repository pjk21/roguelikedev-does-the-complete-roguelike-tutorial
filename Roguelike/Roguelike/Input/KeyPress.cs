using BearLib;
using System;

namespace Roguelike.Input
{
    public class KeyPress : IEquatable<KeyPress>
    {
        public int Key { get; }
        public bool Shift { get; }
        public bool Control { get; }
        public bool Alt { get; }

        public KeyPress(int key, bool shift = false, bool control = false, bool alt = false)
        {
            Key = key;
            Shift = shift;
            Control = control;
            Alt = alt;
        }

        public bool Check(int key)
        {
            return Key == key &&
                Terminal.Check(Terminal.TK_SHIFT) == Shift &&
                Terminal.Check(Terminal.TK_CONTROL) == Control &&
                Terminal.Check(Terminal.TK_ALT) == Alt;
        }

        public bool Equals(KeyPress other)
        {
            if (other == null)
            {
                return false;
            }

            return Key == other.Key &&
                Shift == other.Shift &&
                Control == other.Control &&
                Alt == other.Alt;
        }

        public override bool Equals(object obj)
        {
            if (obj is KeyPress)
            {
                return Equals(obj as KeyPress);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return new { Key, Shift, Control, Alt }.GetHashCode();
        }
    }
}
