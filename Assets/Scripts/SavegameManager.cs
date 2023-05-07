using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SavegameManager : MonoBehaviourSingleton<SavegameManager>
{
    public SavegameData Data = new();
    private readonly HashSet<ISavegameSavedListener> listeners = new();

    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void AddSavegameSavedListener(ISavegameSavedListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveSavegameSavedListener(ISavegameSavedListener listener)
    {
        listeners.Remove(listener);
    }

    private void Load()
    {
        var path = GetSavegamePath();

        try
        {
            var json = File.ReadAllText(path);
            Data = JsonUtility.FromJson<SavegameData>(json);
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"Failed to read from {path} with exception {exception}");
        }
    }

    public void Save()
    {
        var path = GetSavegamePath();

        try
        {
            var json = JsonUtility.ToJson(Data);
            File.WriteAllText(path, json);
            listeners.ToList().ForEach(listener => listener.OnSavegameSaved());
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to write to {path} with exception {exception}");
        }
    }

    public void Clear()
    {
        Data = new();
        Save();
        Debug.Log($"{name} is cleared");
    }

    private string GetSavegamePath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }
}
