using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class CreditsScreen : ScreenBase
    {
        public CreditsScreen(Action<GameTime> iMainMenuCallback)
        {
            _bounds = GraphicsHelper.GamePlayArea;
            _mainMenuCallback = iMainMenuCallback;
            _lightPoints = new List<PointLight>();
        }

        public override void Draw(Vector2? iOffset = null)
        {
            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity, iOffset);

            _backgroundEffect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);

            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect, iOffset);

            _header.Draw(iOffset);
            _textLines.ForEach(iTl => iTl.Draw(iOffset));
            _mainMenuButton.Draw(iOffset);
        }

        protected override void DoLoad()
        {
            _backgroundTexture = AssetHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = AssetHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();

            var bigMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);

            var bigMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);

            var headerYLocation = _bounds.Y + bigMarginY;
            var headerXLocation = _bounds.X + bigMarginX;
            var headerHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.CreditsScreenSettings.HeaderHeightAsPercentage);
            var headerWidth = GraphicsHelper.GamePlayArea.Width - bigMarginX - bigMarginX;
            _header = new UiNeonFloatingText(
                new Rectangle(headerXLocation, headerYLocation, headerWidth, headerHeight),
                "CREDITS",
                Color.White);

            var creditWidth = _bounds.Width - 2 * bigMarginX;
            var creditHeight = (int)(_bounds.Height * SettingsManager.CreditsScreenSettings.CreditLineHeightAsPercentage);

            var line1LabelY = _bounds.Y + headerYLocation + headerHeight + bigMarginY;
            var line1LabelX = headerXLocation;
            var line1LabelWidth = creditWidth;
            var line1LabelHeight = creditHeight;
            var line1 = new UiFloatingText(
                new Rectangle(line1LabelX, line1LabelY, line1LabelWidth, line1LabelHeight),
                "Created by Luke Burnett",
                Color.White,
                Color.Black);

            var line2LabelY = line1LabelY + line1LabelHeight;
            var line2LabelX = headerXLocation;
            var line2LabelWidth = creditWidth;
            var line2LabelHeight = creditHeight;
            var line2 = new UiFloatingText(
                new Rectangle(line2LabelX, line2LabelY, line2LabelWidth, line2LabelHeight),
                "Play tested by Sarah Lizarraga and Nolan Perry-Arnold",
                Color.White,
                Color.Black);

            var line3LabelY = line2LabelY + line2LabelHeight;
            var line3LabelX = headerXLocation;
            var line3LabelWidth = creditWidth;
            var line3LabelHeight = creditHeight;
            var line3 = new UiFloatingText(
                new Rectangle(line3LabelX, line3LabelY, line3LabelWidth, line3LabelHeight),
                "Brick wall background by Bernard Hermant at unsplash.com",
                Color.White,
                Color.Black);

            var line4LabelY = line3LabelY + line3LabelHeight;
            var line4LabelX = headerXLocation;
            var line4LabelWidth = creditWidth;
            var line4LabelHeight = creditHeight;
            var line4 = new UiFloatingText(
                new Rectangle(line4LabelX, line4LabelY, line4LabelWidth, line4LabelHeight),
                "\"Hello Denver\" text font by Good Apples at 1001fonts.com",
                Color.White,
                Color.Black);

            var line5LabelY = line4LabelY + line4LabelHeight;
            var line5LabelX = headerXLocation;
            var line5LabelWidth = creditWidth;
            var line5LabelHeight = creditHeight;
            var line5 = new UiFloatingText(
                new Rectangle(line5LabelX, line5LabelY, line5LabelWidth, line5LabelHeight),
                "Music and Sound Effects by Fesliyan Studios",
                Color.White,
                Color.Black);

            var mainMenuWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.CreditsScreenSettings.MainMenuButtonWidthAsPercentage);
            var mainMenuHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.CreditsScreenSettings.MainMenuButtonHeightAsPercentage);
            var mainMenuY = _bounds.Height - bigMarginY - mainMenuHeight;
            var mainMenuX = (GraphicsHelper.GamePlayArea.Width - mainMenuWidth) / 2;
            _mainMenuButton = new UiMenuNeonButton(
                new Rectangle(mainMenuX, mainMenuY, mainMenuWidth, mainMenuHeight),
                "MAIN MENU",
                Color.BlueViolet,
                OnMainMenu);

            _textLines = new List<IUiElement>
            {
                line1,
                line2,
                line3,
                line4,
                line5
            };

            _lightPoints.AddRange(_header.LightPoints);
            _lightPoints.AddRange(_mainMenuButton.LightPoints);
        }

        protected override bool UpdateTransitionIn(GameTime iGameTime)
        {
            if (_header.State == NeonLightState.Off)
            {
                _header.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _mainMenuButton.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }

            UpdateUiElements(iGameTime);

            return _header.State == NeonLightState.On &&
                   _mainMenuButton.State == NeonLightState.On;
        }

        protected override void UpdateDefault(GameTime iGameTime)
        {
            UpdateUiElements(iGameTime);
        }

        protected override bool UpdateTransitionOut(GameTime iGameTime)
        {
            if (_header.State == NeonLightState.On)
            {
                _header.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _mainMenuButton.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }

            UpdateUiElements(iGameTime);

            return _header.State == NeonLightState.Off &&
                   _mainMenuButton.State == NeonLightState.Off;
        }

        private readonly Rectangle _bounds;
        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private readonly Action<GameTime> _mainMenuCallback;
        private readonly List<PointLight> _lightPoints;

        private IUiNeonElement _header;
        private List<IUiElement> _textLines;
        private IUiNeonElement _mainMenuButton;

        private void UpdateUiElements(GameTime iGameTime)
        {
            _header.Update(iGameTime);

            _textLines.ForEach(iTl => iTl.Update(iGameTime));

            _mainMenuButton.Update(iGameTime);
        }

        private void OnMainMenu(GameTime iGameTime)
        {
            _mainMenuCallback(iGameTime);
        }
    }
}