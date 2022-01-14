using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class KeyboardControl : IUiElement
    {
        public KeyboardControl(Rectangle iBounds, Action<string> iOnLetterClickedCallback, Action iOnDeleteAction, Action iOnEnterCallback)
        {
            _bounds = iBounds;
            _onLetterClickedCallback = iOnLetterClickedCallback;
            _onDeleteAction = iOnDeleteAction;
            _onEnterCallback = iOnEnterCallback;

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
            _buttons.ForEach(b => b.Update(iGameTime));
        }

        public void Draw()
        {
            _buttons.ForEach(b => b.Draw());
        }

        public void OnGuessEntered(string iCurrentWord, List<Disposition> iDispositions)
        {
            Debug.Assert(iCurrentWord.Length == iDispositions.Count);

            for (var ii = 0; ii < iCurrentWord.Length; ii++)
            {
                var thisGuessChar = iCurrentWord[ii].ToString();
                var thisDisposition = iDispositions[ii];

                var matchingButton = _buttons.FirstOrDefault(b => b.GetText() == thisGuessChar);
                Debug.Assert(matchingButton != null);
                var currentDisposition = matchingButton.GetDisposition();
                if (currentDisposition == Disposition.Undecided ||
                    currentDisposition == Disposition.Incorrect)
                {
                    matchingButton.SetDisposition(thisDisposition);
                }
                else if (currentDisposition == Disposition.Misplaced)
                {
                    if (thisDisposition == Disposition.Correct)
                        matchingButton.SetDisposition(thisDisposition);
                }
            }
        }

        private readonly Rectangle _bounds;

        private readonly List<UiTextButton> _buttons;
        private readonly Action<string> _onLetterClickedCallback;
        private readonly Action _onDeleteAction;
        private readonly Action _onEnterCallback;

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
                    () => OnKeyPressed(thisKeyString));

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
                    () => OnKeyPressed(thisKeyString));

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
                () => OnKeyPressed(keyStrings[0])));

            for (var ii = 1; ii < keyStrings.Count - 1; ii++)
            {
                var thisKeyString = keyStrings[ii];

                var thisButton = new UiTextButton(
                    new Point((int)(_bounds.X + iKeyboardMargin + (3 * iKeyMargin) + specialButtonWidths + ((ii - 1) * (iKeyMargin + iKeyWidth + iKeyMargin))), (int)(_bounds.Y + iKeyboardMargin + (5 * iKeyMargin) + (2 * iKeyHeight))),
                    (int)iKeyWidth,
                    (int)iKeyHeight,
                    thisKeyString,
                    () => OnKeyPressed(thisKeyString));

                buttons.Add(thisButton);
            }

            buttons.Add(new UiTextButton(
                new Point((int)(_bounds.X + iKeyboardMargin + (3 * iKeyMargin) + specialButtonWidths + (7 * (iKeyMargin + iKeyWidth + iKeyMargin))), (int)(_bounds.Y + iKeyboardMargin + (5 * iKeyMargin) + (2 * iKeyHeight))),
                (int)specialButtonWidths,
                (int)iKeyHeight,
                keyStrings[8],
                () => OnKeyPressed(keyStrings[8])));

            return buttons;
        }

        private void OnKeyPressed(string iKeyString)
        {
            if (iKeyString == "DEL")
                _onDeleteAction();
            else if (iKeyString == "EN")
                _onEnterCallback();
            else
                _onLetterClickedCallback(iKeyString);
        }
    }
}