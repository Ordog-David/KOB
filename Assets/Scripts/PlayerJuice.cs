using System;
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

    //[Header("Components - Audio")]
    //[SerializeField] private AudioSource jumpSFX;
    //[SerializeField] private AudioSource landSFX;

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

    internal void PlayFallEffects()
    {
        playerAnimator.SetTrigger("Fall");
    }

    public void PlayLandEffects()
    {
        // Play an animation, some particles, and a sound effect when the player lands
        playerAnimator.ResetTrigger("Fall");
        playerAnimator.ResetTrigger("Jump");
        playerAnimator.SetTrigger("Land");
        //landParticles.Play();

        //if (!landSFX.isPlaying && landSFX.enabled)
        //{
        //    landSFX.Play();
        //}

        //moveParticles.Play();
    }

    internal void PlaySwingEffects()
    {
        playerAnimator.SetTrigger("Swing");
    }

    private void PlayAirEffects()
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
