using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StickBlastCase.Game.Views
{
    public interface IDraggableObjectView
    {
        //  MEMBERS
        Transform transform { get; }
        //  METHODS
        void Initialize(int id, float width, Vector2 originalPos, Action<int> onSelected, Action<int, PointerEventData> onDragged, Action<int> onDeselected);

        void StartDrag();
        void UpdateDrag(Vector3 screenPosition);
        void CancelDrag();
        void EndDragAndPlace(IGridCellView gridCell);
        IGridCellView GetCell(int cellIndex);
        List<GridCellView> GetCells();
    }
}