using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace WordGame_Lib
{
    public static class AudioHelper
    {
        public static void PlaySong(Song iSong)
        {
            MediaPlayer.Play(iSong);
            MediaPlayer.IsRepeating = true;
        }

        public static void SetMusicVolume(int iValue)
        {
            var volVal = (float)(iValue - SettingsManager.Sound.SoundVolumeMin) / (SettingsManager.Sound.SoundVolumeMax - SettingsManager.Sound.SoundVolumeMin);
            MediaPlayer.Volume = volVal;
        }

        public static void PlaySoundEffect(SoundEffect iSoundEffect, float iVolume = 1.0f)
        {
            iSoundEffect.Play(iVolume, 0.0f, 0.0f);
        }

        public static void PlayPerpetualStormSoundEffect(SoundEffect iSoundEffect)
        {
            Debug.Assert(sStormSoundEffectInstance == null);

            sStormSoundEffectInstance = iSoundEffect.CreateInstance();
            sStormSoundEffectInstance.IsLooped = true;
            SetStormVolume(GameSettingsManager.Settings.StormVolume);
            sStormSoundEffectInstance.Play();
        }

        public static void SetStormVolume(int iValue)
        {
            Debug.Assert(sStormSoundEffectInstance != null);

            var volVal = (float)(iValue - SettingsManager.Sound.SoundVolumeMin) / (SettingsManager.Sound.SoundVolumeMax - SettingsManager.Sound.SoundVolumeMin);
            sStormSoundEffectInstance.Volume = volVal;
        }

        public static void Update(GameTime iGameTime)
        {

        }

        private static SoundEffectInstance sStormSoundEffectInstance;
    }
}
