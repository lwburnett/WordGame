using System;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Screens
{
    public class SettingsScreen : IScreen
    {
        public SettingsScreen(Action iMainMenuCallback)
        {
            _mainMenuCallback = iMainMenuCallback;
        }

        public void OnNavigateTo()
        {
            throw new System.NotImplementedException();
        }

        public void Update(GameTime iGameTime)
        {
            throw new System.NotImplementedException();
        }

        public void Draw()
        {
            throw new System.NotImplementedException();
        }

        private readonly Action _mainMenuCallback;
    }
}