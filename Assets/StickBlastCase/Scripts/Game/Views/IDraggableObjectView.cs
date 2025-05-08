using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StickBlastCase.Game.Views
{
    public interface IDraggableObjectView
    {
        //  MEMBERS
        Transform transform { get; }
        //  METHODS
        void Initialize(int id, float width, float height, Vector2 originalPos, Action<int> onSelected, Action<int, PointerEventData> onDragged, Action<int> onDeselected);

        void StartDrag();
        void UpdateDrag(Vector3 screenPosition);
        void CancelDrag();
        void EndDragAndPlace(IGridGapView gap);
    }
}