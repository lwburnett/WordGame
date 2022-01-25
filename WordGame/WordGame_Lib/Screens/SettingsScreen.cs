using System;
using Microsoft.Xna.Framework;
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
        }

        public void OnNavigateTo()
        {
            var bigMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);
            var medMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);

            var medMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);

            var bigMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);

            var headerYLocation = _bounds.Y + bigMarginY;
            var headerXLocation = _bounds.X + bigMarginX;
            var headerHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.SettingsScreenSettings.HeaderHeightAsPercentage);
            var headerWidth = GraphicsHelper.GamePlayArea.Width - bigMarginX - bigMarginX;
            _header = new UiFloatingText(
                new Rectangle(headerXLocation, headerYLocation, headerWidth, headerHeight),
                "Settings",
                Color.White,
                1.5f);

            var settingsEditBounds = new Rectangle(
                headerXLocation,
                _bounds.Y + headerYLocation + headerHeight + medMarginY,
                _bounds.Width - 2 * bigMarginX,
                (int)(_bounds.Height * SettingsManager.SettingsScreenSettings.SettingsListHeightAsPercentage));

            var altColorLabelY = settingsEditBounds.Y;
            var altColorLabelX = settingsEditBounds.X;
            var altColorLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent); 
            var altColorLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent);
            _altColorLabel = new UiFloatingText(
                new Rectangle(altColorLabelX, altColorLabelY, altColorLabelWidth, altColorLabelHeight),
                "Alternate Color Scheme",
                Color.White);

            var altColorToggleY = settingsEditBounds.Y;
            var altColorToggleX = altColorLabelX + altColorLabelWidth + medMarginX;
            var altColorToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent);
            var altColorToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent);
            _altColorToggle = new UiToggleSwitch(
                new Rectangle(altColorToggleX, altColorToggleY, altColorToggleWidth, altColorToggleHeight), 
                _settings.AlternateKeyColorScheme,
                OnToggleAlternateColorScheme);

            var saveWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.SettingsScreenSettings.SaveButtonWidthAsPercentage);
            var saveY = settingsEditBounds.Y + settingsEditBounds.Height + medMarginY;
            var saveX = (GraphicsHelper.GamePlayArea.Width - saveWidth) / 2;
            var saveHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.SettingsScreenSettings.SaveButtonHeightAsPercentage);
            _saveButton = new UiTextButton(
                new Rectangle(saveX, saveY, saveWidth, saveHeight),
                "Save",
                OnSave);
        }

        public void Update(GameTime iGameTime)
        {
            _header.Update(iGameTime);
            _saveButton.Update(iGameTime);
            _altColorLabel.Update(iGameTime);
            _altColorToggle.Update(iGameTime);
        }

        public void Draw()
        {
            _header.Draw();
            _saveButton.Draw();
            _altColorLabel.Draw();
            _altColorToggle.Draw();
        }

        private readonly Rectangle _bounds;
        private GameSettings _settings;
        private readonly Action _mainMenuCallback;

        private UiFloatingText _header;
        private UiFloatingText _altColorLabel;
        private UiToggleSwitch _altColorToggle;
        private UiTextButton _saveButton;

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