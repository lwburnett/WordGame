using System;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class UiToggleSwitch : IUiElement
    {
        public UiToggleSwitch(Rectangle iBounds, bool iInitialValue, Action<bool> iOnToggleCallback)
        {
            _currentValue = iInitialValue;
            _onToggleCallback = iOnToggleCallback;

            _onButton = new UiTextButton(iBounds, "On", () => OnToggle(false));
            _offButton = new UiTextButton(iBounds, "Off", () => OnToggle(true));
        }

        public void Update(GameTime iGameTime)
        {
            if (_currentValue)
                _onButton.Update(iGameTime);
            else
                _offButton.Update(iGameTime);
        }

        public void Draw()
        {
            if (_currentValue)
                _onButton.Draw();
            else
                _offButton.Draw();
        }

        private bool _currentValue;
        private readonly Action<bool> _onToggleCallback;

        private readonly UiTextButton _onButton;
        private readonly UiTextButton _offButton;

        private void OnToggle(bool iNewValue)
        {
            _currentValue = iNewValue;
            _onToggleCallback(iNewValue);
        }
    }
}