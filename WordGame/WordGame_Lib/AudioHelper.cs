using System;
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

        public static void PlaySoundEffect(SoundEffect iSoundEffect, float iVolume = 1.0f)
        {
            iSoundEffect.Play(iVolume, 0.0f, 0.0f);
        }
    }
}
