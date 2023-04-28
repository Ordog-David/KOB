using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavegameManager : MonoBehaviour
{
    private static SavegameManager instance;
    public static SavegameManager Instance {
        get { return instance; }
    }

    public SavegameData Data = new();
    private List<ISavegameSavedListener> listeners = new();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Savegame manager is destroyed");
        }
        else
        {
            instance = this;
            instance.Load();
            DontDestroyOnLoad(gameObject);
            Debug.Log("Savegame manager is loaded");
        }
    }

    public void AddSavegameSavedListener(ISavegameSavedListener listener)
    {
        listeners.Add(listener);
    }

    public void Reset()
    {
        Data = new();
        Save();
        Debug.Log("Savegame manager is resetted");
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

    private string GetSavegamePath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }
}
