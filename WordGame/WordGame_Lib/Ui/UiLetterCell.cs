using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiLetterCell : NeonUiElementBase
    {
        public UiLetterCell(Rectangle iBounds) :
            base(0)
        {
            Bounds = iBounds;
            _floatingText = new UiFloatingText(iBounds, string.Empty, Color.White, Color.Black);

            _texture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "LetterBoxOutline"));
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "NeonSpriteShader")).Clone();
            _shaderInnerColorParameter = _shader.Parameters["InnerColor"];
            _shaderOuterColorParameter = _shader.Parameters["OuterColor"];
            SetDisposition(Disposition.Undecided);
        }

        public override void Draw()
        {
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader);
            _floatingText.Draw();
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

            _currentIntensity = intensity;

            _shaderInnerColorParameter.SetValue(new Vector4(innerColor.R / 255f, innerColor.G / 255f, innerColor.B / 255f, innerColor.A / 255f));
            _shaderOuterColorParameter.SetValue(new Vector4(outerColor.R / 255f, outerColor.G / 255f, outerColor.B / 255f, outerColor.A / 255f));

            _pointLight = new PointLight(outerColor, Bounds.Center.ToVector2(), GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.LetterCell.RadiusAsPercentageOfWidth, intensity);
        }

        public override Rectangle Bounds { get; }
        public override List<PointLight> LightPoints => new List<PointLight> { _pointLight };

        protected override float FullIntensity => _currentIntensity;

        private readonly UiFloatingText _floatingText;
        private readonly Texture2D _texture;
        private readonly Effect _shader;
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