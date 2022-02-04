using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WordGame_Lib.Ui;

namespace WordGame_Lib
{
    public class GamePlayInstance : IUiElement, ILightSource
    {
        public GamePlayInstance(OrderedUniqueList<string> iWordDatabase, OrderedUniqueList<string> iSecretWordDatabase, Action<GameTime> iOnMainMenuCallback)
        {
            _wordDatabase = iWordDatabase;
            _secretWordDatabase = iSecretWordDatabase;
            _onMainMenuCallback = iOnMainMenuCallback;
            _playSessionHasFinished = false;
            _rng = new Random();
            LightPoints = new List<PointLight>();
        }

        public void LoadLevel()
        {
            var mainMenuXPos = GraphicsHelper.GamePlayArea.X + (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);
            var mainMenuYPos = GraphicsHelper.GamePlayArea.Y + (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.MainMenuButtonYAsPercentage);
            var mainMenuWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GamePlaySettings.MainMenuButtonWidthAsPercentage);
            var mainMenuHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.MainMenuButtonHeightAsPercentage);
            _mainMenuButton = new UiMenuNeonButton(
                new Rectangle(mainMenuXPos, mainMenuYPos, mainMenuWidth, mainMenuHeight),
                "MAIN MENU",
                SettingsManager.GamePlaySettings.MainMenuButtonColor,
                _onMainMenuCallback);

            var playAgainYPos = GraphicsHelper.GamePlayArea.Y + (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.PlayAgainButtonYAsPercentage);
            var playAgainWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GamePlaySettings.PlayAgainButtonWidthAsPercentage);
            var playAgainXPos =
                GraphicsHelper.GamePlayArea.X +
                GraphicsHelper.GamePlayArea.Width -
                (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage) -
                playAgainWidth;
            var playAgainHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.PlayAgainButtonHeightAsPercentage);
            _playAgainButton = new UiMenuNeonButton(
                new Rectangle(playAgainXPos, playAgainYPos, playAgainWidth, playAgainHeight),
                "PLAY AGAIN",
                SettingsManager.GamePlaySettings.PlayAgainButtonColor,
                OnPlayAgain);


            var keyboardHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.KeyboardHeightAsPercentage);
            var keyboardYPosition = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.KeyboardYPosAsPercentage);

            const int keyboardXPosition = 0;
            var keyboardWidth = GraphicsHelper.GamePlayArea.Width;

            var keyboardRectangle = new Rectangle(keyboardXPosition, keyboardYPosition, keyboardWidth, keyboardHeight);
            _keyboard = new KeyboardControl(keyboardRectangle, OnLetterPressed, OnDelete, OnEnter);

            var gridHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.KeyboardYPosAsPercentage);
            var gridWidth = GraphicsHelper.GamePlayArea.Width;
            var gridRectangle = new Rectangle(0, 0, gridWidth, gridHeight);

            _letterGrid = new LetterGridControl(gridRectangle);

            _notification = null;
            _secretWord = _secretWordDatabase[_rng.Next(_secretWordDatabase.Count)];
            //_numGuesses = 0;
            //_isSuccess = false;
        }

        public List<PointLight> LightPoints { get; }

        public bool UpdateTransitionIn(GameTime iGameTime)
        {
            if (_letterGrid.State == NeonLightState.Off)
            {
                _letterGrid.StartFadeIn(iGameTime, TimeSpan.FromSeconds(0.5));
                _letterGrid.TurnOnRow(iGameTime);
                _playAgainButton.StartFadeIn(iGameTime, TimeSpan.FromSeconds(0.5));
                _mainMenuButton.StartFadeIn(iGameTime, TimeSpan.FromSeconds(0.5));
            }

            UpdateUiElements(iGameTime);

            return _letterGrid.State == NeonLightState.On &&
                   _letterGrid.State == NeonLightState.On &&
                   _letterGrid.State == NeonLightState.On;
        }

        public void Update(GameTime iGameTime)
        {
            UpdateUiElements(iGameTime);
        }
        public bool UpdateTransitionOut(GameTime iGameTime)
        {
            if (_letterGrid.State == NeonLightState.On)
            {
                _letterGrid.StartFadeOut(iGameTime, TimeSpan.FromSeconds(0.5));
                _playAgainButton.StartFadeOut(iGameTime, TimeSpan.FromSeconds(0.5));
                _mainMenuButton.StartFadeOut(iGameTime, TimeSpan.FromSeconds(0.5));
            }

            UpdateUiElements(iGameTime);

            return _letterGrid.State == NeonLightState.Off &&
                   _letterGrid.State == NeonLightState.Off &&
                   _letterGrid.State == NeonLightState.Off;
        }

        public void Draw(Vector2? iOffset = null)
        {
            if (!_playSessionHasFinished)
            {
                _keyboard.Draw(iOffset);
            }
            else
            {
                _mainMenuButton.Draw(iOffset);
                _playAgainButton.Draw(iOffset);
            }

            _letterGrid.Draw(iOffset);
            _notification?.Draw(iOffset);
        }

        private readonly Random _rng;
        private readonly OrderedUniqueList<string> _wordDatabase;
        private readonly OrderedUniqueList<string> _secretWordDatabase;
        private readonly Action<GameTime> _onMainMenuCallback;
        private KeyboardControl _keyboard;
        private LetterGridControl _letterGrid;
        private UiFloatingText _notification;
        //private int _numGuesses;
        //private bool _isSuccess;

        private bool _playSessionHasFinished;
        private IUiNeonElement _mainMenuButton;
        private IUiNeonElement _playAgainButton;

        private string _secretWord;

        private void UpdateUiElements(GameTime iGameTime)
        {
            _letterGrid.Update(iGameTime);
            LightPoints.Clear();
            LightPoints.AddRange(_letterGrid.LightPoints);

            if (!_playSessionHasFinished)
            {
                _keyboard.Update(iGameTime);
            }
            else
            {
                _mainMenuButton.Update(iGameTime);
                _playAgainButton.Update(iGameTime);

                LightPoints.AddRange(_mainMenuButton.LightPoints);
                LightPoints.AddRange(_playAgainButton.LightPoints);
            }

            _notification?.Update(iGameTime);
        }

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

        private void OnEnter(GameTime iGameTime)
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

            //_numGuesses++;
            _letterGrid.OnGuessEntered(dispositionList.ToList(), iGameTime);
            _keyboard.OnGuessEntered(currentWord, dispositionList.ToList());

            if (currentWord == _secretWord)
            {
                SetNotification($"Correct!! The word was {_secretWord}");
                //_isSuccess = true;
                _playSessionHasFinished = true;
            }
            else if (_letterGrid.IsFinished())
            {
                SetNotification($"Incorrect. The word was {_secretWord}");
                //_isSuccess = false;
                _playSessionHasFinished = true;
            }
            else
            {
                _letterGrid.TurnOnRow(iGameTime);
            }
        }

        private void SetNotification(string iText)
        {
            var height = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.NotificationHeightAsPercentage);
            var yLocation = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GamePlaySettings.KeyboardYPosAsPercentage - height);

            _notification = new UiFloatingText(
                new Rectangle(0, yLocation, GraphicsHelper.GamePlayArea.Width, height),
                iText, 
                Color.White,
                Color.Black);
        }

        private void OnPlayAgain(GameTime iGameTime)
        {
            _playSessionHasFinished = false;
            _letterGrid.Reset(iGameTime);
            _keyboard.Reset();

            _notification = null;
            _secretWord = _secretWordDatabase[_rng.Next(_secretWordDatabase.Count)];
        }
    }
}
