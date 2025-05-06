using UnityEngine;

namespace StickBlastCase.Main.Storage
{
    public class StorageManager : MonoBehaviour,
                                  IStorageManager
    {
        //  METHODS
        public bool UserDataExist(string name)
        {
            return PlayerPrefs.HasKey(name);
        }

        public bool GetOrCreateUserDataAsBool(string name, bool defaultValue)
        {
            if (PlayerPrefs.HasKey(name))
            {
                return PlayerPrefs.GetInt(name) > 0;
            }
            PlayerPrefs.SetInt(name, defaultValue ? 1 : 0);
            return defaultValue;
        }

        public int GetOrCreateUserDataAsInt(string name, int defaultValue)
        {
            if (PlayerPrefs.HasKey(name))
            {
                return PlayerPrefs.GetInt(name);
            }
            PlayerPrefs.SetInt(name, defaultValue);
            return defaultValue;
        }

        public string GetOrCreateUserDataAsString(string name, string defaultValue)
        {
            if (PlayerPrefs.HasKey(name))
            {
                return PlayerPrefs.GetString(name);
            }
            PlayerPrefs.SetString(name, defaultValue);
            return defaultValue;
        }

        public bool GetUserDataAsBool(string name)
        {
            return PlayerPrefs.GetInt(name) > 0;
        }

        public int GetUserDataAsInt(string name)
        {
            return PlayerPrefs.GetInt(name);
        }

        public string GetUserDataAsString(string name)
        {
            return PlayerPrefs.GetString(name);
        }

        public void SetUserDataAsBool(string name, bool value)
        {
            PlayerPrefs.SetInt(name, value ? 1 : 0);
        }

        public void SetUserDataInt(string name, int value)
        {
            PlayerPrefs.SetInt(name, value);
        }

        public void SetUserDataAsString(string name, string value)
        {
            PlayerPrefs.SetString(name, value);
        }

    }
}