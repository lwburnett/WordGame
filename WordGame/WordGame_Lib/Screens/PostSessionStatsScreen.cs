using System;
using Microsoft.Xna.Framework;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class PostSessionStatsScreen : ScreenBase
    {
        public PostSessionStatsScreen(Rectangle iBounds, SessionStats iStats, Action<GameTime> iOnMainMenuCallback, Action<GameTime> iOnPlayAgainCallback)
        {
            _bounds = iBounds;
            _stats = iStats;
            _onMainMenuCallback = iOnMainMenuCallback;
            _onPlayAgainCallback = iOnPlayAgainCallback;
        }

        public override void Draw()
        {
            _header.Draw();
            _subHeader.Draw();
            //_definition.Draw();
            _mainMenuButton.Draw();
            _playAgainButton.Draw();
        }

        protected override void DoLoad()
        {

            var bigMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);
            var mediumMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);
            var smallMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.SmallMarginAsPercentage);

            var bigMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);
            //var mediumMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.MediumMarginAsPercentage);
            //var smallMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.SmallMarginAsPercentage);

            var headerYLocation = _bounds.Y + bigMarginY;
            var headerXLocation = _bounds.X + bigMarginX;
            var headerHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.HeaderHeightAsPercentage);
            var headerWidth = GraphicsHelper.GamePlayArea.Width - bigMarginX - bigMarginX;
            _header = new UiFloatingText(
                new Rectangle(headerXLocation, headerYLocation, headerWidth, headerHeight),
                _stats.Success ? "Success!" : "Failure!",
                Color.White,
                Color.Black,
                1.5f);

            var subHeaderY = headerYLocation + headerHeight + smallMarginY;
            var subHeaderX = headerXLocation;
            var subHeaderHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.SubHeaderHeightAsPercentage);
            var subHeaderWidth = headerWidth;
            _subHeader = new UiFloatingText(
                new Rectangle(subHeaderX, subHeaderY, subHeaderWidth, subHeaderHeight),
                $"Guesses: {_stats.NumGuesses}\nWord: {_stats.SecretWord}",
                Color.White,
                Color.Black,
                1.5f);

            var defY = subHeaderY + subHeaderHeight + smallMarginY;
            //var defX = headerXLocation;
            var defHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.DefinitionHeightAsPercentage);
            //var defWidth = headerWidth;
            // _definition = new UiFloatingText(
            //     new Rectangle(defX, defY, defWidth, defHeight),
            //     $"{_stats.SecretWord}: {_stats.SecretWordDefinition}",
            //     Color.White);

            var mainMenuWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.ButtonWidthAsPercentage);
            var mainMenuY = defY + defHeight + mediumMarginY;
            var mainMenuX = (GraphicsHelper.GamePlayArea.Width - mainMenuWidth) / 2;
            var mainMenuHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.ButtonHeightAsPercentage);
            _mainMenuButton = new UiTextButton(
                new Rectangle(mainMenuX, mainMenuY, mainMenuWidth, mainMenuHeight),
                "Main Menu",
                _onMainMenuCallback);

            var playAgainWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.ButtonWidthAsPercentage);
            var playAgainY = mainMenuY + mainMenuHeight + mediumMarginY;
            var playAgainX = (GraphicsHelper.GamePlayArea.Width - playAgainWidth) / 2;
            var playAgainHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.ButtonHeightAsPercentage);
            _playAgainButton = new UiTextButton(
                new Rectangle(playAgainX, playAgainY, playAgainWidth, playAgainHeight),
                "Play Again",
                _onPlayAgainCallback);
        }

        protected override bool UpdateTransitionIn(GameTime iGameTime)
        {
            return true;
        }

        protected override void UpdateDefault(GameTime iGameTime)
        {
            _header.Update(iGameTime);
            _subHeader.Update(iGameTime);
            //_definition.Update(iGameTime);
            _mainMenuButton.Update(iGameTime);
            _playAgainButton.Update(iGameTime);
        }

        protected override bool UpdateTransitionOut(GameTime iGameTime)
        {
            return true;
        }

        private readonly Rectangle _bounds;
        private readonly SessionStats _stats;
        private readonly Action<GameTime> _onMainMenuCallback;
        private readonly Action<GameTime> _onPlayAgainCallback;

        private UiFloatingText _header;
        private UiFloatingText _subHeader;
        //private UiFloatingText _definition;
        private UiTextButton _mainMenuButton;
        private UiTextButton _playAgainButton;
    }
}