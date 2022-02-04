using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly float? _aspectRatioOverride;

        private Task _screenLoadTask;
        private ScreenId? _screenToTransitionTo;
        private TimeSpan? _screenTransitionPanStartTime;
        private Vector2? _currentScreenRenderOffset;
        private Vector2? _newScreenRenderOffset;

        public GameMaster(float? iAspectRatioOverride = null)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = PlatformUtilsHelper.GetIsMouseInput();

            _idToScreenDictionary = new Dictionary<ScreenId, IScreen>();
            foreach (var enumValue in Enum.GetValues(typeof(ScreenId)).Cast<ScreenId>())
            {
                _idToScreenDictionary.Add(enumValue, null);
            }

            _aspectRatioOverride = iAspectRatioOverride;
        }

        protected override void Initialize()
        {
            var height = GraphicsDevice.DisplayMode.Height;
            if (!_aspectRatioOverride.HasValue)
            {
                _graphics.PreferredBackBufferHeight = height;
                _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                _graphics.IsFullScreen = true;
            }
            else
            {
                // The .95 is a dumb little fix for windows so the bottom doesn't get cut off
                _graphics.PreferredBackBufferHeight = (int)(height * .95f);
                _graphics.PreferredBackBufferWidth = (int)(height / _aspectRatioOverride.Value);
                _graphics.IsFullScreen = false;
            }
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var windowHeight = Window.ClientBounds.Height;
            var windowWidth = Window.ClientBounds.Width;

            var chosenWidth = _aspectRatioOverride.HasValue ? (int)(windowHeight / _aspectRatioOverride.Value) : GraphicsDevice.DisplayMode.Width;
            var chosenHeight = windowHeight;

            var topLeftGamePlayAreaX = (int)((windowWidth / 2.0f) - (chosenWidth / 2.0f));
            var topLeftGamePlayAreaY = 0;
            var gamePlayArea = new Rectangle(topLeftGamePlayAreaX, topLeftGamePlayAreaY, chosenWidth, chosenHeight);

            GraphicsHelper.RegisterContentManager(Content);
            GraphicsHelper.RegisterGraphicsDevice(GraphicsDevice);
            GraphicsHelper.RegisterSpriteBatch(_spriteBatch);
            GraphicsHelper.RegisterGamePlayArea(gamePlayArea);

            _wordDatabase = LoadDatabaseFromTxtFile(Path.Combine("Data", "WordDatabase.txt"));

            _secretWordDatabase = LoadDatabaseFromTxtFile(Path.Combine("Data", "SecretWordDatabase.txt"));
            
            GameSettingsManager.RegisterFilePath(Path.Combine("Data", "GameSettings.txt"));

            GameSettingsManager.ReadSettingFromDiskAsync();

            OnStartupScreen();
        }

        private OrderedUniqueList<string> LoadDatabaseFromTxtFile(string iFileName)
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

            if (_currentScreenId == ScreenId.StartupScreen && !_screenToTransitionTo.HasValue)
            {
                OnMainMenu(iGameTime);
            }

            _idToScreenDictionary[_currentScreenId].Update(iGameTime);
            if (_screenToTransitionTo.HasValue)
            {
                // if (_screenLoadTask.IsCompleted && !_idToScreenDictionary[_currentScreenId].IsVisible)
                // {
                //     _currentScreenId = _screenToTransitionTo.Value;
                //     _screenLoadTask = null;
                //     _screenToTransitionTo = null;
                //     _idToScreenDictionary[_currentScreenId].StartTransitionIn(iGameTime);
                // }

                if (_screenLoadTask.IsCompleted && !_idToScreenDictionary[_currentScreenId].IsVisible)
                {
                    if (!_screenTransitionPanStartTime.HasValue)
                        _screenTransitionPanStartTime = iGameTime.TotalGameTime;

                    if (_currentScreenId != ScreenId.StartupScreen)
                    {
                        var timeDiff = iGameTime.TotalGameTime - _screenTransitionPanStartTime.Value;

                        if (timeDiff <= TimeSpan.FromSeconds(1.0))
                        {
                            var panRight = ShouldPanRight();

                            var lerpValue = (float)(timeDiff.TotalSeconds / TimeSpan.FromSeconds(1.0).TotalSeconds);

                            if (panRight)
                            {
                                _currentScreenRenderOffset = new Vector2(-GraphicsHelper.GamePlayArea.Width * lerpValue, 0);
                                _newScreenRenderOffset = _currentScreenRenderOffset + new Vector2(GraphicsHelper.GamePlayArea.Width, 0);
                            }
                            else
                            {
                                _currentScreenRenderOffset = new Vector2(GraphicsHelper.GamePlayArea.Width * lerpValue, 0);
                                _newScreenRenderOffset = _currentScreenRenderOffset - new Vector2(GraphicsHelper.GamePlayArea.Width, 0);
                            }
                        }
                        else
                        {
                            _currentScreenId = _screenToTransitionTo.Value;
                            _currentScreenRenderOffset = null;
                            _newScreenRenderOffset = null;
                            _screenLoadTask = null;
                            _screenToTransitionTo = null;
                            _screenTransitionPanStartTime = null;
                            _idToScreenDictionary[_currentScreenId].StartTransitionIn(iGameTime);
                        }
                    }
                    else
                    {
                        _currentScreenId = _screenToTransitionTo.Value;
                        _currentScreenRenderOffset = null;
                        _newScreenRenderOffset = null;
                        _screenLoadTask = null;
                        _screenToTransitionTo = null;
                        _screenTransitionPanStartTime = null;
                        _idToScreenDictionary[_currentScreenId].StartTransitionIn(iGameTime);
                    }
                }
            }

            base.Update(iGameTime);
        }

        protected override void Draw(GameTime iGameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (_screenToTransitionTo.HasValue && _screenLoadTask != null && _screenLoadTask.IsCompleted && _newScreenRenderOffset.HasValue)
                _idToScreenDictionary[_screenToTransitionTo.Value].Draw(_newScreenRenderOffset.Value);

            _idToScreenDictionary[_currentScreenId].Draw(_currentScreenRenderOffset);

            GraphicsHelper.Flush();

            base.Draw(iGameTime);
        }

        private enum ScreenId
        {
            StartupScreen,
            MainMenu,
            GamePlay,
            Settings
        }

        private bool ShouldPanRight()
        {
            if (!_screenToTransitionTo.HasValue)
                return true;

            if (_currentScreenId == ScreenId.MainMenu && _screenToTransitionTo.Value == ScreenId.GamePlay)
                return true;

            if (_currentScreenId == ScreenId.GamePlay && _screenToTransitionTo.Value == ScreenId.MainMenu)
                return false;

            if (_currentScreenId == ScreenId.MainMenu && _screenToTransitionTo.Value == ScreenId.Settings)
                return false;

            if (_currentScreenId == ScreenId.Settings && _screenToTransitionTo.Value == ScreenId.MainMenu)
                return true;

            Debug.Fail("Somehow got to a weird transition state.");
            return true;
        }

        private void OnStartupScreen()
        {
            _currentScreenId = ScreenId.StartupScreen;
            _idToScreenDictionary[_currentScreenId] = new StartupScreen();
            _idToScreenDictionary[_currentScreenId].Load().Wait();
        }

        private void SetupScreenTransition(ScreenId iId, Func<IScreen> iCreateFunc, GameTime iGameTime)
        {
            _screenToTransitionTo = iId;
            var screen = iCreateFunc();
            _screenLoadTask = screen.Load();

            _idToScreenDictionary[iId] = screen;

            _idToScreenDictionary[_currentScreenId].StartTransitionOut(iGameTime);
        }

        private void OnMainMenu(GameTime iGameTime)
        {
            SetupScreenTransition(ScreenId.MainMenu, () => new MainMenuScreen(OnPlayGame, OnSettings, OnExitGame), iGameTime);
        }

        private void OnPlayGame(GameTime iGameTime)
        {
            SetupScreenTransition(ScreenId.GamePlay, () => new GamePlayScreen(_wordDatabase, _secretWordDatabase, OnMainMenu, OnExitGame), iGameTime);
        }

        private void OnSettings(GameTime iGameTime)
        {
            SetupScreenTransition(ScreenId.Settings, () => new SettingsScreen(OnMainMenu), iGameTime);
        }

        private void OnExitGame(GameTime iGameTime)
        {
            Exit();
        }
    }
}
