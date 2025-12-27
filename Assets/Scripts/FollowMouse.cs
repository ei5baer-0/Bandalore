using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private Transform boxTransform;
    [SerializeField] private Transform centerTransform;
    [SerializeField] private float radius = 1f;

    private Vector2 mousePosition;

    void Start()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateMousePosition();
        FollowMousePosition(boxTransform);
    }

    void UpdateMousePosition()
    {
        mousePosition = Mouse.current.position.ReadValue();
    }

    private void FollowMousePosition(Transform target)
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 offset = mouseWorldPos - centerTransform.position;
        offset = Vector3.ClampMagnitude(offset, radius);
        target.position = centerTransform.position + offset;
    }
}
