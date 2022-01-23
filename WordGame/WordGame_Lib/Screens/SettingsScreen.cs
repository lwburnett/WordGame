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
            

            var saveWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.ButtonWidthAsPercentage);
            var saveY = settingsEditBounds.Y + settingsEditBounds.Height + medMarginY;
            var saveX = (GraphicsHelper.GamePlayArea.Width - saveWidth) / 2;
            var saveHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.ButtonHeightAsPercentage);
            _saveButton = new UiTextButton(
                new Rectangle(saveX, saveY, saveWidth, saveHeight),
                "Save",
                OnSave);
        }

        public void Update(GameTime iGameTime)
        {
            _header.Update(iGameTime);
            _saveButton.Update(iGameTime);
        }

        public void Draw()
        {
            _header.Draw();
            _saveButton.Draw();
        }

        private readonly Rectangle _bounds;
        private readonly GameSettings _settings;
        private readonly Action _mainMenuCallback;

        private UiFloatingText _header;
        private UiTextButton _saveButton;

        private void OnSave()
        {
            _mainMenuCallback();
        }
    }
}