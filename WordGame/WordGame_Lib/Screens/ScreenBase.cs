using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Screens
{
    public abstract class ScreenBase : IScreen
    {
        public Task Load()
        {
            return Task.Run(DoLoad);
        }

        public abstract void Update(GameTime iGameTime);

        public abstract void Draw();

        public virtual void StartTransitionOut(GameTime iGameTime)
        {
            IsVisible = false;
        }

        public virtual void StartTransitionIn(GameTime iGameTime)
        {
            IsVisible = true;
        }

        public bool IsVisible { get; protected set; }

        protected abstract void DoLoad();
    }
}
