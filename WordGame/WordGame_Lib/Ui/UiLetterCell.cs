using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiLetterCell : UiNeonElementBase
    {
        public UiLetterCell(Rectangle iBounds) :
            base(0)
        {
            Bounds = iBounds;
            FullRadius = GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.LetterCell.RadiusAsPercentageOfWidth;
            _floatingText = new UiFloatingText(iBounds, string.Empty, Color.White, Color.Black);

            _texture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "LetterBoxOutline"));
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
            _shaderInnerColorParameter = _shader.Parameters["InnerColor"];
            _shaderOuterColorParameter = _shader.Parameters["OuterColor"];
            SetDisposition(Disposition.Incorrect);
        }

        public override void Draw(Vector2? iOffset = null)
        {
            _shaderInnerColorParameter.SetValue(new Vector4(InnerColorToDraw.R / 255f, InnerColorToDraw.G / 255f, InnerColorToDraw.B / 255f, InnerColorToDraw.A / 255f));
            _shaderOuterColorParameter.SetValue(new Vector4(OuterColorToDraw.R / 255f, OuterColorToDraw.G / 255f, OuterColorToDraw.B / 255f, OuterColorToDraw.A / 255f));
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader, iOffset);
            _floatingText.Draw(iOffset);
        }

        public void SetText(string iKeyString)
        {
            _floatingText.SetText(iKeyString);
        }

        public string GetText()
        {
            return _floatingText.GetText();
        }

        public void SetDisposition(Disposition iDisposition)
        {
            _disposition = iDisposition;

            GetColorForDisposition(_disposition, out var outerColor, out var innerColor, out var intensity);
            _outerColor = outerColor;
            _innerColor = innerColor;

            _currentIntensity = intensity;

            _pointLight = new PointLight(outerColor, Bounds.Center.ToVector2(), FullRadius, intensity);
        }

        public override Rectangle Bounds { get; }
        public override List<PointLight> LightPoints => new List<PointLight> { _pointLight };

        protected override float FullIntensity => _currentIntensity;
        protected override float FullRadius { get; }
        protected override Color InnerColorAtFullIntensity => _innerColor;
        protected override Color OuterColorAtFullIntensity => _outerColor;

        private readonly UiFloatingText _floatingText;
        private readonly Texture2D _texture;
        private readonly Effect _shader;
        private Color _outerColor;
        private Color _innerColor;
        private readonly EffectParameter _shaderOuterColorParameter;
        private readonly EffectParameter _shaderInnerColorParameter;
        private Disposition _disposition;
        private float _currentIntensity;
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
                    outerColor = SettingsManager.NeonSettings.LetterCell.Undecided;
                    innerColor = SettingsManager.NeonSettings.LetterCell.Undecided;
                    intensity = 1.75f;
                    break;
                case Disposition.Incorrect:
                    outerColor = SettingsManager.NeonSettings.LetterCell.Incorrect;
                    innerColor = SettingsManager.NeonSettings.LetterCell.Incorrect;
                    intensity = 0.0f;
                    break;
                case Disposition.Misplaced:
                    outerColor = SettingsManager.NeonSettings.LetterCell.Misplaced;
                    innerColor = Color.White;
                    intensity = 4.0f;
                    break;
                case Disposition.Correct:
                    outerColor = SettingsManager.NeonSettings.LetterCell.Correct;
                    innerColor = Color.White;
                    intensity = 4.0f;
                    break;
                default:
                    outerColor = SettingsManager.NeonSettings.LetterCell.Undecided;
                    innerColor = SettingsManager.NeonSettings.LetterCell.Undecided;
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