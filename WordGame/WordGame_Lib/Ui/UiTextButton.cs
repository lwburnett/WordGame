using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiTextButton : UiButtonBase
    {
        public UiTextButton(Rectangle iBounds, string iText, Action<GameTime> iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            Bounds = iBounds;
            _text = iText;

            _disposition = Disposition.Undecided;
            _floatingText = new UiFloatingText(iBounds, iText, Color.White, Color.Black);

            _texture = GraphicsHelper.LoadContent<Texture2D>(Path.Combine("Textures", "KeyboardKey"));
            _shader = GraphicsHelper.LoadContent<Effect>(Path.Combine("Shaders", "KeyboardKeyShader")).Clone();
            _shaderColorParameter = _shader.Parameters["IntendedColor"];
        }

        public UiTextButton(Point iTopLeft, int iWidth, int iHeight, string iText, Action<GameTime> iOnClickedCallback) : 
            this(new Rectangle(iTopLeft.X, iTopLeft.Y, iWidth, iHeight), iText, iOnClickedCallback)
        {
        }

        public override void Update(GameTime iGameTime)
        {
            base.Update(iGameTime);
        }

        public override void Draw()
        {
            //var color = new Vector4(GetColor().ToVector3() / 255f, 1.0f);
            _shaderColorParameter.SetValue(GetColor().ToVector4());
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader);

            _floatingText.Draw();
        }

        public string GetText()
        {
            return _floatingText.GetText();
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

        private readonly string _text;
        private readonly UiFloatingText _floatingText;
        private Disposition _disposition;
        private readonly Texture2D _texture;
        private readonly Effect _shader;
        private readonly EffectParameter _shaderColorParameter;

        private Color GetColor()
        {
            if (StateOfPress == PressState.Default)
            {
                switch (_disposition)
                {
                    case Disposition.Undecided:
                        return GameSettingsManager.Settings.AlternateKeyColorScheme ? SettingsManager.UiKeyboardColors.IncorrectDefaultColor : SettingsManager.UiKeyboardColors.UndecidedDefaultColor;
                    case Disposition.Incorrect:
                        return GameSettingsManager.Settings.AlternateKeyColorScheme ? SettingsManager.UiKeyboardColors.UndecidedDefaultColor : SettingsManager.UiKeyboardColors.IncorrectDefaultColor;
                    case Disposition.Misplaced:
                        return SettingsManager.UiKeyboardColors.MisplacedDefaultColor;
                    case Disposition.Correct:
                        return SettingsManager.UiKeyboardColors.CorrectDefaultColor;
                    default:
                        Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                        return SettingsManager.UiKeyboardColors.UndecidedDefaultColor;
                }
            }
            else if (StateOfPress == PressState.Hover)
            {
                switch (_disposition)
                {
                    case Disposition.Undecided:
                        return SettingsManager.UiKeyboardColors.UndecidedHoverColor;
                    case Disposition.Incorrect:
                        return SettingsManager.UiKeyboardColors.IncorrectHoverColor;
                    case Disposition.Misplaced:
                        return SettingsManager.UiKeyboardColors.MisplacedHoverColor;
                    case Disposition.Correct:
                        return SettingsManager.UiKeyboardColors.CorrectHoverColor;
                    default:
                        Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                        return SettingsManager.UiKeyboardColors.UndecidedHoverColor;
                }
            }
            else if (StateOfPress == PressState.Pressed)
            {
                switch (_disposition)
                {
                    case Disposition.Undecided:
                        return SettingsManager.UiKeyboardColors.UndecidedPressedColor;
                    case Disposition.Incorrect:
                        return SettingsManager.UiKeyboardColors.IncorrectPressedColor;
                    case Disposition.Misplaced:
                        return SettingsManager.UiKeyboardColors.MisplacedPressedColor;
                    case Disposition.Correct:
                        return SettingsManager.UiKeyboardColors.CorrectPressedColor;
                    default:
                        Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                        return SettingsManager.UiKeyboardColors.UndecidedPressedColor;
                }
            }
            else
            {
                Debug.Fail($"Unknown value of enum {nameof(PressState)}: {StateOfPress}");
                return SettingsManager.UiKeyboardColors.UndecidedDefaultColor;
            }
        }
    }
}