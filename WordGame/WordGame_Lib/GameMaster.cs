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
        private readonly SortedList<string, string> _wordDatabase;

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

            _wordDatabase = new SortedList<string, string>();
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

            var entries = new List<string>();
            using (var stream = TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, "WordDatabase.txt")))
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    entries.Add(line);
            }

            foreach (var entry in entries)
            {
                var pieces = entry.Split('\t');

                if (pieces.Length == 2)
                {
                    _wordDatabase.Add(pieces[0].Trim().ToUpperInvariant(), pieces[1].Trim());
                }
                else
                {
                    Debug.Fail($"Line of dictionary breaks format: {entry}");
                }
            }

            OnMainMenu();
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
            GamePlay
        }

        private void OnMainMenu()
        {
            _currentScreenId = ScreenId.MainMenu;
            _idToScreenDictionary[_currentScreenId] = new MainMenuScreen(OnPlayGame, OnExitGame);
            _idToScreenDictionary[_currentScreenId].OnNavigateTo();
        }

        private void OnPlayGame()
        {
            _currentScreenId = ScreenId.GamePlay;
            _idToScreenDictionary[_currentScreenId] = new GamePlayScreen(_wordDatabase, OnPlayGame, OnMainMenu, OnExitGame);
            _idToScreenDictionary[_currentScreenId].OnNavigateTo();
        }

        private void OnExitGame()
        {
            Exit();
        }
    }
}
