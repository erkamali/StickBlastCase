using StickBlastCase.Game.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace StickBlastCase.Game.Views
{
    public class GridCellView : MonoBehaviour, 
                                IGridCellView
    {
        //  MEMBERS
        [SerializeField] private Image background;
        //      Private
        private IGridGapView Top;
        private IGridGapView Bottom;
        private IGridGapView Left;
        private IGridGapView Right;
        
        //  METHODS
        public void SetColor(Color color)
        {
            background.color = color;
        }
        
        public void SetFilled(bool filled)
        {
            background.color = filled ? Color.green : Color.white;
        }
        
        public bool IsSurrounded()
        {
            return Top?.IsFilled == true &&
                   Bottom?.IsFilled == true &&
                   Left?.IsFilled == true &&
                   Right?.IsFilled == true;
        }

        public void SetTop(IGridGapView gap)
        {
            Top = gap;
        }
        
        public void SetBottom(IGridGapView gap)
        {
            Bottom = gap;
        }
        
        public void SetLeft(IGridGapView gap)
        {
            Left = gap;
        }
        
        public void SetRight(IGridGapView gap)
        {
            Right = gap;
        }
    }
}