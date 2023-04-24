using System;
using UnityEngine;

[Serializable]
public class SavegameData
{
    public string checkpointName;
    public int[] randomDialogueIds = new int[0];
    public string[] visitedDialogueTriggers = new string[0];
    public string[] visitedBlessings = new string[0];
}
