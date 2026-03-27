using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 2f;
    public float gravity = -9.81f;

    private CharacterController cc;
    private Vector3 velocity;
    private bool canJump; // tracks if player can jump

    void Start()
    {
        cc = GetComponent<CharacterController>();
        canJump = true; // start able to jump
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Jump only if allowed and grounded
        if (Input.GetButtonDown("Jump") && cc.isGrounded && canJump)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            canJump = false; // prevent double jump
        }

        // Reset jump when touching the ground
        if (cc.isGrounded && velocity.y <= 0)
        {
            canJump = true;
            velocity.y = -2f; // stick to ground
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Final movement
        Vector3 finalMove = move * speed + velocity;
        cc.Move(finalMove * Time.deltaTime);
    }
}