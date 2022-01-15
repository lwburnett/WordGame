using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace WordGame_Lib.Ui
{
    public class UiTextButton : IUiElement
    {
        public UiTextButton(Rectangle iBounds, string iText, Action iOnClickedCallback)
        {
            _topLeft = new Point(iBounds.X, iBounds.Y);
            _width = iBounds.Width;
            _height = iBounds.Height;
            _text = iText;
            _disposition = Disposition.Undecided;
            _onClickedCallback = iOnClickedCallback;

            var dataSize = _width * _height;
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

            _undecidedDefaultTexture = GraphicsHelper.CreateTexture(colorData1, _width, _height);
            _undecidedOverLapTexture = GraphicsHelper.CreateTexture(colorData2, _width, _height);
            _undecidedPressedTexture = GraphicsHelper.CreateTexture(colorData3, _width, _height);

            _incorrectDefaultTexture = GraphicsHelper.CreateTexture(colorData4, _width, _height);
            _incorrectOverLapTexture = GraphicsHelper.CreateTexture(colorData5, _width, _height);
            _incorrectPressedTexture = GraphicsHelper.CreateTexture(colorData6, _width, _height);

            _misplacedDefaultTexture = GraphicsHelper.CreateTexture(colorData7, _width, _height);
            _misplacedOverLapTexture = GraphicsHelper.CreateTexture(colorData8, _width, _height);
            _misplacedPressedTexture = GraphicsHelper.CreateTexture(colorData9, _width, _height);

            _correctDefaultTexture = GraphicsHelper.CreateTexture(colorData10, _width, _height);
            _correctOverLapTexture = GraphicsHelper.CreateTexture(colorData11, _width, _height);
            _correctPressedTexture = GraphicsHelper.CreateTexture(colorData12, _width, _height);

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");

            Reset();
        }

        public UiTextButton(Point iTopLeft, int iWidth, int iHeight, string iText, Action iOnClickedCallback) : 
            this(new Rectangle(iTopLeft.X, iTopLeft.Y, iWidth, iHeight), iText, iOnClickedCallback)
        {
        }

        public void Update(GameTime iGameTime)
        {
            if (PlatformUtilsHelper.GetIsMouseInput())
            {
                if (IsOverlappingWithMouse(Mouse.GetState().Position))
                {
                    OnOverlap();

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        OnPressed();
                    else if (Mouse.GetState().LeftButton == ButtonState.Released)
                        OnReleased();
                }
                else
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Released)
                    {
                        Reset();
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Released)
                    {
                        OnNotOverlap();
                    }
                }
            }
            else
            {
                var touchState = TouchPanel.GetState();
                if (!touchState.Any())
                {
                    OnNoTouch();
                    return;
                }

                var firstTouch = touchState[0];
                if (IsOverlappingWithMouse(firstTouch.Position.ToPoint()))
                {
                    OnOverlappingTouch();
                }
                else
                {
                    OnNotOverlappingTouch();
                }
            }
        }

        public void Draw()
        {
            void DoDraw(Texture2D iTexture) =>
                GraphicsHelper.DrawTexture(iTexture, _topLeft.ToVector2());

            if (!_isOverlapped && !_isPressed)
            {
                switch (_disposition)
                {
                    case Disposition.Undecided:
                        DoDraw(_undecidedDefaultTexture);
                        break;
                    case Disposition.Incorrect:
                        DoDraw(_incorrectDefaultTexture);
                        break;
                    case Disposition.Misplaced:
                        DoDraw(_misplacedDefaultTexture);
                        break;
                    case Disposition.Correct:
                        DoDraw(_correctDefaultTexture);
                        break;
                    default:
                        Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                        break;
                }
            }
            else if (_isOverlapped && !_isPressed)
            {
                switch (_disposition)
                {
                    case Disposition.Undecided:
                        DoDraw(_undecidedOverLapTexture);
                        break;
                    case Disposition.Incorrect:
                        DoDraw(_incorrectOverLapTexture);
                        break;
                    case Disposition.Misplaced:
                        DoDraw(_misplacedOverLapTexture);
                        break;
                    case Disposition.Correct:
                        DoDraw(_correctOverLapTexture);
                        break;
                    default:
                        Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                        break;
                }
            }
            else
            {
                switch (_disposition)
                {
                    case Disposition.Undecided:
                        DoDraw(_undecidedPressedTexture);
                        break;
                    case Disposition.Incorrect:
                        DoDraw(_incorrectPressedTexture);
                        break;
                    case Disposition.Misplaced:
                        DoDraw(_misplacedPressedTexture);
                        break;
                    case Disposition.Correct:
                        DoDraw(_correctPressedTexture);
                        break;
                    default:
                        Debug.Fail($"Unknown value of enum {nameof(Disposition)}: {_disposition}");
                        break;
                }}

            const float scaling = 1.0f;
            var stringDimensions = _textFont.MeasureString(_text) * scaling;
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(_topLeft.X + (_width - stringDimensions.X) / 2f, _topLeft.Y + (_height - stringDimensions.Y) / 2f), Color.White);
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

        private Point _topLeft;
        private readonly int _width;
        private readonly int _height;
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

        private bool _isPressed;
        private bool _isOverlapped;
        private readonly Action _onClickedCallback;

        private bool IsOverlappingWithMouse(Point iPosition)
        {
            return iPosition.X > _topLeft.X &&
                   iPosition.X < _topLeft.X + _width &&
                   iPosition.Y > _topLeft.Y &&
                   iPosition.Y < _topLeft.Y + _height;
        }

        private void OnPressed()
        {
            _isPressed = true;
        }

        private void OnReleased()
        {
            if (_isPressed && _isOverlapped)
                _onClickedCallback();

            _isPressed = false;
        }

        private void Reset()
        {
            _isPressed = false;
            _isOverlapped = false;
        }

        private void OnOverlap()
        {
            _isOverlapped = true;
        }

        private void OnNotOverlap()
        {
            _isOverlapped = false;
        }

        private void OnNoTouch()
        {
            if (_isPressed && _isOverlapped)
                _onClickedCallback();

            Reset();
        }

        private void OnOverlappingTouch()
        {
            _isOverlapped = true;
            _isPressed = true;
        }

        private void OnNotOverlappingTouch()
        {
            Reset();
        }
    }
}