using Com.Bit34Games.Director.Unity;
using StickBlastCase.Game;
using UnityEngine;
using StickBlastCase.Main.Settings;
using StickBlastCase.Main.Storage;
using StickBlastCase.Game.Models;
using StickBlastCase.Game.Views;

namespace StickBlastCase.Main
{
    public class MainContext : DirectorUnityContext
    {
        //  MEMBERS
        //      Editor
        [Header("Managers")]
        [SerializeField] private StorageManager     _storageManager;
        [SerializeField] private SettingsManager    _settingsManager;
        [Header("Prefabs")]
        [SerializeField] private GameObject _gameViewPrefab;
        
        //  METHODS
        private void Start()
        {
            InitializeContext();
            StartContext();
        }
        
        protected override void InitializeBindings()
        {
            MediationBinder.BindView<GameView>()    .ToMediator<GameMediator>().As<IGameView>();
            
            Injector.AddBinding<GameModel>()    .ToType<GameModel>();
            Injector.AddBinding<IGameModel>()   .ToType<GameModel>();
        }
        
        protected override void Launch()
        {
            Instantiate(_gameViewPrefab, transform);
            
            GameEvents.StartGame();
        }
    }
}
