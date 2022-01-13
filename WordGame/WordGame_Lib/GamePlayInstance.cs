using System;
using Microsoft.Xna.Framework;

namespace WordGame_Lib
{
    public class GamePlayInstance
    {
        public GamePlayInstance(Action iOnGamePlaySessionFinishedCallback)
        {
            _playSessionHasFinished = false;
            _onGamePlaySessionFinishedCallback = iOnGamePlaySessionFinishedCallback;
        }

        private bool _playSessionHasFinished;
        private readonly Action _onGamePlaySessionFinishedCallback;

        public void LoadLevel()
        {
            // TODO construct initial UI
        }

        public void Update(GameTime iGameTime)
        {
            if (_playSessionHasFinished)
                return;
        }

        public void Draw()
        {
            // TODO draw UI
        }
    }
}
