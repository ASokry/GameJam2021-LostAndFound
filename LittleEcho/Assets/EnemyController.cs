using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /* ( Impotant Instructions )
        The "movePoints" array needs at least 2 empty gameObject transform positions (not including the Moth Object position)
        You can increase the array size and add more empty gameObjects. I recommend duplicating the child empty object.

        The Moth object can only move in straight lines, keep that in mind when placing the move points in the level
        This is not too imporatnt, but it would be good to place the Moth object at the first move point
    */

    private Rigidbody2D rb;
    public enum MothStates { IDLE, MOVING, CHOOSING };
    public MothStates state = MothStates.IDLE;
    private bool facingRight = true;
    private Vector2 startPos;

    public Transform[] movePoints;
    private int nextPoint = 0;

    private float speed;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float rate = 0.01f;
    [SerializeField] private float theSpeed;

    private bool traverseUp = true;
    public float timeBetweenMove = 4f;
    private float moveCountdown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        // Throws Error if the movePoints Array is less than 2
        if (movePoints.Length < 2)
        {
            Debug.LogError("No Move Points");
        }
        moveCountdown = timeBetweenMove;
    }

    private void Update()
    {
        theSpeed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        // Check if the movePoints array is not empty
        if (movePoints.Length >= 2)
        {
            if (state == MothStates.MOVING)
            {
                // Move to next move point
                Moving(movePoints[nextPoint]);
            }
            else if (state == MothStates.CHOOSING)
            {
                // Countdown Before Choosing
                WaitBeforeChoosing();
            }
            else if (state == MothStates.IDLE)
            {
                moveCountdown = timeBetweenMove;
                // After choosing next point, begin to move again
                state = MothStates.MOVING;
            }
        }

        // Flips the GameObject based on current position and direction of movement
        if (transform.position.x < startPos.x && facingRight == true)
        {
            // If object is moving left but is facing right, flip
            Flip();
        }
        else if (transform.position.x >= startPos.x && facingRight == false)
        {
            // If object is moving right but is facing left, flip
            Flip();
        }
    }

    void Moving(Transform target)
    {
        state = MothStates.MOVING;

        //Vector3 dir = (this.transform.position - target.position).normalized;
        //rb.AddForce(dir);

        if (Vector2.Distance(transform.position, target.position) <= Vector2.Distance(startPos, target.position)/2)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, theSpeed * Time.deltaTime);
            speed -= rate;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, theSpeed * Time.deltaTime);
            speed += rate;
        }

        if (Vector2.Distance(transform.position, target.position) <= 0.01f)
        {
            state = MothStates.CHOOSING;
        }
    }

    void WaitBeforeChoosing()
    {
        if (moveCountdown <= 0)
        {
            moveCountdown = 0;
            // Choose next move point
            ChooseNextMovePoint();
        }
        else
        {
            moveCountdown -= Time.deltaTime;
        }
    }

    void ChooseNextMovePoint()
    {
        // Check if we are traversing up the array (ie 1-3 or 3-1)
        if (traverseUp)
        {
            // Check current if index is less then array length
            if (nextPoint < movePoints.Length - 1)
            {
                // Iterate up to next index
                nextPoint++;
                state = MothStates.IDLE;
                speed = minSpeed;
                startPos = transform.position;
            }
            else
            {
                // If current index is equal to array length, begin to traverse down
                traverseUp = false;
            }
        }
        else
        {
            // Check current index is greater than zero
            if (nextPoint > 0)
            {
                // Iterate down to next index
                nextPoint--;
                state = MothStates.IDLE;
                speed = minSpeed;
                startPos = transform.position;
            }
            else
            {
                // If current index is equal to 0, begin to traverse back up
                traverseUp = true;
            }
        }
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
}
