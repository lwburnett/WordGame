using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public interface IUiElement
    {
        void Update(GameTime iGameTime);
        void Draw();
    }
}
