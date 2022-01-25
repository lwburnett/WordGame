using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordGame_Lib.Screens;

namespace WordGame_Lib
{
    public class GameMaster : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ScreenId _currentScreenId;
        private readonly Dictionary<ScreenId, IScreen> _idToScreenDictionary;
        private OrderedUniqueList<string> _wordDatabase;
        private OrderedUniqueList<string> _secretWordDatabase;

        public GameMaster()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = PlatformUtilsHelper.GetIsMouseInput();

            _idToScreenDictionary = new Dictionary<ScreenId, IScreen>();
            foreach (var enumValue in Enum.GetValues(typeof(ScreenId)).Cast<ScreenId>())
            {
                _idToScreenDictionary.Add(enumValue, null);
            }
        }

        protected override void Initialize()
        {
            var height = GraphicsDevice.DisplayMode.Height * .95f;
            _graphics.PreferredBackBufferHeight = (int)height;
            _graphics.PreferredBackBufferWidth = (int)(height / SettingsManager.GameMasterSettings.TargetScreenAspectRatio);
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var windowHeight = Window.ClientBounds.Height;
            var windowWidth = Window.ClientBounds.Width;

            var chosenWidth = (int)(windowHeight / SettingsManager.GameMasterSettings.TargetScreenAspectRatio);
            var chosenHeight = windowHeight;

            var topLeftGamePlayAreaX = (int)((windowWidth / 2.0f) - (chosenWidth / 2.0f));
            var topLeftGamePlayAreaY = 0;
            var gamePlayArea = new Rectangle(topLeftGamePlayAreaX, topLeftGamePlayAreaY, chosenWidth, chosenHeight);

            GraphicsHelper.RegisterContentManager(Content);
            GraphicsHelper.RegisterGraphicsDevice(GraphicsDevice);
            GraphicsHelper.RegisterSpriteBatch(_spriteBatch);
            GraphicsHelper.RegisterGamePlayArea(gamePlayArea);

            _wordDatabase = LoadDatabaseFromTxtFile("WordDatabase.txt");

            _secretWordDatabase = LoadDatabaseFromTxtFile("SecretWordDatabase.txt");
            
            GameSettingsManager.RegisterFilePath(Path.Combine("Settings", "GameSettings.txt"));

            GameSettingsManager.ReadSettingFromDiskAsync();

            OnMainMenu();
        }

        OrderedUniqueList<string> LoadDatabaseFromTxtFile(string iFileName)
        {
            var entries = new List<string>();
            using (var stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, iFileName)))
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    entries.Add(line);
            }

            var wordsToReturn = new OrderedUniqueList<string>();
            foreach (var entry in entries)
            {
                if (entry.Trim().Length == 5)
                {
                    wordsToReturn.Add(entry.Trim().ToUpperInvariant());
                }
                else
                {
                    Debug.Fail($"Line of word database breaks format: {entry}");
                }
            }

            return wordsToReturn;
        }

        protected override void Update(GameTime iGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _idToScreenDictionary[_currentScreenId].Update(iGameTime);

            base.Update(iGameTime);
        }

        protected override void Draw(GameTime iGameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _idToScreenDictionary[_currentScreenId].Draw();
            _spriteBatch.End();

            base.Draw(iGameTime);
        }

        private enum ScreenId
        {
            MainMenu,
            GamePlay,
            Settings
        }

        private void OnMainMenu()
        {
            _currentScreenId = ScreenId.MainMenu;
            _idToScreenDictionary[_currentScreenId] = new MainMenuScreen(OnPlayGame, OnSettings, OnExitGame);
            _idToScreenDictionary[_currentScreenId].OnNavigateTo();
        }

        private void OnPlayGame()
        {
            _currentScreenId = ScreenId.GamePlay;
            _idToScreenDictionary[_currentScreenId] = new GamePlayScreen(_wordDatabase, _secretWordDatabase, OnPlayGame, OnMainMenu, OnExitGame);
            _idToScreenDictionary[_currentScreenId].OnNavigateTo();
        }

        private void OnSettings()
        {
            _currentScreenId = ScreenId.Settings;
            _idToScreenDictionary[_currentScreenId] = new SettingsScreen(OnMainMenu);
            _idToScreenDictionary[_currentScreenId].OnNavigateTo();
        }

        private void OnExitGame()
        {
            Exit();
        }
    }
}
