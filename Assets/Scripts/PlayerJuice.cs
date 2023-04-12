using UnityEngine;

// This script handles purely aesthetic things like particles
public class PlayerJuice : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator playerAnimator;
    private PlayerGround playerGround;
    private PlayerMovement playerMovement;

    [Header("Components - Particles")]
    //[SerializeField] private ParticleSystem moveParticles;
    //[SerializeField] private ParticleSystem jumpParticles;
    //[SerializeField] private ParticleSystem landParticles;

    [Header("Components - Audio")]
    //[SerializeField] private AudioSource jumpSFX;
    //[SerializeField] private AudioSource landSFX;

    [Header("Current State")]
    public bool playerGrounded = true;

    void Start()
    {
        playerGround = GetComponent<PlayerGround>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // We need to change the character's running animation to suit their current speed
        float movementSpeed = Mathf.Clamp(Mathf.Abs(playerMovement.GetVelocity().x), 0, playerMovement.GetMaxSpeed());
        playerAnimator.SetFloat("MovementSpeed", movementSpeed);

        if (playerGrounded == false && playerGround.IsOnGround() == true)
        {
            // By checking for this, and then immediately setting playerGrounded to true, we only run this code once
            // when the player hits the ground 
            playerGrounded = true;
            PlayLandEffects();
        }
        else if (playerGrounded == true && playerGround.IsOnGround() == false)
        {
            playerGrounded = false;
            PlayWalkEffects();
        }
    }

    private void PlayLandEffects()
    {
        // Play an animation, some particles, and a sound effect when the player lands
        playerAnimator.SetTrigger("Land");
        //landParticles.Play();

        //if (!landSFX.isPlaying && landSFX.enabled)
        //{
        //    landSFX.Play();
        //}

        //moveParticles.Play();
    }

    private void PlayWalkEffects()
    {
        // Player has left the ground, so stop playing the running particles
        //moveParticles.Stop();
    }

    public void PlayJumpEffects()
    {
        // Play these effects when the player jumps. Called from the jump script
        playerAnimator.ResetTrigger("Land");
        playerAnimator.SetTrigger("Jump");

        //if (jumpSFX.enabled)
        //{
        //    jumpSFX.Play();
        //}

        //jumpParticles.Play();
    }
}
