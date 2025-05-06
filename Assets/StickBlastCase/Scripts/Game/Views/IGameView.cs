namespace StickBlastCase.Game.Views
{
    public interface IGameView
    {
        //  METHODS
        void Initialize(IGameMediator gameMediator);
        void CreateGrid(int colCount, int rowCount);
    }
}