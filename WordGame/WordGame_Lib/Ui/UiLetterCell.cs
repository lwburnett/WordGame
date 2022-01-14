using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiLetterCell : IUiElement
    {
        public UiLetterCell(Rectangle iBounds)
        {
            _bounds = iBounds;
            _text = string.Empty;
            _disposition = Disposition.Undecided;

            var width = _bounds.Width;
            var height = _bounds.Height;

            var colorData1 = new Color[width * height];
            for (var xx = 0; xx < height; xx++)
            {
                for (var yy = 0; yy < _bounds.Width; yy++)
                {
                    const int outlineWidth = 3;

                    if (xx < outlineWidth || yy < outlineWidth || yy > width - outlineWidth - 1 || xx > width - outlineWidth - 1)
                        colorData1[(xx * (width)) + yy] = Color.Gray;
                }
            }

            var dataSize = _bounds.Width * _bounds.Height;
            var colorData2 = new Color[dataSize];
            var colorData3 = new Color[dataSize];
            var colorData4 = new Color[dataSize];
            for (var ii = 0; ii < dataSize; ii++)
            {
                colorData2[ii] = Color.Gray;
                colorData3[ii] = Color.LightGoldenrodYellow;
                colorData4[ii] = Color.ForestGreen;
            }

            _emptyTexture = GraphicsHelper.CreateTexture(colorData1, _bounds.Width, _bounds.Height);
            _incorrectTexture = GraphicsHelper.CreateTexture(colorData2, _bounds.Width, _bounds.Height);
            _misplacedTexture = GraphicsHelper.CreateTexture(colorData3, _bounds.Width, _bounds.Height);
            _correctTexture = GraphicsHelper.CreateTexture(colorData4, _bounds.Width, _bounds.Height);

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
        }

        public void Update(GameTime iGameTime)
        {
        }

        public void Draw()
        {
            switch (_disposition)
            {
                case Disposition.Undecided:
                    GraphicsHelper.DrawTexture(_emptyTexture, new Vector2(_bounds.Left, _bounds.Top));
                    break;
                case Disposition.Incorrect:
                    GraphicsHelper.DrawTexture(_incorrectTexture, new Vector2(_bounds.Left, _bounds.Top));
                    break;
                case Disposition.Misplaced:
                    GraphicsHelper.DrawTexture(_misplacedTexture, new Vector2(_bounds.Left, _bounds.Top));
                    break;
                case Disposition.Correct:
                    GraphicsHelper.DrawTexture(_correctTexture, new Vector2(_bounds.Left, _bounds.Top));
                    break;
                default:
                    GraphicsHelper.DrawTexture(_emptyTexture, new Vector2(_bounds.Left, _bounds.Top));
                    Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                    break;
            }

            if (string.IsNullOrWhiteSpace(_text)) return;
            
            const float scaling = 1.0f;
            var stringDimensions = _textFont.MeasureString(_text) * scaling;
            GraphicsHelper.DrawString(
                _textFont, 
                _text, 
                new Vector2(_bounds.X + (_bounds.Width - stringDimensions.X) / 2f, _bounds.Y + (_bounds.Height - stringDimensions.Y) / 2f), 
                Color.White);
        }

        public void SetText(string iKeyString)
        {
            _text = iKeyString;
        }

        public string GetText()
        {
            return _text;
        }

        public void SetDisposition(Disposition iDisposition)
        {
            _disposition = iDisposition;
        }

        private readonly Rectangle _bounds;
        private string _text;
        private readonly Texture2D _emptyTexture;
        private readonly Texture2D _incorrectTexture;
        private readonly Texture2D _misplacedTexture;
        private readonly Texture2D _correctTexture;
        private readonly SpriteFont _textFont;
        private Disposition _disposition;
    }
}