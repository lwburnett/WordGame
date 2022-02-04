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
            _boundingBox = iBounds;
            _textColor = iTextColor;
            _textBorderColor = iTexBorderColor;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>(Path.Combine("Fonts", "PrototypeFont"));
            _scaling = iScaling;

            SetText(iText);
        }

        public void Update(GameTime iGameTime)
        {
        }

        public void Draw(Vector2? iOffset = null)
        {
            if (string.IsNullOrWhiteSpace(_text))
                return;

            var pos = _renderedStringBounds.Location.ToVector2();
            if (_textBorderColor.HasValue)
            {
                var borderWidth = GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.TextBorderWidthAsPercentage;
                GraphicsHelper.DrawStringWithBorder(_textFont, _text, pos, borderWidth, _textColor, _textBorderColor.Value, _scaling, 0f, iOffset);
            }
            else
                GraphicsHelper.DrawString(_textFont, _text, pos, _textColor, _scaling, 0f, iOffset);
        }

        public string GetText() => _text;

        public void SetText(string iNewText)
        {
            var words = iNewText.Split(' ');

            var singleLines = new List<string>();
            var singleLineBuffer = string.Empty;
            foreach (var thisWord in words)
            {
                var thisLine = singleLineBuffer + thisWord;

                var thisStringDimension = _textFont.MeasureString(thisLine) * _scaling;
                if (thisStringDimension.X <= _boundingBox.Width)
                    singleLineBuffer = $"{thisLine} ";
                else
                {
                    singleLines.Add(singleLineBuffer);
                    singleLineBuffer = $"{thisWord} ";
                }
            }

            if (singleLineBuffer.Length > 0)
                singleLines.Add(singleLineBuffer);

            // ReSharper disable once InconsistentNaming
            _text = string.Join("\n", singleLines.Select(l => l.Trim()));

            var stringDimensions = _textFont.MeasureString(_text) * _scaling;
            var topLeft = _boundingBox.Center - (stringDimensions / 2.0f).ToPoint();
            _renderedStringBounds = new Rectangle(topLeft, stringDimensions.ToPoint());
        }

        private readonly Rectangle _boundingBox;
        private Rectangle _renderedStringBounds;
        private string _text;
        private readonly SpriteFont _textFont;
        private readonly Color _textColor;
        private readonly Color? _textBorderColor;
        private readonly float _scaling;
    }
}