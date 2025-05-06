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

        [SerializeField] private Transform _draggableObjectsContainer;

        //      Private
        private int _colCount;
        private int _rowCount;
        private IGridCellView[,]    _gridCells;
        private IGridGapView[,]     _horizontalGaps;
        private IGridGapView[,]     _verticalGaps;
        
        private IGameMediator _gameMediator;

        //  METHODS
        public void Initialize(IGameMediator gameMediator)
        {
            _gameMediator = gameMediator;
        }
        
        public void CreateGrid(int colCount, int rowCount)
        {
            _colCount = colCount;
            _rowCount = rowCount;
            
            _gridCells = new IGridCellView[_colCount, _rowCount];
            _horizontalGaps = new IGridGapView[_colCount, _rowCount - 1];
            _verticalGaps = new IGridGapView[_colCount - 1, _rowCount];
            
            foreach (Transform child in _gridContainer)
                Destroy(child.gameObject);

            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    GameObject cellGO = Instantiate(_gridCellPrefab, _gridContainer);
                    RectTransform cellRect = cellGO.GetComponent<RectTransform>();
                    cellRect.anchoredPosition = new Vector2(col * (_gridCellSize + _gridGapThickness), row * (_gridCellSize + _gridGapThickness));
                    
                    GridCellView gridCellView = cellGO.GetComponent<GridCellView>();

                    gridCellView.SetColor(Color.white);
                    
                    _gridCells[col, row] = gridCellView;

                    if (row < rowCount - 1)
                    {
                        GameObject gapGO = Instantiate(_gridGapPrefab, _gridContainer);
                        GridGapView gapView = gapGO.GetComponent<GridGapView>();
                        gapView.direction = GridGapDirections.Horizontal;
                        gapView.SetSize(_gridCellSize, _gridGapThickness);
                        gapGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                            col * (_gridCellSize + _gridGapThickness),
                            row * (_gridCellSize + _gridGapThickness) + _gridCellSize / 2 + _gridGapThickness / 2
                        );
                        
                        _horizontalGaps[col, row] = gapView;
                    }

                    if (col < colCount - 1)
                    {
                        GameObject gapGO = Instantiate(_gridGapPrefab, _gridContainer);
                        GridGapView gapView = gapGO.GetComponent<GridGapView>();
                        gapView.direction = GridGapDirections.Vertical;
                        gapView.SetSize(_gridCellSize, _gridGapThickness);
                        gapGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                            col * (_gridCellSize + _gridGapThickness) + _gridCellSize / 2 + _gridGapThickness / 2,
                            row * (_gridCellSize + _gridGapThickness)
                        );
                        
                        _verticalGaps[col, row] = gapView;
                    }
                }
            }
            
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    IGridCellView cell = _gridCells[col, row];

                    // Top gap (only if not topmost row)
                    if (row < _rowCount - 1)
                    {
                        cell.SetTop(_horizontalGaps[col, row]);
                    }

                    // Bottom gap (only if not bottom row)
                    if (row > 0)
                    {
                        cell.SetBottom(_horizontalGaps[col, row - 1]);
                    }

                    // Right gap (only if not rightmost column)
                    if (col < _colCount - 1)
                    {
                        cell.SetRight(_verticalGaps[col, row]);
                    }

                    // Left gap (only if not leftmost column)
                    if (col > 0)
                    {
                        cell.SetLeft(_verticalGaps[col - 1, row]);
                    }
                }
            }
        }
    }
}