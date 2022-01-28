using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiTextButton : UiButtonBase
    {
        public UiTextButton(Rectangle iBounds, string iText, Action iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            Bounds = iBounds;

            _text = iText;
            _disposition = Disposition.Undecided;

            var dataSize = iBounds.Width * iBounds.Height;
            var colorData1 = new Color[dataSize];
            var colorData2 = new Color[dataSize];
            var colorData3 = new Color[dataSize];
            var colorData4 = new Color[dataSize];
            var colorData5 = new Color[dataSize];
            var colorData6 = new Color[dataSize];
            var colorData7 = new Color[dataSize];
            var colorData8 = new Color[dataSize];
            var colorData9 = new Color[dataSize];
            var colorData10 = new Color[dataSize];
            var colorData11 = new Color[dataSize];
            var colorData12 = new Color[dataSize];
            for (var ii = 0; ii < dataSize; ii++)
            {
                colorData1[ii] = SettingsManager.UiKeyboardColors.UndecidedDefaultColor;
                colorData2[ii] = SettingsManager.UiKeyboardColors.UndecidedHoverColor;
                colorData3[ii] = SettingsManager.UiKeyboardColors.UndecidedPressedColor;

                colorData4[ii] = SettingsManager.UiKeyboardColors.IncorrectDefaultColor;
                colorData5[ii] = SettingsManager.UiKeyboardColors.IncorrectHoverColor;
                colorData6[ii] = SettingsManager.UiKeyboardColors.IncorrectPressedColor;

                colorData7[ii] = SettingsManager.UiKeyboardColors.MisplacedDefaultColor;
                colorData8[ii] = SettingsManager.UiKeyboardColors.MisplacedHoverColor;
                colorData9[ii] = SettingsManager.UiKeyboardColors.MisplacedPressedColor;

                colorData10[ii] = SettingsManager.UiKeyboardColors.CorrectDefaultColor;
                colorData11[ii] = SettingsManager.UiKeyboardColors.CorrectHoverColor;
                colorData12[ii] = SettingsManager.UiKeyboardColors.CorrectPressedColor;
            }

            if (!GameSettingsManager.Settings.AlternateKeyColorScheme)
            {
                _undecidedDefaultTexture = GraphicsHelper.CreateTexture(colorData1, iBounds.Width, iBounds.Height);
                _undecidedOverLapTexture = GraphicsHelper.CreateTexture(colorData2, iBounds.Width, iBounds.Height);
                _undecidedPressedTexture = GraphicsHelper.CreateTexture(colorData3, iBounds.Width, iBounds.Height);

                _incorrectDefaultTexture = GraphicsHelper.CreateTexture(colorData4, iBounds.Width, iBounds.Height);
                _incorrectOverLapTexture = GraphicsHelper.CreateTexture(colorData5, iBounds.Width, iBounds.Height);
                _incorrectPressedTexture = GraphicsHelper.CreateTexture(colorData6, iBounds.Width, iBounds.Height);
            }
            else
            {
                _undecidedDefaultTexture = GraphicsHelper.CreateTexture(colorData4, iBounds.Width, iBounds.Height);
                _undecidedOverLapTexture = GraphicsHelper.CreateTexture(colorData5, iBounds.Width, iBounds.Height);
                _undecidedPressedTexture = GraphicsHelper.CreateTexture(colorData6, iBounds.Width, iBounds.Height);

                _incorrectDefaultTexture = GraphicsHelper.CreateTexture(colorData1, iBounds.Width, iBounds.Height);
                _incorrectOverLapTexture = GraphicsHelper.CreateTexture(colorData2, iBounds.Width, iBounds.Height);
                _incorrectPressedTexture = GraphicsHelper.CreateTexture(colorData3, iBounds.Width, iBounds.Height);
            }

            _misplacedDefaultTexture = GraphicsHelper.CreateTexture(colorData7, iBounds.Width, iBounds.Height);
            _misplacedOverLapTexture = GraphicsHelper.CreateTexture(colorData8, iBounds.Width, iBounds.Height);
            _misplacedPressedTexture = GraphicsHelper.CreateTexture(colorData9, iBounds.Width, iBounds.Height);

            _correctDefaultTexture = GraphicsHelper.CreateTexture(colorData10, iBounds.Width, iBounds.Height);
            _correctOverLapTexture = GraphicsHelper.CreateTexture(colorData11, iBounds.Width, iBounds.Height);
            _correctPressedTexture = GraphicsHelper.CreateTexture(colorData12, iBounds.Width, iBounds.Height);

            _textFont = GraphicsHelper.LoadContent<SpriteFont>(Path.Combine("Fonts", "PrototypeFont"));
        }

        public UiTextButton(Point iTopLeft, int iWidth, int iHeight, string iText, Action iOnClickedCallback) : 
            this(new Rectangle(iTopLeft.X, iTopLeft.Y, iWidth, iHeight), iText, iOnClickedCallback)
        {
        }

        public override void Draw()
        {
            base.Draw();

            const float offsetScalar = 1.1f;
            const float scaling = 1.0f;
            var stringDimensions = _textFont.MeasureString(_text) * scaling;
            var pos = new Vector2(Bounds.X + (Bounds.Width - stringDimensions.X) / 2f, Bounds.Y + (Bounds.Height - stringDimensions.Y) / 2f);
            GraphicsHelper.DrawString(_textFont, _text, pos + new Vector2(-offsetScalar * scaling, -offsetScalar * scaling), Color.Black);
            GraphicsHelper.DrawString(_textFont, _text, pos + new Vector2(offsetScalar * scaling, -offsetScalar * scaling), Color.Black);
            GraphicsHelper.DrawString(_textFont, _text, pos + new Vector2(-offsetScalar * scaling, offsetScalar * scaling), Color.Black);
            GraphicsHelper.DrawString(_textFont, _text, pos + new Vector2(offsetScalar * scaling, offsetScalar * scaling), Color.Black);
            GraphicsHelper.DrawString(_textFont, _text, pos, Color.White);
        }

        public string GetText()
        {
            return _text;
        }

        public Disposition GetDisposition()
        {
            return _disposition;
        }

        public void SetDisposition(Disposition iDisposition)
        {
            _disposition = iDisposition;
        }

        protected override Rectangle Bounds { get; }

        protected override Texture2D GetDefaultTexture()
        {
            switch (_disposition)
            {
                case Disposition.Undecided:
                    return _undecidedDefaultTexture;
                case Disposition.Incorrect:
                    return _incorrectDefaultTexture;
                case Disposition.Misplaced:
                    return _misplacedDefaultTexture;
                case Disposition.Correct:
                    return _correctDefaultTexture;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                    return _undecidedDefaultTexture;
            }
        }

        protected override Texture2D GetHoverTexture()
        {
            switch (_disposition)
            {
                case Disposition.Undecided:
                    return _undecidedOverLapTexture;
                case Disposition.Incorrect:
                    return _incorrectOverLapTexture;
                case Disposition.Misplaced:
                    return _misplacedOverLapTexture;
                case Disposition.Correct:
                    return _correctOverLapTexture;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                    return _undecidedOverLapTexture;
            }
        }

        protected override Texture2D GetPressedTexture()
        {
            switch (_disposition)
            {
                case Disposition.Undecided:
                    return _undecidedPressedTexture;
                case Disposition.Incorrect:
                    return _incorrectPressedTexture;
                case Disposition.Misplaced:
                    return _misplacedPressedTexture;
                case Disposition.Correct:
                    return _correctPressedTexture;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                    return _undecidedPressedTexture;
            }
        }

        private readonly string _text;
        private Disposition _disposition;
        private readonly Texture2D _undecidedDefaultTexture;
        private readonly Texture2D _undecidedOverLapTexture;
        private readonly Texture2D _undecidedPressedTexture;
        private readonly Texture2D _incorrectDefaultTexture;
        private readonly Texture2D _incorrectOverLapTexture;
        private readonly Texture2D _incorrectPressedTexture;
        private readonly Texture2D _misplacedDefaultTexture;
        private readonly Texture2D _misplacedOverLapTexture;
        private readonly Texture2D _misplacedPressedTexture;
        private readonly Texture2D _correctDefaultTexture;
        private readonly Texture2D _correctOverLapTexture;
        private readonly Texture2D _correctPressedTexture;
        private readonly SpriteFont _textFont;
        
    }
}