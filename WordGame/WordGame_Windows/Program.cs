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

            using var game = new GameMaster();
            game.Run();
        }
    }
}
