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
            _view.CreateGrid(3, 3);
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

#endregion
    }
}