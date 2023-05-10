using Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedHiveProjectile : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private AudioSource hitSFX;

    private SpriteRenderer projectileRenderer;
    private Collider2D projectileCollider;
    private Light2D projectileLight;

    private void Start()
    {
        projectileRenderer = GetComponent<SpriteRenderer>();
        projectileCollider = GetComponent<CapsuleCollider2D>();
        projectileLight = GetComponentInChildren<Light2D>();
    }

    private void Update()
    {
        transform.position = transform.position + transform.rotation * Vector3.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        projectileRenderer.enabled = false;
        projectileLight.enabled = false;
        projectileCollider.enabled = false;

        var point = Camera.main.WorldToViewportPoint(transform.position);
        if (point.x >= 0f && point.x <= 1f && point.y >= 0f && point.y <= 1f && point.z > 0f)
        {
            this.PlaySoundThen(hitSFX, () => Destroy(gameObject));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
