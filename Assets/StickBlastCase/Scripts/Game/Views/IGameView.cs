using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public interface IGameView
    {
        //  METHODS
        void Initialize(IGameMediator gameMediator, int draggableObjectCount);
        void CreateGrid(int colCount, int rowCount);
        IGridGapView GetGapAt(int col, int row);
        Vector2Int GetGapGridIndex(IGridGapView gap);
        
        void StartObjectDrag(int draggableObjectId);
        void EndObjectDrag(int draggableObjectId);
        void CancelObjectDrag();
    }
}