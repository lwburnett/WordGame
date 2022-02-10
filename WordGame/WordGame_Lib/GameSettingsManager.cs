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
            sPropertyReadWriteLock = new object();
            sFileReadWriteLock = new object();
            sHaveLoadedFromDisk = false;
            sGameSettings = new GameSettings();
        }

        public static GameSettings Settings
        {
            get
            {
                lock (sPropertyReadWriteLock)
                {
                    return sGameSettings.DeepCopy();
                }
            }
            private set
            {
                lock (sPropertyReadWriteLock)
                {
                    sGameSettings = value;
                }
            }
        }

        public static void RegisterFilePath(string iFilePath)
        {
            Debug.Assert(!sHasRegisteredFilePath, "Already registered a file path!");
            Debug.Assert(!string.IsNullOrWhiteSpace(iFilePath), "Invalid file path given.");

            lock (sFileReadWriteLock)
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

        private static readonly object sPropertyReadWriteLock;
        private static readonly object sFileReadWriteLock;
        private static bool sHaveLoadedFromDisk;
        private static GameSettings sGameSettings;

        private static void DoReadSettings()
        {
            try
            {
                var lines = new List<string>();
                lock (sFileReadWriteLock)
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
                    var vibration = true;
                    var rainVisual = true;
                    var stormVolume = 5;
                    var musicVolume = 5;
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

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
                            case nameof(GameSettings.Vibration):
                                var success4 = bool.TryParse(pieces[1], out vibration);
                                Debug.Assert(success4, $"Failed to read value of line: {line}");
                                break;
                            case nameof(GameSettings.RainVisual):
                                var success5 = bool.TryParse(pieces[1], out rainVisual);
                                Debug.Assert(success5, $"Failed to read value of line: {line}");
                                break;
                            case nameof(GameSettings.StormVolume):
                                var success6 = int.TryParse(pieces[1], out stormVolume);
                                Debug.Assert(success6, $"Failed to read value of line: {line}");
                                break;
                            case nameof(GameSettings.MusicVolume):
                                var success7 = int.TryParse(pieces[1], out musicVolume);
                                Debug.Assert(success7, $"Failed to read value of line: {line}");
                                break;
                            default:
                                Debug.Fail($"Unknown settings key {pieces[0]}");
                                break;
                        }
                    }

                    Settings = new GameSettings(alternateKeyColorScheme, neonLightPulse, neonLightFlicker, vibration, rainVisual, stormVolume, musicVolume);
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
                    $"{nameof(settings.NeonLightPulse)}, {settings.NeonLightPulse}",
                    $"{nameof(settings.NeonLightFlicker)}, {settings.NeonLightFlicker}",
                    $"{nameof(settings.Vibration)}, {settings.Vibration}",
                    $"{nameof(settings.RainVisual)}, {settings.RainVisual}",
                    $"{nameof(settings.StormVolume)}, {settings.StormVolume}",
                    $"{nameof(settings.MusicVolume)}, {settings.MusicVolume}"
                };

                lock (sFileReadWriteLock)
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
}