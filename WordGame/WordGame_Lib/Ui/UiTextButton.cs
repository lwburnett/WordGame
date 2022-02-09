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

            _disposition = Disposition.Undecided;
            _floatingText = new UiFloatingText(iBounds, iText, Color.White, Color.Black);

            _texture = AssetHelper.LoadContent<Texture2D>(Path.Combine("Textures", "KeyboardKey"));
            _shader = AssetHelper.LoadContent<Effect>(Path.Combine("Shaders", "KeyboardKeyShader")).Clone();
            _shaderColorParameter = _shader.Parameters["IntendedColor"];
        }

        public UiTextButton(Point iTopLeft, int iWidth, int iHeight, string iText, Action<GameTime> iOnClickedCallback) : 
            this(new Rectangle(iTopLeft.X, iTopLeft.Y, iWidth, iHeight), iText, iOnClickedCallback)
        {
        }

        public override void Draw(Vector2? iOffset = null)
        {
            //var color = new Vector4(GetColor().ToVector3() / 255f, 1.0f);
            _shaderColorParameter.SetValue(GetColor().ToVector4());
            GraphicsHelper.DrawTexture(_texture, Bounds, _shader, iOffset);

            _floatingText.Draw(iOffset);
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
                        return GameSettingsManager.Settings.AlternateKeyColorScheme ? SettingsManager.UiKeyboardColors.IncorrectHoverColor : SettingsManager.UiKeyboardColors.UndecidedHoverColor;
                    case Disposition.Incorrect:
                        return GameSettingsManager.Settings.AlternateKeyColorScheme ? SettingsManager.UiKeyboardColors.UndecidedHoverColor : SettingsManager.UiKeyboardColors.IncorrectHoverColor;
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
                        return GameSettingsManager.Settings.AlternateKeyColorScheme ? SettingsManager.UiKeyboardColors.IncorrectPressedColor : SettingsManager.UiKeyboardColors.UndecidedPressedColor;
                    case Disposition.Incorrect:
                        return GameSettingsManager.Settings.AlternateKeyColorScheme ? SettingsManager.UiKeyboardColors.UndecidedPressedColor : SettingsManager.UiKeyboardColors.IncorrectPressedColor;
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