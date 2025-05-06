using StickBlastCase.Main.Settings.Constants;
using StickBlastCase.Main.Storage;
using UnityEngine;


namespace StickBlastCase.Main.Settings
{
    public class SettingsManager : MonoBehaviour, 
                                   ISettingsManager
    {
        //  MEMBERS
        public bool IsMusicEnabled { get; private set; }
        public bool IsSoundEnabled { get; private set; }
        public bool IsVibrationEnabled { get; private set; }
        //      Private
        private bool            _isInitialized;
        private IStorageManager _storageManager;


        //  METHODS
#region Unity callbacks
    
        public void Initialize(IStorageManager storageManager)
        {
            if (_isInitialized == false)
            {
                _isInitialized = true;
                _storageManager = storageManager;
                
                IsMusicEnabled     = _storageManager.GetOrCreateUserDataAsBool(SettingFields.IsMusicEnabled.ToString(),     true);
                IsSoundEnabled     = _storageManager.GetOrCreateUserDataAsBool(SettingFields.IsSoundEnabled.ToString(),     true);
                IsVibrationEnabled = _storageManager.GetOrCreateUserDataAsBool(SettingFields.IsVibrationEnabled.ToString(), true);

                SettingsEvents.OnChangeMusicEnabled     += SetMusicEnabled;
                SettingsEvents.OnChangeSoundEnabled     += SetSoundEnabled;
                SettingsEvents.OnChangeVibrationEnabled += SetVibrationEnabled;
            }
        }

#endregion

        private void SetMusicEnabled(bool state)
        {
            IsMusicEnabled = state;
            _storageManager.SetUserDataAsBool(SettingFields.IsMusicEnabled.ToString(), IsMusicEnabled);
            SettingsEvents.MusicEnabledChanged(state);
        }

        public void SetSoundEnabled(bool state)
        {
            IsSoundEnabled = state;
            _storageManager.SetUserDataAsBool(SettingFields.IsSoundEnabled.ToString(), IsSoundEnabled);
            SettingsEvents.SoundEnabledChanged(state);
        }

        public void SetVibrationEnabled(bool state)
        {
            IsVibrationEnabled = state;
            _storageManager.SetUserDataAsBool(SettingFields.IsVibrationEnabled.ToString(), IsVibrationEnabled);
            SettingsEvents.VibrationEnabledChanged(state);
        }

    }
}