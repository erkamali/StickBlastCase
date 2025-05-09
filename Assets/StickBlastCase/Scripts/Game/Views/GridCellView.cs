using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GridCellView : MonoBehaviour, 
                                IGridCellView
    {
        //  MEMBERS
        public int              Col             { get; private set; }
        public int              Row             { get; private set; }
        public GridCellShapes   Shape           { get { return _shape; } }
        public bool             IsHighlighted   { get; private set; }
        //      Editor
        [SerializeField] private GridCellShapes _shape;
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _highlightImage;
        //      Private
        private Color _baseColor;
        
        //  METHODS
        public void Initialize(int column, int row, GridCellShapes shape, Color color)
        {
            Col = column;
            Row = row;
            _shape = shape;
            
            _baseColor = color;
            _background.color = _baseColor;
        }

        public void SetHighlighted(bool highlighted)
        {
            if (IsHighlighted == highlighted) return;

            IsHighlighted = highlighted;
            
            _highlightImage.SetActive(highlighted);
        }
        
        public void SetFilled(bool filled)
        {
            _background.color = filled ? Color.green : Color.white;
        }
    }
}