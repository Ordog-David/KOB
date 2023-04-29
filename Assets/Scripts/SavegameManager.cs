using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavegameManager : MonoBehaviourSingleton<SavegameManager>
{
    public SavegameData Data = new();
    private readonly List<ISavegameSavedListener> listeners = new();

    protected override void Initialize()
    {
        Load();
    }

    public void AddSavegameSavedListener(ISavegameSavedListener listener)
    {
        listeners.Add(listener);
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
            listeners.ForEach(listener => listener.OnSavegameSaved());
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
