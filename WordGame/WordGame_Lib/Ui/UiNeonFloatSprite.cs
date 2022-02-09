using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiNeonFloatSprite : UiNeonElementBase
    {
        public UiNeonFloatSprite(Rectangle iBounds, string iTextureName, Color iOuterColor) :
            base(null)
        {
            Bounds = iBounds;
            FullIntensity = 4.0f;
            FullRadius = GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.Sprite.RadiusAsPercentageOfWidth;
            OuterColorAtFullIntensity = iOuterColor;
            InnerColorAtFullIntensity = new Color(255f, 255f, 255f, 255f);

            _texture = AssetHelper.LoadContent<Texture2D>(iTextureName);
            _shader = AssetHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
            _shaderOuterColorParameter = _shader.Parameters["OuterColor"];
            _shaderInnerColorParameter = _shader.Parameters["InnerColor"];
            var pointLight = new PointLight(iOuterColor, iBounds.Center.ToVector2(), FullRadius, 0.0f);
            LightPoints = new List<PointLight> { pointLight };
        }

        public override void Draw(Vector2? iOffset = null)
        {
            _shaderOuterColorParameter.SetValue(new Vector4(OuterColorToDraw.R / 255f, OuterColorToDraw.G / 255f, OuterColorToDraw.B / 255f, OuterColorToDraw.A / 255f));
            _shaderInnerColorParameter.SetValue(new Vector4(InnerColorToDraw.R / 255f, InnerColorToDraw.G / 255f, InnerColorToDraw.B / 255f, InnerColorToDraw.A / 255f));
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader, iOffset);
        }

        public override List<PointLight> LightPoints { get; }
        public override Rectangle Bounds { get; }

        protected sealed override float FullIntensity { get; }
        protected sealed override float FullRadius { get; }
        protected sealed override Color OuterColorAtFullIntensity { get; }
        protected sealed override Color InnerColorAtFullIntensity { get; }

        private readonly Texture2D _texture;
        private readonly Effect _shader;
        private readonly EffectParameter _shaderOuterColorParameter;
        private readonly EffectParameter _shaderInnerColorParameter;
    }
}