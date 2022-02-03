﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Screens
{
    public class StartupScreen : ScreenBase
    {
        public StartupScreen()
        {
            _colorData = new SafeData<List<Color>>(iVal => iVal?.Select(iC => iC).ToList());
        }

        public override void Update(GameTime iGameTime)
        {
        }

        public override void Draw()
        {
            if (_texture == null)
            {
                var colorData = _colorData.Value?.ToArray();
                Debug.Assert(_colorData.Value != null);

                _texture = GraphicsHelper.CreateTexture(colorData, GraphicsHelper.GamePlayArea.Width, GraphicsHelper.GamePlayArea.Height);
            }

            GraphicsHelper.DrawTexture(_texture, new Vector2(0,0));
        }


        protected override void DoLoad()
        {
            var width = GraphicsHelper.GamePlayArea.Width;
            var height = GraphicsHelper.GamePlayArea.Height;
            var dataSize = width * height;

            var colorData = new List<Color>(dataSize);
            for (var ii = 0; ii < dataSize; ii++)
            {
                colorData.Add(Color.Black);
            }

            _colorData.Value = colorData;
        }

        private readonly SafeData<List<Color>> _colorData;
        private Texture2D _texture;
    }
}