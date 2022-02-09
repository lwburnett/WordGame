using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WordGame_Lib.Ui
{
    public class UiToggleSwitch : IUiNeonElement
    {
        public UiToggleSwitch(Rectangle iBounds, bool iInitialValue, Action<bool> iOnToggleCallback)
        {
            _currentValue = iInitialValue;
            _onToggleCallback = iOnToggleCallback;

            _onButton = new UiNeonSpriteButton(iBounds, Path.Combine("Textures", "CheckMark"), SettingsManager.MainMenuSettings.StartButtonColor, iGt => OnToggle(false));
            _offButton = new UiNeonSpriteButton(iBounds, Path.Combine("Textures", "ExMark"), SettingsManager.MainMenuSettings.ExitButtonColor, iGt => OnToggle(true));

            _rng = new Random();
            _flickerSfx = new List<SoundEffect>
            {
                GraphicsHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker1")),
                GraphicsHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker2")),
                GraphicsHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker3")),
                GraphicsHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker4")),
            };

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

        public void Draw(Vector2? iOffset = null)
        {
            if (_currentValue)
                _onButton.Draw(iOffset);
            else
                _offButton.Draw(iOffset);
        }
        public void StartFadeIn(GameTime iGameTime, TimeSpan iDuration)
        {
            _onButton.StartFadeIn(iGameTime, iDuration);
            _offButton.StartFadeIn(iGameTime, iDuration);
        }

        public void StartFadeOut(GameTime iGameTime, TimeSpan iDuration)
        {
            _onButton.StartFadeOut(iGameTime, iDuration);
            _offButton.StartFadeOut(iGameTime, iDuration);
        }

        public NeonLightState State => _currentValue ? _onButton.State : _offButton.State;

        public List<PointLight> LightPoints { get; }

        private bool _currentValue;
        private readonly Action<bool> _onToggleCallback;

        private readonly IUiNeonElement _onButton;
        private readonly IUiNeonElement _offButton;
        private readonly Random _rng;
        private readonly List<SoundEffect> _flickerSfx;

        private void OnToggle(bool iNewValue)
        {
            _currentValue = iNewValue;
            LightPoints.Clear();
            LightPoints.AddRange(_currentValue ? _onButton.LightPoints : _offButton.LightPoints);
            _onToggleCallback(iNewValue);
            
            AudioHelper.PlaySoundEffect(_flickerSfx[_rng.Next(4)], SettingsManager.Sound.FlickerVolume);
        }
    }
}