using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WordGame_Lib.Screens
{
    public class GamePlayScreen : IScreen
    {
        public GamePlayScreen(OrderedUniqueList<string> iWordDatabase, OrderedUniqueList<string> iSecretWordDatabase, Action iOnPlayAgainCallback, Action iOnMainMenuCallback, Action iOnExitCallback)
        {
            _gamePlayInstance = new GamePlayInstance(iWordDatabase, iSecretWordDatabase, OnGamePlaySessionFinished);
            _onExitCallback = iOnExitCallback;
            _onPlayAgainCallback = iOnPlayAgainCallback;
            _onMainMenuCallback = iOnMainMenuCallback;
            _subScreen = SubScreen.GamePlay;

            _backgroundTexture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();
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
            var lightPoints = _gamePlayInstance.LightPoints;
            GraphicsHelper.CalculatePointLightShaderParameters(lightPoints, out var positions, out var colors, out var radii, out var intensity);

            _backgroundEffect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect);

            switch (_subScreen)
            {
                case SubScreen.GamePlay:
                    _gamePlayInstance.Draw();
                    break;
                case SubScreen.PostSessionStats:
                    _postSessionStatsScreen.Draw();
                    break;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(SubScreen)}: {_subScreen}");
                    break;
            }
        }

        private readonly Texture2D _backgroundTexture;
        private readonly Effect _backgroundEffect;
        private readonly Action _onExitCallback;
        private readonly Action _onPlayAgainCallback;
        private readonly Action _onMainMenuCallback;
        private readonly GamePlayInstance _gamePlayInstance;
        private SubScreen _subScreen;
        private IScreen _postSessionStatsScreen;

        private void OnGamePlaySessionFinished(SessionStats iStats)
        {
            var bounds = GraphicsHelper.GamePlayArea;

            _subScreen = SubScreen.PostSessionStats;
            _postSessionStatsScreen = new PostSessionStatsScreen(bounds, iStats, OnMainMenu, OnPlayAgain);
            _postSessionStatsScreen.OnNavigateTo();
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