using System.Collections.Generic;

namespace StickBlastCase.Game.Views
{
    public interface IGameMediator
    {
        // Checks if dragged object is valid over this gap
        bool CheckDraggableOverGap(int objectId, IGridGapView gap);

        // Returns all gaps this object would cover if dropped here
        List<IGridGapView> GetAffectedGapsIfPlaced(int objectId, IGridGapView gap);
    }
}