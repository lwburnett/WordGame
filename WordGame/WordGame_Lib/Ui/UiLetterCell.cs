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

            _texture = GraphicsHelper.LoadContent<Texture2D>("LetterBoxOutline");
            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
            _shader = GraphicsHelper.LoadContent<Effect>("LetterBoxOutlineShader").Clone();
            _shaderInnerColorParameter = _shader.Parameters["InnerColor"];
            _shaderOuterColorParameter = _shader.Parameters["OuterColor"];
            SetDisposition(Disposition.Undecided);
        }

        public void Update(GameTime iGameTime)
        {
        }

        public void Draw()
        {
            GraphicsHelper.DrawTexture(_texture, _bounds, _shader);

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

            Color outerColor;
            switch (_disposition)
            {
                case Disposition.Undecided:
                    outerColor = SettingsManager.ColorSettings.UndecidedDefaultColor;
                    break;
                case Disposition.Incorrect:
                    outerColor = SettingsManager.ColorSettings.IncorrectDefaultColor;
                    break;
                case Disposition.Misplaced:
                    outerColor = SettingsManager.ColorSettings.MisplacedDefaultColor;
                    break;
                case Disposition.Correct:
                    outerColor = SettingsManager.ColorSettings.CorrectDefaultColor;
                    break;
                default:
                    outerColor = SettingsManager.ColorSettings.UndecidedDefaultColor;
                    Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                    break;
            }

            var innerFactor = 255 / 255f;
            var outerFactor = 1 / 255f;
            _shaderInnerColorParameter.SetValue(new Vector4(outerColor.R * innerFactor, outerColor.G * innerFactor, outerColor.B * innerFactor, outerColor.A * innerFactor));
            _shaderOuterColorParameter.SetValue(new Vector4(outerColor.R * outerFactor, outerColor.G * outerFactor, outerColor.B * outerFactor, outerColor.A * outerFactor));
        }

        private readonly Rectangle _bounds;
        private string _text;
        private readonly Texture2D _texture;
        private readonly Effect _shader;
        private readonly EffectParameter _shaderOuterColorParameter;
        private readonly EffectParameter _shaderInnerColorParameter;
        private readonly SpriteFont _textFont;
        private Disposition _disposition;
    }
}