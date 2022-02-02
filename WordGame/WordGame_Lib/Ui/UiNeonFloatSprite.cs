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
            OuterColorAtFullIntensity = iOuterColor;
            InnerColorAtFullIntensity = new Color(255f, 255f, 255f, 255f);

            _texture = GraphicsHelper.LoadContent<Texture2D>(iTextureName);
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
            _shaderOuterColorParameter = _shader.Parameters["OuterColor"];
            _shaderInnerColorParameter = _shader.Parameters["InnerColor"];
            var pointLight = new PointLight(iOuterColor, iBounds.Center.ToVector2(), GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.Sprite.RadiusAsPercentageOfWidth, FullIntensity);
            LightPoints = new List<PointLight> { pointLight };
        }

        public override void Draw()
        {
            _shaderOuterColorParameter.SetValue(new Vector4(OuterColorAtFullIntensity.R / 255f, OuterColorAtFullIntensity.G / 255f, OuterColorAtFullIntensity.B / 255f, OuterColorAtFullIntensity.A / 255f));
            _shaderInnerColorParameter.SetValue(new Vector4(InnerColorAtFullIntensity.R / 255f, InnerColorAtFullIntensity.G / 255f, InnerColorAtFullIntensity.B / 255f, InnerColorAtFullIntensity.A / 255f));
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader);
        }

        public override List<PointLight> LightPoints { get; }
        public override Rectangle Bounds { get; }

        protected sealed override float FullIntensity { get; }
        protected sealed override Color OuterColorAtFullIntensity { get; }
        protected sealed override Color InnerColorAtFullIntensity { get; }

        private readonly Texture2D _texture;
        private readonly Effect _shader;
        private readonly EffectParameter _shaderOuterColorParameter;
        private readonly EffectParameter _shaderInnerColorParameter;
    }
}