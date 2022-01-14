using System.Collections.Generic;
using System.Linq;
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

        public UiFloatingText(Rectangle iBounds, string iText, Color iTextColor, float iScaling = 1.0f)
        {
            _textColor = iTextColor;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
            _scaling = iScaling;

            var words = iText.Split(' ');

            var singleLines = new List<string>();
            var singleLineBuffer = string.Empty;
            for (var ii = 0; ii < words.Length; ii++)
            {
                var thisWord = words[ii];
                var thisLine = singleLineBuffer + thisWord;

                var thisStringDimension = _textFont.MeasureString(thisLine) * _scaling;
                if (thisStringDimension.X <= iBounds.Width) 
                    singleLineBuffer = $"{thisLine} ";
                else
                {
                    singleLines.Add(singleLineBuffer);
                    singleLineBuffer = $"{thisWord} ";
                }
            }

            if (singleLines.Count == 0)
                singleLines.Add(iText);

            _text = string.Join("\n", singleLines.Select(l => l.Trim()));

            var stringDimensions = _textFont.MeasureString(_text) * _scaling;

            var left = iBounds.Left + (iBounds.Width / 2) - ((int)stringDimensions.X / 2);
            var top = iBounds.Top + (iBounds.Height / 2) - ((int)stringDimensions.Y / 2);

            _topLeft = new Point(left, top);
        }

        public void Update(GameTime iGameTime)
        {
        }

        public void Draw()
        {
            GraphicsHelper.DrawString(_textFont, _text, _topLeft.ToVector2(), _textColor, _scaling);
        }

        private Point _topLeft;
        private readonly string _text;
        private readonly SpriteFont _textFont;
        private readonly Color _textColor;
        private readonly float _scaling;
    }
}