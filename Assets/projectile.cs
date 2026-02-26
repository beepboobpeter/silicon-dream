using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;        // forward speed
    public float wobbleAmount = 0.3f; // side-to-side offset
    public float wobbleSpeed = 5f;    // wobble frequency
    public float lifetime = 2f;       // destroy after 2 seconds

    private Vector3 moveDirection;
    private Transform player;
    private EyeDetection detection;
    private Vector3 wobbleAxis;
    private float startTime;

    // Initialize references from EyeAttack
    public void Initialize(Transform playerTransform, EyeDetection eyeDetection)
    {
        player = playerTransform;
        detection = eyeDetection;

        // Calculate initial direction toward player
        if (detection.canSeePlayer)
        {
            moveDirection = (player.position - transform.position).normalized;
        }
        else
        {
            moveDirection = transform.forward;
        }

        // Choose a perpendicular axis for wobble
        wobbleAxis = Vector3.Cross(moveDirection, Vector3.up).normalized;
        startTime = Time.time;

        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (player != null && detection != null && detection.canSeePlayer)
        {
            // Update direction toward player while visible
            moveDirection = (player.position - transform.position).normalized;
            wobbleAxis = Vector3.Cross(moveDirection, Vector3.up).normalized;
        }

        // Apply wobble
        Vector3 wobble = wobbleAxis * Mathf.Sin((Time.time - startTime) * wobbleSpeed) * wobbleAmount;

        // Move projectile
        transform.position += (moveDirection + wobble).normalized * speed * Time.deltaTime;

        // Rotate visually toward main direction
        transform.forward = moveDirection;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}