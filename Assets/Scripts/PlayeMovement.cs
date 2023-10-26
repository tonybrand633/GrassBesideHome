using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayeMovement : MonoBehaviour
{
    //组件
    public Animator animator;
    public SpriteRenderer sr;
    BoxCollider2D col;

    //用于Animator的速度
    public float _speedAnimator;
    //人物是否反转？
    public bool revertFlip;

    //定义碰撞检测的层级
    public LayerMask collisionLayer;

    //定义移动的速度
    public float maxSpeed = 5f;


    //物体的实时移动速度
    private Vector2 velocity;

    private Bounds colBounds;

    Vector2 detectPoint;


    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
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

        velocity = new Vector2 (horizontalInput,verticalInput);

        // Move the object based on the velocity
        _speedAnimator = velocity.magnitude * maxSpeed;
        transform.position += (Vector3)velocity *maxSpeed* Time.deltaTime;

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

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(detectPoint, Vector3.one*0.1f);
    }
}
