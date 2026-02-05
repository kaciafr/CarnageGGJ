using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 200f;
    public float sensY = 200f;

    public Transform orientation;

    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mouseInput = Mouse.current.delta.ReadValue();

        float mouseX = mouseInput.x * sensX * Time.deltaTime;
        float mouseY = mouseInput.y * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (orientation != null)
            orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}