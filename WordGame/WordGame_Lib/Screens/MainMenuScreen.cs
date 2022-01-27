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

            var titleWord1TopLeftX = GraphicsHelper.GamePlayArea.X + (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.TitleWord1XAsPercentage);
            var titleWord1TopLeftY = GraphicsHelper.GamePlayArea.Y + (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.TitleWord1YAsPercentage);
            var titleWord1Height = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.TitleWord1HeightAsPercentage);
            var titleWord1Width = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.TitleWord1WidthAsPercentage);

            var titleWord2TopLeftX = GraphicsHelper.GamePlayArea.X + (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.TitleWord2XAsPercentage);
            var titleWord2TopLeftY = GraphicsHelper.GamePlayArea.Y + (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.TitleWord2YAsPercentage);
            var titleWord2Height = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.TitleWord2HeightAsPercentage);
            var titleWord2Width = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.TitleWord2WidthAsPercentage);

            var buttonWidth = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.ButtonWidthAsFractionOfPlayAreaWidth);
            var buttonHeight = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.ButtonHeightAsFractionOfPlayAreaHeight);

            var playButtonTopLeftX = GraphicsHelper.GamePlayArea.X + (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.PlayButtonXAsPercentage);
            var playButtonTopLeftY = GraphicsHelper.GamePlayArea.Y + (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.PlayButtonYAsPercentage);
            var playButtonHeight = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.PlayButtonHeightAsPercentage);
            var playButtonWidth = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.PlayButtonWidthAsPercentage);

            var settingsButtonTopLeftX = GraphicsHelper.GamePlayArea.X + (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.SettingsButtonXAsPercentage);
            var settingsButtonTopLeftY = GraphicsHelper.GamePlayArea.Y + (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.SettingsButtonYAsPercentage);
            var settingsButtonHeight = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.SettingsButtonHeightAsPercentage);
            var settingsButtonWidth = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.SettingsButtonWidthAsPercentage);

            var exitButtonTopLeftX = GraphicsHelper.GamePlayArea.X + (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.ExitButtonXAsPercentage);
            var exitButtonTopLeftY = GraphicsHelper.GamePlayArea.Y + (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.ExitButtonYAsPercentage);
            var exitButtonHeight = (int)(gamePlayAreaHeight * SettingsManager.MainMenuSettings.ExitButtonHeightAsPercentage);
            var exitButtonWidth = (int)(gamePlayAreaWidth * SettingsManager.MainMenuSettings.ExitButtonWidthAsPercentage);


            _titleWord1 = new UiNeonFloatingText(
                new Rectangle(titleWord1TopLeftX, titleWord1TopLeftY, titleWord1Width, titleWord1Height),
                "WORD",
                SettingsManager.MainMenuSettings.TitleTextColor);

            _titleWord2 = new UiNeonFloatingText(
                new Rectangle(titleWord2TopLeftX, titleWord2TopLeftY, titleWord2Width, titleWord2Height),
                "NOIR",
                SettingsManager.MainMenuSettings.TitleTextColor);

            _playButton = new UiMenuNeonButton(
                new Rectangle(playButtonTopLeftX, playButtonTopLeftY, playButtonWidth, playButtonHeight), 
                "PLAY", 
                SettingsManager.MainMenuSettings.StartButtonColor, 
                OnPlayClicked);
            _settingsButton = new UiMenuNeonButton(
                new Rectangle(settingsButtonTopLeftX, settingsButtonTopLeftY, settingsButtonWidth, settingsButtonHeight), 
                "SETTINGS",
                SettingsManager.MainMenuSettings.SettingsButtonColor,
                OnSettingsClicked);
            _exitButton = new UiMenuNeonButton(
                new Rectangle(exitButtonTopLeftX, exitButtonTopLeftY, exitButtonWidth, exitButtonHeight), 
                "EXIT",
                SettingsManager.MainMenuSettings.ExitButtonColor,
                OnExitClicked);

            var allPoints = new List<PointLight>();
            allPoints.AddRange(_titleWord1.LightPoints);
            allPoints.AddRange(_titleWord2.LightPoints);
            allPoints.AddRange(_playButton.LightPoints);
            allPoints.AddRange(_settingsButton.LightPoints);
            allPoints.AddRange(_exitButton.LightPoints);

            CalculateShaderParameter(allPoints, out var positions, out var colors, out var radii);

            _backgroundEffect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
        }

        public void Update(GameTime iGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _onExitCallback();

            _titleWord1.Update(iGameTime);
            _titleWord2.Update(iGameTime);
            _playButton.Update(iGameTime);
            _settingsButton.Update(iGameTime);
            _exitButton.Update(iGameTime);
        }

        public void Draw()
        {
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect);
            _titleWord1.Draw();
            _titleWord2.Draw();
            _playButton.Draw();
            _settingsButton.Draw();
            _exitButton.Draw();
        }

        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private UiNeonFloatingText _titleWord1;
        private UiNeonFloatingText _titleWord2;
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
                    oPositions[ii] = new Vector2(pointLightData.Position.X, pointLightData.Position.Y);
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