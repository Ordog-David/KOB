using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private PlayerGround playerGround;
    private PlayerJuice playerJuice;
    private Rigidbody2D playerBody;
    private GrapplingPoint rope;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Movement speed on ground")] private float walkAnimationSpeed = 1f;

    [Header("Jump Stats")]
    [SerializeField, Range(2f, 5.5f)] [Tooltip("Maximum jump height")] private float jumpHeight = 2f;
    [SerializeField, Range(0f, 20f)] [Tooltip("Movement speed in air")] private float jumpSpeed = 5f;

    // A Jump Duration nem jó Lenti kommnetelt módszer alapján kell újra számolni 0.2f - 1.25f, to 1 - 10.
    [SerializeField, Range(0.2f, 1.25f)] [Tooltip("How long it takes to reach that height before coming back down")] private float timeToJumpApex = 0.3f;
    [SerializeField] [Tooltip("The fastest speed the character can fall")] private float fallingSpeedLimit = 10f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("How long should coyote time last?")] private float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("How far from ground should we cache your jump?")] private float jumpBuffer = 0.15f;

    [Header("Current State")]
    private bool canMove = true;
    private float directionX = 0;
    private Vector2 velocity = Vector2.zero;

    public bool desiredJump = false;
    private bool currentlyJumping = false;
    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerGround = GetComponent<PlayerGround>();
        playerJuice = GetComponentInChildren<PlayerJuice>();
        rope = GetComponentInChildren<GrapplingPoint>();
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

    public void OnJump(InputAction.CallbackContext context)
    {
        // Ugrás gomb lenyomásakor
        if (canMove && context.started)
        {
            desiredJump = true;
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
            var scale = transform.localScale.y;
            transform.localScale = new Vector3(directionX * scale, scale, 1);
        }

        SetPhysics();

        // Jump Buffer
        if (desiredJump)
        {
            jumpBufferCounter += Time.deltaTime;

            if (jumpBufferCounter > jumpBuffer)
            {
                // Jump Buffer lejár
                desiredJump = false;
                jumpBufferCounter = 0;
            }
        }

        // Zuhanás észlelése és coyote time
        if (currentlyJumping == false && playerGround.IsOnGround() == false)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }
    }

    private void SetPhysics()
    {
        // Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used
        // later?
        var newGravityY = -2 * jumpHeight / (timeToJumpApex * timeToJumpApex);
        playerBody.gravityScale = newGravityY / Physics2D.gravity.y;
    }

    private void FixedUpdate()
    {
        if (rope.IsEnabled())
        {
            if (desiredJump && CanJump())
            {
                velocity = playerBody.velocity;
                StartJumping();
                playerBody.velocity = velocity;
            }

            if (directionX != 0)
            {
                playerBody.AddForce(Vector2.right * directionX, ForceMode2D.Impulse);
            }
        }
        else
        {
            velocity = playerBody.velocity;

            // Probalkozás ugrani
            if (desiredJump && CanJump())
            {
                StartJumping();
            }
            else
            {
                CalculateGravity();
            }

            CaclulateVelocity();

            // Sebesség átadása a RigidBodynak
            playerBody.velocity = velocity;
        }
    }

    private void CaclulateVelocity()
    {
        // Levegőbeli sebesesség
        velocity.x = directionX * jumpSpeed;
    }

    private void CalculateGravity()
    {
        // Egyhelyben állás
        if (Mathf.Abs(playerBody.velocity.y) < 0.01f)
        {
            if (playerGround.IsOnGround())
            {
                currentlyJumping = false;
            }
        }

        velocity.y = Mathf.Clamp(velocity.y, -fallingSpeedLimit, 100);
    }

    private bool CanJump()
    {
        return playerGround.IsOnGround() || (coyoteTimeCounter != 0.0f && coyoteTimeCounter < coyoteTime) ||
            rope.IsEnabled();
    }

    private void StartJumping()
    {
        // Ugrás kezdése
        desiredJump = false;
        jumpBufferCounter = 0;
        coyoteTimeCounter = 0;
        rope.ReleaseGrapplePoint();

        // Ugrás ereje
        var jumpSpeedY = Mathf.Sqrt(-2f * Physics2D.gravity.y * playerBody.gravityScale * jumpHeight);

        // Sebesség hozzáadása
        velocity.y += jumpSpeedY;
        currentlyJumping = true;

        // Késöbbre effekteknek
        playerJuice.PlayJumpEffects();
    }

    // Késöbbre trambulinnak
    public void BounceUp(float bounceAmount)
    {
        playerBody.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public float GetMaxSpeed()
    {
        return walkAnimationSpeed;
    }

    public Vector2 GetVelocity()
    {
        return new Vector2(directionX == 0? 0f : walkAnimationSpeed, 0f);
    }

    /*
    timeToApexStat = scale(1, 10, 0.2f, 2.5f, numberFromPlatformerToolkit)


      public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

    */
}
