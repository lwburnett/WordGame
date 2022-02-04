using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

        public override void Draw(Vector2? iOffset = null)
        {
            _neonSprite.Draw(iOffset);
        }
        public void StartFadeIn(GameTime iGameTime, TimeSpan iDuration)
        {
            _neonSprite.StartFadeIn(iGameTime, iDuration);
        }

        public void StartFadeOut(GameTime iGameTime, TimeSpan iDuration)
        {
            _neonSprite.StartFadeOut(iGameTime, iDuration);
        }

        public NeonLightState State => _neonSprite.State;

        protected override Rectangle Bounds => _neonSprite.Bounds;

        public List<PointLight> LightPoints => _neonSprite.LightPoints;

        private readonly UiNeonElementBase _neonSprite;
    }
}