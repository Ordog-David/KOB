using UnityEngine;

public class DusiiSecret : MonoBehaviour
{
    [SerializeField] public bool good;
    [SerializeField] private Vector3 goodTeleportPosition;
    [SerializeField] private Vector3 badTeleportPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && good == true)
        {
            collision.transform.position = goodTeleportPosition;
            
        }
        else if (collision.name == "Player" && good == false)
        {
            collision.transform.position = badTeleportPosition;
        }
    }
}
