using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public abstract class NeonUiElementBase : INeonUiElement
    {
        protected NeonUiElementBase(double? iPulseOffsetLerpValue)
        {
            var pulseLerpValue = iPulseOffsetLerpValue ?? new Random().NextDouble();
            _pulseOffset = SettingsManager.NeonSettings.PulsePeriodSec * pulseLerpValue;
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
        }

        public abstract void Draw();

        public abstract List<PointLight> LightPoints { get; }

        public abstract Rectangle Bounds { get; }

        protected abstract float FullIntensity { get; }

        private readonly double _pulseOffset;
    }
}
