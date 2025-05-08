using StickBlastCase.Game.Constants;
using UnityEngine;

namespace StickBlastCase.Game.Views
{
    public interface IGridGapView
    {
        //  MEMBERS
        Transform Transform { get; }
        bool IsFilled { get; }
        
        //  METHODS
        void SetDirection(GridGapDirections direction);
        void SetFilled(bool state);
        void SetSize(float cellSize, float gapThickness);

        void SetHighlighted(bool state);
    }
}