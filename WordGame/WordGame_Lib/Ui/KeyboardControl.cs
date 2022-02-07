using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class KeyboardControl : IUiElement
    {
        public KeyboardControl(Rectangle iBounds, Action<string> iOnLetterClickedCallback, Action iOnDeleteAction, Action<GameTime> iOnEnterCallback)
        {
            _bounds = iBounds;
            _onLetterClickedCallback = iOnLetterClickedCallback;
            _onDeleteAction = iOnDeleteAction;
            _onEnterCallback = iOnEnterCallback;
            _isAcceptingInput = true;

            var keyboardMargin = SettingsManager.KeyboardSettings.KeyboardMarginAsPercentage * _bounds.Width;
            var keyMargin = SettingsManager.KeyboardSettings.KeyMarginAsPercentage * _bounds.Width;
            var keyWidth = (_bounds.Width - (2.0f * keyboardMargin) - (20.0f * keyMargin)) / 10.0f;
            var keyHeight = (_bounds.Height - (2.0f * keyboardMargin) - (6.0f * keyMargin)) / 3.0f;

            var topRowButtons = CreateTopRow(keyboardMargin, keyMargin, keyWidth, keyHeight);
            var middleRowButtons = CreateMiddleRow(keyboardMargin, keyMargin, keyWidth, keyHeight);
            var bottomRowButtons = CreateBottomRow(keyboardMargin, keyMargin, keyWidth, keyHeight);

            _buttons = new List<UiTextButton>();
            _buttons.AddRange(topRowButtons);
            _buttons.AddRange(middleRowButtons);
            _buttons.AddRange(bottomRowButtons);
        }

        public void Update(GameTime iGameTime)
        {
            if (_isAcceptingInput)
                _buttons.ForEach(iB => iB.Update(iGameTime));
        }

        public void Draw(Vector2? iOffset = null)
        {
            _buttons.ForEach(iB => iB.Draw(iOffset));
        }

        public void SetDispositionForKey(char iKeyChar, Disposition iDisposition)
        {
            var matchingButton = _buttons.FirstOrDefault(iB => iB.GetText() == iKeyChar.ToString());

            if (matchingButton != null)
            {
                var currentDisposition = matchingButton.GetDisposition();
                if (currentDisposition == Disposition.Undecided ||
                    currentDisposition == Disposition.Incorrect)
                {
                    matchingButton.SetDisposition(iDisposition);
                }
                else if (currentDisposition == Disposition.Misplaced)
                {
                    if (iDisposition == Disposition.Correct)
                        matchingButton.SetDisposition(iDisposition);
                }
            }
            else
            {
                Debug.Fail($"Couldn't find matching key for char {iKeyChar}");
            }
        }

        public void Reset()
        {
            _buttons.ForEach(iB => iB.SetDisposition(Disposition.Undecided));
        }

        public void TurnOnInput()
        {
            _isAcceptingInput = true;
        }

        public void TurnOffInput()
        {
            _isAcceptingInput = false;
        }

        private readonly Rectangle _bounds;

        private readonly List<UiTextButton> _buttons;
        private readonly Action<string> _onLetterClickedCallback;
        private readonly Action _onDeleteAction;
        private readonly Action<GameTime> _onEnterCallback;
        private bool _isAcceptingInput;

        private List<UiTextButton> CreateTopRow(float iKeyboardMargin, float iKeyMargin, float iKeyWidth, float iKeyHeight)
        {
            var keyStrings = new List<string>{ "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
            var buttons = new List<UiTextButton>();

            for (var ii = 0; ii < keyStrings.Count; ii++)
            {
                var thisKeyString = keyStrings[ii];
                
                var thisButton = new UiTextButton(
                    new Point((int)(_bounds.X + iKeyboardMargin + iKeyMargin + (ii * (iKeyMargin + iKeyWidth + iKeyMargin))), (int)(_bounds.Y + iKeyboardMargin + iKeyMargin)),
                    (int)iKeyWidth,
                    (int)iKeyHeight,
                    thisKeyString,
                    iGt => OnKeyPressed(thisKeyString, iGt));

                buttons.Add(thisButton);
            }

            return buttons;
        }
        
        private List<UiTextButton> CreateMiddleRow(float iKeyboardMargin, float iKeyMargin, float iKeyWidth, float iKeyHeight)
        {
            var keyStrings = new List<string> { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
            var buttons = new List<UiTextButton>();

            for (var ii = 0; ii < keyStrings.Count; ii++)
            {
                var thisKeyString = keyStrings[ii];

                var thisButton = new UiTextButton(
                    new Point((int)(_bounds.X + iKeyboardMargin + iKeyMargin + (iKeyWidth / 2.0F) + (ii * (iKeyMargin + iKeyWidth + iKeyMargin))), (int)(_bounds.Y + iKeyboardMargin + (3 * iKeyMargin) + iKeyHeight)),
                    (int)iKeyWidth,
                    (int)iKeyHeight,
                    thisKeyString,
                    iGt => OnKeyPressed(thisKeyString, iGt));

                buttons.Add(thisButton);
            }

            return buttons;
        }

        private List<UiTextButton> CreateBottomRow(float iKeyboardMargin, float iKeyMargin, float iKeyWidth, float iKeyHeight)
        {
            var keyStrings = new List<string> { "EN", "Z", "X", "C", "V", "B", "N", "M", "DEL" };
            var buttons = new List<UiTextButton>();

            var specialButtonWidths = 3.0f * iKeyWidth / 2.0f;
            buttons.Add(new UiTextButton(
                new Point((int)(_bounds.X + iKeyboardMargin + iKeyMargin), (int)(_bounds.Y + iKeyboardMargin + (5 * iKeyMargin) + (2 * iKeyHeight))),
                (int)specialButtonWidths,
                (int)iKeyHeight,
                keyStrings[0],
                iGt => OnKeyPressed(keyStrings[0], iGt)));

            for (var ii = 1; ii < keyStrings.Count - 1; ii++)
            {
                var thisKeyString = keyStrings[ii];

                var thisButton = new UiTextButton(
                    new Point((int)(_bounds.X + iKeyboardMargin + (3 * iKeyMargin) + specialButtonWidths + ((ii - 1) * (iKeyMargin + iKeyWidth + iKeyMargin))), (int)(_bounds.Y + iKeyboardMargin + (5 * iKeyMargin) + (2 * iKeyHeight))),
                    (int)iKeyWidth,
                    (int)iKeyHeight,
                    thisKeyString,
                    iGt => OnKeyPressed(thisKeyString, iGt));

                buttons.Add(thisButton);
            }

            buttons.Add(new UiTextButton(
                new Point((int)(_bounds.X + iKeyboardMargin + (3 * iKeyMargin) + specialButtonWidths + (7 * (iKeyMargin + iKeyWidth + iKeyMargin))), (int)(_bounds.Y + iKeyboardMargin + (5 * iKeyMargin) + (2 * iKeyHeight))),
                (int)specialButtonWidths,
                (int)iKeyHeight,
                keyStrings[8],
                iGt => OnKeyPressed(keyStrings[8], iGt)));

            return buttons;
        }

        private void OnKeyPressed(string iKeyString, GameTime iGameTime)
        {
            if (iKeyString == "DEL")
                _onDeleteAction();
            else if (iKeyString == "EN")
                _onEnterCallback(iGameTime);
            else
                _onLetterClickedCallback(iKeyString);
        }
    }
}