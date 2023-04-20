using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : MonoBehaviour
{
    public float speed = 5f;

    // Update is called once per frame
    private void Update()
    {
        transform.position = transform.position + transform.rotation * Vector3.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D _)
    {
        Destroy(gameObject);
    }
}
