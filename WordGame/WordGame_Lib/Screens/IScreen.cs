using Microsoft.Xna.Framework;

namespace WordGame_Lib.Screens
{
    interface IScreen
    {
        void OnNavigateTo();

        void Update(GameTime iGameTime);

        void Draw();
    }
}
