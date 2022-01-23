using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WordGame_Lib
{
    public static class GameSettingsManager
    {
        static GameSettingsManager()
        {
            ReadWriteLock = new object();
            sHaveLoadedFromDisk = false;
            sGameSettings = new GameSettings();
        }

        public static GameSettings Settings
        {
            get
            {
                lock (ReadWriteLock)
                {
                    return sGameSettings.DeepCopy();
                }
            }
            private set
            {
                lock (ReadWriteLock)
                {
                    sGameSettings = value;
                }
            }
        }

        public static void ReadSettingFromDiskAsync(string iFilePath)
        {
            if (!sHaveLoadedFromDisk)
            {
                Task.Run(() => DoReadSettings(iFilePath));

                sHaveLoadedFromDisk = true;
            }
            else
            {
                Debug.Fail("Have already loaded settings from disk!");
            }
        }

        private static readonly object ReadWriteLock;
        private static bool sHaveLoadedFromDisk;
        private static GameSettings sGameSettings;

        private static void DoReadSettings(string iFilePath)
        {
            var lines = new List<string>();
            using (var stream = TitleContainer.OpenStream(iFilePath))
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }

            if (lines.Any())
            {
                var alternateKeyColorScheme = false;
                foreach (var line in lines)
                {
                    var pieces = line.Split(',').Select(p => p.Trim()).ToList();

                    if (pieces.Count != 2)
                    {
                        Debug.Fail($"Invalid line in settings file: {line}");
                        continue;
                    }

                    switch (pieces[0])
                    {
                        case nameof(GameSettings.AlternateKeyColorScheme):
                            var success = bool.TryParse(pieces[0], out alternateKeyColorScheme);
                            Debug.Assert(success, $"Failed to read value of line: {line}");
                            break;
                        default:
                            Debug.Fail($"Unknown settings key {pieces[0]}");
                            break;
                    }
                }

                Settings = new GameSettings(alternateKeyColorScheme);
            }
            else
            {
                Settings = new GameSettings();
            }
        }
    }

    public class GameSettings
    {
        public GameSettings()
        {
            AlternateKeyColorScheme = false;
        }

        public GameSettings(bool iAlternateKeyColorScheme)
        {
            AlternateKeyColorScheme = iAlternateKeyColorScheme;
        }

        public bool AlternateKeyColorScheme { get; }

        public GameSettings DeepCopy()
        {
            return new GameSettings(AlternateKeyColorScheme);
        }
    }
}