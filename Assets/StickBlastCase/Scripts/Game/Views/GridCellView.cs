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
        public bool             IsFilled        { get; private set; }
        //      Editor
        [SerializeField] private GridCellShapes _shape;
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _highlightImage;
        [SerializeField] private GameObject _filledImage;
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
            
            IsHighlighted = false;
            IsFilled = false;
        }

        public void SetHighlighted(bool highlighted)
        {
            if (IsHighlighted == highlighted) return;

            IsHighlighted = highlighted;
            
            _highlightImage.SetActive(highlighted);
        }
        
        public void SetFilled(bool filled)
        {
            if (IsFilled == filled) return;

            IsFilled = filled;
            
            _filledImage.SetActive(filled);

            if (filled)
            {
                SetHighlighted(false);
            }
        }
    }
}