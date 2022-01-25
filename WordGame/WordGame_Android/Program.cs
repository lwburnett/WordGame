using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using WordGame_Lib;

namespace WordGame_Android
{
    [Activity(
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.FullUser,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Program : AndroidGameActivity
    {
        private GameMaster _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            PlatformUtilsHelper.SetIsMouseInput(false);
            FileManager.RegisterOpenStreamReadCallback(OpenReadStream);
            FileManager.RegisterOpenStreamWriteCallback(OpenReadWriteStream);

            _game = new GameMaster();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();
        }

        private static Stream OpenReadStream(string iFileName)
        {
            var baseDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

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
                //Debug.Fail($"Failed to open read stream at {iFileName}");
                return null;
            }
        }

        private static Stream OpenReadWriteStream(string iFileName)
        {
            var baseDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            if (!string.IsNullOrWhiteSpace(baseDirectory))
            {
                var path = Path.Combine(baseDirectory, iFileName);

                if (File.Exists(path))
                    return File.OpenWrite(path);
                else
                {
                    var parentDirectory = Path.GetDirectoryName(path);
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Directory.CreateDirectory(parentDirectory);
                    return File.Create(path);
                }
            }
            else
            {
                //Debug.Fail($"Failed to open read stream at {iFileName}");
                return null;
            }
        }
    }
}
