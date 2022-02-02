using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame_Lib
{
    public static class GameSettingsManager
    {
        static GameSettingsManager()
        {
            sHasRegisteredFilePath = false;
            PropertyReadWriteLock = new object();
            FileReadWriteLock = new object();
            sHaveLoadedFromDisk = false;
            sGameSettings = new GameSettings();
        }

        public static GameSettings Settings
        {
            get
            {
                lock (PropertyReadWriteLock)
                {
                    return sGameSettings.DeepCopy();
                }
            }
            private set
            {
                lock (PropertyReadWriteLock)
                {
                    sGameSettings = value;
                }
            }
        }

        public static void RegisterFilePath(string iFilePath)
        {
            Debug.Assert(!sHasRegisteredFilePath, "Already registered a file path!");
            Debug.Assert(!string.IsNullOrWhiteSpace(iFilePath), "Invalid file path given.");

            lock (FileReadWriteLock)
            {
                sSettingsFilePath = iFilePath;
            }
            sHasRegisteredFilePath = true;
        }

        public static void ReadSettingFromDiskAsync()
        {
            if (!sHaveLoadedFromDisk)
            {
                Task.Run(DoReadSettings);

                sHaveLoadedFromDisk = true;
            }
            else
            {
                Debug.Fail("Have already loaded settings from disk!");
            }
        }

        public static void UpdateSettings(GameSettings iSettings)
        {
            Settings = iSettings;
        }

        public static void WriteSettingsToDiskAsync()
        {
            Task.Run(DoWriteSettings);
        }

        private static string sSettingsFilePath;
        private static bool sHasRegisteredFilePath;

        private static readonly object PropertyReadWriteLock;
        private static readonly object FileReadWriteLock;
        private static bool sHaveLoadedFromDisk;
        private static GameSettings sGameSettings;

        private static void DoReadSettings()
        {
            try
            {
                var lines = new List<string>();
                lock (FileReadWriteLock)
                {
                    if (FileManager.TryOpenStreamReadSafe(sSettingsFilePath, out var stream))
                    {
                        using (stream)
                        using (var reader = new StreamReader(stream, Encoding.ASCII))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                                lines.Add(line);
                        }
                    }
                }

                if (lines.Any())
                {
                    var alternateKeyColorScheme = false;
                    var neonLightPulse = true;
                    var neonLightFlicker = true;
                    foreach (var line in lines)
                    {
                        var pieces = line.Split(',').Select(iP => iP.Trim()).ToList();

                        if (pieces.Count != 2)
                        {
                            Debug.Fail($"Invalid line in settings file: {line}");
                            // ReSharper disable once HeuristicUnreachableCode
                            continue;
                        }

                        switch (pieces[0])
                        {
                            case nameof(GameSettings.AlternateKeyColorScheme):
                                var success1 = bool.TryParse(pieces[1], out alternateKeyColorScheme);
                                Debug.Assert(success1, $"Failed to read value of line: {line}");
                                break;
                            case nameof(GameSettings.NeonLightPulse):
                                var success2 = bool.TryParse(pieces[1], out neonLightPulse);
                                Debug.Assert(success2, $"Failed to read value of line: {line}");
                                break;
                            case nameof(GameSettings.NeonLightFlicker):
                                var success3 = bool.TryParse(pieces[1], out neonLightFlicker);
                                Debug.Assert(success3, $"Failed to read value of line: {line}");
                                break;
                            default:
                                Debug.Fail($"Unknown settings key {pieces[0]}");
                                break;
                        }
                    }

                    Settings = new GameSettings(alternateKeyColorScheme, neonLightPulse, neonLightFlicker);
                }
                else
                {
                    Settings = new GameSettings();
                    DoWriteSettings();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }

        private static void DoWriteSettings()
        {
            try
            {
                var settings = Settings.DeepCopy();

                var lines = new List<string>
                {
                    $"{nameof(settings.AlternateKeyColorScheme)}, {settings.AlternateKeyColorScheme}",
                    $"{nameof(settings.NeonLightPulse)}, {settings.NeonLightPulse}"
                };

                lock (FileReadWriteLock)
                {
                    if (FileManager.TryOpenStreamWriteSafe(sSettingsFilePath, out var stream))
                    {
                        using (stream)
                        using (var writer = new StreamWriter(stream, Encoding.ASCII))
                        {
                            foreach (var line in lines)
                            {
                                writer.WriteLineAsync(line).Wait();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }

    public class GameSettings
    {
        public GameSettings()
        {
            AlternateKeyColorScheme = false;
            NeonLightPulse = true;
            NeonLightFlicker = true;
        }

        public GameSettings(bool iAlternateKeyColorScheme, bool iNeonLightPulse, bool iNeonLightFlicker)
        {
            AlternateKeyColorScheme = iAlternateKeyColorScheme;
            NeonLightPulse = iNeonLightPulse;
            NeonLightFlicker = iNeonLightFlicker;
        }

        public bool AlternateKeyColorScheme { get; }
        public bool NeonLightPulse { get; }
        public bool NeonLightFlicker { get; }

        public GameSettings DeepCopy()
        {
            return new GameSettings(AlternateKeyColorScheme, NeonLightPulse, NeonLightFlicker);
        }
    }
}