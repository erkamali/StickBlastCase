using StickBlastCase.Main.Storage;

namespace StickBlastCase.Main.Settings
{
    public interface ISettingsManager
    {
        //  METHODS
        void Initialize(IStorageManager storageManager);
        
        bool IsMusicEnabled { get; }
        bool IsSoundEnabled { get; }
        bool IsVibrationEnabled { get; }
    }
}