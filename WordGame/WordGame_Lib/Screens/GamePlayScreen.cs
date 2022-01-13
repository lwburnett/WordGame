using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WordGame_Lib.Screens
{
    public class GamePlayScreen : IScreen
    {
        public GamePlayScreen(Action iOnPlayAgainCallback, Action iOnMainMenuCallback, Action iOnExitCallback)
        {
            _gamePlayInstance = new GamePlayInstance(OnGamePlaySessionFinished);
            _onExitCallback = iOnExitCallback;
            _onPlayAgainCallback = iOnPlayAgainCallback;
            _onMainMenuCallback = iOnMainMenuCallback;
            _subScreen = SubScreen.GamePlay;
        }

        public void OnNavigateTo()
        {
            _gamePlayInstance.LoadLevel();
        }

        public void Update(GameTime iGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _onExitCallback();

            switch (_subScreen)
            {
                case SubScreen.GamePlay:
                    _gamePlayInstance.Update(iGameTime);
                    break;
                case SubScreen.PostSessionStats:
                    _postSessionStatsScreen.Update(iGameTime);
                    break;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(SubScreen)}: {_subScreen}");
                    break;
            }
        }

        public void Draw()
        {
            _gamePlayInstance.Draw();

            switch (_subScreen)
            {
                case SubScreen.GamePlay:
                    break;
                case SubScreen.PostSessionStats:
                    _postSessionStatsScreen.Draw();
                    break;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(SubScreen)}: {_subScreen}");
                    break;
            }
        }

        private readonly Action _onExitCallback;
        private readonly Action _onPlayAgainCallback;
        private readonly Action _onMainMenuCallback;
        private readonly GamePlayInstance _gamePlayInstance;
        private SubScreen _subScreen;
        private IScreen _postSessionStatsScreen;

        private void OnGamePlaySessionFinished()
        {
            // _subScreen = SubScreen.PostSessionStats;
            // _postSessionStatsScreen = new PostSessionStatsScreen(OnPlayAgain, OnMainMenu);
            // _postSessionStatsScreen.OnNavigateTo();
        }

        private void OnMainMenu()
        {
            _onMainMenuCallback();
        }

        private void OnPlayAgain()
        {
            _onPlayAgainCallback();
        }

        private enum SubScreen
        {
            GamePlay,
            PostSessionStats
        }
    }
}