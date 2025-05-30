using System;
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
        private List<IGridCellData> _highlightedCells;
        private Dictionary<IGridCellData, GridCellView> _linkedGridCells;
        
        //  METHODS

#region IMediator implementations

        public void OnRegister()
        {
            _view.Initialize(this, 3);
            
            _highlightedCells = new List<IGridCellData>();
            _linkedGridCells  = new Dictionary<IGridCellData, GridCellView>();

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
        }

        private void RemoveListeners()
        {
            GameEvents.OnGameStarted -= OnGameStarted;

            ViewEvents.OnSelectDraggableObject      -= OnDraggableObjectSelected;
            ViewEvents.OnDeselectDraggableObject    -= OnDraggableObjectDeselected;
        }

        private void OnGameStarted()
        {
            _model.CreateGrid(3, 3);
            
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
            if (_highlightedCells.Count <= 0)
            {
                ClearHighlightedCells();
                _view.CancelObjectDrag();
            }
            else
            {
                for (int i = 0; i < _highlightedCells.Count; i++)
                {
                    IGridCellData highlightedCell = _highlightedCells[i];
                    _model.SetGridCellFilled(highlightedCell, true);
                }
            
                ClearHighlightedCells();
                _view.EndObjectDrag(draggableObjectId);

                List<GridCellData> cornersToBeFilled = _model.GetCornerCellsToBeFilled();
                foreach (GridCellData corner in cornersToBeFilled)
                {
                    _view.SetGridCellFilled(corner.Col, corner.Row, corner.IsFilled);
                }
                
                List<GridCellData> squaresToBeFilled = _model.GetSquareCellsToBeFilled();
                foreach (GridCellData square in squaresToBeFilled)
                {
                    _view.SetGridCellFilled(square.Col, square.Row, square.IsFilled);
                }
                
                HashSet<IGridCellData> cellsToClear = _model.ClearCompleteRowsAndColumns();
                if (cellsToClear.Count > 0)
                {
                    foreach (IGridCellData cellToClear in cellsToClear)
                    {
                        _view.SetGridCellFilled(cellToClear.Col, cellToClear.Row, false);
                        if (_linkedGridCells.ContainsKey(cellToClear))
                        {
                            _view.HideDraggableObjectGridCell(_linkedGridCells[cellToClear]);
                        }
                    }
                    
                    List<IGridCellData> verticalAndHorizontalGapsToClear = _model.ClearVerticalAndHorizontalGaps();
                    if (verticalAndHorizontalGapsToClear.Count > 0)
                    {
                        foreach (IGridCellData gapToClear in verticalAndHorizontalGapsToClear)
                        {
                            _view.SetGridCellFilled(gapToClear.Col, gapToClear.Row, false);
                            if (_linkedGridCells.ContainsKey(gapToClear))
                            {
                                _view.HideDraggableObjectGridCell(_linkedGridCells[gapToClear]);
                            }
                        }
                    }
                
                    List<IGridCellData> cornersToClear = _model.ClearCorners();
                    if (cornersToClear.Count > 0)
                    {
                        foreach (IGridCellData cornerToClear in cornersToClear)
                        {
                            _view.SetGridCellFilled(cornerToClear.Col, cornerToClear.Row, false);
                        }
                    }
                }
            }
        }

#region IGameMediator implementations

        public void ClearHighlightedCells()
        {
            _highlightedCells.Clear();
            _linkedGridCells.Clear();
        }
        
        public bool CheckCellUnderneath(int nearestCellCol, int nearestCellRow, GridCellShapes draggingObjectCellShape)
        {
            IGridCellData nearestCellData = _model.GetGridCell(nearestCellCol, nearestCellRow);
            if (nearestCellData.IsFilled)
            {
                return false;
            }

            if (nearestCellData.Shape != draggingObjectCellShape)
            {
                return false;
            }

            _highlightedCells.Add(nearestCellData);
            return true;
        }

        public void LinkCells(int nearestCellCol, int nearestCellRow, GridCellView gridCellView)
        {
            IGridCellData nearestCellData = _model.GetGridCell(nearestCellCol, nearestCellRow);
            _linkedGridCells.Add(nearestCellData, gridCellView);
        }

#endregion
    }
}