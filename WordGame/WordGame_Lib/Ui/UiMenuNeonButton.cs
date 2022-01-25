using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiMenuNeonButton : UiButtonBase
    {
        public UiMenuNeonButton(Rectangle iBounds, string iText, Action iOnClickedCallback) :
            base(iBounds, iOnClickedCallback)
        {
            
        }
    
        public override void Update(GameTime iGameTime)
        {
            throw new System.NotImplementedException();
        }
    
        public override void Draw()
        {
            throw new System.NotImplementedException();
        }

        protected override Texture2D GetDefaultTexture()
        {
            throw new NotImplementedException();
        }

        protected override Texture2D GetHoverTexture()
        {
            throw new NotImplementedException();
        }

        protected override Texture2D GetPressedTexture()
        {
            throw new NotImplementedException();
        }
    }
}