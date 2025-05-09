using System.Collections.Generic;
using Com.Bit34Games.Director.Unity;
using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GameView : DirectorUnityView,
                            IGameView
    {
        //  MEMBERS
        //      Editor
        [SerializeField] private float _cellSize;
        [Header("Self References")]
        [SerializeField] private RectTransform  _gridContainer;
        [SerializeField] private Transform      _draggableObjectContainer;
        
        [SerializeField] private GraphicRaycaster _raycaster;
        
        [Header("Resource References")]
        [SerializeField] private GameObject     _gridCellPrefab;
        [SerializeField] private GameObject     _gridGapPrefab;
        [SerializeField] private GameResources  _gameResources;

        //      Private
        private int _colCount;
        private int _rowCount;
        private float _xOffset;
        private float _yOffset;
        private int _draggableObjectCount;
        private IGridCellView[,]    _gridCells;
        
        private int _draggingObjectId;
        private Dictionary<int, IDraggableObjectView> _draggableObjects;
        
        private List<GridCellView> _cellsToHighlight;

        private IGameMediator   _gameMediator;

        //  METHODS
        public void Initialize(IGameMediator gameMediator, int draggableObjectCount)
        {
            _gameMediator = gameMediator;
            
            _draggableObjectCount = draggableObjectCount;
            
            _draggingObjectId = -1;
            _draggableObjects = new Dictionary<int, IDraggableObjectView>();
            
            _cellsToHighlight   = new List<GridCellView>();
        }
        
        public void Clear()
        {
            _draggingObjectId = -1;
            _draggableObjects.Clear();
        }

        public void SetupGrid(int colCount, int rowCount)
        {
            _colCount = colCount;
            _rowCount = rowCount;
            
            _gridCells = new IGridCellView[_colCount, _rowCount];
            
            _xOffset = (_colCount / 2) * _cellSize;
            _yOffset = (_rowCount / 2) * _cellSize;
        }

        public void AddGridCell(int col, int row, GridCellShapes shape)
        {
            GameObject gridCellPrefab = _gameResources.GetGridCellPrefab((int)shape);
            GameObject gridCellGO = Instantiate(gridCellPrefab, _gridContainer);
            gridCellGO.gameObject.name = shape.ToString() + "_col_" + col + "_row_" + row;
            GridCellView gridCell = gridCellGO.GetComponent<GridCellView>();
            gridCell.Initialize(col, row, shape, Color.gray);

            float y = ((row - 0.5f) * _cellSize) - _yOffset;
            float x = (col * _cellSize) - _xOffset;

            gridCellGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                            
            _gridCells[col, row] = gridCell;
        }

        public void AddDraggableObjects()
        {
            float spacing = _cellSize * 2;
            float totalWidth = _draggableObjectCount * spacing;

            RectTransform draggableObjectContainerRectTransform = (RectTransform)_draggableObjectContainer;
            Rect rect = draggableObjectContainerRectTransform.rect;
            float startX = (rect.width - totalWidth) / 2f;

            for (int i = 0; i < _draggableObjectCount; i++)
            {
                GameObject shapeGO = Instantiate(_gameResources.GetShapePrefab(i), _draggableObjectContainer);
                RectTransform rt = shapeGO.GetComponent<RectTransform>();

                float xOffset = startX + i * spacing;
                Vector2 originalPos = new Vector2(xOffset, 0);

                IDraggableObjectView draggableObjectView = shapeGO.GetComponent<IDraggableObjectView>();
                draggableObjectView.Initialize(i, _cellSize, originalPos, OnSelected, OnDragged, OnDeselected);

                _draggableObjects.Add(i, draggableObjectView);
            }
        }

        private void OnSelected(int draggableObjectId)
        {
            ViewEvents.SelectDraggableObject(draggableObjectId);
        }
        
        private void OnDragged(int draggableObjectId, PointerEventData pointerEventData)
        {
            foreach (IGridCellView gridCell in _gridCells)
            {
                gridCell.SetHighlighted(false);
            }
            _cellsToHighlight.Clear();
            _gameMediator.ClearHighlightedCells();
            
            IDraggableObjectView draggingObject = _draggableObjects[draggableObjectId];
            foreach (GridCellView draggingObjectCell in draggingObject.GetCells())
            {
                if (draggingObjectCell.Shape == GridCellShapes.None)
                {
                    continue;
                }
                
                RectTransform cellRect = draggingObjectCell.GetComponent<RectTransform>();
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, cellRect.position);

                GridCellView nearestGridCell = FindNearestGridCell(screenPos);

                if (nearestGridCell == null)
                {
                    break;
                }

                if (_gameMediator.CheckCellUnderneath(nearestGridCell.Col, nearestGridCell.Row, draggingObjectCell.Shape) == false)
                {
                    break;
                }
                
                _cellsToHighlight.Add(nearestGridCell);
            }

            for (int i = 0; i < _cellsToHighlight.Count; i++)
            {
                _cellsToHighlight[i].SetHighlighted(true);
            }
        }

        private void OnDeselected(int draggableObjectId)
        {
            if (_draggingObjectId == draggableObjectId)
            {
                ViewEvents.DeselectDraggableObject(draggableObjectId);
            }
        }
        
        public void StartObjectDrag(int objectId)
        {
            _draggingObjectId = objectId;
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];
            draggableObject.StartDrag();
        }

        public void EndObjectDrag(int objectId)
        {
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];
            draggableObject.EndDragAndPlace(_cellsToHighlight);
            draggableObject.transform.SetParent(_gridContainer.transform);
        }

        public void CancelObjectDrag()
        {
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];
            draggableObject.CancelDrag();
            _draggingObjectId = -1;
        }
        
        private GridCellView FindNearestGridCell(Vector2 screenPosition)
        {
            GridCellView nearestCell = null;
            float shortestDistance = float.MaxValue;

            foreach (GridCellView gridCell in _gridCells)
            {
                RectTransform cellRect = gridCell.transform.gameObject.GetComponent<RectTransform>();

                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    cellRect.parent as RectTransform,
                    screenPosition,
                    null,
                    out localPoint
                );

                float distance = Vector2.Distance(localPoint, cellRect.anchoredPosition);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestCell = gridCell;
                }
            }
            
            if (shortestDistance <= _cellSize / 2f)
            {
                return nearestCell;
            }

            return null;
        }
    }
}