using Com.Bit34Games.Director.Mediation;

namespace StickBlastCase.Game.Views
{
    public class GameMediator : IMediator, 
                                IGameMediator
    {
        //  MEMBERS
        [Inject] private IGameView  _view;
        
        //  METHODS

#region IMediator implementations

        public void OnRegister()
        {
            _view.Initialize(this, 3);

            AddListeners();
        }

        public void OnRemove()
        {
            RemoveListeners();
        }

#endregion

        private void AddListeners()
        {
            GameEvents.OnGameStarted += OnGameStarted;
        }

        private void RemoveListeners()
        {
            GameEvents.OnGameStarted -= OnGameStarted;
        }

        private void OnGameStarted()
        {
            _view.CreateGrid(5, 5);
        }
    }
}