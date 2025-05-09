using System.Collections.Generic;
using StickBlastCase.Game.Constants;
using UnityEngine;

namespace StickBlastCase.Game.Models
{
    public class GameModel : IGameModel
    {
        //  MEMBERS
        public int LevelColCount { get; private set; }
        public int LevelRowCount { get; private set; }
        public int GridColCount { get; private set; }

        public int GridRowCount { get; private set; }

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
                    bool upperCellFilled = r > 0 && _gridCells[c, r - 1].IsFilled;

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

        public HashSet<IGridCellData> ClearCompleteRowsAndColumns()
        {
            List<int> completeRows = new List<int>();
            List<int> completeCols = new List<int>();
            HashSet<IGridCellData> cellsToClear = new HashSet<IGridCellData>();

            // Find complete rows
            for (int row = 0; row < GridRowCount; row++)
            {
                if (row % 2 == 0)
                {
                    continue;
                }

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
                {
                    completeRows.Add(row);
                    for (int col = 0; col < GridColCount; col++)
                    {
                        IGridCellData cell = _gridCells[col, row];
                        cellsToClear.Add(cell);
                    }
                }
            }

            // Find complete columns
            for (int col = 0; col < GridColCount; col++)
            {
                if (col % 2 == 0)
                {
                    continue;
                }

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
                {
                    completeCols.Add(col);
                    for (int row = 0; row < GridRowCount; row++)
                    {
                        IGridCellData cell = _gridCells[col, row];
                        cellsToClear.Add(cell);
                    }
                }
            }

            // Clear all collected cells
            foreach (IGridCellData cell in cellsToClear)
            {
                cell.SetFilled(false);
            }

            return cellsToClear;
        }

        public List<IGridCellData> ClearVerticalAndHorizontalGaps()
        {
            List<IGridCellData> cellsToClear = new List<IGridCellData>();

            // Loop through all the cells in the grid
            for (int c = 0; c < GridColCount; c++)
            {
                for (int r = 0; r < GridRowCount; r++)
                {
                    IGridCellData cell = _gridCells[c, r];

                    // Only check V or H cells
                    if (cell.Shape == GridCellShapes.VerticalGap || cell.Shape == GridCellShapes.HorizontalGap)
                    {
                        bool hasFilledNeighbor = false;

                        // Check orthogonal neighbors (above, below, left, right)
                        foreach (Vector2Int offset in GetNeighbors())
                        {
                            int nc = c + offset.x;
                            int nr = r + offset.y;

                            // Check if the neighboring cell is a square and filled
                            if (nc >= 0 && nc < GridColCount && nr >= 0 && nr < GridRowCount)
                            {
                                IGridCellData neighbor = _gridCells[nc, nr];
                                if (neighbor.Shape == GridCellShapes.Square && neighbor.IsFilled)
                                {
                                    hasFilledNeighbor = true;
                                    break;
                                }
                            }
                        }

                        // If no filled neighbor, mark this V/H cell for clearing
                        if (!hasFilledNeighbor)
                        {
                            cell.SetFilled(false);
                            cellsToClear.Add(cell);
                        }
                    }
                }
            }

            return cellsToClear;
        }

        public List<IGridCellData> ClearCorners()
        {
            List<IGridCellData> cellsToClear = new List<IGridCellData>();

            // Loop through all the cells in the grid
            for (int c = 0; c < GridColCount; c++)
            {
                for (int r = 0; r < GridRowCount; r++)
                {
                    IGridCellData cell = _gridCells[c, r];

                    // Only check Corner (C) cells
                    if (cell.Shape == GridCellShapes.Corner)
                    {
                        bool hasFilledNeighbor = false;

                        // Check orthogonal neighbors (above, below, left, right)
                        foreach (Vector2Int offset in GetNeighbors())
                        {
                            int nc = c + offset.x;
                            int nr = r + offset.y;

                            // Check if the neighboring cell is a V or H cell and is filled
                            if (nc >= 0 && nc < GridColCount && nr >= 0 && nr < GridRowCount)
                            {
                                IGridCellData neighbor = _gridCells[nc, nr];
                                if ((neighbor.Shape == GridCellShapes.VerticalGap ||
                                     neighbor.Shape == GridCellShapes.HorizontalGap) && neighbor.IsFilled)
                                {
                                    hasFilledNeighbor = true;
                                    break;
                                }
                            }
                        }

                        // If no filled V/H neighbor, mark this C cell for clearing
                        if (!hasFilledNeighbor)
                        {
                            cell.SetFilled(false);
                            cellsToClear.Add(cell);
                        }
                    }
                }
            }

            return cellsToClear;
        }

        private List<Vector2Int> GetNeighbors()
        {
            return new List<Vector2Int>
            {
                new Vector2Int(0, -1), // Up
                new Vector2Int(0, 1), // Down
                new Vector2Int(-1, 0), // Left
                new Vector2Int(1, 0) // Right
            };
        }
    }
}