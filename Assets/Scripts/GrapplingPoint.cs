using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingPoint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform ropeLaunchPoint;
    [SerializeField] private GrapplingRope rope;
    [SerializeField] private Camera mainCamera;
    private SpringJoint2D springJoint;

    [Header("Settings")]
    [SerializeField] private string grappableTag = "Grapplable";
    [SerializeField] private float ropeShortensBy = 2f;

    private Vector2 grapplePoint;
    private Vector2 grappleDistanceVector;

    private void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();

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
        Vector2 distanceVector = mainCamera.ScreenToWorldPoint(Input.mousePosition) - ropeLaunchPoint.position;
        if (Physics2D.Raycast(ropeLaunchPoint.position, distanceVector.normalized))
        {
            RaycastHit2D hit = Physics2D.Raycast(ropeLaunchPoint.position, distanceVector.normalized);
            if (hit.transform.gameObject.CompareTag(grappableTag))
            {
                Debug.Log(hit.point);
                grapplePoint = hit.point;
                grappleDistanceVector = grapplePoint - (Vector2)ropeLaunchPoint.position;
                rope.enabled = true;
                Debug.Log("rope enabled");
            }
        }
    }

    public void ReleaseGrapplePoint()
    {
        rope.enabled = false;
        springJoint.enabled = false;
    }

    public void Grapple()
    {
        springJoint.autoConfigureDistance = false;
        springJoint.distance = grappleDistanceVector.magnitude - ropeShortensBy;
        springJoint.connectedAnchor = grapplePoint;
        springJoint.enabled = true;
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
