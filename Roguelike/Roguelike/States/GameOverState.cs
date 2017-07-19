﻿using BearLib;
using Roguelike.Input;

namespace Roguelike.States
{
    public class GameOverState : IState
    {
        public void Initialize()
        {

        }

        public bool Update()
        {
            if (InputManager.LastCommand == Command.Quit)
            {
                return false;
            }

            return true;
        }

        public void Draw()
        {
            Terminal.Print(1, 1, "Game Over!");
        }
    }
}