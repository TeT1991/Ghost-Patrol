namespace NuclearDecline
{
    public interface ISaveService
    {
        void SaveInt(string key, int value);
        int LoadInt(string key, int defaultValue = 0);

        void SaveFloat(string key, float value);
        float LoadFloat(string key, float defaultValue = 0f);

        void SaveString(string key, string value);
        string LoadString(string key, string defaultValue = "");

        void SaveBool(string key, bool value);
        bool LoadBool(string key, bool defaultValue = false);

        bool HasKey(string key);
        void DeleteKey(string key);

        void Save();
        void Load();

        int GetInt(string key, int defaultValue = 0);
        void SetInt(string key, int value);

        float GetFloat(string key, float defaultValue = 0f);
        void SetFloat(string key, float value);

        string GetString(string key, string defaultValue = "");
        void SetString(string key, string value);

        bool GetBool(string key, bool defaultValue = false);
        void SetBool(string key, bool value);
    }
}
