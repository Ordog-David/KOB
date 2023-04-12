using System;
using System.IO;
using UnityEngine;

public class SavegameManager : MonoBehaviour
{
    public static SavegameManager Instance { get; private set; }

    public SavegameData Data = new();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Save()
    {
        var path = GetSavegamePath();

        try
        {
            var json = JsonUtility.ToJson(Data);
            File.WriteAllText(path, json);
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to write to {path} with exception {exception}");
        }
    }

    public void Load()
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
