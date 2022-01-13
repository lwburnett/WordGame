using System;
using Microsoft.Xna.Framework;
using WordGame_Lib.Ui;

namespace WordGame_Lib
{
    public class GamePlayInstance
    {
        public GamePlayInstance(Action iOnGamePlaySessionFinishedCallback)
        {
            _playSessionHasFinished = false;
            _onGamePlaySessionFinishedCallback = iOnGamePlaySessionFinishedCallback;
        }

        private bool _playSessionHasFinished;
        private readonly Action _onGamePlaySessionFinishedCallback;

        public void LoadLevel()
        {
            // TODO construct initial UI
            var keyboardHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.KeyboardHeightAsPercentage);
            var keyboardYPosition = GraphicsHelper.GamePlayArea.Height - keyboardHeight;

            const int keyboardXPosition = 0;
            var keyboardWidth = GraphicsHelper.GamePlayArea.Width;

            var keyboardRectangle = new Rectangle(keyboardXPosition, keyboardYPosition, keyboardWidth, keyboardHeight);
            _keyboard = new KeyboardControl(keyboardRectangle);
        }

        public void Update(GameTime iGameTime)
        {
            if (_playSessionHasFinished)
                return;

            _keyboard.Update(iGameTime);
        }

        public void Draw()
        {
            _keyboard.Draw();
        }

        private KeyboardControl _keyboard;
    }
}
