using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiNeonFloatSprite : NeonUiElementBase
    {
        public UiNeonFloatSprite(Rectangle iBounds, string iTextureName, Color iOuterColor)
        {
            Bounds = iBounds;
            FullIntensity = 4.0f;

            _texture = GraphicsHelper.LoadContent<Texture2D>(iTextureName);
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
            _shader.Parameters["OuterColor"].SetValue(new Vector4(iOuterColor.R / 255f, iOuterColor.G / 255f, iOuterColor.B / 255f, iOuterColor.A / 255f));
            _shader.Parameters["InnerColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            var pointLight = new PointLight(iOuterColor, iBounds.Center.ToVector2(), GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.Sprite.RadiusAsPercentageOfWidth, FullIntensity);
            LightPoints = new List<PointLight> { pointLight };
        }

        public override void Draw()
        {
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader);
        }

        public override List<PointLight> LightPoints { get; }
        public override Rectangle Bounds { get; }

        protected sealed override float FullIntensity { get; }

        private readonly Texture2D _texture;
        private readonly Effect _shader;
    }
}