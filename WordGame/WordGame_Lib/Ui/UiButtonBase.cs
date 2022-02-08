using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace WordGame_Lib.Ui
{
    public abstract class UiButtonBase : IUiElement
    {
        protected UiButtonBase(Action<GameTime> iOnClickedCallback)
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
                        OnReleased(iGameTime);
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
                    OnNoTouch(iGameTime);
                }
                else
                {
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

            if (!_isOverlapped && !_isPressed)
            {
                StateOfPress = PressState.Default;
            }
            else if (_isOverlapped && !_isPressed)
            {
                StateOfPress = PressState.Hover;
            }
            else
            {
                StateOfPress = PressState.Pressed;
            }
        }

        public abstract void Draw(Vector2? iOffset = null);

        protected abstract Rectangle Bounds { get; }

        protected enum PressState
        {
            Default,
            Hover,
            Pressed
        }

        protected PressState StateOfPress;
        private readonly Action<GameTime> _onClickedCallback; 
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
            if (GameSettingsManager.Settings.Vibration)
                PlatformUtilsHelper.VibrateDevice(SettingsManager.Sound.VibrationDuration);
        }

        private void OnReleased(GameTime iGameTime)
        {
            if (_isPressed && _isOverlapped)
            {
                _onClickedCallback(iGameTime);
                if (GameSettingsManager.Settings.Vibration)
                    PlatformUtilsHelper.VibrateDevice(SettingsManager.Sound.VibrationDuration);
            }

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

        private void OnNoTouch(GameTime iGameTime)
        {
            if (_isPressed && _isOverlapped)
            {
                _onClickedCallback(iGameTime);
                if (GameSettingsManager.Settings.Vibration)
                    PlatformUtilsHelper.VibrateDevice(SettingsManager.Sound.VibrationDuration);
            }

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