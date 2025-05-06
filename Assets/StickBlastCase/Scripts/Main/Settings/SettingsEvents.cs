using System;

namespace StickBlastCase.Main.Settings
{
    public static class SettingsEvents
    {
        public static event Action<bool> OnChangeMusicEnabled;
        public static event Action<bool> OnMusicEnabledChanged;
        public static event Action<bool> OnChangeSoundEnabled;
        public static event Action<bool> OnSoundEnabledChanged;
        public static event Action<bool> OnChangeVibrationEnabled;
        public static event Action<bool> OnVibrationEnabledChanged;

        public static void ChangeMusicEnabled(bool state) { OnChangeMusicEnabled?.Invoke(state); }
        public static void MusicEnabledChanged(bool state) { OnMusicEnabledChanged?.Invoke(state); }
        public static void ChangeSoundEnabled(bool state) { OnChangeSoundEnabled?.Invoke(state); }
        public static void SoundEnabledChanged(bool state) { OnSoundEnabledChanged?.Invoke(state); }
        public static void ChangeVibrationEnabled(bool state) { OnChangeVibrationEnabled?.Invoke(state); }
        public static void VibrationEnabledChanged(bool state) { OnVibrationEnabledChanged?.Invoke(state); }
    }
}