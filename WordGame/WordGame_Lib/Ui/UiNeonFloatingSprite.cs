using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiNeonSpriteButton : UiButtonBase, INeonUiElement
    {
        public UiNeonSpriteButton(Rectangle iBounds, string iTextureName, Color iOuterColor, Action iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            Bounds = iBounds;

            _texture = GraphicsHelper.LoadContent<Texture2D>(iTextureName);
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
            _shader.Parameters["OuterColor"].SetValue(new Vector4(iOuterColor.R / 255f, iOuterColor.G / 255f, iOuterColor.B / 255f, iOuterColor.A / 255f));
            _shader.Parameters["InnerColor"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            var pointLight = new PointLight(iOuterColor, iBounds.Center.ToVector2(), GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonTextSettings.RadiusAsPercentageOfWidth / 1.5f, 4.0f);
            LightPoints = new List<PointLight> { pointLight };
        }

        public override void Draw()
        {
            base.Draw();

            GraphicsHelper.DrawTexture(_texture, Bounds, _shader);
        }

        protected override Rectangle Bounds { get; }

        protected override Texture2D GetDefaultTexture() => null;

        protected override Texture2D GetHoverTexture() => null;

        protected override Texture2D GetPressedTexture() => null;

        public List<PointLight> LightPoints { get; }
        
        private readonly Texture2D _texture;
        private readonly Effect _shader;
    }
}