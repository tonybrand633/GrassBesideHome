using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeMovement : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sr;
    public float Speed;
    public float _speedAnimator;
    public bool revertFlip;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        animator.SetFloat("Speed", _speedAnimator);
    }

    void Movement() 
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(h, v);
        Vector3 temp = transform.position;
        temp.x += dir.x * Speed * Time.deltaTime;
        temp.y += dir.y * Speed * Time.deltaTime;

        transform.position = temp;
        _speedAnimator = dir.magnitude * Speed;

        //使用SpriteRenderer实现转向
        if (revertFlip)
        {
            if (h < 0)
            {
                sr.flipX = false;
            }
            else if (h > 0)
            {
                sr.flipX = true;
            }
        }
        else 
        {
            if (h < 0)
            {
                sr.flipX = true;
            }
            else if (h > 0)
            {
                sr.flipX = false;
            }
        }
    }
}
