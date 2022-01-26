using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiMenuNeonButton : UiButtonBase
    {
        public UiMenuNeonButton(Rectangle iBounds, string iText, Color iTextColor, Action iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            _text = iText;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("NeonFont");

            const float factor = 1.75f;
            _textColorInner = new Color(iTextColor.R * factor, iTextColor.G * factor, iTextColor.B * factor, iTextColor.A);
            _textColorOuter = iTextColor;

            var defaultStringDimensionsOuter = _textFont.MeasureString(_text) * 1.0f;
            var possibleXScalingOuter = iBounds.Width / defaultStringDimensionsOuter.X;
            var possibleYScalingOuter = iBounds.Height / defaultStringDimensionsOuter.Y;

            _scaling = Math.Min(possibleXScalingOuter, possibleYScalingOuter);
            var realStringDimensionsOuter = _textFont.MeasureString(_text) * _scaling;

            Bounds = new Rectangle(
                (int)(iBounds.X + (iBounds.Width - realStringDimensionsOuter.X) / 2f),
                (int)(iBounds.Y + (iBounds.Height - realStringDimensionsOuter.Y) / 2f),
                (int)realStringDimensionsOuter.X,
                (int)realStringDimensionsOuter.Y);
        }
    
        public override void Draw()
        {
            base.Draw();
            
            const float scalar = 1.25f;
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(-scalar * _scaling, -scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(scalar * _scaling, -scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(-scalar * _scaling, scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(scalar * _scaling, scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y), _textColorInner, _scaling);
        }

        protected override Rectangle Bounds { get; }

        protected override Texture2D GetDefaultTexture() => null;

        protected override Texture2D GetHoverTexture() => null;

        protected override Texture2D GetPressedTexture() => null;

        private readonly string _text;
        private readonly SpriteFont _textFont;
        private readonly Color _textColorInner;
        private readonly Color _textColorOuter;
        private readonly float _scaling;
    }
}