using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class SettingsScreen : IScreen
    {
        public SettingsScreen(Action iMainMenuCallback)
        {
            _bounds = GraphicsHelper.GamePlayArea;
            _settings = GameSettingsManager.Settings;
            _mainMenuCallback = iMainMenuCallback;
            _lightPoints = new List<PointLight>();
        }

        public void OnNavigateTo()
        {
            _backgroundTexture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();

            var bigMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);
            var medMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);

            var medMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);

            var bigMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);

            var headerYLocation = _bounds.Y + bigMarginY;
            var headerXLocation = _bounds.X + bigMarginX;
            var headerHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.SettingsScreenSettings.HeaderHeightAsPercentage);
            var headerWidth = GraphicsHelper.GamePlayArea.Width - bigMarginX - bigMarginX;
            _header = new UiNeonFloatingText(
                new Rectangle(headerXLocation, headerYLocation, headerWidth, headerHeight),
                "SETTINGS",
                Color.White);

            var settingsEditBounds = new Rectangle(
                headerXLocation,
                _bounds.Y + headerYLocation + headerHeight + medMarginY,
                _bounds.Width - 2 * bigMarginX,
                (int)(_bounds.Height * SettingsManager.SettingsScreenSettings.SettingsListHeightAsPercentage));

            var altColorLabelY = settingsEditBounds.Y;
            var altColorLabelX = settingsEditBounds.X;
            var altColorLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent); 
            var altColorLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _altColorLabel = new UiFloatingText(
                new Rectangle(altColorLabelX, altColorLabelY, altColorLabelWidth, altColorLabelHeight),
                "Alternate Color Scheme",
                Color.White,
                Color.Black);

            var altColorToggleY = settingsEditBounds.Y;
            var altColorToggleX = altColorLabelX + altColorLabelWidth + medMarginX;
            var altColorToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent / 1.5f);
            var altColorToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _altColorToggle = new UiToggleSwitch(
                new Rectangle(altColorToggleX, altColorToggleY, altColorToggleWidth, altColorToggleHeight), 
                _settings.AlternateKeyColorScheme,
                OnToggleAlternateColorScheme);

            var saveWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.SettingsScreenSettings.SaveButtonWidthAsPercentage);
            var saveY = settingsEditBounds.Y + settingsEditBounds.Height + medMarginY;
            var saveX = (GraphicsHelper.GamePlayArea.Width - saveWidth) / 2;
            var saveHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.SettingsScreenSettings.SaveButtonHeightAsPercentage);
            _saveButton = new UiMenuNeonButton(
                new Rectangle(saveX, saveY, saveWidth, saveHeight),
                "SAVE",
                Color.BlueViolet,
                OnSave);
        }

        public void Update(GameTime iGameTime)
        {
            _header.Update(iGameTime);
            _saveButton.Update(iGameTime);
            _altColorLabel.Update(iGameTime);
            _altColorToggle.Update(iGameTime);

            _lightPoints.Clear();
            _lightPoints.AddRange(_header.LightPoints);
            _lightPoints.AddRange(_altColorToggle.LightPoints);
            _lightPoints.AddRange(_saveButton.LightPoints);
        }

        public void Draw()
        {
            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity);

            _backgroundEffect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);
            
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect);

            _header.Draw();
            _saveButton.Draw();
            _altColorLabel.Draw();
            _altColorToggle.Draw();
        }

        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private readonly Rectangle _bounds;
        private GameSettings _settings;
        private readonly Action _mainMenuCallback;

        private INeonUiElement _header;
        private IUiElement _altColorLabel;
        private INeonUiElement _altColorToggle;
        private INeonUiElement _saveButton;
        private readonly List<PointLight> _lightPoints;

        private void OnSave()
        {
            GameSettingsManager.UpdateSettings(_settings);
            GameSettingsManager.WriteSettingsToDiskAsync();
            _mainMenuCallback();
        }

        private void OnToggleAlternateColorScheme(bool iNewValue)
        {
            _settings = new GameSettings(iNewValue);
        }
    }
}