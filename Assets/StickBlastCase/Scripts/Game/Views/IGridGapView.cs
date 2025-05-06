namespace StickBlastCase.Game.Views
{
    public interface IGridGapView
    {
        //  MEMBERS
        bool IsFilled { get; }
        
        //  METHODS
        void SetFilled(bool state);
        void SetSize(float cellSize, float gapThickness);
    }
}