using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WordGame_Lib.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        public GamePlayScreen(
            OrderedUniqueList<string> iWordDatabase, 
            OrderedUniqueList<string> iSecretWordDatabase,
            Action<GameTime> iOnMainMenuCallback, 
            Action<GameTime> iOnExitCallback)
        {
            _gamePlayInstance = new GamePlayInstance(iWordDatabase, iSecretWordDatabase, iOnMainMenuCallback);
            _onExitCallback = iOnExitCallback;

        }

        public override void Update(GameTime iGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _onExitCallback(iGameTime);

            _gamePlayInstance.Update(iGameTime);
        }

        public override void Draw()
        {
            var lightPoints = _gamePlayInstance.LightPoints;
            GraphicsHelper.CalculatePointLightShaderParameters(lightPoints, out var positions, out var colors, out var radii, out var intensity);

            _paramScreenDimensions.SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _paramPointLightPosition.SetValue(positions);
            _paramPointLightColor.SetValue(colors);
            _paramPointLightRadius.SetValue(radii);
            _paramPointLightIntensity.SetValue(intensity);
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect);

            _gamePlayInstance.Draw();
        }

        protected override void DoLoad()
        {
            _backgroundTexture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();

            _paramScreenDimensions = _backgroundEffect.Parameters["ScreenDimensions"];
            _paramPointLightPosition = _backgroundEffect.Parameters["PointLightPosition"];
            _paramPointLightColor = _backgroundEffect.Parameters["PointLightColor"];
            _paramPointLightRadius = _backgroundEffect.Parameters["PointLightRadius"];
            _paramPointLightIntensity = _backgroundEffect.Parameters["PointLightIntensity"];

            _gamePlayInstance.LoadLevel();
        }

        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;

        private EffectParameter _paramScreenDimensions;
        private EffectParameter _paramPointLightPosition;
        private EffectParameter _paramPointLightColor;
        private EffectParameter _paramPointLightRadius;
        private EffectParameter _paramPointLightIntensity;

        private readonly Action<GameTime> _onExitCallback;
        private readonly GamePlayInstance _gamePlayInstance;
        //private IScreen _postSessionStatsScreen;
    }
}