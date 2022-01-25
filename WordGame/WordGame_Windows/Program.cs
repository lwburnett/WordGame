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

            FileManager.RegisterOpenStreamReadCallback(OpenReadStream);
            FileManager.RegisterOpenStreamWriteCallback(OpenReadWriteStream);

            using var game = new GameMaster();
            game.Run();
        }

        private static Stream OpenReadStream(string iFileName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrWhiteSpace(baseDirectory))
            {
                var path = Path.Combine(baseDirectory, iFileName);

                if (File.Exists(path))
                    return File.OpenRead(path);
                else
                    return null;
            }
            else
            {
                Debug.Fail($"Failed to open read stream at {iFileName}");
                return null;
            }
        }

        private static Stream OpenReadWriteStream(string iFileName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrWhiteSpace(baseDirectory))
            {
                var path = Path.Combine(baseDirectory, iFileName);

                if (!File.Exists(path))
                    return File.OpenWrite(path);
                else
                {
                    var parentDirectory = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(parentDirectory);
                    return File.Create(path);
                }
            }
            else
            {
                Debug.Fail($"Failed to open read stream at {iFileName}");
                return null;
            }
        }
    }
}
