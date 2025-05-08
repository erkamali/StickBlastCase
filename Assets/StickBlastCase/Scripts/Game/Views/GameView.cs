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
        [SerializeField] private float          _gridCellSize;
        [SerializeField] private float          _gridGapThickness;
        
        [Header("Self References")]
        [SerializeField] private RectTransform  _gridContainer;
        [SerializeField] private Transform      _draggableObjectContainer;
        
        [SerializeField] private GraphicRaycaster _raycaster;
        
        [Header("Resource References")]
        [SerializeField] private GameObject     _gridCellPrefab;
        [SerializeField] private GameObject     _gridGapPrefab;
        [SerializeField] private GameResources _gameResources;

        //      Private
        private int _colCount;
        private int _rowCount;
        private int _draggableObjectCount;
        private IGridCellView[,]    _gridCells;
        private IGridGapView[,]     _horizontalGaps;
        private IGridGapView[,]     _verticalGaps;
        
        private int _draggingObjectId;
        private Dictionary<int, IDraggableObjectView> _draggableObjects;
        private List<IGridGapView> _highlightedGaps;

        private IGameMediator   _gameMediator;

        //  METHODS
        public void Initialize(IGameMediator gameMediator, int draggableObjectCount)
        {
            _gameMediator = gameMediator;
            
            _draggableObjectCount = draggableObjectCount;
            
            _draggingObjectId = -1;
            _draggableObjects = new Dictionary<int, IDraggableObjectView>();
            
            _highlightedGaps = new List<IGridGapView>();
        }
        
        public void Clear()
        {
            _draggingObjectId = -1;
            _draggableObjects.Clear();
        }
        
        public void CreateGrid(int colCount, int rowCount)
        {
            _colCount = colCount;
            _rowCount = rowCount;
            
            _gridCells = new IGridCellView[_colCount, _rowCount];
            _horizontalGaps = new IGridGapView[_colCount, _rowCount + 1];   // Include top-most and bottom-most boundaries 
            _verticalGaps = new IGridGapView[_colCount + 1, _rowCount];     // Include left-most and right-most boundaries 

            foreach (Transform child in _gridContainer)
                Destroy(child.gameObject);

            float totalWidth = _colCount * (_gridCellSize + _gridGapThickness) - _gridGapThickness;
            float totalHeight = _rowCount * (_gridCellSize + _gridGapThickness) - _gridGapThickness;

            float offsetX = totalWidth / 2;
            float offsetY = totalHeight / 2;

            for (int row = 0; row <= _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    GameObject gapGO = Instantiate(_gridGapPrefab, _gridContainer);
                    GridGapView gapView = gapGO.GetComponent<GridGapView>();
                    gapView.SetDirection(GridGapDirections.Horizontal);
                    gapView.SetSize(_gridCellSize, _gridGapThickness);

                    float y = (row - 0.5f) * (_gridCellSize + _gridGapThickness) - offsetY;
                    float x = col * (_gridCellSize + _gridGapThickness) - offsetX;

                    gapGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                    _horizontalGaps[col, row] = gapView;
                }
            }

            // Create all vertical gaps (between and outside columns)
            for (int col = 0; col <= _colCount; col++)
            {
                for (int row = 0; row < _rowCount; row++)
                {
                    GameObject gapGO = Instantiate(_gridGapPrefab, _gridContainer);
                    GridGapView gapView = gapGO.GetComponent<GridGapView>();
                    gapView.SetDirection(GridGapDirections.Vertical);
                    gapView.SetSize(_gridCellSize, _gridGapThickness);

                    float x = (col - 0.5f) * (_gridCellSize + _gridGapThickness) - offsetX;
                    float y = row * (_gridCellSize + _gridGapThickness) - offsetY;

                    gapGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                    _verticalGaps[col, row] = gapView;
                }
            }

            // Create cells and connect their 4 surrounding gaps
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    GameObject cellGO = Instantiate(_gridCellPrefab, _gridContainer);
                    RectTransform cellRect = cellGO.GetComponent<RectTransform>();
                    cellRect.anchoredPosition = new Vector2(col * (_gridCellSize + _gridGapThickness) - offsetX, row * (_gridCellSize + _gridGapThickness) - offsetY); // Adjust for centering

                    GridCellView gridCellView = cellGO.GetComponent<GridCellView>();
                    gridCellView.SetColor(Color.white);

                    // Assign gaps
                    gridCellView.SetTop(_horizontalGaps[col, row + 1]);
                    gridCellView.SetBottom(_horizontalGaps[col, row]);
                    gridCellView.SetLeft(_verticalGaps[col, row]);
                    gridCellView.SetRight(_verticalGaps[col + 1, row]);

                    _gridCells[col, row] = gridCellView;
                }
            }

            _draggableObjectContainer.transform.localPosition = Vector2.down * _gridCellSize * 5;
            float midIndex = (_draggableObjectCount - 1) / 2;
            for (int i = 0; i < _draggableObjectCount; i++)
            {
                // Create the draggable object and position it at the center of the grid
                GameObject IShapeGO = Instantiate(_gameResources.GetShapePrefab((int)DraggableObjectShapes.I), _draggableObjectContainer);
                RectTransform rt = IShapeGO.GetComponent<RectTransform>();
                Vector2 originalPos = Vector2.right * _gridCellSize * (i - midIndex);
                IDraggableObjectView draggableObjectView = IShapeGO.GetComponent<IDraggableObjectView>();
                draggableObjectView.Initialize(i, _gridGapThickness, _gridCellSize, originalPos, OnSelected, OnDragged, OnDeselected);
                _draggableObjects.Add(i, draggableObjectView);
            }
        }

        private void OnSelected(int draggableObjectId)
        {
            ViewEvents.SelectDraggableObject(draggableObjectId);
        }
        
        private void OnDragged(int draggableObjectId, PointerEventData pointerEventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            _raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == gameObject)
                    continue; // skip self

                if (result.gameObject.GetComponent<IGridGapView>() != null)
                {
                    Debug.Log("Hovering over: " + result.gameObject.name);
                }
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

        public void EndPileDragAndStartPlace(IGridGapView gap)
        {
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];
            draggableObject.EndDragAndPlace(gap);
            _draggingObjectId = -1;
        }
        
        public IGridGapView GetGapAt(int col, int row)
        {
            if (col < 0 || col >= _colCount || row < 0 || row >= _rowCount)
                return null;

            // Try horizontal first
            if (row <= _rowCount && col < _colCount)
                return _horizontalGaps[col, row];

            // Then vertical
            if (col <= _colCount && row < _rowCount)
                return _verticalGaps[col, row];

            return null;
        }
        
        public Vector2Int GetGapGridIndex(IGridGapView gap)
        {
            for (int col = 0; col < _colCount; col++)
            {
                for (int row = 0; row <= _rowCount; row++)
                {
                    if (_horizontalGaps[col, row] == gap)
                        return new Vector2Int(col, row);
                }
            }

            for (int col = 0; col <= _colCount; col++)
            {
                for (int row = 0; row < _rowCount; row++)
                {
                    if (_verticalGaps[col, row] == gap)
                        return new Vector2Int(col, row);
                }
            }

            return new Vector2Int(-1, -1); // Not found
        }
        
        /*
        private void Update()
        {
            if (_draggingObjectId == -1) return;
            
            IDraggableObjectView draggableObject = _draggableObjects[_draggingObjectId];

            // Convert mouse position to local UI space
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridContainer, mousePosition, null, out Vector2 localPoint);

            // Find the gap directly under the mouse (could be any type)
            IGridGapView hoveredGap = FindGapUnderMouse(localPoint);
    
            // Clear previous highlights
            foreach (IGridGapView gap in _highlightedGaps)
            {
                gap.SetHighlighted(false);
            }
            _highlightedGaps.Clear();

            if (hoveredGap != null)
            {
                // Ask mediator which gaps would be affected if this object were placed on hoveredGap
                List<IGridGapView> candidateGaps = _gameMediator.GetAffectedGapsIfPlaced(_draggingObjectId, hoveredGap);

                if (candidateGaps != null && _gameMediator.CheckDraggableOverGap(_draggingObjectId, hoveredGap))
                {
                    foreach (IGridGapView gap in candidateGaps)
                    {
                        gap.SetHighlighted(true);
                        _highlightedGaps.Add(gap);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        EndPileDragAndStartPlace(hoveredGap);
                    }

                    return;
                }
            }

            // If we reach here, no valid placement found or mouse not over any gap
            if (Input.GetMouseButtonUp(0))
            {
                ViewEvents.CancelDraggableObjectDrag();
            }
        }
        */
        
        private IGridGapView FindGapUnderMouse(Vector2 localPoint)
        {
            const float detectionRadius = 10f;
            float closestDistance = float.MaxValue;
            IGridGapView closestGap = null;

            foreach (IGridGapView gap in _horizontalGaps)
            {
                if (gap == null) continue;
                RectTransform rt = ((Component)gap).GetComponent<RectTransform>();
                float dist = Vector2.Distance(rt.anchoredPosition, localPoint);
                if (dist < detectionRadius && dist < closestDistance)
                {
                    closestDistance = dist;
                    closestGap = gap;
                }
            }

            foreach (IGridGapView gap in _verticalGaps)
            {
                if (gap == null) continue;
                RectTransform rt = ((Component)gap).GetComponent<RectTransform>();
                float dist = Vector2.Distance(rt.anchoredPosition, localPoint);
                if (dist < detectionRadius && dist < closestDistance)
                {
                    closestDistance = dist;
                    closestGap = gap;
                }
            }

            return closestGap;
        }
    }
}