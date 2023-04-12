using UnityEngine;

// Erre szükség van a mozgáshoz; Ellenörzi a karakter a földön van-e?
public class PlayerGround : MonoBehaviour
{
    [Header("Collider Settings")]
    [SerializeField] [Tooltip("Length of the ground-checking collider")] private float groundLength = 0.95f;
    [SerializeField] [Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField] [Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;

    [Header("Current State")]
    private bool onGround = true;

    private void Update()
    {
        // A játékos valamin áll-e?
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
                   Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
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

        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    public bool IsOnGround()
    {
        return onGround;
    }
}
