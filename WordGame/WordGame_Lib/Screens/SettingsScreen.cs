using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordGame_Lib.Ui;

namespace WordGame_Lib.Screens
{
    public class SettingsScreen : ScreenBase
    {
        public SettingsScreen(Action<GameTime> iMainMenuCallback)
        {
            _bounds = GraphicsHelper.GamePlayArea;
            _settings = GameSettingsManager.Settings;
            _mainMenuCallback = iMainMenuCallback;
            _lightPoints = new List<PointLight>();
        }

        public override void Draw(Vector2? iOffset = null)
        {
            GraphicsHelper.CalculatePointLightShaderParameters(_lightPoints, out var positions, out var colors, out var radii, out var intensity, iOffset);

            _backgroundEffect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height));
            _backgroundEffect.Parameters["PointLightPosition"].SetValue(positions);
            _backgroundEffect.Parameters["PointLightColor"].SetValue(colors);
            _backgroundEffect.Parameters["PointLightRadius"].SetValue(radii);
            _backgroundEffect.Parameters["PointLightIntensity"].SetValue(intensity);
            
            GraphicsHelper.DrawTexture(_backgroundTexture, GraphicsHelper.GamePlayArea, _backgroundEffect, iOffset);

            _header.Draw(iOffset);
            _saveButton.Draw(iOffset);
            _altColorLabel.Draw(iOffset);
            _altColorToggle.Draw(iOffset);
            _neonPulseLabel.Draw(iOffset);
            _neonPulseToggle.Draw(iOffset);
            _neonFlickerLabel.Draw(iOffset);
            _neonFlickerToggle.Draw(iOffset);
            _vibrationLabel.Draw(iOffset);
            _vibrationToggle.Draw(iOffset);
            _rainVisualLabel.Draw(iOffset);
            _rainVisualToggle.Draw(iOffset);
            _stormVolumeLabel.Draw(iOffset);
            _stormVolumePicker.Draw(iOffset);
            _musicVolumeLabel.Draw(iOffset);
            _musicVolumePicker.Draw(iOffset);
        }

        protected override void DoLoad()
        {
            _backgroundTexture = AssetHelper.LoadContent<Texture2D>(Path.Combine("Textures", "Bricks1"));
            _backgroundEffect = AssetHelper.LoadContent<Effect>(Path.Combine("Shaders", "BrickShader")).Clone();

            var bigMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);
            var medMarginY = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);

            var medMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.MediumMarginAsPercentage);

            var bigMarginX = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.GeneralVisualSettings.BigMarginAsPercentage);

            var headerYLocation = _bounds.Y + bigMarginY;
            var headerXLocation = _bounds.X + bigMarginX;
            var headerHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.SettingsScreenSettings.HeaderHeightAsPercentage);
            var headerWidth = GraphicsHelper.GamePlayArea.Width - bigMarginX - bigMarginX;
            _header = new UiNeonFloatingText(
                new Rectangle(headerXLocation, headerYLocation, headerWidth, headerHeight),
                "SETTINGS",
                Color.White);

            var settingsEditBounds = new Rectangle(
                headerXLocation,
                _bounds.Y + headerYLocation + headerHeight + medMarginY,
                _bounds.Width - 2 * bigMarginX,
                (int)(_bounds.Height * SettingsManager.SettingsScreenSettings.SettingsListHeightAsPercentage));

            var altColorLabelY = settingsEditBounds.Y;
            var altColorLabelX = settingsEditBounds.X;
            var altColorLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var altColorLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _altColorLabel = new UiFloatingText(
                new Rectangle(altColorLabelX, altColorLabelY, altColorLabelWidth, altColorLabelHeight),
                "Alternate Color Scheme",
                Color.White,
                Color.Black);

            var altColorToggleY = settingsEditBounds.Y;
            var altColorToggleX = altColorLabelX + altColorLabelWidth + medMarginX;
            var altColorToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent / 1.5f);
            var altColorToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _altColorToggle = new UiToggleSwitch(
                new Rectangle(altColorToggleX, altColorToggleY, altColorToggleWidth, altColorToggleHeight),
                _settings.AlternateKeyColorScheme,
                OnToggleAlternateColorScheme);

            var neonPulseLabelY = altColorLabelY + altColorToggleHeight + medMarginY;
            var neonPulseLabelX = settingsEditBounds.X;
            var neonPulseLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var neonPulseLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _neonPulseLabel = new UiFloatingText(
                new Rectangle(neonPulseLabelX, neonPulseLabelY, neonPulseLabelWidth, neonPulseLabelHeight),
                "Neon Light Pulse",
                Color.White,
                Color.Black);

            var neonPulseToggleY = neonPulseLabelY;
            var neonPulseToggleX = neonPulseLabelX + neonPulseLabelWidth + medMarginX;
            var neonPulseToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent / 1.5f);
            var neonPulseToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _neonPulseToggle = new UiToggleSwitch(
                new Rectangle(neonPulseToggleX, neonPulseToggleY, neonPulseToggleWidth, neonPulseToggleHeight),
                _settings.NeonLightPulse,
                OnToggleNeonLightPulse);

            var neonFlickerLabelY = neonPulseLabelY + neonPulseLabelHeight + medMarginY;
            var neonFlickerLabelX = settingsEditBounds.X;
            var neonFlickerLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var neonFlickerLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _neonFlickerLabel = new UiFloatingText(
                new Rectangle(neonFlickerLabelX, neonFlickerLabelY, neonFlickerLabelWidth, neonFlickerLabelHeight),
                "Neon Light Flicker",
                Color.White,
                Color.Black);

            var neonFlickerToggleY = neonFlickerLabelY;
            var neonFlickerToggleX = neonFlickerLabelX + neonFlickerLabelWidth + medMarginX;
            var neonFlickerToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent / 1.5f);
            var neonFlickerToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _neonFlickerToggle = new UiToggleSwitch(
                new Rectangle(neonFlickerToggleX, neonFlickerToggleY, neonFlickerToggleWidth, neonFlickerToggleHeight),
                _settings.NeonLightFlicker,
                OnToggleNeonLightFlicker);

            var vibrationLabelY = neonFlickerLabelY + neonFlickerLabelHeight + medMarginY;
            var vibrationLabelX = settingsEditBounds.X;
            var vibrationLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var vibrationLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _vibrationLabel = new UiFloatingText(
                new Rectangle(vibrationLabelX, vibrationLabelY, vibrationLabelWidth, vibrationLabelHeight),
                "Button Vibration",
                Color.White,
                Color.Black);

            var vibrationToggleY = vibrationLabelY;
            var vibrationToggleX = vibrationLabelX + vibrationLabelWidth + medMarginX;
            var vibrationToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent / 1.5f);
            var vibrationToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _vibrationToggle = new UiToggleSwitch(
                new Rectangle(vibrationToggleX, vibrationToggleY, vibrationToggleWidth, vibrationToggleHeight),
                _settings.Vibration,
                OnToggleVibration);

            var rainVisualLabelY = vibrationLabelY + vibrationLabelHeight + medMarginY;
            var rainVisualLabelX = settingsEditBounds.X;
            var rainVisualLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var rainVisualLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _rainVisualLabel = new UiFloatingText(
                new Rectangle(rainVisualLabelX, rainVisualLabelY, rainVisualLabelWidth, rainVisualLabelHeight),
                "Rain Visual",
                Color.White,
                Color.Black);

            var rainVisualToggleY = rainVisualLabelY;
            var rainVisualToggleX = rainVisualLabelX + rainVisualLabelWidth + medMarginX;
            var rainVisualToggleWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent / 1.5f);
            var rainVisualToggleHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _rainVisualToggle = new UiToggleSwitch(
                new Rectangle(rainVisualToggleX, rainVisualToggleY, rainVisualToggleWidth, rainVisualToggleHeight),
                _settings.RainVisual,
                OnToggleRainVisual);
            
            var stormVolumeLabelY = rainVisualToggleY + rainVisualToggleHeight + medMarginY;
            var stormVolumeLabelX = settingsEditBounds.X;
            var stormVolumeLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var stormVolumeLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _stormVolumeLabel = new UiFloatingText(
                new Rectangle(stormVolumeLabelX, stormVolumeLabelY, stormVolumeLabelWidth, stormVolumeLabelHeight),
                "Storm Volume",
                Color.White,
                Color.Black);

            var stormVolumePickerY = stormVolumeLabelY;
            var stormVolumePickerWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent);
            var stormVolumePickerX = stormVolumeLabelX + stormVolumeLabelWidth + medMarginX - (stormVolumePickerWidth - rainVisualToggleWidth) / 2;
            var stormVolumePickerHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _stormVolumePicker = new UiValuePicker(
                new Rectangle(stormVolumePickerX, stormVolumePickerY, stormVolumePickerWidth, stormVolumePickerHeight),
                _settings.StormVolume,
                OnChangeStormVolume,
                SettingsManager.Sound.SoundVolumeMin,
                SettingsManager.Sound.SoundVolumeMax);

            var musicVolumeLabelY = stormVolumePickerY + stormVolumePickerHeight + medMarginY;
            var musicVolumeLabelX = settingsEditBounds.X;
            var musicVolumeLabelWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.LabelColumnWidthAsPercent);
            var musicVolumeLabelHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _musicVolumeLabel = new UiFloatingText(
                new Rectangle(musicVolumeLabelX, musicVolumeLabelY, musicVolumeLabelWidth, musicVolumeLabelHeight),
                "Music Volume",
                Color.White,
                Color.Black);

            var musicVolumePickerY = musicVolumeLabelY;
            var musicVolumePickerWidth = (int)(settingsEditBounds.Width * SettingsManager.SettingsScreenSettings.SettingColumnWidthAsPercent);
            var musicVolumePickerX = musicVolumeLabelX + musicVolumeLabelWidth + medMarginX - (musicVolumePickerWidth - rainVisualToggleWidth) / 2;
            var musicVolumePickerHeight = (int)(settingsEditBounds.Height * SettingsManager.SettingsScreenSettings.IndividualSettingRowHeightAsPercent / 1.5f);
            _musicVolumePicker = new UiValuePicker(
                new Rectangle(musicVolumePickerX, musicVolumePickerY, musicVolumePickerWidth, musicVolumePickerHeight),
                _settings.MusicVolume,
                OnChangeMusicVolume,
                SettingsManager.Sound.SoundVolumeMin,
                SettingsManager.Sound.SoundVolumeMax);

            var saveWidth = (int)(GraphicsHelper.GamePlayArea.Width * SettingsManager.SettingsScreenSettings.SaveButtonWidthAsPercentage);
            var saveY = settingsEditBounds.Y + settingsEditBounds.Height + medMarginY;
            var saveX = (GraphicsHelper.GamePlayArea.Width - saveWidth) / 2;
            var saveHeight = (int)(GraphicsHelper.GamePlayArea.Height * SettingsManager.SettingsScreenSettings.SaveButtonHeightAsPercentage);
            _saveButton = new UiMenuNeonButton(
                new Rectangle(saveX, saveY, saveWidth, saveHeight),
                "SAVE",
                Color.BlueViolet,
                OnSave);
        }

        protected override bool UpdateTransitionIn(GameTime iGameTime)
        {
            if (_header.State == NeonLightState.Off)
            {
                _header.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _altColorToggle.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _neonPulseToggle.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _neonFlickerToggle.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _vibrationToggle.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _rainVisualToggle.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _stormVolumePicker.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _musicVolumePicker.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _saveButton.StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }

            UpdateUiElements(iGameTime);

            return _header.State == NeonLightState.On &&
                   _altColorToggle.State == NeonLightState.On &&
                   _neonPulseToggle.State == NeonLightState.On &&
                   _neonFlickerToggle.State == NeonLightState.On &&
                   _vibrationToggle.State == NeonLightState.On &&
                   _rainVisualToggle.State == NeonLightState.On &&
                   _stormVolumePicker.State == NeonLightState.On &&
                   _musicVolumePicker.State == NeonLightState.On &&
                   _saveButton.State == NeonLightState.On;
        }

        protected override void UpdateDefault(GameTime iGameTime)
        {
            UpdateUiElements(iGameTime);
        }

        protected override bool UpdateTransitionOut(GameTime iGameTime)
        {
            if (_header.State == NeonLightState.On)
            {
                _header.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _altColorToggle.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _neonPulseToggle.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _neonFlickerToggle.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _vibrationToggle.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _rainVisualToggle.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _stormVolumePicker.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _musicVolumePicker.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
                _saveButton.StartFadeOut(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }

            UpdateUiElements(iGameTime);

            return _header.State == NeonLightState.Off &&
                   _altColorToggle.State == NeonLightState.Off &&
                   _neonPulseToggle.State == NeonLightState.Off &&
                   _neonFlickerToggle.State == NeonLightState.Off &&
                   _vibrationToggle.State == NeonLightState.Off &&
                   _rainVisualToggle.State == NeonLightState.Off &&
                   _stormVolumePicker.State == NeonLightState.Off &&
                   _musicVolumePicker.State == NeonLightState.Off &&
                   _saveButton.State == NeonLightState.Off;
        }

        private Texture2D _backgroundTexture;
        private Effect _backgroundEffect;
        private readonly Rectangle _bounds;
        private GameSettings _settings;
        private readonly Action<GameTime> _mainMenuCallback;

        private IUiNeonElement _header;
        private IUiElement _altColorLabel;
        private IUiNeonElement _altColorToggle;
        private IUiElement _neonPulseLabel;
        private IUiNeonElement _neonPulseToggle;
        private IUiElement _neonFlickerLabel;
        private IUiNeonElement _neonFlickerToggle;
        private IUiElement _vibrationLabel;
        private IUiNeonElement _vibrationToggle;
        private IUiElement _rainVisualLabel;
        private IUiNeonElement _rainVisualToggle;
        private IUiElement _stormVolumeLabel;
        private IUiNeonElement _stormVolumePicker;
        private IUiElement _musicVolumeLabel;
        private IUiNeonElement _musicVolumePicker;

        private IUiNeonElement _saveButton;
        private readonly List<PointLight> _lightPoints;

        private void UpdateUiElements(GameTime iGameTime)
        {
            _header.Update(iGameTime);
            _saveButton.Update(iGameTime);
            _altColorLabel.Update(iGameTime);
            _altColorToggle.Update(iGameTime);
            _neonPulseLabel.Update(iGameTime);
            _neonPulseToggle.Update(iGameTime);
            _neonFlickerLabel.Update(iGameTime);
            _neonFlickerToggle.Update(iGameTime);
            _vibrationLabel.Update(iGameTime);
            _vibrationToggle.Update(iGameTime);
            _rainVisualLabel.Update(iGameTime);
            _rainVisualToggle.Update(iGameTime);
            _stormVolumeLabel.Update(iGameTime);
            _stormVolumePicker.Update(iGameTime);
            _musicVolumeLabel.Update(iGameTime);
            _musicVolumePicker.Update(iGameTime);

            _lightPoints.Clear();
            _lightPoints.AddRange(_header.LightPoints);
            _lightPoints.AddRange(_altColorToggle.LightPoints);
            _lightPoints.AddRange(_saveButton.LightPoints);
            _lightPoints.AddRange(_neonPulseToggle.LightPoints);
            _lightPoints.AddRange(_neonFlickerToggle.LightPoints);
            _lightPoints.AddRange(_vibrationToggle.LightPoints);
            _lightPoints.AddRange(_rainVisualToggle.LightPoints);
            _lightPoints.AddRange(_stormVolumePicker.LightPoints);
            _lightPoints.AddRange(_musicVolumePicker.LightPoints);
        }

        private void OnSave(GameTime iGameTime)
        {
            GameSettingsManager.UpdateSettings(_settings);
            GameSettingsManager.WriteSettingsToDiskAsync();
            _mainMenuCallback(iGameTime);
        }

        private void OnToggleAlternateColorScheme(bool iNewValue)
        {
            _settings = new GameSettings(iNewValue, _settings.NeonLightPulse, _settings.NeonLightFlicker, _settings.Vibration, _settings.RainVisual, _settings.StormVolume, _settings.MusicVolume);
        }

        private void OnToggleNeonLightPulse(bool iNewValue)
        {
            _settings = new GameSettings(_settings.AlternateKeyColorScheme, iNewValue, _settings.NeonLightFlicker, _settings.Vibration, _settings.RainVisual, _settings.StormVolume, _settings.MusicVolume);
        }

        private void OnToggleNeonLightFlicker(bool iNewValue)
        {
            _settings = new GameSettings(_settings.AlternateKeyColorScheme, _settings.NeonLightPulse, iNewValue, _settings.Vibration, _settings.RainVisual, _settings.StormVolume, _settings.MusicVolume);
        }

        private void OnToggleVibration(bool iNewValue)
        {
            _settings = new GameSettings(_settings.AlternateKeyColorScheme, _settings.NeonLightPulse, _settings.NeonLightFlicker, iNewValue, _settings.RainVisual, _settings.StormVolume, _settings.MusicVolume);
        }

        private void OnToggleRainVisual(bool iNewValue)
        {
            _settings = new GameSettings(_settings.AlternateKeyColorScheme, _settings.NeonLightPulse, _settings.NeonLightFlicker, _settings.Vibration, iNewValue, _settings.StormVolume, _settings.MusicVolume);
        }

        private void OnChangeStormVolume(int iNewValue)
        {
            AudioHelper.SetStormVolume(iNewValue);
            _settings = new GameSettings(_settings.AlternateKeyColorScheme, _settings.NeonLightPulse, _settings.NeonLightFlicker, _settings.Vibration, _settings.RainVisual, iNewValue, _settings.MusicVolume);
        }

        private void OnChangeMusicVolume(int iNewValue)
        {
            AudioHelper.SetMusicVolume(iNewValue);
            _settings = new GameSettings(_settings.AlternateKeyColorScheme, _settings.NeonLightPulse, _settings.NeonLightFlicker, _settings.Vibration, _settings.RainVisual, _settings.StormVolume, iNewValue);
        }
    }
}