using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Screens
{
    public interface IScreen
    {
        Task Load();

        void Update(GameTime iGameTime);

        void Draw(Vector2? iOffset = null);

        void StartTransitionOut(GameTime iGameTime);

        void StartTransitionIn(GameTime iGameTime);

        bool IsVisible { get; }
    }
}
