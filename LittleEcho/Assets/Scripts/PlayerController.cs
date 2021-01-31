using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;

    // Movement Variables
    public bool facingRight = true;
    public float walkSpeed;
    public float flySpeed;
    public float flyForce;

    // Check Ground Variables
    public bool isGrounded = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    [SerializeField]
    private SoundWave flapPrefab;

    [SerializeField]
    private float flapCooldown = 0.2F;
    private float flapTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody Component of the GameObject
        rb = GetComponent<Rigidbody2D>();

        flapTimer = flapCooldown;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Paused)
        {
            UpdateInput();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Paused)
        {
            // Check if the GameObject is touching the ground by checking if a small circle is overlapping an Object with Layer "Ground"
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            // Get horizontal move input (x-axis)
            moveInput = Input.GetAxis("Horizontal");
            Move();

            // Flips the GameObject based on direction of movement
            if (facingRight == false && moveInput > 0) // Check if Moving to the right
            {
                Flip();
            }
            else if (facingRight == true && moveInput < 0) // Check if Moving to the left
            {
                Flip();
            }
        }
    }

    void Move()
    {
        float speed;
        if (isGrounded)
        {
            // If GameObject is touching the ground, use walk speed
            speed = walkSpeed;
        }
        else
        {
            // If GameObject is touching the ground, use fly speed
            speed = flySpeed;
        }
        // Move Position based on MoveInput
        transform.position += new Vector3(moveInput,0,0) * Time.deltaTime * speed;

        // Add velocity speed to the direction of movement
        //rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        // Get local scale of GameObject
        Vector3 scalar = transform.localScale;
        // Reverse the scale
        scalar.x *= -1;
        // Assign the local scale to new scale
        transform.localScale = scalar;
    }

    private void UpdateInput()
    {
        flapTimer += Time.deltaTime;

        if (flapTimer > flapCooldown)
        {
            // Uses W/S and UP/DOWN inputs
            if (Input.GetButtonDown("Flap"))
            {
                flapTimer = 0;

                rb.velocity = Vector2.up * flyForce;

                Instantiate(flapPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
