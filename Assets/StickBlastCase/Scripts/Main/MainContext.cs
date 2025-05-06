using Com.Bit34Games.Director.Unity;
using StickBlastCase.Main.Settings;
using StickBlastCase.Main.Storage;
using UnityEngine;

namespace StickBlastCase.Main
{
    public class MainContext : DirectorUnityContext
    {
        //  MEMBERS
        //      From Editor
        [Header("Managers")]
        [SerializeField] private StorageManager     _storageManager;
        [SerializeField] private SettingsManager    _settingsManager;
        [Header("Prefabs")]
        [SerializeField] private GameObject _gameViewPrefab;
    }
}
