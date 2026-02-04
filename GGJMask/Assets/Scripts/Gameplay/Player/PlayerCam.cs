using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 200f;
    public float sensY = 200f;

    public Transform orientation;

    float xRotation;
    float yRotation;
    
    /*
    void Update()
    {
        Vector2 mouseInput = Mouse.current.delta.ReadValue();}

        float mouseX = mouseInput.x * sensX * Time.deltaTime;
        float mouseY = mouseInput.y * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (orientation != null)
            orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }*/
}