using System;
using System.Collections.Generic;
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

            var gridHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.LetterGridHeightAsPercentage);
            var gridWidth = GraphicsHelper.GamePlayArea.Width;
            var gridRectangle = new Rectangle(0, 0, gridWidth, gridHeight);

            _letterGrid = new LetterGridControl(gridRectangle);

            _notification = null;
            _secretWord = "APPLE";
        }

        public void Update(GameTime iGameTime)
        {
            if (_playSessionHasFinished)
                return;

            _keyboard.Update(iGameTime);
            _letterGrid.Update(iGameTime);
            _notification?.Update(iGameTime);
        }

        public void Draw()
        {
            _keyboard.Draw();
            _letterGrid.Draw();
            _notification?.Draw();
        }

        private KeyboardControl _keyboard;
        private LetterGridControl _letterGrid;
        private UiFloatingText _notification;

        private bool _playSessionHasFinished;
        private readonly Action _onGamePlaySessionFinishedCallback;

        private string _secretWord;

        private void OnLetterPressed(string iKeyString)
        {
            _letterGrid.LetterPressed(iKeyString);
        }

        private void OnDelete()
        {
            _letterGrid.Delete();
        }

        private void OnEnter()
        {
            var currentWord = _letterGrid.GetCurrentWord();

            if (currentWord.Length != 5)
            {
                SetNotification("Not Enough Letters");
                return;
            }

            //TODO Check if it is a word

            var dispositionList = new List<bool?>();
            for (int ii = 0; ii < 5; ii++)
            {
                var thisGuessLetter = currentWord[ii];

                // Null is no match, false is match but misplaced, true is match and properly placed
                bool? thisGuessLetterDisposition = null;

                for (int jj = 0; jj < 5; jj++)
                {
                    var thisSecretLetter = _secretWord[jj];

                    if (thisGuessLetter == thisSecretLetter)
                    {
                        if (ii == jj)
                            thisGuessLetterDisposition = true;
                        else
                            thisGuessLetterDisposition = false;
                    }
                }

                dispositionList.Add(thisGuessLetterDisposition);
            }

            _letterGrid.OnGuessEntered(dispositionList);

            if (_letterGrid.IsFinished())
            {
                if (currentWord == _secretWord)
                {
                    SetNotification("Correct!!");
                }
                else
                {
                    SetNotification($"Incorrect. The word was {_secretWord}");
                }
            }
        }

        private void SetNotification(string iText)
        {
            var height = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.NotificationHeightAsPercentage);
            var yLocation = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.LetterGridHeightAsPercentage);

            _notification = new UiFloatingText(
                new Rectangle(0, yLocation, GraphicsHelper.GamePlayArea.Width, height),
                iText, 
                Color.White);
        }
    }
}
