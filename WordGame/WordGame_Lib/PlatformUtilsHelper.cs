namespace WordGame_Lib
{
    public static class PlatformUtilsHelper
    {
        public static bool GetIsMouseInput()
        {
            return sIsMouseInput;
        }

        public static void SetIsMouseInput(bool iIsMouseInput)
        {
            sIsMouseInput = iIsMouseInput;
        }

        private static bool sIsMouseInput;
    }
}
