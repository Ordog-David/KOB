using UnityEngine;

// This script handles purely aesthetic things like particles
public class PlayerJuice : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator playerAnimator;
    private PlayerMovement playerMovement;

    //[Header("Components - Particles")]
    //[SerializeField] private ParticleSystem moveParticles;
    //[SerializeField] private ParticleSystem jumpParticles;
    //[SerializeField] private ParticleSystem landParticles;

    [Header("Components - Audio")]
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource landSFX;
    [SerializeField] private AudioSource hurtSFX;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource saveSFX;
    [SerializeField] private AudioSource healthResetterSFX;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // We need to change the character's running animation to suit their current speed
        playerAnimator.SetFloat("MovementSpeed", playerMovement.GetWalkSpeed());
    }

    private void OnAnimatorMove()
    {
        if (playerMovement.GetState() == PlayerMovement.PlayerMovementState.Walking)
        {
            playerAnimator.ApplyBuiltinRootMotion();
        }
    }

    public void PlayLandEffects()
    {
        // Play an animation, some particles, and a sound effect when the player lands
        playerAnimator.ResetTrigger("Fall");
        playerAnimator.ResetTrigger("Jump");
        playerAnimator.SetTrigger("Land");

        //landParticles.Play();
        landSFX.Play();

        //moveParticles.Play();
    }

    public void PlayJumpEffects()
    {
        PlayAirEffects();

        // Play these effects when the player jumps. Called from the jump script
        playerAnimator.ResetTrigger("Land");
        playerAnimator.SetTrigger("Jump");

        //jumpParticles.Play();
        jumpSFX.Play();
    }

    public void PlaySwingEffects()
    {
        PlayAirEffects();
        playerAnimator.SetTrigger("Swing");
    }

    public void PlayFallEffects()
    {
        PlayAirEffects();
        playerAnimator.SetTrigger("Fall");
    }

    private void PlayAirEffects()
    {
        // Player has left the ground, so stop playing the running particles
        //moveParticles.Stop();
    }

    public void PlayHurtEffects()
    {
        hurtSFX.Play();
    }

    public void PlayDeathEffects()
    {
        deathSFX.Play();
    }

    public void PlaySaveEffects()
    {
        saveSFX.Play();
    }

    public void PlayHealthResetterEffects()
    {
        healthResetterSFX.Play();
    }
}
