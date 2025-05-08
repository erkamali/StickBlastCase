using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public interface IGameView
    {
        //  METHODS
        void Initialize(IGameMediator gameMediator, int draggableObjectCount);
        void CreateGrid(int colCount, int rowCount);
        
        void StartObjectDrag(int draggableObjectId);
        void EndObjectDrag(int draggableObjectId);
        void CancelObjectDrag();
    }
}