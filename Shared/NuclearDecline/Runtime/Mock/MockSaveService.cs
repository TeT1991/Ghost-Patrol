using UnityEngine;

namespace NuclearDecline
{
    public sealed class MockSaveService : ISaveService
    {
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public float LoadFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string LoadString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public bool LoadBool(string key, bool defaultValue = false)
        {
            int fallback = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(key, fallback) != 0;
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return LoadInt(key, defaultValue);
        }

        public void SetInt(string key, int value)
        {
            SaveInt(key, value);
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return LoadFloat(key, defaultValue);
        }

        public void SetFloat(string key, float value)
        {
            SaveFloat(key, value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return LoadString(key, defaultValue);
        }

        public void SetString(string key, string value)
        {
            SaveString(key, value);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return LoadBool(key, defaultValue);
        }

        public void SetBool(string key, bool value)
        {
            SaveBool(key, value);
        }

        public void Save()
        {
            PlayerPrefs.Save();
            Debug.Log("[NuclearDecline] Mock save flushed.");
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void Load()
        {
            Debug.Log("[NuclearDecline] Mock save loaded from PlayerPrefs.");
        }
    }
}
