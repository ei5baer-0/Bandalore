using UnityEngine;
using UnityEngine.InputSystem;

public class LineConnector : MonoBehaviour
{
    public enum State
    {
        Idle, Extending, Retracting, Pulling
    }

    [Header("References")]
    [SerializeField] private LineRenderer mainLine;
    [SerializeField] private LineRenderer outlineLine;
    [SerializeField] private Transform objectA;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask connectableLayers;
    [SerializeField] private Transform transformJojo;
    [SerializeField] private GameObject objectJojo;

    [Header("Variables")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float lineExtendSpeed = 2f;
    [SerializeField] private float lineRetractSpeed = 3f;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private float pullDuration = 1f;
    [SerializeField] private float cooldown = 3f;

    public State state = State.Idle;
    private Vector2 targetEnd;
    private Vector2 pullTarget;
    private float progress;
    private float pullTimer;
    private bool hitSomething;
    private float cooldownTimer;

    void Start()
    {
        cooldownTimer = cooldown;
        objectJojo.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && state == State.Idle && cooldownTimer <= 0)
        {
            StartLine();
            cooldownTimer = cooldown;
            objectJojo.SetActive(true);
        }
        StateSwitch();
        CooldownTimer();
        JojoFollow();
    }

    private void StateSwitch()
    {
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

    private void CooldownTimer()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void StartLine()
    {
        progress = 0f;
        state = State.Extending;

        mainLine.enabled = true;
        outlineLine.enabled = true;

        Vector2 start = objectA.position;

        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 dir = (mouseWorld - start).normalized;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, maxDistance, connectableLayers);

        hitSomething = hit.collider != null;

        targetEnd = hitSomething ? hit.point : start + dir * maxDistance;

        pullTarget = targetEnd;

        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, start);
        outlineLine.SetPosition(0, start);
        outlineLine.SetPosition(1, start);
    }

    private void ExtendLine()
    {
        progress += Time.deltaTime * lineExtendSpeed;
        progress = Mathf.Clamp01(progress);

        Vector2 start = objectA.position;
        Vector2 currentEnd = Vector2.Lerp(start, targetEnd, progress);

        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, currentEnd);
        outlineLine.SetPosition(0, start);
        outlineLine.SetPosition(1, currentEnd);

        if (progress >= 1f)
        {
            state = hitSomething ? State.Pulling : State.Retracting;
            pullTimer = pullDuration;
        }
    }

    private void RetractLine()
    {
        progress -= Time.deltaTime * lineRetractSpeed;
        progress = Mathf.Clamp01(progress);

        Vector2 start = objectA.position;
        Vector2 currentEnd = Vector2.Lerp(start, targetEnd, progress);

        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, currentEnd);
        outlineLine.SetPosition(0, start);
        outlineLine.SetPosition(1, currentEnd);

        if (progress <= 0f)
        {
            EndLine();
        }
    }

    private void PullObject()
    {
        pullTimer -= Time.deltaTime;

        objectA.position = Vector2.MoveTowards(
            objectA.position,
            pullTarget,
            pullSpeed * Time.deltaTime
        );

        mainLine.SetPosition(0, objectA.position);
        mainLine.SetPosition(1, pullTarget);
        outlineLine.SetPosition(0, objectA.position);
        outlineLine.SetPosition(1, pullTarget);

        if (pullTimer <= 0f ||
            Vector2.Distance(objectA.position, pullTarget) < 0.05f)
        {
            EndLine();
        }
    }

    private void JojoFollow()
    {
        if (state == State.Extending || state == State.Retracting || state == State.Pulling)
        {
            Vector3 lineEnd = mainLine.GetPosition(1);
            transformJojo.position = new Vector3(lineEnd.x, lineEnd.y, transformJojo.position.z);
        }
    }

    private void EndLine()
    {
        mainLine.enabled = false;
        outlineLine.enabled = false;
        objectJojo.SetActive(false);
        state = State.Idle;
    }
}
