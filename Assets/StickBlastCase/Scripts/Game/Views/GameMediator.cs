using System.Collections.Generic;
using Com.Bit34Games.Director.Mediation;
using StickBlastCase.Game.Constants;
using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public class GameMediator : IMediator, 
                                IGameMediator
    {
        //  MEMBERS
        [Inject] private IGameView  _view;
        //      Private
        private Dictionary<int, DraggableObjectShapes> _objectShapes;
        
        //  METHODS

#region IMediator implementations

        public void OnRegister()
        {
            _view.Initialize(this, 3);
            
            _objectShapes = new Dictionary<int, DraggableObjectShapes>();

            AddListeners();
        }

        public void OnRemove()
        {
            RemoveListeners();
        }

#endregion

        private void AddListeners()
        {
            GameEvents.OnGameStarted += OnGameStarted;

            ViewEvents.OnSelectDraggableObject      += OnDraggableObjectSelected;
            ViewEvents.OnDeselectDraggableObject    += OnDraggableObjectDeselected;
            ViewEvents.OnCancelDraggableObjectDrag  += OnDraggableObjectDragCancelled;
        }

        private void RemoveListeners()
        {
            GameEvents.OnGameStarted -= OnGameStarted;

            ViewEvents.OnSelectDraggableObject      -= OnDraggableObjectSelected;
            ViewEvents.OnDeselectDraggableObject    -= OnDraggableObjectDeselected;
            ViewEvents.OnCancelDraggableObjectDrag  -= OnDraggableObjectDragCancelled;
        }

        private void OnGameStarted()
        {
            _view.CreateGrid(5, 5);
        }

        private void OnDraggableObjectSelected(int draggableObjectId)
        {
            _view.StartObjectDrag(draggableObjectId);
        }

        private void OnDraggableObjectDeselected(int draggableObjectId)
        {
            _view.EndObjectDrag(draggableObjectId);
        }

        private void OnDraggableObjectDragCancelled()
        {
            _view.CancelObjectDrag();
        }

#region IGameMediator implementations

        public bool CheckDraggableOverGap(int objectId, IGridGapView centerGap)
        {
            List<IGridGapView> affectedGaps = GetAffectedGapsIfPlaced(objectId, centerGap);
            if (affectedGaps == null) return false;

            // Example rule: all affected gaps must be empty (implement IsOccupied or similar)
            foreach (IGridGapView gap in affectedGaps)
            {
                if (gap.IsFilled) return false;
            }

            return true;
        }

#endregion

        public void RegisterDraggableObject(int id, DraggableObjectShapes shape)
        {
            _objectShapes[id] = shape;
        }

        public List<IGridGapView> GetAffectedGapsIfPlaced(int objectId, IGridGapView centerGap)
        {
            if (!_objectShapes.ContainsKey(objectId)) return null;

            DraggableObjectShapes shape = _objectShapes[objectId];
            List<Vector2Int> offsets = GetShapeOffsets(shape);

            List<IGridGapView> affected = new();
            Vector2Int centerIndex = _view.GetGapGridIndex(centerGap);  // You need to implement this

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int targetIndex = centerIndex + offset;
                IGridGapView gap = _view.GetGapAt(targetIndex.x, targetIndex.y);  // Implement this too

                if (gap == null)
                    return null; // Out of bounds or missing

                affected.Add(gap);
            }

            return affected;
        }
        
        private List<Vector2Int> GetShapeOffsets(DraggableObjectShapes shape)
        {
            switch (shape)
            {
                case DraggableObjectShapes.I:
                    return new List<Vector2Int>
                    {
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(0, 2),
                    };
                case DraggableObjectShapes.U:
                    return new List<Vector2Int>
                    {
                        new Vector2Int(-1, 1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1),
                    };
                // Add more shapes here
                default:
                    return new List<Vector2Int> { new Vector2Int(0, 0) };
            }
        }
    }
}