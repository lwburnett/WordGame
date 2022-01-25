using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiMenuNeonButton : UiButtonBase
    {
        public UiMenuNeonButton(Rectangle iBounds, string iText, Action iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            _text = iText;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("NeonFont");

            var defaultStringDimensions = _textFont.MeasureString(_text) * 1.0f;
            var possibleXScaling = iBounds.Width / defaultStringDimensions.X;
            var possibleYScaling = iBounds.Height / defaultStringDimensions.Y;

            _scaling = Math.Min(possibleXScaling, possibleYScaling);
            var realStringDimensions = _textFont.MeasureString(_text) * _scaling;

            Bounds = new Rectangle(
                (int)(iBounds.X + (iBounds.Width - realStringDimensions.X) / 2f),
                (int)(iBounds.Y + (iBounds.Height - realStringDimensions.Y) / 2f),
                (int)realStringDimensions.X,
                (int)realStringDimensions.Y);
        }
    
        public override void Draw()
        {
            base.Draw();
            
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y), Color.LightGreen, _scaling);
        }

        protected override Rectangle Bounds { get; }

        protected override Texture2D GetDefaultTexture() => null;

        protected override Texture2D GetHoverTexture() => null;

        protected override Texture2D GetPressedTexture() => null;

        private readonly string _text;
        private readonly SpriteFont _textFont;
        private readonly float _scaling;
    }
}