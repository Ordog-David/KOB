using System;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GrapplingPoint grapplingPoint;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("General Settings")]
    [SerializeField] private int precision = 40;
    [SerializeField] [Range(0, 20)] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings")]
    [SerializeField] private AnimationCurve ropeAnimationCurve;
    [SerializeField] [Range(0.01f, 4)] private float startingWaveSize = 2;

    [Header("Rope Progression Settings")]
    [SerializeField] private AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;

    private float waveSize = 0;
    private float moveTime = 0;
    private bool isGrappling = true;
    private bool straightLine = true;

    public void Start()
    {
    }

    private void OnEnable()
    {
        moveTime = 0;
        lineRenderer.positionCount = precision;
        waveSize = startingWaveSize;
        straightLine = false;

        LinePointsToFirePoint();

        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < precision; i++)
        {
            lineRenderer.SetPosition(i, grapplingPoint.GetRopeLaunchPosition());
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    void DrawRope()
    {
        if (straightLine == false)
        {
            if (VectorsEqual(lineRenderer.GetPosition(precision - 1), grapplingPoint.GetGrapplePoint()))
            {
                straightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (isGrappling == false)
            {
                grapplingPoint.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (lineRenderer.positionCount != 2)
                {
                    lineRenderer.positionCount = 2;
                }

                DrawRopeNoWaves();
            }
        }
    }

    private bool VectorsEqual(Vector3 value1, Vector3 value2)
    {
        Debug.Log(value1.x);
        Debug.Log(value2.x);
        return FloatsEqual(value1.x, value2.x) && FloatsEqual(value1.y, value2.y);
    }

    private bool FloatsEqual(float value1, float value2)
    {
        return Mathf.Abs(value1 - value2) < 0.01;
    }

    void DrawRopeWaves()
    {
        for (int i = 0; i < precision; i++)
        {
            float delta = (float)i / ((float)precision - 1f);
            Vector2 offset = ropeAnimationCurve.Evaluate(delta) * waveSize * Vector2.Perpendicular(grapplingPoint.GetGrappleDistanceVector()).normalized;
            Vector2 targetPosition = Vector2.Lerp(grapplingPoint.GetRopeLaunchPosition(), grapplingPoint.GetGrapplePoint(), delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapplingPoint.GetRopeLaunchPosition(), targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            lineRenderer.SetPosition(i, currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        lineRenderer.SetPosition(0, grapplingPoint.GetRopeLaunchPosition());
        lineRenderer.SetPosition(1, grapplingPoint.GetGrapplePoint());
    }
}
