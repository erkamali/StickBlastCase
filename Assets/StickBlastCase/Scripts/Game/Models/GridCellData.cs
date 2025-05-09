using StickBlastCase.Game.Constants;

namespace StickBlastCase.Game.Models
{
    public class GridCellData : IGridCellData
    {
        public int              Col     { get; private set; }
        public int              Row     { get; private set; }
        public GridCellShapes   Shape   { get; private set; }
        
        public bool IsFilled { get; set; }

        public GridCellData(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public void SetShape(GridCellShapes shape)
        {
            Shape = shape;
        }

        public void SetFilled(bool filled)
        {
            IsFilled = filled;
        }
    }
}