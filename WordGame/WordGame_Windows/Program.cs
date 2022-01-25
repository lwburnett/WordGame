using System;
using System.Diagnostics;
using System.IO;
using WordGame_Lib;

namespace WordGame_Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            PlatformUtilsHelper.SetIsMouseInput(true);

            FileManager.RegisterBaseDirectory(AppDomain.CurrentDomain.BaseDirectory);

            using var game = new GameMaster();
            game.Run();
        }
    }
}
