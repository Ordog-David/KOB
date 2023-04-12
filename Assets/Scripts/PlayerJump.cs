using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private const float defaultGravityMultiplier = 1f;

    [Header("Components")]
    private Rigidbody2D playerBody;
    private PlayerGround playerGround;
    private PlayerJuice playerJuice;
    private PlayerMovement playerMovement;

    [Header("Options")]
    [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] private float jumpHeight = 2f;

    // A Jump Duration nem j� Lenti kommnetelt m�dszer alapj�n kell �jra sz�molni 0.2f - 1.25f, to 1 - 10.

    [SerializeField, Range(0.2f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] private float timeToJumpApex = 0.3f;
    [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] private float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] private float downwardMovementMultiplier = 1f;
    [SerializeField][Tooltip("The fastest speed the character can fall")] private float speedLimit = 10f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] private float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] private float jumpBuffer = 0.15f;

    [Header("Current State")]
    private bool desiredJump = false;
    public bool currentlyJumping = false;
    private float jumpBufferCounter = 0f;
    private float coyoteTimeCounter = 0f;
    private float gravityMultiplier = defaultGravityMultiplier;
    public Vector2 velocity = Vector2.zero;

    void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerGround = GetComponent<PlayerGround>();
        playerJuice = GetComponentInChildren<PlayerJuice>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Ugr�s gomb lenyom�sakor
        if (playerMovement.GetCanMove())
        {
            if (context.started)
            {
                desiredJump = true;
            }
        }
    }

    void Update()
    {
        SetPhysics();

        // Jump Buffer
        if (desiredJump)
        {
            jumpBufferCounter += Time.deltaTime;

            if (jumpBufferCounter > jumpBuffer)
            {
                // Jump Buffer lej�r
                desiredJump = false;
                jumpBufferCounter = 0;
            }
        }

        // Zuhan�s �szlel�se �s coyote time
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
        float newGravityY = (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        playerBody.gravityScale = (newGravityY / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void FixedUpdate()
    {
        velocity = playerBody.velocity;

        //Probalkoz�s ugrani
        if (desiredJump)
        {
            StartJumping();
            playerBody.velocity = velocity;

            // At kell ugrani egy frame-re a szam�tast kul�nben double jump bug
            return;
        }

        CalculateGravity();
    }

    private void CalculateGravity()
    {
        // F�lfel�
        if (playerBody.velocity.y > 0.01f)
        {
            if (playerGround.IsOnGround())
            {
                // Ha valami mozg�n �ll
                gravityMultiplier = defaultGravityMultiplier;
            }
            else
            {
                gravityMultiplier = upwardMovementMultiplier;
            }
        }
        // Lefel� mozg�s
        else if (playerBody.velocity.y < -0.01f)
        {
            if (playerGround.IsOnGround())
            {
                // Ha valami mozg�n �ll
                gravityMultiplier = defaultGravityMultiplier;
            }
            else
            {
                // Lefel� es�s
                gravityMultiplier = downwardMovementMultiplier;
            }

        }
        // Egyhelyben �ll�s
        else
        {
            if (playerGround.IsOnGround())
            {
                currentlyJumping = false;
            }

            gravityMultiplier = defaultGravityMultiplier;
        }

        playerBody.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    private void StartJumping()
    {
        // Ugr�s kezd�se
        if (playerGround.IsOnGround() || (coyoteTimeCounter != 0.0f && coyoteTimeCounter < coyoteTime))
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            // Ugr�s ereje
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * playerBody.gravityScale * jumpHeight);

            // Sebess�g hozz�ad�sa
            velocity.y += jumpSpeed;
            currentlyJumping = true;

            if (playerJuice != null)
            {
                // K�s�bbre effekteknek
                playerJuice.PlayJumpEffects();
            }
        }
    }

    // K�s�bbre trambulinnak
    public void BounceUp(float bounceAmount)
    {
        playerBody.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
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