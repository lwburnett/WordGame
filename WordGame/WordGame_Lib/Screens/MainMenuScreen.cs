using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class MainMenuScreen : ScreenBase
    {
        public MainMenuScreen(Action<GameTime> iOnPlayCallback, Action<GameTime> iOnSettingsCallback, Action<GameTime> iOnExitCallback)
        {
            _onPlayCallback = iOnPlayCallback;
            _onSettingsCallback = iOnSettingsCallback;
            _onExitCallback = iOnExitCallback;
            _lightPoints = new List<PointLight>();
        }

        public override void Update(GameTime iGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _onExitCallback(iGameTime);

            _titleWord1.Update(iGameTime);
            _titleWord2.Update(iGameTime);
            _playButton.Update(iGameTime);
            _settingsButton.Update(iGameTime);
            _exitButton.Update(iGameTime);

            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity);
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);
        }

        public override void Draw()
        {
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect);
            _titleWord1.Draw();
            _titleWord2.Draw();
            _playButton.Draw();
            _settingsButton.Draw();
            _exitButton.Draw();
        }

        private readonly List<PointLight> _lightPoints;
        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private UiNeonFloatingText _titleWord1;
        private UiNeonFloatingText _titleWord2;
        private UiMenuNeonButton _playButton;
        private UiMenuNeonButton _settingsButton;
        private UiMenuNeonButton _exitButton;
        private readonly Action<GameTime> _onPlayCallback;
        private readonly Action<GameTime> _onSettingsCallback;
        private readonly Action<GameTime> _onExitCallback;

        protected override void DoLoad()
        {
            _backgroundTexture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();

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

            var pulseOffsetInterpolationValue = new Random().NextDouble();
            _titleWord1 = new UiNeonFloatingText(
                new Rectangle(titleWord1TopLeftX, titleWord1TopLeftY, titleWord1Width, titleWord1Height),
                "WORD",
                SettingsManager.MainMenuSettings.TitleTextColor,
                pulseOffsetInterpolationValue);

            _titleWord2 = new UiNeonFloatingText(
                new Rectangle(titleWord2TopLeftX, titleWord2TopLeftY, titleWord2Width, titleWord2Height),
                "NOIR",
                SettingsManager.MainMenuSettings.TitleTextColor,
                pulseOffsetInterpolationValue);

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

            _lightPoints.AddRange(_titleWord1.LightPoints);
            _lightPoints.AddRange(_titleWord2.LightPoints);
            _lightPoints.AddRange(_playButton.LightPoints);
            _lightPoints.AddRange(_settingsButton.LightPoints);
            _lightPoints.AddRange(_exitButton.LightPoints);

            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity);

            _backgroundEffect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);
        }

        private void OnPlayClicked(GameTime iGameTime)
        {
            _onPlayCallback(iGameTime);
        }

        private void OnSettingsClicked(GameTime iGameTime)
        {
            _onSettingsCallback(iGameTime);
        }

        private void OnExitClicked(GameTime iGameTime)
        {
            _onExitCallback(iGameTime);
        }
    }
}