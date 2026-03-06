using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    public float wobbleAmount = 0.3f;  // side-to-side offset strength
    public float wobbleSpeed = 5f;     // sine wave frequency

    [Header("Lost Target")]
    public float driftUpSpeed = 5f;     // how fast it floats upward after losing the player

    [Header("Lifetime")]
    public float lifetime = 3f;         // always destroyed after this many seconds

    Transform player;
    EyeDetection detection;
    Vector3 moveDirection;
    Vector3 wobbleAxis;
    float spawnTime;
    bool lost;

    public void Initialize(Transform playerTransform, EyeDetection eyeDetection)
    {
        player = playerTransform;
        detection = eyeDetection;
        lost = false;

        moveDirection = (player.position - transform.position).normalized;
        wobbleAxis = Vector3.Cross(moveDirection, Vector3.up).normalized;
        spawnTime = Time.time;

        // always self-destruct after lifetime, no matter what
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        bool canSee = player != null && detection != null && detection.canSeePlayer;

        if (canSee && !lost)
        {
            // follow the player with a small sine wave wobble
            moveDirection = (player.position - transform.position).normalized;
            wobbleAxis = Vector3.Cross(moveDirection, Vector3.up).normalized;

            float wobble = Mathf.Sin((Time.time - spawnTime) * wobbleSpeed) * wobbleAmount;
            Vector3 velocity = (moveDirection + wobbleAxis * wobble).normalized * speed;

            transform.position += velocity * Time.deltaTime;
            transform.forward = moveDirection;
        }
        else
        {
            // lost the player — drift upward
            if (!lost)
            {
                lost = true;

                // disable collider so it doesn't hit walls while drifting
                Collider col = GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;
            }

            transform.position += Vector3.up * driftUpSpeed * Time.deltaTime;
            transform.forward = Vector3.up;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!lost)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!lost)
            Destroy(gameObject);
    }
}
