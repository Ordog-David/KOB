using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerMovementState
    {
        Frozen,
        Walking,
        Jumping,
        Swinging,
        Falling
    }

    [Header("Components")]
    private PlayerGround playerGround;
    private PlayerJuice playerJuice;
    private Rigidbody2D playerBody;
    private RopeLaunchPoint rope;

    [Header("Walk Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Movement speed on ground")] private float walkSpeed = 1f;

    [Header("Jump Stats")]
    [SerializeField, Range(0f, 20f)] [Tooltip("Movement speed in air after jumping from ground")] private float jumpSpeedAfterWalking = 5f;
    [SerializeField, Range(2f, 5.5f)] [Tooltip("Maximum jump height")] private float jumpHeight = 2f;
    [SerializeField, Range(0.2f, 1.25f)] [Tooltip("How long it takes to reach that height before coming back down")] private float timeToJumpApex = 0.3f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("How long should coyote time last?")] private float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("How far from ground should we cache your jump?")] private float jumpBuffer = 0.15f;

    [Header("Swing Stats")]
    [SerializeField] [Tooltip("The fastest speed the character can swing")] private float maxSwingSpeed = 20f;

    [Header("Fall Stats")]
    [SerializeField] [Tooltip("The slowest speed the character can fall")] private float minFallingSpeed = 5f;
    [SerializeField] [Tooltip("The fastest speed the character can fall")] private float maxFallingSpeed = 15f;

    [Header("Current State")]
    public PlayerMovementState state;
    public float directionX = 0;
    private bool desiredJump = false;
    
    public Vector2 velocity = Vector2.zero;
    public float jumpSpeed;

    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerGround = GetComponent<PlayerGround>();
        playerJuice = GetComponentInChildren<PlayerJuice>();
        rope = GetComponentInChildren<RopeLaunchPoint>();

        state = playerGround.IsOnGround() ? PlayerMovementState.Walking : PlayerMovementState.Falling;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Mozgato inputkor
        // -1 balra, 0 ha nem nyomod és 1 jobbra
        if (state != PlayerMovementState.Frozen)
        {
            directionX = context.ReadValue<float>();
            jumpSpeed = jumpSpeedAfterWalking;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Ugrás gomb lenyomásakor
        if (context.started && state != PlayerMovementState.Frozen)
        {
            desiredJump = true;
        }
    }

    private void Update()
    {
        // Ha éppen halott vagy ne mozogj
        if (state == PlayerMovementState.Frozen)
        {
            directionX = 0;
        }

        // Forduláshoz
        // És irányszámításhoz
        if (directionX != 0)
        {
            var scale = transform.localScale.y;
            transform.localScale = new Vector3(directionX * scale, scale, 1);

            playerGround.SetDirectionX(directionX);
        }

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
        if (state != PlayerMovementState.Jumping && playerGround.IsOnGround() == false)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        // Gravitáció újraszámolása
        var newGravityY = -2 * jumpHeight / (timeToJumpApex * timeToJumpApex);
        playerBody.gravityScale = newGravityY / Physics2D.gravity.y;

        velocity = playerBody.velocity;
        switch (state)
        {
            case PlayerMovementState.Frozen:
                playerBody.velocity = Vector2.zero;
                break;

            case PlayerMovementState.Walking:
                Walking();
                break;

            case PlayerMovementState.Jumping:
                Jumping();
                break;

            case PlayerMovementState.Swinging:
                Swinging();
                break;

            case PlayerMovementState.Falling:
                Falling();
                break;
        }
    }

    private void Walking()
    {
        if (desiredJump && CanJump())
        {
            StartJumping(jumpSpeedAfterWalking);
        }
        else if (rope.IsStraight() && playerGround.IsOnGround() == false)
        {
            StartSwinging();
        }
        else if (playerGround.IsOnGround() == false && InCoyoteTime() == false)
        {
            StartFalling();
        }
    }

    private void Jumping()
    {
        if (playerGround.IsOnGround())
        {
            StartWalking();
        }
        else
        {
            if (rope.IsStraight())
            {
                StartSwinging();
            }
            else if (velocity.y < -minFallingSpeed)
            {
                StartFalling();
            }
            else
            {
                SetAirVelocity();
            }
        }
    }

    private void Swinging()
    {
        if (playerGround.IsOnGround())
        {
            rope.ReleaseGrapplePoint();
            StartWalking();
        }
        else
        {
            if (desiredJump)
            {
                rope.ReleaseGrapplePoint();
                StartJumping(Mathf.Abs(playerBody.velocity.x));
            }
            else if (rope.IsEnabled() == false)
            {
                StartFalling();
            }
            else if (directionX != 0f && Mathf.Abs(velocity.x) < maxSwingSpeed)
            {
                playerBody.AddForce(Vector2.right * directionX, ForceMode2D.Impulse);
            }
        }
    }

    private void Falling()
    {
        if (playerGround.IsOnGround())
        {
            StartWalking();
        }
        else
        {
            if (rope.IsStraight())
            {
                StartSwinging();
            }
            else
            {
                SetAirVelocity();
                velocity.y = Mathf.Clamp(velocity.y, -maxFallingSpeed, 100);
                playerBody.velocity = velocity;
            }
        }
    }

    private void StartWalking()
    {
        state = PlayerMovementState.Walking;

        // Effektek
        playerJuice.PlayLandEffects();
    }

    private void StartJumping(float speed)
    {
        state = PlayerMovementState.Jumping;

        // Ugrás kezdése
        desiredJump = false;
        jumpBufferCounter = 0;
        coyoteTimeCounter = 0;
        rope.ReleaseGrapplePoint();

        jumpSpeed = speed;

        // Sebesség meghatározása
        velocity.x = directionX * jumpSpeed;
        velocity.y += Mathf.Sqrt(-2f * Physics2D.gravity.y * playerBody.gravityScale * jumpHeight);
        playerBody.velocity = velocity;

        // Effektek
        playerJuice.PlayJumpEffects();
    }

    private void StartSwinging()
    {
        state = PlayerMovementState.Swinging;

        // Effektek
        playerJuice.PlaySwingEffects();
    }

    private void StartFalling()
    {
        state = PlayerMovementState.Falling;

        // Effektek
        playerJuice.PlayFallEffects();
    }

    private void SetAirVelocity()
    {
        // Levegőbeli sebesesség
        velocity.x = directionX * jumpSpeed;
        playerBody.velocity = velocity;
    }

    private bool CanJump()
    {
        return playerGround.IsOnGround() || InCoyoteTime();
    }

    private bool InCoyoteTime()
    {
        return coyoteTimeCounter != 0.0f && coyoteTimeCounter < coyoteTime;
    }

    // Késöbbre trambulinnak
    public void BounceUp(float bounceAmount)
    {
        playerBody.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }

    public void SetFrozen(bool frozen)
    {
        if (frozen)
        {
            state = PlayerMovementState.Frozen;
        }
        else
        {
            state = PlayerMovementState.Walking;
        }
    }

    public PlayerMovementState GetState()
    {
        return state;
    }

    public float GetWalkSpeed()
    {
        return directionX == 0f ? 0f : walkSpeed;
    }
}
