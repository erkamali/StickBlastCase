using StickBlastCase.Game.Constants;
using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public interface IGameView
    {
        //  METHODS
        void Initialize(IGameMediator gameMediator, int draggableObjectCount);
        void SetupGrid(int colCount, int rowCount);
        void AddGridCell(int col, int row, GridCellShapes shape);
        void AddDraggableObjects();
        
        void StartObjectDrag(int draggableObjectId);
        void EndObjectDrag(int draggableObjectId);
        void CancelObjectDrag();

        void SetGridCellHighlighted(int col, int row, bool highlighted);
        void SetGridCellFilled(int col, int row, bool filled);
        void HideDraggableObjectGridCell(GridCellView draggableObjectGridCell);
    }
}