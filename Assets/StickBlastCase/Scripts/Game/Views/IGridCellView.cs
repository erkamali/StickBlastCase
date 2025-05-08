using StickBlastCase.Game.Constants;
using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public interface IGridCellView
    {
        //  MEMBERS
        Transform transform { get; }
        int Col { get; }
        int Row { get; }
        GridCellShapes Shape { get; }
        
        //  METHODS
        void Initialize(int column, int row, GridCellShapes shape);
        void SetColor(Color color);
        void SetFilled(bool filled);
    }
}