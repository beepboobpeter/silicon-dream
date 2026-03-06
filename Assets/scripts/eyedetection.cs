using UnityEngine;

public class EyeDetection : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform eyeOrigin; // assign an empty child in front of the eye; falls back to transform.position

    [Header("Detection Settings")]
    public float detectionRange = 20f;
    public float viewAngle = 60f; // total cone angle (half on each side)

    [Header("Timing")]
    public float loseSightDelay = 0.5f; // seconds before officially losing the player

    [HideInInspector]
    public bool canSeePlayer;

    float lastSeenTime;
    RaycastHit[] hitBuffer = new RaycastHit[20];

    Vector3 Origin => eyeOrigin != null ? eyeOrigin.position : transform.position;

    void Update()
    {
        if (player == null)
        {
            canSeePlayer = false;
            return;
        }

        bool currentlyVisible = CheckVisibility();

        if (currentlyVisible)
        {
            lastSeenTime = Time.time;
            canSeePlayer = true;
        }
        else if (Time.time - lastSeenTime > loseSightDelay)
        {
            canSeePlayer = false;
        }
    }

    bool CheckVisibility()
    {
        Vector3 origin = Origin;
        Vector3 dirToPlayer = player.position - origin;
        float distance = dirToPlayer.magnitude;

        if (distance > detectionRange)
            return false;

        // horizontal angle check — flatten both vectors so height difference doesn't inflate the angle
        Vector3 flatForward = new Vector3(transform.forward.x, 0f, transform.forward.z);
        Vector3 flatDir = new Vector3(dirToPlayer.x, 0f, dirToPlayer.z);

        if (flatForward.sqrMagnitude > 0.001f && flatDir.sqrMagnitude > 0.001f)
        {
            float angle = Vector3.Angle(flatForward, flatDir);
            if (angle > viewAngle * 0.5f)
                return false;
        }

        // raycast — get all hits and find the closest one that isn't this eye
        int hitCount = Physics.RaycastNonAlloc(origin, dirToPlayer.normalized, hitBuffer, detectionRange);

        float closestDist = Mathf.Infinity;
        Transform closestHit = null;

        for (int i = 0; i < hitCount; i++)
        {
            // skip any collider that belongs to this eye (self or children)
            if (hitBuffer[i].transform.IsChildOf(transform) || hitBuffer[i].transform == transform)
                continue;

            if (hitBuffer[i].distance < closestDist)
            {
                closestDist = hitBuffer[i].distance;
                closestHit = hitBuffer[i].transform;
            }
        }

        // the closest thing the ray hit (ignoring the eye) should be the player
        return closestHit == player;
    }
}
