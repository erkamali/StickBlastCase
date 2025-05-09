using System.Collections.Generic;
using StickBlastCase.Game.Constants;

namespace StickBlastCase.Game.Models
{
    public class GameModel : IGameModel
    {
        //  MEMBERS
        public int LevelColCount    { get; private set; }
        public int LevelRowCount    { get; private set; }
        public int GridColCount     { get; private set; }
        public int GridRowCount     { get; private set; }
        //      Private
        private GridCellData[,] _gridCells;
        
        //  METHODS
        public void Load()
        {
             
        }

        public void Unload()
        {
            _gridCells = null;
        }
        
        public void CreateGrid(int colCount, int rowCount)
        {
            LevelColCount = colCount;
            LevelRowCount = rowCount;

            GridColCount = (LevelColCount * 2) + 1;
            GridRowCount = (LevelRowCount * 2) + 1;

            _gridCells = new GridCellData[GridColCount, GridRowCount];

            for (int c = 0; c < GridColCount; c++)
            {
                for (int r = 0; r < GridRowCount; r++)
                {
                    _gridCells[c, r] = new GridCellData(c, r);
                }
            }
        }

        public void SetGridCellShape(int col, int row, GridCellShapes shape)
        {
            GetGridCell(col, row).SetShape(shape);
        }

        public IGridCellData GetGridCell(int col, int row)
        {
            return _gridCells[col, row];
        }

        public void SetGridCellFilled(IGridCellData gridCell, bool filled)
        {
            gridCell.SetFilled(filled);
        }
        
        public List<GridCellData> GetCornerCellsToBeFilled()
        {
            List<GridCellData> cornersToBeFilled = new List<GridCellData>();
            
            for (int c = 0; c < GridColCount; c++)
            {
                for (int r = 0; r < GridRowCount; r++)
                {
                    GridCellData cell = _gridCells[c, r];

                    if (cell.Shape != GridCellShapes.Corner)
                    {
                        continue;
                    }

                    if (cell.IsFilled)
                    {
                        continue;
                    }

                    bool hasFilledNeighbor = false;
                    
                    // Up
                    if (r > 0 && _gridCells[c, r - 1].IsFilled)
                    {
                        hasFilledNeighbor = true;
                    }
                    // Down
                    else if (r < GridRowCount - 1 && _gridCells[c, r + 1].IsFilled)
                    {
                        hasFilledNeighbor = true;
                    }
                    // Left
                    else if (c > 0 && _gridCells[c - 1, r].IsFilled)
                    {
                        hasFilledNeighbor = true;
                    }
                    // Right
                    else if (c < GridColCount - 1 && _gridCells[c + 1, r].IsFilled)
                    {
                        hasFilledNeighbor = true;
                    }

                    if (hasFilledNeighbor)
                    {
                        cell.SetFilled(true);
                        cornersToBeFilled.Add(cell);
                    }
                }
            }

            return cornersToBeFilled;
        }
        
        public List<GridCellData> GetSquareCellsToBeFilled()
        {
            List<GridCellData> squaresToBeFilled = new List<GridCellData>();
            
            for (int c = 0; c < GridColCount; c++)
            {
                for (int r = 0; r < GridRowCount; r++)
                {
                    GridCellData cell = _gridCells[c, r];

                    if (cell.Shape != GridCellShapes.Square)
                    {
                        continue;
                    }

                    if (cell.IsFilled)
                    {
                        continue;
                    }

                    // Up
                    bool upperCellFilled = r > 0 && _gridCells[c, r-1].IsFilled;
                    
                    // Down
                    bool lowerCellFilled = r < GridRowCount - 1 && _gridCells[c, r + 1].IsFilled;
                    
                    // Left
                    bool leftCellFilled = c > 0 && _gridCells[c - 1, r].IsFilled;
                    
                    // Right
                    bool rightCellFilled = c < GridColCount - 1 && _gridCells[c + 1, r].IsFilled;

                    if (upperCellFilled && lowerCellFilled && leftCellFilled && rightCellFilled)
                    {
                        cell.SetFilled(true);
                        squaresToBeFilled.Add(cell);
                    }
                }
            }

            return squaresToBeFilled;
        }
        
        public (List<int> completeRows, List<int> completeCols) ClearCompleteRowsAndColumns()
        {
            List<int> completeRows = new List<int>();
            List<int> completeCols = new List<int>();

            // Find complete rows
            for (int row = 0; row < GridRowCount; row++)
            {
                bool rowComplete = true;

                for (int col = 0; col < GridColCount; col++)
                {
                    IGridCellData cell = _gridCells[col, row];
                    if (cell.Shape == GridCellShapes.Square && !cell.IsFilled)
                    {
                        rowComplete = false;
                        break;
                    }
                }

                if (rowComplete)
                    completeRows.Add(row);
            }

            // Find complete columns
            for (int col = 0; col < GridColCount; col++)
            {
                bool colComplete = true;

                for (int row = 0; row < GridRowCount; row++)
                {
                    IGridCellData cell = _gridCells[col, row];
                    if (cell.Shape == GridCellShapes.Square && !cell.IsFilled)
                    {
                        colComplete = false;
                        break;
                    }
                }

                if (colComplete)
                    completeCols.Add(col);
            }

            // Clear filled squares in complete rows
            foreach (int row in completeRows)
            {
                for (int col = 0; col < GridColCount; col++)
                {
                    IGridCellData cell = _gridCells[col, row];
                    cell.SetFilled(false);
                }
            }

            // Clear filled squares in complete columns
            foreach (int col in completeCols)
            {
                for (int row = 0; row < GridRowCount; row++)
                {
                    IGridCellData cell = _gridCells[col, row];
                    cell.SetFilled(false);
                }
            }

            return (completeRows, completeCols);
        }
    }
}