using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class UiMenuNeonButton : UiButtonBase, IUiNeonElement
    {
        public UiMenuNeonButton(Rectangle iBounds, string iText, Color iTextColor, Action<GameTime> iOnClickedCallback) :
            base(iOnClickedCallback)
        {
            _neonText = new UiNeonFloatingText(iBounds, iText, iTextColor);
        }

        public override void Update(GameTime iGameTime)
        {
            base.Update(iGameTime);

            _neonText.Update(iGameTime);
        }

        public override void Draw(Vector2? iOffset = null)
        {
            _neonText.Draw(iOffset);
        }

        public NeonLightState State => _neonText.State;
        public void StartFadeIn(GameTime iGameTime, TimeSpan iDuration)
        {
            _neonText.StartFadeIn(iGameTime, iDuration);
        }

        public void StartFadeOut(GameTime iGameTime, TimeSpan iDuration)
        {
            _neonText.StartFadeOut(iGameTime, iDuration);
        }

        public List<PointLight> LightPoints => _neonText.LightPoints;

        protected override Rectangle Bounds => _neonText.Bounds;

        private readonly UiNeonFloatingText _neonText;
    }
}