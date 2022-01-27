using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class MainMenuScreen : IScreen
    {
        public MainMenuScreen(Action iOnPlayCallback, Action iOnSettingsCallback, Action iOnExitCallback)
        {
            _onPlayCallback = iOnPlayCallback;
            _onSettingsCallback = iOnSettingsCallback;
            _onExitCallback = iOnExitCallback;
        }

        public void OnNavigateTo()
        {
            _backgroundTexture = GraphicsHelper.LoadContent<Texture2D>("Bricks1");
            _backgroundEffect = GraphicsHelper.LoadContent<Effect>("BrickShader");

            var gamePlayAreaWidth = GraphicsHelper.GamePlayArea.Width;
            var gamePlayAreaHeight = GraphicsHelper.GamePlayArea.Height;
            
            var buttonWidth = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.ButtonWidthAsFractionOfPlayAreaWidth);
            var buttonHeight = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.ButtonHeightAsFractionOfPlayAreaHeight);

            var playButtonTopLeftX = GraphicsHelper.GamePlayArea.X + (gamePlayAreaWidth - buttonWidth) / 2;
            var playButtonTopLeftY = GraphicsHelper.GamePlayArea.Y + (gamePlayAreaHeight - 4 * buttonHeight) / 2;

            var settingsButtonTopLeftX = GraphicsHelper.GamePlayArea.X + (gamePlayAreaWidth - buttonWidth) / 2;
            var settingsButtonTopLeftY = GraphicsHelper.GamePlayArea.Y + (gamePlayAreaHeight - buttonHeight) / 2;

            var exitButtonTopLeftX = GraphicsHelper.GamePlayArea.X + (gamePlayAreaWidth - buttonWidth) / 2;
            var exitButtonTopLeftY = GraphicsHelper.GamePlayArea.Y + (gamePlayAreaHeight + 2 * buttonHeight) / 2;

            _playButton = new UiMenuNeonButton(
                new Rectangle(playButtonTopLeftX, playButtonTopLeftY, buttonWidth, buttonHeight), 
                "PLAY", 
                SettingsManager.MainMenuSettings.StartButtonColor, 
                OnPlayClicked);
            _settingsButton = new UiMenuNeonButton(
                new Rectangle(settingsButtonTopLeftX, settingsButtonTopLeftY, buttonWidth, buttonHeight), 
                "SETTINGS",
                SettingsManager.MainMenuSettings.SettingsButtonColor,
                OnSettingsClicked);
            _exitButton = new UiMenuNeonButton(
                new Rectangle(exitButtonTopLeftX, exitButtonTopLeftY, buttonWidth, buttonHeight), 
                "EXIT",
                SettingsManager.MainMenuSettings.ExitButtonColor,
                OnExitClicked);

            var allPoints = new List<PointLight>();
            allPoints.AddRange(_playButton.LightPoints);
            allPoints.AddRange(_settingsButton.LightPoints);
            allPoints.AddRange(_exitButton.LightPoints);

            CalculateShaderParameter(allPoints, out var positions, out var colors, out var radii);

            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
        }

        public void Update(GameTime iGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _onExitCallback();

            _playButton.Update(iGameTime);
            _settingsButton.Update(iGameTime);
            _exitButton.Update(iGameTime);
        }

        public void Draw()
        {
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect);
            _playButton.Draw();
            _settingsButton.Draw();
            _exitButton.Draw();
        }

        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private UiMenuNeonButton _playButton;
        private UiMenuNeonButton _settingsButton;
        private UiMenuNeonButton _exitButton;
        private readonly Action _onPlayCallback;
        private readonly Action _onSettingsCallback;
        private readonly Action _onExitCallback;

        private void OnPlayClicked()
        {
            _onPlayCallback();
        }

        private void OnSettingsClicked()
        {
            _onSettingsCallback();
        }

        private void OnExitClicked()
        {
            _onExitCallback();
        }

        // ReSharper disable InconsistentNaming
        private void CalculateShaderParameter(List<PointLight> iAllPoints, out Vector2[] oPositions, out Vector4[] oColors, out float[] oRadii)
        {
            const int maxLights = 30;

            oPositions = new Vector2[maxLights];
            oColors = new Vector4[maxLights];
            oRadii = new float[maxLights];
            
            for (var ii = 0; ii < maxLights; ii++)
            {
                if (ii < iAllPoints.Count)
                {
                    var pointLightData = iAllPoints[ii];
                    // I think the Y coordinate of shader math has 0 at the bottom of the screen and counts positive going up
                    oPositions[ii] = new Vector2(pointLightData.Position.X, GraphicsHelper.GamePlayArea.Height - pointLightData.Position.Y);
                    oColors[ii] = new Vector4(pointLightData.LightColor.R / 255f, pointLightData.LightColor.G / 255f, pointLightData.LightColor.B / 255f, pointLightData.LightColor.A / 255f);
                    oRadii[ii] = pointLightData.Radius;
                }
                else
                {
                    oPositions[ii] = Vector2.Zero;
                    oColors[ii] = Vector4.Zero;
                    oRadii[ii] = 0.0f;
                }
            }
        }
        // ReSharper restore InconsistentNaming
    }
}