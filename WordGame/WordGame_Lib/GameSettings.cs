namespace WordGame_Lib
{
    public class GameSettings
    {
        public GameSettings()
        {
            AlternateKeyColorScheme = false;
            NeonLightPulse = true;
            NeonLightFlicker = true;
            Vibration = true;
            RainVisual = true;
            StormVolume = 5;
            MusicVolume = 5;
        }

        public GameSettings(bool iAlternateKeyColorScheme, bool iNeonLightPulse, bool iNeonLightFlicker, bool iVibration, bool iRainVisual, int iStormVolume, int iMusicVolume)
        {
            AlternateKeyColorScheme = iAlternateKeyColorScheme;
            NeonLightPulse = iNeonLightPulse;
            NeonLightFlicker = iNeonLightFlicker;
            Vibration = iVibration;
            RainVisual = iRainVisual;
            StormVolume = iStormVolume;
            MusicVolume = iMusicVolume;
        }

        public bool AlternateKeyColorScheme { get; }
        public bool NeonLightPulse { get; }
        public bool NeonLightFlicker { get; }
        public bool Vibration { get; }
        public bool RainVisual { get; }
        public int StormVolume { get; }
        public int MusicVolume { get; }

        public GameSettings DeepCopy()
        {
            return new GameSettings(AlternateKeyColorScheme, NeonLightPulse, NeonLightFlicker, Vibration, RainVisual, StormVolume, MusicVolume);
        }
    }
}