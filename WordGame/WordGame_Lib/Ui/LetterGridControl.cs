using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WordGame_Lib.Ui
{
    public class LetterGridControl : IUiElement
    {
        public LetterGridControl(Rectangle iBounds)
        {
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

        private readonly List<UiLetterCell> _cells;

        private List<UiLetterCell> CreateCells(float iGridMargin, float iCellMargin, float iCellWidth, float iCellHeight)
        {
            const int numRows = 5;
            const int numColumns = 6;

            var cells = new List<UiLetterCell>();

            for (int ii = 0; ii < numRows; ii++)
            for (int jj = 0; jj < numColumns; jj++)
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