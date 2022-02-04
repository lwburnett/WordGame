using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Screens
{
    public abstract class ScreenBase : IScreen
    {
        protected ScreenBase()
        {
            _transitionState = TransitionState.Default;
        }

        public Task Load()
        {
            return Task.Run(DoLoad);
        }

        public void Update(GameTime iGameTime)
        {
            switch (_transitionState)
            {
                case TransitionState.In:
                    if (UpdateTransitionIn(iGameTime))
                    {
                        IsVisible = true;
                        _transitionState = TransitionState.Default;
                    }
                    break;
                case TransitionState.Default:
                    UpdateDefault(iGameTime);
                    break;
                case TransitionState.Out:
                    if (UpdateTransitionOut(iGameTime))
                    {
                        IsVisible = false;
                    }
                    break;
                default:
                    Debug.Fail($"Unknown value of enum {nameof(TransitionState)}: {_transitionState}");
                    // ReSharper disable once HeuristicUnreachableCode
                    UpdateDefault(iGameTime);
                    break;
            }
        }

        public abstract void Draw();

        public virtual void StartTransitionOut(GameTime iGameTime)
        {
            _transitionState = TransitionState.Out;
        }

        public virtual void StartTransitionIn(GameTime iGameTime)
        {
            _transitionState = TransitionState.In;
        }

        public bool IsVisible { get; protected set; }

        protected abstract void DoLoad();

        protected abstract bool UpdateTransitionIn(GameTime iGameTime);
        protected abstract void UpdateDefault(GameTime iGameTime);
        protected abstract bool UpdateTransitionOut(GameTime iGameTime);

        private TransitionState _transitionState;
        private enum TransitionState
        {
            In,
            Default,
            Out
        }
    }
}
