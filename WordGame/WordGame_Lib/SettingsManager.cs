using System;
using Microsoft.Xna.Framework;

namespace WordGame_Lib
{
    public static class SettingsManager
    {
        public static class GeneralVisualSettings
        {
            public const float BigMarginAsPercentage = .05f;
            public const float MediumMarginAsPercentage = .025f;
            public const float SmallMarginAsPercentage = .01f;

            public const float TextBorderWidthAsPercentage = .002f;
        }

        public static class MainMenuSettings
        {
            public static Color TitleTextColor = new Color(244, 231, 34);
            public static Color StartButtonColor = new Color(68, 214, 44);
            public static Color SettingsButtonColor = new Color(77, 77, 255);
            public static Color CreditsButtonColor = new Color(210, 39, 48);

            public const float TitleWord1XAsPercentage = .1f;
            public const float TitleWord1YAsPercentage = .1f;
            public const float TitleWord1HeightAsPercentage = .1f;
            public const float TitleWord1WidthAsPercentage = .5f;

            public const float TitleWord2XAsPercentage = .4f;
            public const float TitleWord2YAsPercentage = .18f;
            public const float TitleWord2HeightAsPercentage = .1f;
            public const float TitleWord2WidthAsPercentage = .5f;

            public const float PlayButtonXAsPercentage = .5f;
            public const float PlayButtonYAsPercentage = .45f;
            public const float PlayButtonHeightAsPercentage = .075f;
            public const float PlayButtonWidthAsPercentage = .5f;

            public const float SettingsButtonXAsPercentage = .1f;
            public const float SettingsButtonYAsPercentage = .65f;
            public const float SettingsButtonHeightAsPercentage = .06f;
            public const float SettingsButtonWidthAsPercentage = .55f;

            public const float CreditsButtonXAsPercentage = .55f;
            public const float CreditsButtonYAsPercentage = .85f;
            public const float CreditsButtonHeightAsPercentage = .06f;
            public const float CreditsButtonWidthAsPercentage = .4f;
        }

        public static class GamePlaySettings
        {
            public const float KeyboardHeightAsPercentage = .25f;
            public const float KeyboardYPosAsPercentage = .7f;
            public const float NotificationHeightAsPercentage = .05f;

            public static float MainMenuButtonYAsPercentage = .75f;
            public static float MainMenuButtonHeightAsPercentage = .065f;
            public static float MainMenuButtonWidthAsPercentage = .7f;
            public static Color MainMenuButtonColor = new Color(255, 173, 0);

            public static float PlayAgainButtonYAsPercentage = .85f;
            public static float PlayAgainButtonHeightAsPercentage = .065f;
            public static float PlayAgainButtonWidthAsPercentage = .7f;
            public static Color PlayAgainButtonColor = new Color(77, 77, 255);

            public const double PostGuessDispositionRevealFrequencySeconds = .4;
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

        public static class UiKeyboardColors
        {
            public static Color UndecidedDefaultColor = Color.LightGray;
            public static Color UndecidedHoverColor = Color.DarkGray;
            public static Color UndecidedPressedColor = Color.Gray;

            public static Color IncorrectDefaultColor = new Color(119, 119, 119);
            public static Color IncorrectHoverColor = new Color(99, 99, 99);
            public static Color IncorrectPressedColor = new Color(69, 69, 69);

            public static Color MisplacedDefaultColor = new Color(226, 214, 67);
            public static Color MisplacedHoverColor = new Color(204, 194, 71);
            public static Color MisplacedPressedColor = new Color(168, 159, 48);

            public static Color CorrectDefaultColor = new Color(48, 185, 8);
            public static Color CorrectHoverColor = new Color(43, 163, 8);
            public static Color CorrectPressedColor = new Color(43, 163, 8);
        }

        public static class PostSessionStatsSettings
        {
            public const float HeaderHeightAsPercentage = .1f;
            public const float SubHeaderHeightAsPercentage = .1f;
            public const float DefinitionHeightAsPercentage = .4f;
            public const float ButtonWidthAsPercentage = .75f;
            public const float ButtonHeightAsPercentage = .1f;
        }

        public static class SettingsScreenSettings
        {
            public const float HeaderHeightAsPercentage = .1f;
            public const float SettingsListHeightAsPercentage = .65f;
            public const float SaveButtonWidthAsPercentage = .75f;
            public const float SaveButtonHeightAsPercentage = .1f;
            public const float LabelColumnWidthAsPercent = .75f;
            public const float SettingColumnWidthAsPercent = .2f;
            public const float IndividualSettingRowHeightAsPercent = .1666f;
        }

        public static class NeonSettings
        {
            public const float PulsePeriodSec = 4.0f;
            public const float PulseIntensityAmplitude = .05f;

            public const float MinimumSecondsBetweenFlicker = 15f;
            public const float MaximumSecondsBetweenFlicker = 30f;


            public static Color NeonLightOffColor = new Color(30, 30, 30);
            public static TimeSpan VisualTransitionDuration = TimeSpan.FromSeconds(.5);

            public static class Text
            {
                public const float RadiusAsPercentageOfWidth = .333f;
                public const float MinDistOfPointLightsAsPercentage = .05f;
                public const float MaxDistOfPointLightsAsPercentage = .1f;
            }

            public static class LetterCell
            {
                public const float RadiusAsPercentageOfWidth = .22f;
                public static Color Undecided = new Color(230, 230, 230);
                public static Color Incorrect = NeonLightOffColor;
                public static Color Misplaced = new Color(214, 231, 34);
                public static Color Correct = new Color(68, 214, 44);
            }

            public static class Sprite
            {
                public const float RadiusAsPercentageOfWidth = .22f;
            }
        }

        public static class Sound
        {
            public static TimeSpan VibrationDuration = TimeSpan.FromMilliseconds(5);
            public const float FlickerVolume = .6f;

            public const int SoundVolumeMin = 0;
            public const int SoundVolumeMax = 9;
        }

        public static class Storm
        {
            public const float RainDropHeightAsPercentage = .05f;
            public const float RainDropWidthAsPercentage = .0035f;
            public const float RainDropSpeedAsPercentage = 2.0f;
            public static Color RainDropColor = new Color(94, 182, 249);
            public const float RainDropAlpha = .2f;
        }

        public static class CreditsScreenSettings
        {
            public const float HeaderHeightAsPercentage = .1f;
            public const float CreditLineHeightAsPercentage = .1f;
            public const float MainMenuButtonWidthAsPercentage = .75f;
            public const float MainMenuButtonHeightAsPercentage = .1f;
        }
    }
}
