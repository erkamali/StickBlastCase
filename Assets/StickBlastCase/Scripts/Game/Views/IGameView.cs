namespace StickBlastCase.Game.Views
{
    public interface IGameView
    {
        //  METHODS
        void Initialize(IGameMediator gameMediator, int draggableObjectCount);
        void CreateGrid(int colCount, int rowCount);
    }
}