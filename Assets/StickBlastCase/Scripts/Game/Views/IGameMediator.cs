using System.Collections.Generic;
using StickBlastCase.Game.Constants;

namespace StickBlastCase.Game.Views
{
    public interface IGameMediator
    {
        void ClearHighlightedCells();
        bool CheckCellUnderneath(int nearestCellCol, int nearestCellRow, GridCellShapes draggingObjectCellShape);
    }
}