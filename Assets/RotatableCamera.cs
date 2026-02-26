using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player;           // assign your player
    public Vector3 offset = new Vector3(0f, 2f, -5f); // default camera offset
    public float mouseSensitivity = 100f;
    public float maxYaw = 45f;        // left/right limit
    public float maxPitch = 20f;      // up/down limit

    private float yaw = 0f;
    private float pitch = 10f; // default slightly looking down

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.position = player.position + offset;
    }

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Update rotation
        yaw += mouseX * mouseSensitivity * Time.deltaTime;
        pitch -= mouseY * mouseSensitivity * Time.deltaTime;

        // Clamp rotation
        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Set camera position behind player with rotation
        transform.position = player.position + rotation * offset;

        // Look at player
        transform.LookAt(player.position + Vector3.up * 1.5f); // slightly above player center
    }
}