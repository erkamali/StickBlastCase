using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GridGapView : MonoBehaviour,
                               IGridGapView
    {
        //  MEMBERS
        [SerializeField] private Image image;
        public GridGapDirections direction;
        
        public bool IsFilled { get; private set; }

        public void SetFilled(bool state)
        {
            IsFilled = state;
            image.color = IsFilled ? Color.yellow : Color.gray;
        }

        public void SetSize(float cellSize, float gapThickness)
        {
            var rt = GetComponent<RectTransform>();
            if (direction == GridGapDirections.Horizontal)
                rt.sizeDelta = new Vector2(cellSize, gapThickness);
            else
                rt.sizeDelta = new Vector2(gapThickness, cellSize);
        }
    }
}