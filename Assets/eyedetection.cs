using UnityEngine;

public class EyeDetection : MonoBehaviour
{
    public Transform player;          // assign your player
    public float detectionRange = 20f;
    public LayerMask obstacleLayers;  // assign walls/floors
    [HideInInspector]
    public bool canSeePlayer;

    void Update()
    {
        if (player == null)
        {
            canSeePlayer = false;
            return;
        }

        // Check distance
        Vector3 dirToPlayer = player.position - transform.position;
        if (dirToPlayer.magnitude > detectionRange)
        {
            canSeePlayer = false;
            return;
        }

        // Raycast toward player to see if an obstacle is in the way
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, detectionRange))
        {
            canSeePlayer = hit.transform == player; // true only if player is hit first
        }
        else
        {
            canSeePlayer = true; // nothing blocking
        }
    }
}