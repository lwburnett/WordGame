using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace WordGame_Lib.Ui
{
    public abstract class UiButtonBase : IUiElement
    {
        protected UiButtonBase(Action iOnClickedCallback)
        {
            _onClickedCallback = iOnClickedCallback;
        }

        public virtual void Update(GameTime iGameTime)
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

        public virtual void Draw()
        {
            void DoDraw(Texture2D iTexture)
            {
                if (iTexture != null)
                    GraphicsHelper.DrawTexture(iTexture, new Vector2(Bounds.X, Bounds.Y));
            }

            if (!_isOverlapped && !_isPressed)
            {
                DoDraw(GetDefaultTexture());
            }
            else if (_isOverlapped && !_isPressed)
            {
                DoDraw(GetHoverTexture());
            }
            else
            {
                DoDraw(GetPressedTexture());
            }
        }

        protected abstract Rectangle Bounds { get; }
        protected abstract Texture2D GetDefaultTexture();
        protected abstract Texture2D GetHoverTexture();
        protected abstract Texture2D GetPressedTexture();

        private readonly Action _onClickedCallback; 
        private bool _isPressed;
        private bool _isOverlapped;

        private bool IsOverlappingWithMouse(Point iPosition)
        {
            return iPosition.X > Bounds.X &&
                   iPosition.X < Bounds.X + Bounds.Width &&
                   iPosition.Y > Bounds.Y &&
                   iPosition.Y < Bounds.Y + Bounds.Height;
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