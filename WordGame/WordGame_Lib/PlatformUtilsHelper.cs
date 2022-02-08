using System;
using System.Diagnostics;

namespace WordGame_Lib
{
    public static class PlatformUtilsHelper
    {
        #region Input Type

        public static bool GetIsMouseInput()
        {
            return sIsMouseInput;
        }

        public static void SetIsMouseInput(bool iIsMouseInput)
        {
            sIsMouseInput = iIsMouseInput;
        }

        private static bool sIsMouseInput;

        #endregion

        #region Vibration

        public static void VibrateDevice(TimeSpan iDuration)
        {
            Debug.Assert(sVibrateDeviceCallback != null);

            sVibrateDeviceCallback(iDuration);
        }

        public static void RegisterVibrateDeviceCallback(Action<TimeSpan> iCallback)
        {
            Debug.Assert(sVibrateDeviceCallback == null);

            sVibrateDeviceCallback = iCallback;
        }

        private static Action<TimeSpan> sVibrateDeviceCallback;

        #endregion
    }
}
