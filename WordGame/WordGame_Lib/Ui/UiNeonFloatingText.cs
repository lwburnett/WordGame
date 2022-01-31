using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordGame_Lib.Ui
{
    public class UiNeonFloatingText : NeonUiElementBase
    {
        public UiNeonFloatingText(Rectangle iBounds, string iText, Color iTextColor)
        {
            _text = iText;
            FullIntensity = 1.75f;

            _textFont = GraphicsHelper.LoadContent<SpriteFont>(Path.Combine("Fonts", "NeonFont"));

            const float factor = 1.75f;
            _textColorInner = new Color(iTextColor.R * factor, iTextColor.G * factor, iTextColor.B * factor, iTextColor.A);
            _textColorOuter = iTextColor;

            var defaultStringDimensionsOuter = _textFont.MeasureString(_text) * 1.0f;
            var possibleXScalingOuter = iBounds.Width / defaultStringDimensionsOuter.X;
            var possibleYScalingOuter = iBounds.Height / defaultStringDimensionsOuter.Y;

            _scaling = Math.Min(possibleXScalingOuter, possibleYScalingOuter);
            var realStringDimensionsOuter = _textFont.MeasureString(_text) * _scaling;

            Bounds = new Rectangle(
                (int)(iBounds.X + (iBounds.Width - realStringDimensionsOuter.X) / 2f),
                (int)(iBounds.Y + (iBounds.Height - realStringDimensionsOuter.Y) / 2f),
                (int)realStringDimensionsOuter.X,
                (int)realStringDimensionsOuter.Y);

            LightPoints = CalculateLightPoints();
        }

        public override void Draw()
        {
            const float scalar = 1.25f;
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(-scalar * _scaling, -scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(scalar * _scaling, -scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(-scalar * _scaling, scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y) + new Vector2(scalar * _scaling, scalar * _scaling), _textColorOuter, _scaling);
            GraphicsHelper.DrawString(_textFont, _text, new Vector2(Bounds.X, Bounds.Y), _textColorInner, _scaling);

            //DrawPointLightsDebug();
        }

        public override List<PointLight> LightPoints { get; }

        public override Rectangle Bounds { get; }

        protected sealed override float FullIntensity { get; }

        private readonly string _text;
        private readonly SpriteFont _textFont;
        private readonly Color _textColorInner;
        private readonly Color _textColorOuter;
        private readonly float _scaling;

        private List<PointLight> CalculateLightPoints()
        {
            var minDist = GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.Text.MinDistOfPointLightsAsPercentage;
            var maxDist = GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.Text.MaxDistOfPointLightsAsPercentage;
            var radius = GraphicsHelper.GamePlayArea.Width * SettingsManager.NeonSettings.Text.RadiusAsPercentageOfWidth;

            var lights = new List<PointLight>();
            if (Bounds.Width <= minDist)
            {
                lights.Add(new PointLight(_textColorOuter, Bounds.Center.ToVector2(), radius, FullIntensity));
            }
            else
            {
                var minNumInterPointsNeeded = (int)Math.Floor(Bounds.Width / maxDist);

                var spaceBetweenPoints = (float)Bounds.Width / (minNumInterPointsNeeded + 1);
                var height = Bounds.Y + Bounds.Height / 2.0f;

                for (var ii = 0; ii < minNumInterPointsNeeded + 2; ii++)
                {
                    lights.Add(new PointLight(_textColorOuter, new Vector2(Bounds.X + (ii * spaceBetweenPoints), height), radius, FullIntensity));
                }
            }

            return lights;
        }

        // private void DrawPointLightsDebug()
        // {
        //     const int radius = 4;
        //     var diameter = radius * 2;
        //     var colorData = new Color[diameter * diameter];
        //
        //     for (var xx = 0; xx < diameter; xx++)
        //     {
        //         for (var yy = 0; yy < diameter; yy++)
        //         {
        //             var thisIndex = xx * diameter + yy;
        //             var distanceFromCenter = new Vector2(xx - radius, yy - radius);
        //
        //             colorData[thisIndex] = Math.Abs(distanceFromCenter.Length()) < radius ?
        //                 _textColorOuter : Color.Transparent;
        //         }
        //     }
        //
        //     var texture = GraphicsHelper.CreateTexture(colorData, diameter, diameter);
        //     texture.SetData(colorData);
        //
        //     foreach (var point in LightPoints)
        //     {
        //         GraphicsHelper.DrawTexture(texture, point.Position);
        //     }
        // }
    }
}