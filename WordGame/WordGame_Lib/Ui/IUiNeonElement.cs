using System;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public enum NeonLightState
    {
        Off,
        FadeIn,
        On,
        FadeOut
    }

    public interface IUiNeonElement : IUiElement, ILightSource
    {
        NeonLightState State { get; }
        void StartFadeIn(GameTime iGameTime, TimeSpan iDuration);
        void StartFadeOut(GameTime iGameTime, TimeSpan iDuration);
    }
}