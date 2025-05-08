using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GridGapView : MonoBehaviour,
                               IGridGapView
    {
        //  MEMBERS
        [SerializeField] private Image _image;
        
        public GridGapDirections Direction { get; private set; }
        public Transform Transform { get { return transform; } }
        public bool IsFilled { get; private set; }
        public bool IsHighlighted { get; private set; }
        
        private Color _baseColor;
        private static readonly Color HighlightTint = new Color(1f, 1f, 0.5f, 1f); // Light yellow tint

        public void Initialize(Color color)
        {
            _baseColor = color;
            if (!IsHighlighted)
            {
                _image.color = _baseColor;
            }
        }

        public void SetDirection(GridGapDirections direction)
        {
            Direction = direction;
        }
        
        public void SetHighlighted(bool state)
        {
            if (IsHighlighted == state) return;

            IsHighlighted = state;

            if (state)
            {
                _image.color = _baseColor * HighlightTint;
            }
            else
            {
                _image.color = _baseColor;
            }
        }

        public void SetFilled(bool state)
        {
            IsFilled = state;
            _image.color = IsFilled ? Color.yellow : Color.gray;
        }

        public void SetSize(float cellSize, float gapThickness)
        {
            var rt = GetComponent<RectTransform>();
            if (Direction == GridGapDirections.Horizontal)
                rt.sizeDelta = new Vector2(cellSize, gapThickness);
            else
                rt.sizeDelta = new Vector2(gapThickness, cellSize);
        }
    }
}