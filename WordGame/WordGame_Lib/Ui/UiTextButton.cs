using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WordGame_Lib.Ui
{
    public class UiTextButton : IUiElement
    {
        public UiTextButton(Point iTopLeft, int iWidth, int iHeight, string iText, Action iOnClickedCallback)
        {
            _topLeft = iTopLeft;
            _width = iWidth;
            _height = iHeight;
            _text = iText;
            _onClickedCallback = iOnClickedCallback;

            var dataSize = _width * _height;
            var colorData1 = new Color[dataSize];
            var colorData2 = new Color[dataSize];
            var colorData3 = new Color[dataSize];
            for (var ii = 0; ii < dataSize; ii++)
            {
                colorData1[ii] = Color.LightGray;
                colorData2[ii] = Color.DarkGray;
                colorData3[ii] = Color.Gray;
            }

            _defaultTexture = GraphicsHelper.CreateTexture(colorData1, _width, _height);
            _overLapTexture = GraphicsHelper.CreateTexture(colorData2, _width, _height);
            _pressedTexture = GraphicsHelper.CreateTexture(colorData3, _width, _height);

            _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");

            Reset();
        }

        public bool IsOverlappingWithMouse(Point iPosition)
        {
            return iPosition.X > _topLeft.X &&
                   iPosition.X < _topLeft.X + _width &&
                   iPosition.Y > _topLeft.Y &&
                   iPosition.Y < _topLeft.Y + _height;
        }

        public void OnPressed()
        {
            _isPressed = true;
        }

        public void OnReleased()
        {
            if (_isPressed && _isOverlapped)
                _onClickedCallback();

            _isPressed = false;
        }

        public void Reset()
        {
            _isPressed = false;
            _isOverlapped = false;
        }

        public void OnOverlap()
        {
            _isOverlapped = true;
        }

        public void OnNotOverlap()
        {
            _isOverlapped = false;
        }

        public void Update(GameTime iGameTime)
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

        public void Draw()
        {
            if (!_isOverlapped && !_isPressed)
                GraphicsHelper.DrawTexture(_defaultTexture, _topLeft.ToVector2());
            else if (_isOverlapped && !_isPressed)
                GraphicsHelper.DrawTexture(_overLapTexture, _topLeft.ToVector2());
            else
                GraphicsHelper.DrawTexture(_pressedTexture, _topLeft.ToVector2());

            const float scaling = 1.0f;
            var stringDimensions = _textFont.MeasureString(_text) * scaling;
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(_topLeft.X + (_width - stringDimensions.X) / 2f, _topLeft.Y + (_height - stringDimensions.Y) / 2f), Color.Black);
        }

        private Point _topLeft;
        private readonly int _width;
        private readonly int _height;
        private readonly string _text;
        private readonly Texture2D _defaultTexture;
        private readonly Texture2D _overLapTexture;
        private readonly Texture2D _pressedTexture;
        private readonly SpriteFont _textFont;

        private bool _isPressed;
        private bool _isOverlapped;

        private readonly Action _onClickedCallback;
    }
}