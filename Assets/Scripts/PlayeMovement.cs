using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayeMovement : MonoBehaviour
{
    //���
    public Animator animator;
    public SpriteRenderer sr;
    BoxCollider2D col;

    //����Animator���ٶ�
    public float _speedAnimator;
    //�����Ƿ�ת��
    public bool revertFlip;

    //������ײ���Ĳ㼶
    public LayerMask collisionLayer;

    //�����ƶ����ٶ�
    public float maxSpeed = 5f;


    //�����ʵʱ�ƶ��ٶ�
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

        //ʹ��SpriteRendererʵ��ת��
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
