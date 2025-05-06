namespace StickBlastCase.Main.Storage
{
    public interface IStorageManager
    {
        bool   UserDataExist(string name);
        bool   GetOrCreateUserDataAsBool(string name, bool defaultValue);
        int    GetOrCreateUserDataAsInt(string name, int defaultValue);
        string GetOrCreateUserDataAsString(string name, string defaultValue);
        bool   GetUserDataAsBool(string name);
        int    GetUserDataAsInt(string name);
        string GetUserDataAsString(string name);
        void   SetUserDataAsBool(string name, bool value);
        void   SetUserDataInt(string name, int value);
        void   SetUserDataAsString(string name, string value);
    }
}