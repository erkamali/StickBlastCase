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
            _draggableObjectContainer.transform.localPosition = Vector2.down * _cellSize * 5;
            float midIndex = (_draggableObjectCount - 1) / 2;
            for (int i = 0; i < _draggableObjectCount; i++)
            {
                // Create the draggable object and position it at the center of the grid
                GameObject IShapeGO = Instantiate(_gameResources.GetShapePrefab(i), _draggableObjectContainer);
                RectTransform rt = IShapeGO.GetComponent<RectTransform>();
                Vector2 originalPos = Vector2.right * _cellSize * 2 * (i - midIndex);
                IDraggableObjectView draggableObjectView = IShapeGO.GetComponent<IDraggableObjectView>();
                draggableObjectView.Initialize(i, _cellSize, originalPos, OnSelected, OnDragged, OnDeselected);
                _draggableObjects.Add(i, draggableObjectView);
            }
        }

        /*
        public void CreateGrid(int colCount, int rowCount)
        {
            _colCount = (colCount * 2) + 1;
            _rowCount = (rowCount * 2) + 1;
            
            _gridCells = new IGridCellView[_colCount, _rowCount];
            
            float offsetX = (_colCount / 2) * _cellSize;
            float offsetY = (_rowCount / 2) * _cellSize;

            GameObject gridCellPrefab;
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    if (row % 2 == 0) // Horizontal gap row
                    {
                        if (col % 2 == 0) // Corner
                        {
                            gridCellPrefab = _gameResources.GetGridCellPrefab((int)GridCellShapes.Corner);
                            GameObject gridCellGO = Instantiate(gridCellPrefab, _gridContainer);
                            gridCellGO.gameObject.name = "Corner_col: " + col + ", row: " + row;
                            GridCellView gridCell = gridCellGO.GetComponent<GridCellView>();
                            gridCell.Initialize(col, row, GridCellShapes.Corner, Color.gray);

                            float y = ((row - 0.5f) * _cellSize) - offsetY;
                            float x = (col * _cellSize) - offsetX;

                            gridCellGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                            
                            _gridCells[col, row] = gridCell;
                        }
                        else // Horizontal gap
                        {
                            gridCellPrefab = _gameResources.GetGridCellPrefab((int)GridCellShapes.HorizontalGap);
                            GameObject gridCellGO = Instantiate(gridCellPrefab, _gridContainer);
                            gridCellGO.gameObject.name = "HorizontalGap_col: " + col + ", row: " + row;
                            GridCellView gridCell = gridCellGO.GetComponent<GridCellView>();
                            gridCell.Initialize(col, row, GridCellShapes.HorizontalGap, Color.gray);

                            float y = ((row - 0.5f) * _cellSize) - offsetY;
                            float x = (col * _cellSize) - offsetX;

                            gridCellGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                            
                            _gridCells[col, row] = gridCell;
                        }
                    }
                    else // Square row
                    {
                        if (col % 2 == 0) // Vertical gap
                        {
                            gridCellPrefab = _gameResources.GetGridCellPrefab((int)GridCellShapes.VerticalGap);
                            GameObject gridCellGO = Instantiate(gridCellPrefab, _gridContainer);
                            gridCellGO.gameObject.name = "VerticalGap_col: " + col + ", row: " + row;
                            GridCellView gridCell = gridCellGO.GetComponent<GridCellView>();
                            gridCell.Initialize(col, row, GridCellShapes.VerticalGap, Color.gray);

                            float y = ((row - 0.5f) * _cellSize) - offsetY;
                            float x = (col * _cellSize) - offsetX;

                            gridCellGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                            
                            _gridCells[col, row] = gridCell;
                        }
                        else // Square
                        {
                            gridCellPrefab = _gameResources.GetGridCellPrefab((int)GridCellShapes.Square);
                            GameObject gridCellGO = Instantiate(gridCellPrefab, _gridContainer);
                            gridCellGO.gameObject.name = "Square_col: " + col + ", row: " + row;
                            GridCellView gridCell = gridCellGO.GetComponent<GridCellView>();
                            gridCell.Initialize(col, row, GridCellShapes.Square, Color.gray);

                            float y = ((row - 0.5f) * _cellSize) - offsetY;
                            float x = (col * _cellSize) - offsetX;

                            gridCellGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                            
                            _gridCells[col, row] = gridCell;
                        }
                    }
                }
            }
            
            _draggableObjectContainer.transform.localPosition = Vector2.down * _cellSize * 5;
            float midIndex = (_draggableObjectCount - 1) / 2;
            for (int i = 0; i < _draggableObjectCount; i++)
            {
                // Create the draggable object and position it at the center of the grid
                GameObject IShapeGO = Instantiate(_gameResources.GetShapePrefab(i), _draggableObjectContainer);
                RectTransform rt = IShapeGO.GetComponent<RectTransform>();
                Vector2 originalPos = Vector2.right * _cellSize * 2 * (i - midIndex);
                IDraggableObjectView draggableObjectView = IShapeGO.GetComponent<IDraggableObjectView>();
                draggableObjectView.Initialize(i, _cellSize, originalPos, OnSelected, OnDragged, OnDeselected);
                _draggableObjects.Add(i, draggableObjectView);
            }
        }
        */

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
                
                if (nearestGridCell.Shape != draggingObjectCell.Shape)
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
            draggableObject.CancelDrag();
        }

        public void CancelObjectDrag()
        {
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];
            draggableObject.CancelDrag();
            _draggingObjectId = -1;
        }

        public void EndPileDragAndStartPlace()
        {
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];
            //draggableObject.EndDragAndPlace();
            _draggingObjectId = -1;
        }

        private int EncodeCoordinates(int col, int row)
        {
            return (_colCount * row) + col;
        }
        
        private GridCellView FindNearestGridCell(Vector2 screenPosition)
        {
            GridCellView nearestCell = null;
            float shortestDistance = float.MaxValue;

            foreach (GridCellView gridCell in _gridCells)
            {
                RectTransform cellRect = gridCell.transform.gameObject.GetComponent<RectTransform>();

                // Convert screen point to local position relative to the cell's parent
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