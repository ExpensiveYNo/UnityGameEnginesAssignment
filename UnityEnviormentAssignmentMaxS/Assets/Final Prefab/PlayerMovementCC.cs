using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 2f;
    public float gravity = -9.81f;
    public int maxJumps = 1;
    public bool grounded = false;

    private CharacterController cc;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        cc.Move(move * speed * Time.deltaTime);

        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            maxJumps--;
        }

      

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            grounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
