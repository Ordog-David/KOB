using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

// This script handles the character being killed and respawning
public class PlayerStatus : MonoBehaviour
{
    private const float animationInterval = 0.2f;

    [Header("Components")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AudioSource playerHurtSFX;
    [SerializeField] private Color healthyColor;
    [SerializeField] private Color hurtColor;
    [SerializeField] private Light2D globalLight;
    private Rigidbody2D playerBody;
    private PlayerMovement playerMovement;
    private Light2D playerLight;
    private RopeLaunchPoint rope;
    private Color defaultColor;


    [Header("Settings")]
    [SerializeField] private int invulnerabilityTime = 5;
    [SerializeField] private float flashDuration;
    [SerializeField] private float lightDecrease;

    [Header("Current State")]
    private float worldRadius;
    public int health;
    public bool hurting = false;
    private Vector3 startingPosition;

    private readonly HashSet<IPlayerRespawnListener> respawnListeners = new();

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerLight = GetComponentInChildren<Light2D>();
        rope = GetComponentInChildren<RopeLaunchPoint>();
        worldRadius = GetWorldRadius();
        startingPosition = transform.position;
        defaultColor = globalLight.color;

        var checkpoint = FindCheckpoint(SavegameManager.Instance.Data.checkpointName);
        if (checkpoint != null)
        {
            CheckpointReached(checkpoint.name);
            transform.position = checkpoint.transform.position;
        }
        else
        {
            ResetHealth();
        }

        respawnListeners.ToList().ForEach(listener => listener.OnPlayerRespawn());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CheckHurt(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        CheckHurt(collider);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var overlappingCollinders = new List<Collider2D>();
            if (playerBody.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingCollinders) > 0)
            {
                var checkpointCollider = overlappingCollinders.FirstOrDefault(collider => collider.CompareTag("Checkpoint"));
                if (checkpointCollider != null)
                {
                    CheckpointReached(checkpointCollider.gameObject.name);
                    Save();
                }
            }
        }
    }

    public void OnRespawn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Respawn();
        }
    }

    public void AddRespawnListener(IPlayerRespawnListener listener)
    {
        respawnListeners.Add(listener);
    }

    public void RemoveRespawnListener(IPlayerRespawnListener listener)
    {
        respawnListeners.Remove(listener);
    }

    private float GetWorldRadius()
    {
        var aspect = (float)Screen.width / Screen.height;
        var worldHeight = Camera.main.orthographicSize;
        var worldWidth = worldHeight * aspect;
        return Mathf.Sqrt(Mathf.Pow(worldHeight, 2) + Mathf.Pow(worldWidth, 2));
    }

    private GameObject FindCheckpoint(string checkpointName)
    {
        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var checkpoint in checkpoints)
        {
            if (checkpoint.name == checkpointName)
            {
                return checkpoint;
            }
        }
        return null;
    }

    // When the player touches a checkpoint, it passes its position to this script
    private void CheckpointReached(string checkpointName)
    {
        ResetHealth();

        SavegameManager.Instance.Data.checkpointName = checkpointName;

        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (var checkpoint in checkpoints)
        {
            var isActive = checkpoint.name == checkpointName;
            checkpoint.GetComponent<Animator>().SetBool("IsActive", isActive);

            var checkpointLight = checkpoint.GetComponentInChildren<Light2D>();
            if (isActive)
            {
                DOTween.To(() => checkpointLight.pointLightOuterRadius, x => checkpointLight.pointLightOuterRadius = x, 5f, 3f);
            }
            else
            {
                DOTween.To(() => checkpointLight.pointLightOuterRadius, x => checkpointLight.pointLightOuterRadius = x, 0.1f, 0.1f);
            }
        }

        Debug.Log("Checkpoint saved");
    }

    private void ResetHealth()
    {
        health = 10;
        DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, health / lightDecrease, animationInterval);
    }

    private void CheckHurt(Collider2D collider)
    {
        // If the player hits layer 8 (things that hurt hime), start the hurt routine
        if (collider.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            if (hurting == false)
            {
                hurting = true;
                Hurt(1);
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Red Projectile"))
        {
            if (hurting == false)
            {
                hurting = true;
                Hurt(2);
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Health Reset"))
        {
            ResetHealth();
        }
    }

    private void Hurt(int damage)
    {
        playerHurtSFX.Play();

        health -= Mathf.Min(damage, health);
        if (health == 0)
        {
            Death();
            return;
        }

        // Start a timer, during which we modify the health light and the character is invulnerable.
        DOTween.Sequence().AppendCallback(
            () => playerLight.color = hurtColor
        ).Append(
            DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, health / lightDecrease, animationInterval)
        ).AppendCallback(
            () => playerLight.color = healthyColor
        ).Append(
            DOTween.Sequence().AppendCallback(TurnOfLight).AppendInterval(animationInterval)
                .AppendCallback(TurnOnLight).AppendInterval(animationInterval).SetLoops(invulnerabilityTime)
        ).OnComplete(Unhurt);
    }

    private void Unhurt()
    {
        hurting = false;
    }

    private void Death()
    {
        //playerAnimator.SetTrigger("Dead");

        Time.timeScale = 0f;

        // Start a timer, before respawning the player. This uses the (excellent) free Unity asset DOTween
        DOTween.To(
            () => playerLight.pointLightOuterRadius,
            x => playerLight.pointLightOuterRadius = x,
            worldRadius,
            4 * animationInterval
        ).OnComplete(Respawn).SetUpdate(true);
    }

    // After the timer ends, respawn the character at the nearest checkpoint and let her move again
    private void Respawn()
    {
        Time.timeScale = 1f;
        playerMovement.SetFrozen(true);
        rope.ReleaseGrapplePoint();

        var checkpoint = FindCheckpoint(SavegameManager.Instance.Data.checkpointName);
        if (checkpoint != null)
        {
            transform.position = checkpoint.transform.position;
        }
        else
        {
            transform.position = startingPosition;
        }

        playerMovement.SetFrozen(false);
        //playerAnimator.SetTrigger("Okay");
        ResetHealth();
        hurting = false;

        respawnListeners.ToList().ForEach(listener => listener.OnPlayerRespawn());
        globalLight.color = defaultColor;
    }

    private void TurnOfLight()
    {
        playerLight.enabled = false;
    }

    private void TurnOnLight()
    {
        playerLight.enabled = true;
    }

    private void Save()
    {
        SavegameManager.Instance.Data.visitedBlessings = GetVisitedBlessings();
        SavegameManager.Instance.Save();
    }

    private string[] GetVisitedBlessings()
    {
        var visitedBlessings = new List<string>();
        foreach (var blessing in FindObjectsOfType<Blessing>(true))
        {
            if (blessing.gameObject.activeSelf == false)
            {
                visitedBlessings.Add(blessing.name);
            }
        }
        return visitedBlessings.ToArray();
    }
}
