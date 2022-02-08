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
        }

        public GameSettings(bool iAlternateKeyColorScheme, bool iNeonLightPulse, bool iNeonLightFlicker, bool iVibration)
        {
            AlternateKeyColorScheme = iAlternateKeyColorScheme;
            NeonLightPulse = iNeonLightPulse;
            NeonLightFlicker = iNeonLightFlicker;
            Vibration = iVibration;
        }

        public bool AlternateKeyColorScheme { get; }
        public bool NeonLightPulse { get; }
        public bool NeonLightFlicker { get; }
        public bool Vibration { get; }

        public GameSettings DeepCopy()
        {
            return new GameSettings(AlternateKeyColorScheme, NeonLightPulse, NeonLightFlicker, Vibration);
        }
    }
}