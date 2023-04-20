using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Vector3 teleportcoordinates;
    [SerializeField] private GameObject NeptuniTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            //NeptuniTrigger.areaColor = false;
            collision.transform.position = teleportcoordinates;
        }
    }
}
