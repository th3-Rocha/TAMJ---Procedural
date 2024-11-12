using UnityEngine;

public class FPSController : MonoBehaviour
{
    public Transform cameraTransform;  // Drag your camera here
    public float mouseSensitivity = 100f;
    public float rotationSmoothTime = 0.1f;

    private Vector2 rotation;
    private Vector2 currentRotation;
    private Vector2 rotationVelocity;

    void Start()
    {
        // Lock the cursor for FPS control
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust rotation (invert mouseY if needed by reversing the sign)
        rotation.x -= mouseY;
        rotation.y += mouseX;

        // Clamp vertical rotation to avoid flipping (limits head movement)
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        // Smooth the rotation
        currentRotation.x = Mathf.SmoothDamp(currentRotation.x, rotation.x, ref rotationVelocity.x, rotationSmoothTime);
        currentRotation.y = Mathf.SmoothDamp(currentRotation.y, rotation.y, ref rotationVelocity.y, rotationSmoothTime);

        // Apply rotation
        cameraTransform.localRotation = Quaternion.Euler(currentRotation.x, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    }
}
