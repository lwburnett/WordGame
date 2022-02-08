using System;
using WordGame_Lib;

namespace WordGame_Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            PlatformUtilsHelper.SetIsMouseInput(true);

            // Registering a blank function for vibration
            PlatformUtilsHelper.RegisterVibrateDeviceCallback(iD => { });

            FileManager.RegisterBaseDirectory(AppDomain.CurrentDomain.BaseDirectory);

            // Specifying 16:9 aspect ratio so th we don't full screen on windows
            using var game = new GameMaster(1.778f);
            game.Run();
        }
    }
}
