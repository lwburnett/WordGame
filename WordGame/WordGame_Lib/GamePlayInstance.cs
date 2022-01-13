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

        public void LoadLevel()
        {
            // TODO construct initial UI
            var keyboardHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.KeyboardHeightAsPercentage);
            var keyboardYPosition = GraphicsHelper.GamePlayArea.Height - keyboardHeight;

            const int keyboardXPosition = 0;
            var keyboardWidth = GraphicsHelper.GamePlayArea.Width;

            var keyboardRectangle = new Rectangle(keyboardXPosition, keyboardYPosition, keyboardWidth, keyboardHeight);
            _keyboard = new KeyboardControl(keyboardRectangle, OnLetterPressed, OnDelete, OnEnter);

            var gridHeight = (int)(GraphicsHelper.GamePlayArea.Height - keyboardHeight);
            var gridWidth = GraphicsHelper.GamePlayArea.Width;
            var gridRectangle = new Rectangle(0, 0, gridWidth, gridHeight);

            _letterGrid = new LetterGridControl(gridRectangle);
        }

        public void Update(GameTime iGameTime)
        {
            if (_playSessionHasFinished)
                return;

            _keyboard.Update(iGameTime);
            _letterGrid.Update(iGameTime);
        }

        public void Draw()
        {
            _keyboard.Draw();
            _letterGrid.Draw();
        }

        private KeyboardControl _keyboard;
        private LetterGridControl _letterGrid;

        private bool _playSessionHasFinished;
        private readonly Action _onGamePlaySessionFinishedCallback;

        private void OnLetterPressed(string iKeyString)
        {
            _letterGrid.LetterPressed(iKeyString);
        }

        private void OnDelete()
        {
            //_letterGrid.Delete();
        }

        private void OnEnter()
        {

        }
    }
}
