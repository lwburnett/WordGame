using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class UiToggleSwitch : INeonUiElement
    {
        public UiToggleSwitch(Rectangle iBounds, bool iInitialValue, Action<bool> iOnToggleCallback)
        {
            _currentValue = iInitialValue;
            _onToggleCallback = iOnToggleCallback;

            _onButton = new UiNeonSpriteButton(iBounds, Path.Combine("Textures", "CheckMark"), SettingsManager.MainMenuSettings.StartButtonColor, () => OnToggle(false));
            _offButton = new UiNeonSpriteButton(iBounds, Path.Combine("Textures", "ExMark"), SettingsManager.MainMenuSettings.ExitButtonColor, () => OnToggle(true));

            LightPoints = new List<PointLight>();
            LightPoints.AddRange(_currentValue ? _onButton.LightPoints : _offButton.LightPoints);
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

        public List<PointLight> LightPoints { get; }

        private bool _currentValue;
        private readonly Action<bool> _onToggleCallback;

        private readonly INeonUiElement _onButton;
        private readonly INeonUiElement _offButton;

        private void OnToggle(bool iNewValue)
        {
            _currentValue = iNewValue;
            LightPoints.Clear();
            LightPoints.AddRange(_currentValue ? _onButton.LightPoints : _offButton.LightPoints);
            _onToggleCallback(iNewValue);
        }
    }
}