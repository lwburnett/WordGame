namespace WordGame_Lib
{
    static class SettingsManager
    {
        public static class GameMasterSettings
        {
            public const float TargetScreenAspectRatio = 1.778f;
        }

        public static class MainMenuSettings
        {
            public const float ButtonWidthAsFractionOfPlayAreaWidth = .25f;
            public const float ButtonHeightAsFractionOfPlayAreaHeight = .075f;
        }

        public static class GamePlaySettings
        {
            public const float KeyboardHeightAsPercentage = .25f;
        }

        public static class KeyboardSettings
        {
            public const float KeyboardMarginAsPercentage = .01f;
            public const float KeyMarginAsPercentage = .005f;
        }
    }
}
