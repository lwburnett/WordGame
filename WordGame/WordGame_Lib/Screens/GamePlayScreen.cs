using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        public GamePlayScreen(
            OrderedUniqueList<string> iWordDatabase, 
            OrderedUniqueList<string> iSecretWordDatabase,
            Action<GameTime> iOnMainMenuCallback)
        {
            _gamePlayInstance = new GamePlayInstance(iWordDatabase, iSecretWordDatabase, iOnMainMenuCallback);

        }

        public override void Draw(Vector2? iOffset = null)
        {
            var lightPoints = _gamePlayInstance.LightPoints;
            GraphicsHelper.CalculatePointLightShaderParameters(lightPoints, out var positions, out var colors, out var radii, out var intensity, iOffset);

            _paramScreenDimensions.SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _paramPointLightPosition.SetValue(positions);
            _paramPointLightColor.SetValue(colors);
            _paramPointLightRadius.SetValue(radii);
            _paramPointLightIntensity.SetValue(intensity);
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect, iOffset);

            _gamePlayInstance.Draw(iOffset);
        }

        protected override void DoLoad()
        {
            _backgroundTexture = AssetHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = AssetHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();

            _paramScreenDimensions = _backgroundEffect.Parameters["ScreenDimensions"];
            _paramPointLightPosition = _backgroundEffect.Parameters["PointLightPosition"];
            _paramPointLightColor = _backgroundEffect.Parameters["PointLightColor"];
            _paramPointLightRadius = _backgroundEffect.Parameters["PointLightRadius"];
            _paramPointLightIntensity = _backgroundEffect.Parameters["PointLightIntensity"];

            _gamePlayInstance.LoadLevel();
        }

        protected override bool UpdateTransitionIn(GameTime iGameTime)
        {
            return _gamePlayInstance.UpdateTransitionIn(iGameTime);
        }

        protected override void UpdateDefault(GameTime iGameTime)
        {
            _gamePlayInstance.Update(iGameTime);
        }

        protected override bool UpdateTransitionOut(GameTime iGameTime)
        {
            return _gamePlayInstance.UpdateTransitionOut(iGameTime);
        }

        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;

        private EffectParameter _paramScreenDimensions;
        private EffectParameter _paramPointLightPosition;
        private EffectParameter _paramPointLightColor;
        private EffectParameter _paramPointLightRadius;
        private EffectParameter _paramPointLightIntensity;
        
        private readonly GamePlayInstance _gamePlayInstance;
        //private IScreen _postSessionStatsScreen;
    }
}