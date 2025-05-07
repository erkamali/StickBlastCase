using Com.Bit34Games.Director.Unity;
using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GameView : DirectorUnityView,
                            IGameView
    {
        //  MEMBERS
        //      Editor
        [SerializeField] private RectTransform  _gridContainer;
        [SerializeField] private GameObject     _gridCellPrefab;
        [SerializeField] private GameObject     _gridGapPrefab;
        [SerializeField] private float          _gridCellSize;
        [SerializeField] private float          _gridGapThickness;

        [SerializeField] private Transform  _draggableObjectContainer;
        
        [SerializeField] private GameResources _gameResources;

        //      Private
        private int _colCount;
        private int _rowCount;
        private int _draggableObjectCount;
        private IGridCellView[,]    _gridCells;
        private IGridGapView[,]     _horizontalGaps;
        private IGridGapView[,]     _verticalGaps;

        private IGameMediator   _gameMediator;

        //  METHODS
        public void Initialize(IGameMediator gameMediator, int draggableObjectCount)
        {
            _gameMediator = gameMediator;
            
            _draggableObjectCount = draggableObjectCount;
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
                    gapView.direction = GridGapDirections.Horizontal;
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
                    gapView.direction = GridGapDirections.Vertical;
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
                rt.anchoredPosition = Vector2.right * _gridCellSize * (i - midIndex);
                IDraggableObjectView draggableObjectView = IShapeGO.GetComponent<IDraggableObjectView>();
                draggableObjectView.SetSize(_gridGapThickness, _gridCellSize);
            }
        }
    }
}