using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiNeonSpriteButton : UiButtonBase, IUiNeonElement
    {
        public UiNeonSpriteButton(Rectangle iBounds, string iTextureName, Color iOuterColor, Action<GameTime> iOnClickedCallback) :
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
            _neonSprite.Draw();
        }

        protected override Rectangle Bounds => _neonSprite.Bounds;

        public List<PointLight> LightPoints => _neonSprite.LightPoints;

        private readonly UiNeonElementBase _neonSprite;
    }
}