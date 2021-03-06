using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace WordGame_Lib.Ui
{
    public class LetterGridControl : IUiNeonElement
    {
        public LetterGridControl(Rectangle iBounds)
        {
            _cursorLocation = 0;
            _currentRow = 0;

            var gridMargin = SettingsManager.GridSettings.GridMarginAsPercentage * iBounds.Width;
            var cellMargin = SettingsManager.GridSettings.CellMarginAsPercentage * iBounds.Width;
            var cellWidth = (iBounds.Width - (2.0f * gridMargin) - (10.0f * cellMargin)) / 5.0f;
            var cellHeight = cellWidth;

            _rng = new Random();
            _flickerSfx = new List<SoundEffect>
            {
                AssetHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker1")),
                AssetHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker2")),
                AssetHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker3")),
                AssetHelper.LoadContent<SoundEffect>(Path.Combine("Audio", "ElectricFlicker4")),
            };

            _cells = CreateCells(gridMargin, cellMargin, cellWidth, cellHeight);
            LightPoints = new List<PointLight>();
            State = NeonLightState.Off;
        }

        public void Update(GameTime iGameTime)
        {
            if (State == NeonLightState.FadeIn)
            {
                var upperIndexBound = _currentRow * CNumCols;
                var allOn = true;
                for (var ii = 0; ii < upperIndexBound; ii++)
                {
                    if (_cells[ii].State != NeonLightState.On)
                    {
                        allOn = false;
                        break;
                    }
                }

                if (allOn)
                    State = NeonLightState.On;
            }
            else if (State == NeonLightState.FadeOut)
            {
                var upperIndexBound = _currentRow * CNumCols;
                var allOff = true;
                for (var ii = 0; ii < upperIndexBound; ii++)
                {
                    if (_cells[ii].State != NeonLightState.Off)
                    {
                        allOff = false;
                        break;
                    }
                }

                if (allOff)
                    State = NeonLightState.Off;
            }

            _cells.ForEach(iC => iC.Update(iGameTime));
            LightPoints.Clear();
            LightPoints.AddRange(_cells.SelectMany(iC => iC.LightPoints));
        }

        public void Draw(Vector2? iOffset = null)
        {
            _cells.ForEach(iC => iC.Draw(iOffset));
        }

        public void LetterPressed(string iKeyString)
        {
            if (_cursorLocation >= (_currentRow + 1) * CNumCols)
                return;

            _cells[_cursorLocation].SetText(iKeyString);
            _cursorLocation++;
        }

        public void Delete()
        {
            if (_cursorLocation - 1 < _currentRow * CNumCols)
                return;

            _cells[_cursorLocation - 1].SetText(string.Empty);
            _cursorLocation--;
        }

        public string GetCurrentWord()
        {
            var word = string.Empty;

            for (var ii = _currentRow * CNumCols; ii < (_currentRow + 1) * CNumCols; ii++)
            {
                word += _cells[ii].GetText().Trim();
            }

            return word;
        }

        public void Reset(GameTime iGameTime)
        {
            _cells.ForEach(iC =>
            {
                iC.SetDisposition(Disposition.Incorrect);
                iC.SetText(string.Empty);
            });

            _cursorLocation = 0;
            _currentRow = 0;

            TurnOnRow(iGameTime, false);
        }

        public void SetDispositionForNextCell(int iColumn, Disposition iDisposition)
        {
            var thisIndex = _currentRow * CNumCols + iColumn;

            _cells[thisIndex].SetDisposition(iDisposition);

            if ((thisIndex + 1) % CNumCols == 0)
                _currentRow++;

            AudioHelper.PlaySoundEffect(_flickerSfx[_rng.Next(4)], SettingsManager.Sound.FlickerVolume);
        }

        public bool IsFinished()
        {
            return _currentRow >= CNumRows;
        }

        public void StartFadeIn(GameTime iGameTime, TimeSpan iDuration)
        {
            State = NeonLightState.On;
        }

        public void StartFadeOut(GameTime iGameTime, TimeSpan iDuration)
        {
            State = NeonLightState.FadeOut;
            foreach (var cell in _cells)
            {
                if (cell.State == NeonLightState.On)
                    cell.StartFadeOut(iGameTime, iDuration);
            }
        }

        public void TurnOnRow(GameTime iGameTime, bool iMoveCursorToNextRow)
        {
            if (_currentRow >= CNumRows)
                return;

            var startingIndex = _currentRow * CNumCols;
            for (var ii = startingIndex; ii < startingIndex + CNumCols; ii++)
            {
                _cells[ii].SetDisposition(Disposition.Undecided);
                _cells[ii].StartFadeIn(iGameTime, SettingsManager.NeonSettings.VisualTransitionDuration);
            }
        }

        public NeonLightState State { get; private set; }

        public List<PointLight> LightPoints { get; }

        private readonly List<UiLetterCell> _cells;
        private int _cursorLocation;
        private int _currentRow;
        private const int CNumRows = 6;
        private const int CNumCols = 5;
        private readonly List<SoundEffect> _flickerSfx;
        private readonly Random _rng;

        private List<UiLetterCell> CreateCells(float iGridMargin, float iCellMargin, float iCellWidth, float iCellHeight)
        {
            var cells = new List<UiLetterCell>();

            for (int ii = 0; ii < CNumRows; ii++)
            for (int jj = 0; jj < CNumCols; jj++)
            {
                var thisCellRectangle = new Rectangle(
                    (int)(iGridMargin + iCellMargin + (jj * (iCellMargin + iCellWidth + iCellMargin))),
                    (int)(iGridMargin + iCellMargin + (ii * (iCellMargin + iCellHeight + iCellMargin))),
                    (int)iCellWidth,
                    (int)iCellHeight);

                cells.Add(new UiLetterCell(thisCellRectangle));
            }

            return cells;
        }
    }
}