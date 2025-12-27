using UnityEngine;
using UnityEngine.InputSystem;

public class LineConnector : MonoBehaviour
{
    private enum State
    {
        Idle,
        Extending,
        Retracting,
        Pulling
    }

    [Header("References")]
    [SerializeField] private LineRenderer mainLine;
    [SerializeField] private LineRenderer outlineLine;
    [SerializeField] private Transform objectA;
    [SerializeField] private Transform objectB;
    [SerializeField] private LayerMask connectableLayers;

    [Header("Variables")]
    [SerializeField] private float lineExtendSpeed = 2f;
    [SerializeField] private float lineRetractSpeed = 3f;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private float pullDuration = 1f;

    private State state = State.Idle;
    private Vector2 targetEnd;
    private Vector2 pullTarget;
    private float progress;
    private float pullTimer;
    private bool hitSomething;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && state == State.Idle)
        {
            StartLine();
        }

        switch (state)
        {
            case State.Extending:
                ExtendLine();
                break;
            case State.Retracting:
                RetractLine();
                break;
            case State.Pulling:
                PullObject();
                break;
        }
    }

    private void StartLine()
    {
        progress = 0f;
        mainLine.enabled = true;
        outlineLine.enabled = true;
        state = State.Extending;

        Vector2 start = objectA.position;
        Vector2 end = objectB.position;
        Vector2 dir = end - start;
        float dist = dir.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, dist, connectableLayers);

        hitSomething = hit.collider != null;
        targetEnd = hitSomething ? hit.point : end;
        pullTarget = hitSomething ? hit.point : end;

        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, start);
        outlineLine.SetPosition(0, start);
        outlineLine.SetPosition(1, start);
    }

    private void ExtendLine()
    {
        progress += Time.deltaTime * lineExtendSpeed;
        progress = Mathf.Clamp01(progress);
        float eased = Mathf.SmoothStep(0f, 1f, progress);

        Vector2 start = objectA.position;
        Vector2 currentEnd = Vector2.Lerp(start, targetEnd, eased);

        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, currentEnd);
        outlineLine.SetPosition(0, start);
        outlineLine.SetPosition(1, currentEnd);

        if (progress >= 1f)
        {
            if (hitSomething)
            {
                pullTimer = pullDuration;
                state = State.Pulling;
            }
            else
            {
                state = State.Retracting;
            }
        }
    }

    private void RetractLine()
    {
        progress -= Time.deltaTime * lineRetractSpeed;
        progress = Mathf.Clamp01(progress);
        float eased = Mathf.SmoothStep(0f, 1f, progress);

        Vector2 start = objectA.position;
        Vector2 currentEnd = Vector2.Lerp(start, targetEnd, eased);

        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, currentEnd);
        outlineLine.SetPosition(0, start);
        outlineLine.SetPosition(1, currentEnd);

        if (progress <= 0f)
        {
            mainLine.enabled = false;
            outlineLine.enabled = false;
            state = State.Idle;
        }
    }

    private void PullObject()
    {
        pullTimer -= Time.deltaTime;

        objectA.position = Vector2.MoveTowards(objectA.position, pullTarget, pullSpeed * Time.deltaTime);

        mainLine.SetPosition(0, objectA.position);
        mainLine.SetPosition(1, pullTarget);
        outlineLine.SetPosition(0, objectA.position);
        outlineLine.SetPosition(1, pullTarget);

        if (pullTimer <= 0f || Vector2.Distance(objectA.position, pullTarget) < 0.05f)
        {
            mainLine.enabled = false;
            outlineLine.enabled = false;
            state = State.Idle;
        }
    }
}
