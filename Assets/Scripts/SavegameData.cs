using System;
using UnityEngine;

[Serializable]
public class SerializedVector3
{
    public float x;
    public float y;
    public float z;

    public SerializedVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public static implicit operator SerializedVector3(Vector3 vector3)
    {
        return new SerializedVector3(vector3);
    }

    internal Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public override string ToString()
    {
        return $"({x}, {y}, {z})";
    }
}

[Serializable]
public class SavegameData
{
    public bool hasPosition = false;
    public SerializedVector3 position;
}
