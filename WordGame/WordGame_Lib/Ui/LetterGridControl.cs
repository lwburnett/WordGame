using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class LetterGridControl : IUiElement
    {
        public LetterGridControl(Rectangle iBounds)
        {
            _cursorLocation = 0;
            _currentRow = 0;

            var gridMargin = SettingsManager.GridSettings.GridMarginAsPercentage * iBounds.Width;
            var cellMargin = SettingsManager.GridSettings.CellMarginAsPercentage * iBounds.Width;
            var cellWidth = (iBounds.Width - (2.0f * gridMargin) - (10.0f * cellMargin)) / 5.0f;
            var cellHeight = cellWidth;

            _cells = CreateCells(gridMargin, cellMargin, cellWidth, cellHeight);
        }

        public void Update(GameTime iGameTime)
        {
            _cells.ForEach(c => c.Update(iGameTime));
        }

        public void Draw()
        {
            _cells.ForEach(c => c.Draw());
        }

        public void LetterPressed(string iKeyString)
        {
            if (_cursorLocation >= (_currentRow + 1) * 5)
                return;

            _cells[_cursorLocation].SetText(iKeyString);
            _cursorLocation++;
        }

        private readonly List<UiLetterCell> _cells;
        private int _cursorLocation;
        private int _currentRow;
        private const int CNumRows = 6;
        private const int CNumCols = 5;

        private List<UiLetterCell> CreateCells(float iGridMargin, float iCellMargin, float iCellWidth, float iCellHeight)
        {
            var cells = new List<UiLetterCell>();

            for (int ii = 0; ii < CNumCols; ii++)
            for (int jj = 0; jj < CNumRows; jj++)
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