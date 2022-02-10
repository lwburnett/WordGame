using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class UiValuePicker : IUiNeonElement
    {
        public UiValuePicker(Rectangle iBounds, int iInitialValue, Action<int> iOnValueChangedCallback, int iLowerBound = int.MinValue, int iUpperBound = int.MaxValue)
        {
            _value = iInitialValue;
            _lowerBound = iLowerBound;
            _upperBound = iUpperBound;
            _onValueChangedCallback = iOnValueChangedCallback;

            var cellWidth = iBounds.Width / 3;

            var incrementBound = new Rectangle(iBounds.X, iBounds.Y, cellWidth, iBounds.Height);
            _decrementButton = new UiNeonSpriteButton(incrementBound, Path.Combine("Textures", "ArrowLeft"), Color.Orange, iGt => OnChangeValue(false));

            var valueBounds = new Rectangle(iBounds.X + cellWidth, iBounds.Y, cellWidth, iBounds.Height);
            _valueText = new UiFloatingText(valueBounds, _value.ToString(), Color.White, Color.Black);

            var decrementBounds = new Rectangle(iBounds.X + 2 * cellWidth, iBounds.Y, cellWidth, iBounds.Height);
            _incrementButton = new UiNeonSpriteButton(decrementBounds, Path.Combine("Textures", "ArrowRight"), Color.Orange, iGt => OnChangeValue(true));

            LightPoints = new List<PointLight>();
            LightPoints.AddRange(_incrementButton.LightPoints);
            LightPoints.AddRange(_decrementButton.LightPoints);
        }

        public void Update(GameTime iGameTime)
        {
            _incrementButton.Update(iGameTime);
            _valueText.Update(iGameTime);
            _decrementButton.Update(iGameTime);

            LightPoints.Clear();
            LightPoints.AddRange(_incrementButton.LightPoints);
            LightPoints.AddRange(_decrementButton.LightPoints);
        }

        public void Draw(Vector2? iOffset = null)
        {
            _incrementButton.Draw(iOffset);
            _valueText.Draw(iOffset);
            _decrementButton.Draw(iOffset);
        }

        public List<PointLight> LightPoints { get; }
        public NeonLightState State => _incrementButton.State;
        public void StartFadeIn(GameTime iGameTime, TimeSpan iDuration)
        {
            _incrementButton.StartFadeIn(iGameTime, iDuration);
            _decrementButton.StartFadeIn(iGameTime, iDuration);
        }

        public void StartFadeOut(GameTime iGameTime, TimeSpan iDuration)
        {
            _incrementButton.StartFadeOut(iGameTime, iDuration);
            _decrementButton.StartFadeOut(iGameTime, iDuration);
        }
        
        private int _value;
        private readonly int _lowerBound;
        private readonly int _upperBound;
        private readonly Action<int> _onValueChangedCallback;

        private readonly IUiNeonElement _incrementButton;
        private readonly UiFloatingText _valueText;
        private readonly IUiNeonElement _decrementButton;

        private void OnChangeValue(bool iIncrement)
        {
            if (iIncrement && _value < _upperBound)
            {
                _value++;
                _valueText.SetText(_value.ToString());
                _onValueChangedCallback(_value);
            }

            if (!iIncrement && _value > _lowerBound)
            {
                _value--;
                _valueText.SetText(_value.ToString());
                _onValueChangedCallback(_value);
            }
        }
    }
}
