﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class PostSessionStatsScreen : IScreen
    {
        public PostSessionStatsScreen(Rectangle iBounds, SessionStats iStats, Action iOnMainMenuCallback, Action iOnPlayAgainCallback)
        {
            _bounds = iBounds;
            _stats = iStats;
            _onMainMenuCallback = iOnMainMenuCallback;
            _onPlayAgainCallback = iOnPlayAgainCallback;
            
            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
        }

        public void OnNavigateTo()
        {
            var bigMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.BigMarginAsPercentage);
            var mediumMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.MediumMarginAsPercentage);
            var smallMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.SmallMarginAsPercentage);

            var bigMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.BigMarginAsPercentage);
            var mediumMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.MediumMarginAsPercentage);
            var smallMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.PostSessionStatsSettings.SmallMarginAsPercentage);

            var headerYLocation = _bounds.Y + bigMarginY;
            var headerXLocation = _bounds.X + bigMarginX;
            var headerHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.HeaderHeightAsPercentage);
            var headerWidth = GraphicsHelper.GamePlayArea.Width - bigMarginX - bigMarginX;
            _header = new UiFloatingText(
                new Rectangle(headerXLocation, headerYLocation, headerWidth, headerHeight),
                _stats.Success ? "Success!" : "Failure!",
                Color.White,
                1.5f);

            var subHeaderY = headerYLocation + headerHeight + smallMarginY;
            var subHeaderX = headerXLocation;
            var subHeaderHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.SubHeaderHeightAsPercentage);
            var subHeaderWidth = headerWidth;
            _subHeader = new UiFloatingText(
                new Rectangle(subHeaderX, subHeaderY, subHeaderWidth, subHeaderHeight),
                $"Guesses: {_stats.NumGuesses}\nWord: {_stats.SecretWord}",
                Color.White,
                1.5f);

            var defY = subHeaderY + subHeaderHeight + smallMarginY;
            var defX = headerXLocation;
            var defHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.PostSessionStatsSettings.DefinitionHeightAsPercentage);
            var defWidth = headerWidth;
            _definition = new UiFloatingText(
                new Rectangle(defX, defY, defWidth, defHeight),
                $"{_stats.SecretWord}: {_stats.SecretWordDefinition}",
                Color.White);
        }

        public void Update(GameTime iGameTime)
        {
            _header.Update(iGameTime);
            _subHeader.Update(iGameTime);
            _definition.Update(iGameTime);
        }

        public void Draw()
        {
            _header.Draw();
            _subHeader.Draw();
            _definition.Draw();
        }

        private readonly Rectangle _bounds;
        private readonly SessionStats _stats;
        private readonly Action _onMainMenuCallback;
        private readonly Action _onPlayAgainCallback;
        private readonly SpriteFont _textFont;

        private UiFloatingText _header;
        private UiFloatingText _subHeader;
        private UiFloatingText _definition;
    }
}