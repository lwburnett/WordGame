using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public abstract class UiNeonElementBase : IUiNeonElement
    {
        protected UiNeonElementBase(double? iPulseOffsetLerpValue)
        {
            var pulseLerpValue = iPulseOffsetLerpValue ?? new Random().NextDouble();
            _pulseOffset = SettingsManager.NeonSettings.PulsePeriodSec * pulseLerpValue;
            _rng = new Random();
            sIsFlickeringThisTick = false;
        }

        public virtual void Update(GameTime iGameTime)
        {
            if (GameSettingsManager.Settings.NeonLightPulse)
            {
                const double pi2 = Math.PI * 2;
                const float period = SettingsManager.NeonSettings.PulsePeriodSec;
                const float amp = SettingsManager.NeonSettings.PulseIntensityAmplitude;

                var gameTimeSeconds = (float)iGameTime.TotalGameTime.TotalSeconds;
                var multiplier = amp * (float)Math.Cos((gameTimeSeconds * pi2 / period) + _pulseOffset) + (1 - amp);

                LightPoints.ForEach(iLp => iLp.Intensity = FullIntensity * multiplier);
            }


            OuterColorToDraw = OuterColorAtFullIntensity;
            InnerColorToDraw = InnerColorAtFullIntensity;

            if (GameSettingsManager.Settings.NeonLightFlicker)
            {
                bool doFlicker;
                if (sTimeOfLastFlickerCheck == iGameTime.TotalGameTime)
                {
                    doFlicker = sIsFlickeringThisTick;
                }
                else
                {
                    sTimeOfLastFlickerCheck = iGameTime.TotalGameTime;
                    if (!_timeOfLastFlicker.HasValue)
                        _timeOfLastFlicker = iGameTime.TotalGameTime;

                    var probOfFlicker = ProbabilityOfFlicker(_timeOfLastFlicker.Value, iGameTime.TotalGameTime);
                    var rngNum = _rng.NextDouble();

                    if (rngNum < probOfFlicker)
                    {
                        sIsFlickeringThisTick = true;
                        doFlicker = true;
                    }
                    else
                    {
                        sIsFlickeringThisTick = false;
                        doFlicker = false;
                    }
                }

                if (doFlicker)
                {
                    _timeOfLastFlicker = iGameTime.TotalGameTime;
                    OuterColorToDraw = ColorLerp(OuterColorAtFullIntensity, SettingsManager.NeonSettings.NeonLightOffColor, .05f);
                    InnerColorToDraw = ColorLerp(InnerColorAtFullIntensity, SettingsManager.NeonSettings.NeonLightOffColor, .05f);

                    LightPoints.ForEach(iLp => iLp.Intensity *= 1 - .05f);
                }
            }
        }

        public abstract void Draw();

        public abstract List<PointLight> LightPoints { get; }

        public abstract Rectangle Bounds { get; }

        protected abstract float FullIntensity { get; }

        protected abstract Color OuterColorAtFullIntensity { get; }
        protected abstract Color InnerColorAtFullIntensity { get; }
        protected Color OuterColorToDraw { get; private set; }
        protected Color InnerColorToDraw { get; private set; }

        private TimeSpan? _timeOfLastFlicker;
        private readonly double _pulseOffset;
        private readonly Random _rng;

        private static TimeSpan sTimeOfLastFlickerCheck = TimeSpan.MinValue;
        private static bool sIsFlickeringThisTick;

        private static float ProbabilityOfFlicker(TimeSpan iTimeOfLastFlicker, TimeSpan iCurrentTime)
        {
            const float t1 = 5f;
            const float t2 = 10f;

            var secondsSinceLastFlicker = (float)(iCurrentTime.TotalSeconds - iTimeOfLastFlicker.TotalSeconds);

            var probability = (secondsSinceLastFlicker - t1) / (t2 - t1);

            return Math.Min(Math.Max(probability, 0f), 1f);
        }

        private static Color ColorLerp(Color iCol1, Color iCol2, float iColLerpValue)
        {
            float LerpSingle(float iVal1, float iVal2, float iValLerpValue)
            {
                var diff = Math.Abs(iVal2 - iVal1);
                if (iVal1 < iVal2)
                    return iVal1 + iValLerpValue * diff;
                else
                    return iVal1 - iValLerpValue * diff;
            }
            
            var colorToReturn = new Color(
                (int)LerpSingle(iCol1.R, iCol2.R, iColLerpValue),
                (int)LerpSingle(iCol1.G, iCol2.G, iColLerpValue),
                (int)LerpSingle(iCol1.B, iCol2.B, iColLerpValue),
                (int)LerpSingle(iCol1.A, iCol2.A, iColLerpValue));

            return colorToReturn;
        }
    }
}
