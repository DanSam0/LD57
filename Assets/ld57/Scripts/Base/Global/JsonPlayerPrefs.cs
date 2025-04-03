using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;


[Serializable]
public class JsonPlayerPrefs
{


    [Serializable]
    class PlayerPref
    {
        public string key;
        public string value;

        public PlayerPref(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }


    private List<PlayerPref> _playerPrefs = new List<PlayerPref>();
    private string _savePath;


    public JsonPlayerPrefs(string savePath)
    {
        _savePath = savePath;

        if (File.Exists(_savePath))
        {
            using (StreamReader reader = new StreamReader(_savePath))
            {
                string json = reader.ReadToEnd();
                JsonPlayerPrefs data = JsonUtility.FromJson<JsonPlayerPrefs>(json);
                _playerPrefs = data._playerPrefs;
            }
        }
    }


    public void DeleteAll()
    {
        _playerPrefs.Clear();
    }


    public void DeleteKey(string key)
    {
        for (int i = _playerPrefs.Count - 1; i >= 0; i--)
        {
            if (_playerPrefs[i].key == key) _playerPrefs.RemoveAt(i);            
        }
    }


    public float GetFloat(string key, float defaultValue = 0f)
    {
        if (TryGetPlayerPref(key, out PlayerPref playerPref))
            if (float.TryParse(playerPref.value, out float value)) return value;
            
        return defaultValue;
    }


    public int GetInt(string key, int defaultValue = 0)
    {
        if (TryGetPlayerPref(key, out PlayerPref playerPref))
            if (int.TryParse(playerPref.value, out int value)) return value;
        
        return defaultValue;
    }


    public string GetString(string key, string defaultValue = "")
    {
        if (TryGetPlayerPref(key, out PlayerPref playerPref)) return playerPref.value;
        
        return defaultValue;
    }


    public bool HasKey(string key)
    {
        foreach(var playerPref in _playerPrefs)
        {
            if (playerPref.key == key) return true;
        }

        return false;
    }


    public void SetValue(string key, string value)
    {
        if (TryGetPlayerPref(key, out PlayerPref playerPref))
            playerPref.value = value; 
        else
            _playerPrefs.Add(new PlayerPref(key, value));        
    }


    private bool TryGetPlayerPref(string key, out PlayerPref playerPref)
    {
        playerPref = null;

        for (int i = 0; i < _playerPrefs.Count; i++)
        {
            if (_playerPrefs[i].key == key)
            {
                playerPref = _playerPrefs[i];
                return true;
            }
        }

        return false;
    }


    public void Save()
    {
        string directory = Path.GetDirectoryName(_savePath);
        Directory.CreateDirectory(directory);

        string json = JsonUtility.ToJson(this);
        using (StreamWriter writer = new StreamWriter(_savePath))
        {
            writer.WriteLine(json);
        }
    }

}
