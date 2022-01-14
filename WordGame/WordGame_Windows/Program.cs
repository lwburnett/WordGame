using System;
using WordGame_Lib;

namespace WordGame_Windows
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new GameMaster(true);
            game.Run();
        }
    }
}
