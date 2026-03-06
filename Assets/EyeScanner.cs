using UnityEngine;

public class EyeScanner : MonoBehaviour
{
    [Header("References")]
    public EyeDetection detection;

    [Header("Scan Settings")]
    public float scanAngle = 45f;   // degrees to each side of the start rotation
    public float scanSpeed = 40f;   // degrees per second

    [Header("Follow Settings")]
    public float followSpeed = 8f;  // smooth rotation speed when tracking player

    float startRotation;
    float scanTimer;
    bool wasFollowing;

    void Start()
    {
        startRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (detection == null)
            return;

        bool isFollowing = detection.canSeePlayer;

        if (isFollowing)
        {
            FollowPlayer();
        }
        else
        {
            // if we just stopped following, sync the scan timer to our current rotation
            // so there's no snap back to an arbitrary scan position
            if (wasFollowing)
            {
                SyncScanTimerToCurrentRotation();
            }

            Scan();
        }

        wasFollowing = isFollowing;
    }

    void Scan()
    {
        scanTimer += Time.deltaTime * scanSpeed;
        float angle = Mathf.PingPong(scanTimer, scanAngle * 2f) - scanAngle;

        transform.rotation = Quaternion.Euler(0f, startRotation + angle, 0f);
    }

    void FollowPlayer()
    {
        Transform player = detection.player;
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            followSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// When transitioning from follow back to scan, figure out where we are
    /// in the ping-pong cycle so the scan resumes from the current facing direction.
    /// </summary>
    void SyncScanTimerToCurrentRotation()
    {
        float currentOffset = Mathf.DeltaAngle(startRotation, transform.eulerAngles.y);
        // clamp to scan range so we don't overshoot
        currentOffset = Mathf.Clamp(currentOffset, -scanAngle, scanAngle);
        // PingPong output = currentOffset + scanAngle  →  scanTimer = that / scanSpeed
        scanTimer = (currentOffset + scanAngle) / scanSpeed;
    }
}
