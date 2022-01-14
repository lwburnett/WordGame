using Microsoft.Xna.Framework;

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
            public const float LetterGridHeightAsPercentage = .65f;
            public const float NotificationHeightAsPercentage = .1f;
        }

        public static class KeyboardSettings
        {
            public const float KeyboardMarginAsPercentage = .01f;
            public const float KeyMarginAsPercentage = .005f;
        }

        public static class GridSettings
        {
            public const float GridMarginAsPercentage = .1f;
            public const float CellMarginAsPercentage = .01f;
        }

        public static class ColorSettings
        {
            public static Color UndecidedDefaultColor = Color.LightGray;
            public static Color UndecidedHoverColor = Color.DarkGray;
            public static Color UndecidedPressedColor = Color.Gray;

            public static Color IncorrectDefaultColor = Color.Gray;
            public static Color IncorrectHoverColor = new Color(115, 115, 115);
            public static Color IncorrectPressedColor = new Color(99, 99, 99);

            public static Color MisplacedDefaultColor = new Color(226, 214, 67);
            public static Color MisplacedHoverColor = new Color(204, 194, 71);
            public static Color MisplacedPressedColor = new Color(168, 159, 48);

            public static Color CorrectDefaultColor = new Color(48, 185, 8);
            public static Color CorrectHoverColor = new Color(43, 163, 8);
            public static Color CorrectPressedColor = new Color(43, 163, 8);
        }
    }
}
