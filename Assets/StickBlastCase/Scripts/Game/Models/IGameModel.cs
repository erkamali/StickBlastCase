namespace StickBlastCase.Game.Models
{
    public interface IGameModel
    {
        //  MEMBERS
        public int LevelColCount    { get; }
        public int LevelRowCount    { get; }
        public int GridColCount     { get; }
        public int GridRowCount     { get; }
        
        //  METHODS
        void CreateGrid(int colCount, int rowCount);
        IGridCellData GetGridCell(int col, int row);
    }
}