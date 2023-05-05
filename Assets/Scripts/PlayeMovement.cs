using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeMovement : MonoBehaviour
{
    //组件
    public Animator animator;
    public SpriteRenderer sr;

    //用于Animator的速度
    public float _speedAnimator;
    //人物是否反转？
    public bool revertFlip;

    //定义碰撞检测的层级
    public LayerMask collisionLayer;

    //定义最大的检测距离，四个方向的点发出射线
    public float collisionDistance = 0.1f;

    //定义移动速度
    public float maxSpeed = 5f;

    //物体的实时移动速度
    private Vector2 velocity;

    private Bounds srBounds;
    Vector2 boundUp;
    Vector2 boundDown;
    Vector2 boundLeft;
    Vector2 boundRight;
    Vector2 boundUpRight;
    Vector2 boundUpLeft;
    Vector2 boundDownRight;
    Vector2 boundDownLeft;

    Vector2 detectPoint;



    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateBoundsPoint();
        animator.SetFloat("Speed", _speedAnimator);
    }
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 curDetectPoint;


        // Calculate the new velocity based on the input and max speed
        Vector2 newVelocity = new Vector2(horizontalInput, verticalInput) * maxSpeed;
        Vector2 dir = new Vector2(horizontalInput, verticalInput);
        // Normalize the vector to get a vector with a magnitude of 1
        dir.Normalize();

        // Get the angle of the vector in radians
        float angleRadians = Mathf.Atan2(dir.y, dir.x);

        // Convert the angle to degrees
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        Debug.Log(Mathf.RoundToInt(angleDegrees));
        string direction;


        switch (Mathf.RoundToInt(angleDegrees))
        {
            case 0:
                direction = "right";
                curDetectPoint = boundRight;
                break;
            case 45:
                direction = "upright";
                curDetectPoint = boundUpRight;
                break;
            case 90:
                direction = "up";
                curDetectPoint = boundUp;
                break;
            case 135:
                direction = "upleft";
                curDetectPoint = boundUpLeft;
                break;
            case 180:
                direction = "left";
                curDetectPoint = boundLeft;
                break;
            case -135:
                direction = "downleft";
                curDetectPoint = boundDownLeft;
                break;
            case -90:
                direction = "down";
                curDetectPoint = boundDown;
                break;
            case -45:
                direction = "downright";
                curDetectPoint = boundRight;
                break;
            default:
                direction = "none";
                curDetectPoint = transform.position;
                break;
        }
        detectPoint = curDetectPoint;
        //Debug.Log(direction);
        // Check for collisions in the direction of movement
        RaycastHit2D hit = Physics2D.Raycast(detectPoint, newVelocity.normalized, collisionDistance, collisionLayer);
        if (hit.collider == null)
        {
            velocity = newVelocity;
        }
        else
        {
            // If there is a collision, limit the movement to the collision point
            velocity = hit.point - (Vector2)transform.position;
        }

        // Move the object based on the velocity
        _speedAnimator = velocity.magnitude * maxSpeed;
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;

        //使用SpriteRenderer实现转向
        if (revertFlip)
        {
            if (horizontalInput < 0)
            {
                sr.flipX = false;
            }
            else if (horizontalInput > 0)
            {
                sr.flipX = true;
            }
        }
        else 
        {
            if (horizontalInput < 0)
            {
                sr.flipX = true;
            }
            else if (horizontalInput > 0)
            {
                sr.flipX = false;
            }
        }
    }

    void UpdateBoundsPoint() 
    {
        srBounds = sr.bounds;
        boundDown = new Vector2(srBounds.center.x, srBounds.min.y);
        boundUp = new Vector2(srBounds.center.x, srBounds.max.y);
        boundLeft = new Vector2(srBounds.min.x, srBounds.center.y);
        boundRight = new Vector2(srBounds.max.x, srBounds.center.y);
        boundDownLeft = new Vector2(srBounds.min.x, srBounds.min.y);
        boundDownRight = new Vector2(srBounds.max.x, srBounds.min.y);
        boundUpLeft = new Vector2(srBounds.min.x, srBounds.max.y);
        boundUpRight = new Vector2(srBounds.max.x, srBounds.max.y);
    }

    // Handle collisions with other objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Limit movement to the collision point
        velocity = collision.contacts[0].point - (Vector2)transform.position;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Limit movement to the collision point
        velocity = collision.contacts[0].point - (Vector2)transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(detectPoint, Vector3.one*0.1f);
    }
}
