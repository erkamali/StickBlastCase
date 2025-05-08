using System;

namespace StickBlastCase.Game.Views
{
    public class ViewEvents
    {
        public static event Action<int> OnSelectDraggableObject;
        public static event Action<int> OnDeselectDraggableObject;
        public static event Action      OnCancelDraggableObjectDrag;
        
        public static void SelectDraggableObject(int id)    {   OnSelectDraggableObject?.Invoke(id); }
        public static void DeselectDraggableObject(int id)  {   OnDeselectDraggableObject?.Invoke(id); }
        public static void CancelDraggableObjectDrag()      {   OnCancelDraggableObjectDrag?.Invoke(); }
    }
}