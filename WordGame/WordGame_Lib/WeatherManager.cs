using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib
{
    public class WeatherManager
    {

        public WeatherManager()
        {
            _rng = new Random();
            _nextDropSpawn = TimeSpan.MinValue;
            _nextSoundEffectPlay = TimeSpan.MinValue;
            _weatherSound = AssetHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "Storm"));
            _drops = new List<RainDrop>();
        }

        public void Update(GameTime iGameTime)
        {
            var playArea = GraphicsHelper.GamePlayArea;

            if (iGameTime.TotalGameTime > _nextDropSpawn)
            {
                var spawnX = (float)(playArea.X + _rng.NextDouble() * playArea.Width);
                var spawnY = playArea.Y - 50f;
                _drops.Add(new RainDrop(new Vector2(spawnX, spawnY), sFallDirection));

                const double nextDropSpawnMinTimeMs = 50;
                const double nextDropSpawnMaxTimeMs = 150;
                _nextDropSpawn = iGameTime.TotalGameTime + TimeSpan.FromMilliseconds(nextDropSpawnMinTimeMs + _rng.NextDouble() * (nextDropSpawnMaxTimeMs - nextDropSpawnMinTimeMs));
            }

            var ii = 0;
            while (ii < _drops.Count)
            {
                var thisDrop = _drops[ii];

                if (thisDrop.Bounds.Y <= playArea.Y + playArea.Height)
                {
                    thisDrop.Update(iGameTime);
                    ii++;
                }
                else
                {
                    _drops.RemoveAt(ii);
                }
            }

            if (iGameTime.TotalGameTime > _nextSoundEffectPlay)
            {
                AudioHelper.PlaySoundEffect(_weatherSound);
                _nextSoundEffectPlay = iGameTime.TotalGameTime + _weatherSound.Duration;
            }
        }

        public void Draw()
        {
            _drops.ForEach(iD => iD.Draw());
        }

        private readonly Random _rng;
        private static readonly Vector2 sFallDirection = new Vector2(0, 4000f);
        private TimeSpan _nextDropSpawn;
        private TimeSpan _nextSoundEffectPlay;

        private readonly SoundEffect _weatherSound;
        private readonly List<RainDrop> _drops;

        private class RainDrop
        {
            static RainDrop()
            {
                const int width = 2;
                const int height = 50;


                var colorData = new Color[width * height];
                for (var ii = 0; ii < width * height; ii++)
                {
                    colorData[ii] = new Color(94, 182, 249);
                }

                sTexture = GraphicsHelper.CreateTexture(colorData, width, height);
            }

            public RainDrop(Vector2 iSpawnPoint, Vector2 iFallVector)
            {
                _position = iSpawnPoint;
                _fallVector = iFallVector;
            }

            public void Update(GameTime iGameTime)
            {
                var dtSeconds = iGameTime.ElapsedGameTime.TotalSeconds;
                var newX = (float)(_position.X + dtSeconds * _fallVector.X);
                var newY = (float)(_position.Y + dtSeconds * _fallVector.Y);

                _position = new Vector2(newX, newY);
            }

            public void Draw()
            {
                GraphicsHelper.DrawTexture(sTexture, _position, iBrushColor: Color.White * 0.15f);
            }

            public Rectangle Bounds => new Rectangle(_position.ToPoint(), new Point(sTexture.Width, sTexture.Height));

            private static readonly Texture2D sTexture;
            private Vector2 _position;
            private readonly Vector2 _fallVector;
        }
    }
}
