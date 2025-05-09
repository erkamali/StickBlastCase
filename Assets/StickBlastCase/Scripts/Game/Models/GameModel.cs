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
    }
}