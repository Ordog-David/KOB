using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class PurpleHiveProjectile : MonoBehaviour
{
    public float speed = 3f;
	public Transform target;
	public PurpleHiveActivator purpleHive;
	[SerializeField] private float rotateSpeed = 200f;
	private Rigidbody2D rigidBody;
	private float timer;
	private Light2D projectileLight;
	private SpriteRenderer projectileRenderer;
	[SerializeField] private float disappearanceTime = 10f;

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		projectileLight = GetComponentInChildren<Light2D>();
		projectileRenderer = GetComponent<SpriteRenderer>();
	}

    private void Update()
    {
		FadeOut();
	}

	void FixedUpdate()
	{
		timer += Time.deltaTime;
		Vector2 direction = (Vector2)target.position - rigidBody.position;
		direction.Normalize();
		float rotateAmount = Vector3.Cross(direction, transform.up).z;
		rigidBody.angularVelocity = -rotateAmount * rotateSpeed;
		rigidBody.velocity = transform.up * speed;
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
		if (collider.gameObject.name != "Player")
        {
			purpleHive.ProjectileMiss();
		}

		// Put a particle effect here
		Destroy(gameObject);

	}
	private void FadeOut()
    {
		projectileRenderer.color = FadeOut(projectileRenderer.color);
		projectileLight.color = FadeOut(projectileLight.color);

		if (projectileLight.color.a < 0.1)
        {
			purpleHive.ProjectileMiss();
			Destroy(gameObject);
        }
	}

    private Color FadeOut(Color color)
    {
		color.a -= Time.deltaTime / disappearanceTime;
		return color;
	}
}
