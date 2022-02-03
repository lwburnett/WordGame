using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public override void Draw()
        {
            _neonText.Draw();
        }

        public List<PointLight> LightPoints => _neonText.LightPoints;

        protected override Rectangle Bounds => _neonText.Bounds;

        private readonly UiNeonFloatingText _neonText;
    }
}