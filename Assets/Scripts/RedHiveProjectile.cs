using UnityEngine;

public class RedHiveProjectile : MonoBehaviour
{
    public float speed = 5f;

    // Update is called once per frame
    private void Update()
    {
        transform.position = transform.position + transform.rotation * Vector3.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D _)
    {
        Destroy(gameObject);
    }
}
