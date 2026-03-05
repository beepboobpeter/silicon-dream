using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ);

        transform.Translate(move * speed * Time.deltaTime, Space.World);

        animator.SetBool("IsRunning", move.sqrMagnitude > 0);
    }
}