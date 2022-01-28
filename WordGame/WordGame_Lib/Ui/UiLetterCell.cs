using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiLetterCell : INeonUiElement
    {
        public UiLetterCell(Rectangle iBounds)
        {
            _bounds = iBounds;
            _text = string.Empty;

            _texture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "LetterBoxOutline"));
            _textFont = GraphicsHelper.LoadContent<SpriteFont>(Path.Combine("Fonts", "PrototypeFont"));
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
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
            const float offsetScalar = 1.1f;
            var pos = new Vector2(_bounds.X + (_bounds.Width - stringDimensions.X) / 2f, _bounds.Y + (_bounds.Height - stringDimensions.Y) / 2f);
            GraphicsHelper.DrawStringWithBorder(_textFont, _text, pos, offsetScalar, Color.White, Color.Black);
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

            GetColorForDisposition(_disposition, out var outerColor, out var innerColor, out var intensity);

            _shaderInnerColorParameter.SetValue(new Vector4(innerColor.R / 255f, innerColor.G / 255f, innerColor.B / 255f, innerColor.A / 255f));
            _shaderOuterColorParameter.SetValue(new Vector4(outerColor.R / 255f, outerColor.G / 255f, outerColor.B / 255f, outerColor.A / 255f));

            _pointLight = new PointLight(outerColor, _bounds.Center.ToVector2(), GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonTextSettings.RadiusAsPercentageOfWidth / 1.5f, intensity);
        }

        public List<PointLight> LightPoints => new List<PointLight> { _pointLight };

        private readonly Rectangle _bounds;
        private string _text;
        private readonly Texture2D _texture;
        private readonly Effect _shader;
        private readonly EffectParameter _shaderOuterColorParameter;
        private readonly EffectParameter _shaderInnerColorParameter;
        private readonly SpriteFont _textFont;
        private Disposition _disposition;
        private PointLight _pointLight;

        // ReSharper disable InconsistentNaming
        private static void GetColorForDisposition(Disposition iDisposition, out Color oOuterColor, out Color oInnerColor, out float oIntensity)
        {
            Color outerColor;
            Color innerColor;
            float intensity;
            switch (iDisposition)
            {
                case Disposition.Undecided:
                    outerColor = SettingsManager.LetterCellColors.Undecided;
                    innerColor = SettingsManager.LetterCellColors.Undecided;
                    intensity = 1.75f;
                    break;
                case Disposition.Incorrect:
                    outerColor = SettingsManager.LetterCellColors.Incorrect;
                    innerColor = SettingsManager.LetterCellColors.Incorrect;
                    intensity = 0.0f;
                    break;
                case Disposition.Misplaced:
                    outerColor = SettingsManager.LetterCellColors.Misplaced;
                    innerColor = Color.White;
                    intensity = 4.0f;
                    break;
                case Disposition.Correct:
                    outerColor = SettingsManager.LetterCellColors.Correct;
                    innerColor = Color.White;
                    intensity = 4.0f;
                    break;
                default:
                    outerColor = SettingsManager.LetterCellColors.Undecided;
                    innerColor = SettingsManager.LetterCellColors.Undecided;
                    intensity = 1.75f;
                    Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {iDisposition}");
                    break;
            }

            oOuterColor = outerColor;
            oInnerColor = innerColor;
            oIntensity = intensity;
        }
        // ReSharper restore InconsistentNaming
    }
}