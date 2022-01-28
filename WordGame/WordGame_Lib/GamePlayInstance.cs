using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WordGame_Lib.Ui;

namespace WordGame_Lib
{
    public class GamePlayInstance : INeonUiElement
    {
        public GamePlayInstance(OrderedUniqueList<string> iWordDatabase, OrderedUniqueList<string> iSecretWordDatabase, Action<SessionStats> iOnGamePlaySessionFinishedCallback)
        {
            _wordDatabase = iWordDatabase;
            _secretWordDatabase = iSecretWordDatabase;
            _playSessionHasFinished = false;
            _onGamePlaySessionFinishedCallback = iOnGamePlaySessionFinishedCallback;
            _rng = new Random();
            LightPoints = new List<PointLight>();
        }

        public void LoadLevel()
        {
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
            _secretWord = _secretWordDatabase[_rng.Next(_secretWordDatabase.Count)];
            _numGuesses = 0;
            _isSuccess = false;

            LightPoints.AddRange(_letterGrid.LightPoints);
        }

        public List<PointLight> LightPoints { get; }

        public void Update(GameTime iGameTime)
        {
            if (_playSessionHasFinished)
            {
                _onGamePlaySessionFinishedCallback(new SessionStats(_isSuccess, _numGuesses, _secretWord));
                return;
            }

            _keyboard.Update(iGameTime);
            _letterGrid.Update(iGameTime);
            LightPoints.Clear();
            LightPoints.AddRange(_letterGrid.LightPoints);

            _notification?.Update(iGameTime);
        }

        public void Draw()
        {
            _keyboard.Draw();
            _letterGrid.Draw();
            _notification?.Draw();
        }

        private readonly Random _rng;
        private readonly OrderedUniqueList<string> _wordDatabase;
        private readonly OrderedUniqueList<string> _secretWordDatabase;
        private KeyboardControl _keyboard;
        private LetterGridControl _letterGrid;
        private UiFloatingText _notification;
        private int _numGuesses;
        private bool _isSuccess;

        private bool _playSessionHasFinished;
        private readonly Action<SessionStats> _onGamePlaySessionFinishedCallback;

        private string _secretWord;

        private void OnLetterPressed(string iKeyString)
        {
            _notification = null;
            _letterGrid.LetterPressed(iKeyString);
        }

        private void OnDelete()
        {
            _notification = null;
            SetNotification(string.Empty);
            _letterGrid.Delete();
        }

        private void OnEnter()
        {
            _notification = null;
            var currentWord = _letterGrid.GetCurrentWord();

            if (currentWord.Length != 5)
            {
                SetNotification("Not Enough Letters");
                return;
            }
            
            if (!_wordDatabase.Contains(currentWord))
            {
                SetNotification("Not a Word");
                return;
            }

            var dispositionList = new List<Disposition>
            {
                Disposition.Incorrect,
                Disposition.Incorrect,
                Disposition.Incorrect,
                Disposition.Incorrect,
                Disposition.Incorrect
            };
            for (var ii = 0; ii < 5; ii++)
            {
                var thisSecretLetter = _secretWord[ii];

                if (thisSecretLetter == currentWord[ii])
                {
                    dispositionList[ii] = Disposition.Correct;
                }
                else
                {
                    for (int jj = 0; jj < 5; jj++)
                    {
                        var thisGuessLetter = currentWord[jj];
                        if (thisSecretLetter == thisGuessLetter)
                        {
                            if (dispositionList[jj] == Disposition.Incorrect)
                            {
                                dispositionList[jj] = Disposition.Misplaced;
                                break;
                            }
                        }
                    }
                }
            }

            _numGuesses++;
            _letterGrid.OnGuessEntered(dispositionList.ToList());
            _keyboard.OnGuessEntered(currentWord, dispositionList.ToList());

            if (currentWord == _secretWord)
            {
                SetNotification("Correct!!");
                _isSuccess = true;
                _playSessionHasFinished = true;
            }
            else if (_letterGrid.IsFinished())
            {
                SetNotification($"Incorrect. The word was {_secretWord}");
                _isSuccess = false;
                _playSessionHasFinished = true;
            }
        }

        private void SetNotification(string iText)
        {
            var height = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.NotificationHeightAsPercentage);
            var yLocation = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.LetterGridHeightAsPercentage);

            _notification = new UiFloatingText(
                new Rectangle(0, yLocation, GraphicsHelper.GamePlayArea.Width, height),
                iText, 
                Color.White,
                Color.Black);
        }
    }
}
