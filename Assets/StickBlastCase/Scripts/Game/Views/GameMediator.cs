using System.Collections.Generic;
using Com.Bit34Games.Director.Mediation;
using StickBlastCase.Game.Constants;
using StickBlastCase.Game.Models;
using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public class GameMediator : IMediator, 
                                IGameMediator
    {
        //  MEMBERS
        [Inject] private IGameView  _view;
        [Inject] private GameModel  _model;
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
            _model.CreateGrid(3, 3);
            
            //_view.CreateGrid(colCount, rowCount);
            _view.SetupGrid(_model.GridColCount, _model.GridRowCount);
            
            for (int row = 0; row < _model.GridRowCount; row++)
            {
                for (int col = 0; col < _model.GridColCount; col++)
                {
                    if (row % 2 == 0) // Horizontal gap row
                    {
                        if (col % 2 == 0) // Corner
                        {
                            _model.SetGridCellShape(col, row, GridCellShapes.Corner);
                            _view.AddGridCell(col, row, GridCellShapes.Corner);
                        }
                        else // Horizontal gap
                        {
                            _model.SetGridCellShape(col, row, GridCellShapes.HorizontalGap);
                            _view.AddGridCell(col, row, GridCellShapes.HorizontalGap);
                        }
                    }
                    else // Square row
                    {
                        if (col % 2 == 0) // Vertical gap
                        {
                            _model.SetGridCellShape(col, row, GridCellShapes.VerticalGap);
                            _view.AddGridCell(col, row, GridCellShapes.VerticalGap);
                        }
                        else // Square
                        {
                            _model.SetGridCellShape(col, row, GridCellShapes.Square);
                            _view.AddGridCell(col, row, GridCellShapes.Square);
                        }
                    }
                }
            }
            
            _view.AddDraggableObjects();
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