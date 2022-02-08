using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using WordGame_Lib;
using Xamarin.Essentials;

namespace WordGame_Android
{
    [Activity(
        MainLauncher = true,
        Icon = "@mipmap/ic_launcher",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Program : AndroidGameActivity
    {
        private GameMaster _game;
        private View _view;

        protected override void OnCreate(Bundle iBundle)
        {
            base.OnCreate(iBundle);

            // This is needed on android according to https://docs.microsoft.com/en-us/xamarin/essentials/get-started?tabs=windows%2Candroid
            Platform.Init(this, iBundle);

            PlatformUtilsHelper.SetIsMouseInput(false);

            PlatformUtilsHelper.RegisterVibrateDeviceCallback(VibrateDevice);

            FileManager.RegisterBaseDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

            _game = new GameMaster();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();
        }

        private static void VibrateDevice(TimeSpan iDuration)
        {
            Vibration.Vibrate(iDuration);
        }
    }
}
