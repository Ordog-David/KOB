using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D playerBody;
    private PlayerGround playerGround;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] private float maxSpeed = 2f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] private float maxAirAcceleration = 80f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] private float maxAirDeceleration = 80f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] private float maxAirTurnSpeed = 80f;

    [Header("Current State")]
    private bool canMove = true;
    public float directionX = 0;
    public Vector2 desiredVelocity = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public bool pressingKey = false;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerGround = GetComponent<PlayerGround>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Mozgato inputkor
        // -1 balra, 0 ha nem nyomod és 1 jobbra
        if (canMove)
        {
            directionX = context.ReadValue<float>();
        }
    }

    private void Update()
    {
        // Ha éppen halott vagy ne mozogj
        if (!canMove)
        {
            directionX = 0;
        }

        // Forduláshoz
        // És irányszámításhoz
        if (directionX != 0)
        {
            float scale = transform.localScale.y;
            transform.localScale = new Vector3(directionX * scale, scale, 1);
            pressingKey = true;
        }
        else
        {
            pressingKey = false;
        }

        // sebesség számítás
        desiredVelocity = new Vector2(directionX, 0f) * maxSpeed;
    }

    private void FixedUpdate()
    {
        velocity = playerBody.velocity;

        // Mozgásfajta
        if (playerGround.IsOnGround())
        {
            RunOnGround();
        }
        else
        {
            RunInAir();
        }
    }

    private void RunOnGround()
    {
        // Földi sebesesség
        velocity.x = desiredVelocity.x;

        playerBody.velocity = velocity;
    }

    private void RunInAir()
    {
        // Levegőben mozgás

        float maxSpeedChange;
        if (pressingKey)
        {
            // If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means
            // we're turning around and so should use the turn speed stat.
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = maxAirTurnSpeed * Time.deltaTime;
            }
            else
            {
                // If they match, it means we're simply running along and so should use the acceleration stat
                maxSpeedChange = maxAirAcceleration * Time.deltaTime;
            }
        }
        else
        {
            // And if we're not pressing a direction at all, use the deceleration stat
            maxSpeedChange = maxAirDeceleration * Time.deltaTime;
        }

        // Gyorsulás
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        // Sebesség átadása a RigidBodynak
        playerBody.velocity = velocity;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }
}
