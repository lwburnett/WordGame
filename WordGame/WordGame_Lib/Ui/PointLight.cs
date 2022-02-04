using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class PointLight
    {
        public PointLight(Color iLightColor, Vector2 iPosition, float iRadius, float iIntensity)
        {
            LightColor = iLightColor;
            Position = iPosition;
            Radius = iRadius;
            Intensity = iIntensity;
        }

        public Color LightColor { get; }
        public Vector2 Position { get; }
        public float Radius { get; set; }
        public float Intensity { get; set; }
    }
}