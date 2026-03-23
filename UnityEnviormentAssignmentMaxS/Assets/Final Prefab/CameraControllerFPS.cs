using UnityEngine;

public class CameraControllerFPS : MonoBehaviour
{
    public float sensitivity = 2f;
    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation to prevent flipping

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate the camera vertically
        transform.parent.Rotate(Vector3.up * mouseX); // Rotate
    }
}
