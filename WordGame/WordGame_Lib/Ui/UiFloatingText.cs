using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiFloatingText : IUiElement
    {
        public UiFloatingText(Point iTopLeft, string iText) :
            this(iTopLeft, iText, Color.Black)
        {
        }

        public UiFloatingText(Point iTopLeft, string iText, Color iTextColor)
        {
            _topLeft = iTopLeft;
            _text = iText;
            _textColor = iTextColor;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
        }

        public UiFloatingText(Rectangle iBounds, string iText, Color iTextColor)
        {
            _text = iText;
            _textColor = iTextColor;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");

            const float scaling = 1.0f;
            var stringDimensions = _textFont.MeasureString(_text) * scaling;

            var left = iBounds.Left + (iBounds.Width / 2) - ((int)stringDimensions.X / 2);
            var top = iBounds.Top + (iBounds.Height / 2) - ((int)stringDimensions.Y / 2);

            _topLeft = new Point(left, top);
        }

        public void Update(GameTime iGameTime)
        {
        }

        public void Draw()
        {
            GraphicsHelper.DrawString(_textFont, _text, _topLeft.ToVector2(), _textColor);
        }

        private Point _topLeft;
        private readonly string _text;
        private readonly SpriteFont _textFont;
        private readonly Color _textColor;
    }
}