using System.Collections.Generic;
using Com.Bit34Games.Director.Unity;
using DG.Tweening;
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

            Rect rect = ((RectTransform)_draggableObjectContainer).rect;
            float startX = (rect.width - totalWidth) / 2f;

            float offscreenX = rect.width + _cellSize * 3;

            for (int i = 0; i < _draggableObjectCount; i++)
            {
                int random = Random.Range(0, _gameResources.ShapeCount);
                GameObject shapeGO = Instantiate(_gameResources.GetShapePrefab(random), _draggableObjectContainer);
                RectTransform rt = shapeGO.GetComponent<RectTransform>();

                float finalX = startX + i * spacing;
                Vector2 startPos = new Vector2(offscreenX, 0);
                Vector2 targetPos = new Vector2(finalX, 0);

                rt.anchoredPosition = startPos;

                IDraggableObjectView draggableObjectView = shapeGO.GetComponent<IDraggableObjectView>();
                draggableObjectView.Initialize(i, _cellSize, targetPos, OnSelected, OnDragged, OnDeselected);

                rt.DOAnchorPos(targetPos, 1f)
                    .SetEase(Ease.OutExpo)
                    .SetDelay(i * 0.05f);

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

        public void SetGridCellHighlighted(int col, int row, bool highlighted)
        {
            _gridCells[col, row].SetHighlighted(highlighted);
        }

        public void SetGridCellFilled(int col, int row, bool filled)
        {
            _gridCells[col, row].SetFilled(filled);
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
            
            
            _draggableObjects.Remove(objectId);
            if (_draggableObjects.Count <= 0)
            {
                AddDraggableObjects();
            }
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