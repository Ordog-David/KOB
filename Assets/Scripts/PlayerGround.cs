using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Erre szükség van a mozgáshoz; Ellenörzi a karakter a földön van-e?
public class PlayerGround : MonoBehaviour
{
    [Header("Collider Settings")]
    [SerializeField] [Tooltip("Length of the ground-checking collider")] private float groundLength = 0.95f;
    [SerializeField] [Tooltip("Left leg offset")] private Vector3 leftLegOffset;
    [SerializeField] [Tooltip("Right leg offset")] private Vector3 rightLegOffset;

    [Header("Layer Masks")]
    [SerializeField] [Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;

    [Header("Current State")]
    private bool onGround = true;
    private float directionX = 1f;

    private void Update()
    {
        // A játékos valamin áll-e?
        onGround = Physics2D.Raycast(transform.position + directionX * rightLegOffset, Vector2.down, groundLength, groundLayer) ||
                   Physics2D.Raycast(transform.position - directionX * leftLegOffset, Vector2.down, groundLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        // Csak a Debug miatt
        if (onGround)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(transform.position + directionX * rightLegOffset, transform.position + directionX * rightLegOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - directionX * leftLegOffset, transform.position - directionX * leftLegOffset + Vector3.down * groundLength);
    }

    public void SetDirectionX(float directionX)
    {
        this.directionX = directionX;
    }

    public bool IsOnGround()
    {
        return onGround;
    }
}
