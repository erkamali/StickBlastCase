using StickBlastCase.Game.Constants;

namespace StickBlastCase.Game.Models
{
    public interface IGridCellData
    {
        //  MEMBERS
        int             Col     { get; }
        int             Row     { get; }
        GridCellShapes  Shape   { get; }
        
        //  METHODS
        void SetShape(GridCellShapes shape);
        void SetFilled(bool filled);
    }
}