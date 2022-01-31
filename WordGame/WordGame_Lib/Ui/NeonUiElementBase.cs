using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public abstract class NeonUiElementBase : INeonUiElement
    {
        protected NeonUiElementBase()
        {
            var rng = new Random();
            _pulseOffset = SettingsManager.NeonSettings.PulsePeriodSec * rng.NextDouble();
        }

        public virtual void Update(GameTime iGameTime)
        {
            const double pi2 = Math.PI * 2;
            const float period = SettingsManager.NeonSettings.PulsePeriodSec;
            const float amp = SettingsManager.NeonSettings.PulseIntensityAmplitude;

            var gameTimeSeconds = (float)iGameTime.TotalGameTime.TotalSeconds;
            var multiplier = amp * (float)Math.Abs(Math.Cos((gameTimeSeconds * pi2 / (2 * period)) + _pulseOffset)) + (1 - amp);

            LightPoints.ForEach(iLp => iLp.Intensity = FullIntensity * multiplier);
        }

        public abstract void Draw();

        public abstract List<PointLight> LightPoints { get; }

        public abstract Rectangle Bounds { get; }

        protected abstract float FullIntensity { get; }

        private readonly double _pulseOffset;
    }
}
