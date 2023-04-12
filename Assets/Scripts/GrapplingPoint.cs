using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingPoint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform ropeLaunchPoint;
    [SerializeField] private GrapplingRope rope;
    [SerializeField] private Camera mainCamera;
    private PlayerJuice playerJuice;
    private DistanceJoint2D distanceJoint;

    [Header("Settings")]
    [SerializeField] private string grappableTag = "Grappable";
    [SerializeField] private float ropeShortensBy = 2f;

    private Vector2 grapplePoint;
    private Vector2 grappleDistanceVector;

    private void Start()
    {
        playerJuice = GetComponent<PlayerJuice>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        ReleaseGrapplePoint();
    }

    public void OnLaunchGrapplingRope(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ReleaseGrapplePoint();
            SetGrapplePoint();
        }
    }

    public void OnReleaseGrapplingRope(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ReleaseGrapplePoint();
        }
    }

    private void SetGrapplePoint()
    {
        var distanceVector = mainCamera.ScreenToWorldPoint(Input.mousePosition) - ropeLaunchPoint.position;
        var hit = Physics2D.Raycast(ropeLaunchPoint.position, distanceVector.normalized);
        if (hit.transform != null && hit.transform.gameObject.CompareTag(grappableTag))
        {
            grapplePoint = hit.point;
            grappleDistanceVector = grapplePoint - (Vector2)ropeLaunchPoint.position;
            rope.enabled = true;
        }
    }

    public void ReleaseGrapplePoint()
    {
        if (rope.enabled)
        {
            rope.enabled = false;
            if (distanceJoint.enabled)
            {
                distanceJoint.enabled = false;
                playerJuice.PlayFallEffects();
            }
        }
    }

    public void Grapple()
    {
        distanceJoint.autoConfigureDistance = false;
        distanceJoint.distance = Mathf.Clamp(grappleDistanceVector.magnitude - ropeShortensBy, 1, Mathf.Infinity);
        distanceJoint.connectedAnchor = grapplePoint;
        distanceJoint.enabled = true;
        playerJuice.PlaySwingEffects();
    }

    public bool IsEnabled()
    {
        return distanceJoint.enabled;
    }

    public Vector3 GetRopeLaunchPosition()
    {
        return ropeLaunchPoint.position;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    public Vector2 GetGrappleDistanceVector()
    {
        return grappleDistanceVector;
    }
}
