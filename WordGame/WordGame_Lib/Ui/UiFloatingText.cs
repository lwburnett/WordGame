using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiFloatingText : IUiElement
    {
        public UiFloatingText(Rectangle iBounds, string iText, Color iTextColor, Color? iTexBorderColor = null, float iScaling = 1.0f)
        {
            _textColor = iTextColor;
            _textBorderColor = iTexBorderColor;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>(Path.Combine("Fonts", "PrototypeFont"));
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

            if (singleLineBuffer.Length > 0)
                singleLines.Add(singleLineBuffer);

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
            const float offsetScalar = 1.1f;
            var pos = _topLeft.ToVector2();
            if (_textBorderColor.HasValue)
                GraphicsHelper.DrawStringWithBorder(_textFont, _text, pos, offsetScalar, _textColor, _textBorderColor.Value, _scaling);
            else
                GraphicsHelper.DrawString(_textFont, _text, pos, _textColor, _scaling);
        }

        private Point _topLeft;
        private readonly string _text;
        private readonly SpriteFont _textFont;
        private readonly Color _textColor;
        private readonly Color? _textBorderColor;
        private readonly float _scaling;
    }
}