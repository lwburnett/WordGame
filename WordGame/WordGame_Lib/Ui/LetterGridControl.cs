using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

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

            _cells = CreateCells(gridMargin, cellMargin, cellWidth, cellHeight);
            LightPoints = new List<PointLight>();
        }

        public void Update(GameTime iGameTime)
        {
            _cells.ForEach(c => c.Update(iGameTime));
            LightPoints.Clear();
            LightPoints.AddRange(_cells.SelectMany(c => c.LightPoints));
        }

        public void Draw()
        {
            _cells.ForEach(c => c.Draw());
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

        public void Reset()
        {
            _cells.ForEach(iC =>
            {
                iC.SetDisposition(Disposition.Undecided);
                iC.SetText(string.Empty);
            });

            _cursorLocation = 0;
            _currentRow = 0;
        }

        public List<PointLight> LightPoints { get; }

        private readonly List<UiLetterCell> _cells;
        private int _cursorLocation;
        private int _currentRow;
        private const int CNumRows = 6;
        private const int CNumCols = 5;

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

        public void OnGuessEntered(List<Disposition> iDispositions)
        {
            for (var ii = 0; ii < 5; ii++)
            {
                var thisIndex = _currentRow * CNumCols + ii;
                var thisCell = _cells[thisIndex];

                thisCell.SetDisposition(iDispositions[ii]);
            }

            _currentRow++;
        }

        public bool IsFinished()
        {
            return _currentRow >= CNumRows;
        }
    }
}