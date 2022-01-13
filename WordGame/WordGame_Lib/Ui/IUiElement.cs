using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    interface IUiElement
    {
        void Update(GameTime iGameTime);
        void Draw();
    }
}
