using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public interface IGridCellView
    {
        //  MEMBERS
        
        //  METHODS
        void SetColor(Color color);
        void SetFilled(bool filled);

        void SetTop(IGridGapView gap);
        void SetBottom(IGridGapView gap);
        void SetLeft(IGridGapView gap);
        void SetRight(IGridGapView gap);
    }
}