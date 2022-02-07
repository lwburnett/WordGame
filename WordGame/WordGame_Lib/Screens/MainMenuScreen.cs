using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public override void Draw(Vector2? iOffset = null)
        {
            _thisTickOffset = iOffset;
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect, iOffset);
            _titleWord1.Draw(iOffset);
            _titleWord2.Draw(iOffset);
            _playButton.Draw(iOffset);
            _settingsButton.Draw(iOffset);
            _exitButton.Draw(iOffset);
        }

        private readonly List<PointLight> _lightPoints;
        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private IUiNeonElement _titleWord1;
        private IUiNeonElement _titleWord2;
        private IUiNeonElement _playButton;
        private IUiNeonElement _settingsButton;
        private IUiNeonElement _exitButton;
        private readonly Action<GameTime> _onPlayCallback;
        private readonly Action<GameTime> _onSettingsCallback;
        private readonly Action<GameTime> _onExitCallback;
        private Vector2? _thisTickOffset;

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
        }
        

        protected override bool UpdateTransitionIn(GameTime iGameTime)
        {
            if (_titleWord1.State == NeonLightState.Off)
            {
                _titleWord1.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _titleWord2.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _playButton.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _settingsButton.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _exitButton.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }

            UpdateUiElements(iGameTime);

            return _titleWord1.State == NeonLightState.On &&
                   _titleWord2.State == NeonLightState.On &&
                   _playButton.State == NeonLightState.On &&
                   _settingsButton.State == NeonLightState.On &&
                   _exitButton.State == NeonLightState.On;
        }

        protected override void UpdateDefault(GameTime iGameTime)
        {
            UpdateUiElements(iGameTime);

            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity, _thisTickOffset);
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);
        }

        protected override bool UpdateTransitionOut(GameTime iGameTime)
        {
            if (_titleWord1.State == NeonLightState.On)
            {
                _titleWord1.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _titleWord2.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _playButton.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _settingsButton.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _exitButton.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }

            UpdateUiElements(iGameTime);

            return _titleWord1.State == NeonLightState.Off &&
                   _titleWord2.State == NeonLightState.Off &&
                   _playButton.State == NeonLightState.Off &&
                   _settingsButton.State == NeonLightState.Off &&
                   _exitButton.State == NeonLightState.Off;
        }

        private void UpdateUiElements(GameTime iGameTime)
        {
            _titleWord1.Update(iGameTime);
            _titleWord2.Update(iGameTime);
            _playButton.Update(iGameTime);
            _settingsButton.Update(iGameTime);
            _exitButton.Update(iGameTime);

            _lightPoints.Clear();
            _lightPoints.AddRange(_titleWord1.LightPoints);
            _lightPoints.AddRange(_titleWord2.LightPoints);
            _lightPoints.AddRange(_playButton.LightPoints);
            _lightPoints.AddRange(_settingsButton.LightPoints);
            _lightPoints.AddRange(_exitButton.LightPoints);

            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity, _thisTickOffset);

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