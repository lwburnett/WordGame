using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace WordGame_Lib.Ui
{
    public class UiTextButton : UiButtonBase
    {
        public UiTextButton(Rectangle iBounds, string iText, Action iOnClickedCallback) :
            base(iBounds, iOnClickedCallback)
        {
            _text = iText;
            _disposition = Disposition.Undecided;

            var dataSize = Bounds.Width * Bounds.Height;
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
                colorData1[ii] = SettingsManager.ColorSettings.UndecidedDefaultColor;
                colorData2[ii] = SettingsManager.ColorSettings.UndecidedHoverColor;
                colorData3[ii] = SettingsManager.ColorSettings.UndecidedPressedColor;

                colorData4[ii] = SettingsManager.ColorSettings.IncorrectDefaultColor;
                colorData5[ii] = SettingsManager.ColorSettings.IncorrectHoverColor;
                colorData6[ii] = SettingsManager.ColorSettings.IncorrectPressedColor;

                colorData7[ii] = SettingsManager.ColorSettings.MisplacedDefaultColor;
                colorData8[ii] = SettingsManager.ColorSettings.MisplacedHoverColor;
                colorData9[ii] = SettingsManager.ColorSettings.MisplacedPressedColor;

                colorData10[ii] = SettingsManager.ColorSettings.CorrectDefaultColor;
                colorData11[ii] = SettingsManager.ColorSettings.CorrectHoverColor;
                colorData12[ii] = SettingsManager.ColorSettings.CorrectPressedColor;
            }

            _undecidedDefaultTexture = GraphicsHelper.CreateTexture(colorData1, Bounds.Width, Bounds.Height);
            _undecidedOverLapTexture = GraphicsHelper.CreateTexture(colorData2, Bounds.Width, Bounds.Height);
            _undecidedPressedTexture = GraphicsHelper.CreateTexture(colorData3, Bounds.Width, Bounds.Height);

            _incorrectDefaultTexture = GraphicsHelper.CreateTexture(colorData4, Bounds.Width, Bounds.Height);
            _incorrectOverLapTexture = GraphicsHelper.CreateTexture(colorData5, Bounds.Width, Bounds.Height);
            _incorrectPressedTexture = GraphicsHelper.CreateTexture(colorData6, Bounds.Width, Bounds.Height);

            _misplacedDefaultTexture = GraphicsHelper.CreateTexture(colorData7, Bounds.Width, Bounds.Height);
            _misplacedOverLapTexture = GraphicsHelper.CreateTexture(colorData8, Bounds.Width, Bounds.Height);
            _misplacedPressedTexture = GraphicsHelper.CreateTexture(colorData9, Bounds.Width, Bounds.Height);

            _correctDefaultTexture = GraphicsHelper.CreateTexture(colorData10, Bounds.Width, Bounds.Height);
            _correctOverLapTexture = GraphicsHelper.CreateTexture(colorData11, Bounds.Width, Bounds.Height);
            _correctPressedTexture = GraphicsHelper.CreateTexture(colorData12, Bounds.Width, Bounds.Height);

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
        }

        public UiTextButton(Point iTopLeft, int iWidth, int iHeight, string iText, Action iOnClickedCallback) : 
            this(new Rectangle(iTopLeft.X, iTopLeft.Y, iWidth, iHeight), iText, iOnClickedCallback)
        {
        }

        public override void Draw()
        {
            base.Draw();

            const float scaling = 1.0f;
            var stringDimensions = _textFont.MeasureString(_text) * scaling;
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X + (Bounds.Width - stringDimensions.X) / 2f, Bounds.Y + (Bounds.Height - stringDimensions.Y) / 2f), Color.White);
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