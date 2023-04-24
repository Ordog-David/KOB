using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (IsOpen())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOpen())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    private bool IsOpen()
    {
        return SavegameManager.Instance.Data.visitedBlessings.Length == FindObjectsOfType<Blessing>(true).Length;
    }
}
