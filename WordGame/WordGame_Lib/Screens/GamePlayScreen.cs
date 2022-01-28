using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordGame_Lib.Ui;

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

            _backgroundTexture = GraphicsHelper.LoadContent<Texture2D>("Bricks1");
            _backgroundEffect = GraphicsHelper.LoadContent<Effect>("BrickShader").Clone();
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

        // ReSharper disable InconsistentNaming
        private static void CalculateShaderParameter(List<PointLight> iAllPoints, out Vector2[] oPositions, out Vector4[] oColors, out float[] oRadii, out float[] oIntensity)
        {
            const int maxLights = 30;

            oPositions = new Vector2[maxLights];
            oColors = new Vector4[maxLights];
            oRadii = new float[maxLights];
            oIntensity = new float[maxLights];

            for (var ii = 0; ii < maxLights; ii++)
            {
                if (ii < iAllPoints.Count)
                {
                    var pointLightData = iAllPoints[ii];

                    if (pointLightData.Intensity >= 1.0f)
                    {
                        // I think the Y coordinate of shader math has 0 at the bottom of the screen and counts positive going up
                        oPositions[ii] = new Vector2(pointLightData.Position.X, pointLightData.Position.Y);
                        oColors[ii] = new Vector4(pointLightData.LightColor.R / 255f, pointLightData.LightColor.G / 255f, pointLightData.LightColor.B / 255f, pointLightData.LightColor.A / 255f);
                        oRadii[ii] = pointLightData.Radius;
                        oIntensity[ii] = pointLightData.Intensity;
                    }
                    else
                    {
                        oPositions[ii] = Vector2.Zero;
                        oColors[ii] = Vector4.Zero;
                        oRadii[ii] = 0.0f;
                        oIntensity[ii] = 0.0f;
                    }
                }
                else
                {
                    oPositions[ii] = Vector2.Zero;
                    oColors[ii] = Vector4.Zero;
                    oRadii[ii] = 0.0f;
                    oIntensity[ii] = 0.0f;
                }
            }
        }
        // ReSharper restore InconsistentNaming
    }
}