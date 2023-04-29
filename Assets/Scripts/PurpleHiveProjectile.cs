using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PurpleHiveProjectile : MonoBehaviour
{
    public float speed = 3f;
    public float rotateSpeed = 200f;
    public float disappearanceTime = 10f;
    public Transform target;
    public PurpleHiveActivator purpleHive;

    private Rigidbody2D rigidBody;
    private SpriteRenderer projectileRenderer;
    private Light2D projectileLight;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        projectileRenderer = GetComponent<SpriteRenderer>();
        projectileLight = GetComponentInChildren<Light2D>();
    }

    private void Update()
    {
        projectileRenderer.color = FadeOut(projectileRenderer.color);
        projectileLight.color = FadeOut(projectileLight.color);

        if (projectileLight.color.a < 0.1)
        {
            purpleHive.ProjectileMiss();
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        var direction = (Vector2)target.position - rigidBody.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -rotateAmount * rotateSpeed;
        rigidBody.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name != "Player")
        {
            purpleHive.ProjectileMiss();
        }

        // Put a particle effect here
        Destroy(gameObject);

    }

    private Color FadeOut(Color color)
    {
        color.a -= Time.deltaTime / disappearanceTime;
        return color;
    }
}
