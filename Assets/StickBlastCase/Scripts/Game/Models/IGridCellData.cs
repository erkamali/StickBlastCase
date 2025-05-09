using StickBlastCase.Game.Constants;

namespace StickBlastCase.Game.Models
{
    public interface IGridCellData
    {
        //  MEMBERS
        int             Col     { get; }
        int             Row     { get; }
        GridCellShapes  Shape   { get; }
        
        bool IsHighlighted  { get; }
        bool IsFilled       { get; }
        
        //  METHODS
        void SetShape(GridCellShapes shape);
        void SetHighlighted(bool highlighted);
        void SetFilled(bool filled);
    }
}