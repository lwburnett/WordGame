using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiNeonSpriteButton : UiButtonBase, IUiNeonElement
    {
        public UiNeonSpriteButton(Rectangle iBounds, string iTextureName, Color iOuterColor, Action iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            _neonSprite = new UiNeonFloatSprite(iBounds, iTextureName, iOuterColor);
        }

        public override void Update(GameTime iGameTime)
        {
            base.Update(iGameTime);

            _neonSprite.Update(iGameTime);
        }

        public override void Draw()
        {
            base.Draw();

            _neonSprite.Draw();
        }

        protected override Rectangle Bounds => _neonSprite.Bounds;

        protected override Texture2D GetDefaultTexture() => null;

        protected override Texture2D GetHoverTexture() => null;

        protected override Texture2D GetPressedTexture() => null;

        public List<PointLight> LightPoints => _neonSprite.LightPoints;

        private readonly UiNeonElementBase _neonSprite;
    }
}