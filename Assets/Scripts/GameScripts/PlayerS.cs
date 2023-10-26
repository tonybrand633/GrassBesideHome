using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerS : MonoBehaviour
{
    //��ɫ����
    [Header("��ɫ����")]
    public float moveSpeed = 5.0f;  // �����ƶ��ٶ�
    public float sprintSpeed = 10.0f;  // �����ƶ��ٶ�    
    public float currentSpeed;  // ��ǰ�ٶ�
    public Vector2 jumpForce = new Vector2(0, 10f);
    public float yVelocity;
    

    //�������
    float moveHorizontal;
    float moveVertical;
    public Vector3 movement;

    //��ɫ���
    Rigidbody2D rb2d;
    public GameObject catFood;
    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D col;

    //��ɫ״̬
    public bool isWalkMode;
    public bool canMoveVelocity;
    public bool isJumping;

    //״̬��
    public StateController<PlayerS> playerStateMachine;
    IdleState idleState;
    RunState runState;
    JumpState jumpState;

    //�¼�
    public static event Action<GameObject> OnCatFoodGenerated;

    //��ɫ�Զ�����
    Bounds colBounds;
    Vector2 top;
    Vector2 rightTop;
    Vector2 rightBottom;
    Vector2 leftTop;
    Vector2 leftBottom;
    Vector2 right;
    Vector2 bottom;
    Vector2 left;
    public LayerMask wallLayer;
    public LayerMask groundLayer;



    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //���ɲ�ͬ״̬��
        idleState = new IdleState(this);
        runState = new RunState(this);
        jumpState = new JumpState(this);

        // ��ʼʱ�趨��ǰ�ٶ�Ϊ�����ƶ��ٶ�
        currentSpeed = moveSpeed;
        playerStateMachine = new StateController<PlayerS>();
        playerStateMachine.InitializeState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        StateMachineCondition();
        DetectPlayerAround();
        playerStateMachine.Update();
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// ״̬���������ö�������
    /// </summary>
    void StateMachineCondition()
    {
        //�жϽ�ɫ���ٶ�
        if (Mathf.Abs(movement.x) > 0.1&&playerStateMachine.currentState!=jumpState)
        {
            playerStateMachine.TransitionState(runState);
        }

        if (movement.x == 0)
        {
            playerStateMachine.TransitionState(idleState);
        }

        //�жϲ����ƽ�ɫ�ĳ���
        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;
        }


        yVelocity = rb2d.velocity.y;
    }

    public void SetAnimation()
    {

    }

    public void SetAniamtionTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void SetAnimationBool(string boolState, bool value)
    {
        anim.SetBool(boolState, value);
    }

    public void SetAniamtionValue(string valueName, float value)
    {
        anim.SetFloat(valueName, value);
    }

    /// <summary>
    /// ��ȡ��ɫ�Ŀ�������
    /// </summary>
    void GetInput()
    {
        if (isWalkMode)
        {
            // ��ȡ�����������
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            //moveVertical = Input.GetAxisRaw("Vertical");

            // �����ƶ�����
            movement = new Vector3(moveHorizontal, 0.0f, 0.0f).normalized;

            // �ж��Ƿ��¼��ټ� (���磬Shift��)
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        //����ģʽ
        else
        {
            // ��ȡ�����������
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");

            // �����ƶ�����
            movement = new Vector3(moveHorizontal, moveVertical, 0.0f).normalized;

            // �ж��Ƿ��¼��ټ� (���磬Shift��)
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        //�жϽ�ɫ����Ծ
        if (Input.GetKeyDown(KeyCode.Space)&&!isJumping)
        {
            rb2d.AddForce(jumpForce, ForceMode2D.Impulse);
            playerStateMachine.TransitionState(jumpState);
        }
    }

    void MovePlayer()
    {
        // ���ʹ��Rigidbody2D�ƶ�������ʹ��velocity���Խ����ƶ�
        if (isWalkMode&&canMoveVelocity)
        {
            rb2d.velocity = new Vector2(movement.x * currentSpeed, rb2d.velocity.y);            
        }
        else
        {
            //��Ծģʽ��һ����֮���
            //rb2d.velocity = new Vector2(movement.x * currentSpeed, rb2d.velocity.y + movement.y * currentSpeed);
        }

        // ������ʹ��Transform���ƶ�
        //transform.position += movement * currentSpeed * Time.deltaTime;       
    }

    /// <summary>
    /// ��ɫ�ļ����Ϊ
    /// һЩ����ļ�⣬�Ƚ���ʵ�֣�������ɫ�Ƿ��ܼ�⵽���壬�Լ���ʲô�������м��
    /// </summary>
    void DetectPlayerAround() 
    {
        colBounds = col.bounds;
        top = Utils.GetTopPoint(colBounds);
        right = Utils.GetRightPoint(colBounds);
        bottom = Utils.GetBottomPoint(colBounds);
        left = Utils.GetLeftPoint(colBounds);
        rightBottom = Utils.GetRightBottomPoint(colBounds);
        rightTop = Utils.GetRightTopPoint(colBounds);
        leftBottom = Utils.GetLeftBottomPoint(colBounds);
        leftTop = Utils.GetLeftTopPoint(colBounds);
        
        CheckForGround();
        CheckForWall();
    }

    void CheckForGround() 
    {
        RaycastHit2D groundHitL,groundHitR,groundHitCenter,groundHitCenterCrossR,groundHitCenterCrossL;
        groundHitL = Physics2D.Raycast(leftBottom, Vector2.down, 0.1f, groundLayer);
        groundHitR = Physics2D.Raycast(rightBottom, Vector2.down, 0.1f, groundLayer);
        groundHitCenter = Physics2D.Raycast(bottom, Vector2.down, 0.1f, groundLayer);
        groundHitCenterCrossR = Physics2D.Raycast(bottom, Vector2.down+Vector2.right, 0.1f, groundLayer);
        groundHitCenterCrossL = Physics2D.Raycast(bottom, Vector2.down + Vector2.left, 0.1f, groundLayer);

        if (groundHitL||groundHitR||groundHitCenter||groundHitCenterCrossL||groundHitCenterCrossR)
        {
            if (Mathf.Abs(movement.x) > 0)
            {
                playerStateMachine.TransitionState(runState);
            } else if (movement.x == 0) 
            {
                playerStateMachine.TransitionState(idleState);
            }
        }
        else 
        {
            playerStateMachine.TransitionState(jumpState);            
        }

    }

    void CheckForWall() 
    {
        RaycastHit2D wallHitLeftTop, wallHitLeftBottom, wallHitRightTop, wallHitRightBottom,wallHitRight,wallHitLeft;
        wallHitLeftTop = Physics2D.Raycast(leftTop, Vector2.left, 0.1f,wallLayer);
        wallHitLeftBottom = Physics2D.Raycast(leftBottom,Vector2.left,0.1f,wallLayer);
        wallHitRightBottom = Physics2D.Raycast(rightBottom,Vector2.right,0.1f,wallLayer);
        wallHitRightTop = Physics2D.Raycast(rightTop,Vector2.right,0.1f,wallLayer);
        wallHitLeft = Physics2D.Raycast(left,Vector2.left,0.1f,wallLayer);
        wallHitRight = Physics2D.Raycast(right, Vector2.right, 0.1f, wallLayer);
        if (wallHitLeftTop||wallHitLeftBottom||wallHitRightBottom||wallHitRightTop) 
        {            
            canMoveVelocity = false;
        }

        if (((wallHitLeftTop||wallHitLeftBottom||wallHitLeft)&&movement.x>0)||((wallHitRightTop||wallHitRightBottom||wallHitRight)&&movement.x<0)||(!wallHitRightTop&&!wallHitLeftTop&&!wallHitLeftBottom&&!wallHitRightBottom&&!wallHitLeft&&!wallHitRight)) 
        {
            bool leftbool = ((wallHitLeftTop || wallHitLeftBottom||wallHitLeft) && movement.x > 0);
            bool rightbool = ((wallHitRightTop || wallHitRightBottom||wallHitRight) && movement.x < 0);
            bool allbool = (!wallHitRightTop && !wallHitLeftTop && !wallHitLeftTop && !wallHitRightBottom&&!wallHitLeft&&!wallHitRight);

            if (wallHitLeftTop.collider!=null) 
            {
                Debug.Log(wallHitLeftTop.collider.gameObject.name);
                Debug.Log("leftbool: " + leftbool);
                Debug.Log("rightbool: " + rightbool);
                Debug.Log("allbool: " + allbool);
            }
            if (wallHitLeftBottom.collider != null)
            {
                Debug.Log(wallHitLeftBottom.collider.gameObject.name);
                Debug.Log("leftbool: " + leftbool);
                Debug.Log("rightbool: " + rightbool);
                Debug.Log("allbool: " + allbool);
            }
            if (wallHitRightTop.collider != null)
            {
                Debug.Log(wallHitRightTop.collider.gameObject.name);
                Debug.Log("leftbool: " + leftbool);
                Debug.Log("rightbool: " + rightbool);
                Debug.Log("allbool: " + allbool);
            }
            if (wallHitRightBottom.collider != null)
            {
                Debug.Log(wallHitRightBottom.collider.gameObject.name);
                Debug.Log("leftbool: " + leftbool);
                Debug.Log("rightbool: " + rightbool);
                Debug.Log("allbool: " + allbool);
            }

            canMoveVelocity = true;            
        }
    }
    /// <summary>
    /// ���������ǵ�ιè��Ϊ-����è��
    /// </summary>
    void GenerateCatFood()
    {
        // ��������B
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 pos = transform.position;
            GameObject objectB = Instantiate(catFood, pos, Quaternion.identity);

            //���OnCatFoodGenerated��Ϊnull�����з�������������¼���ί�У�����ô������Щ���ĵķ���������objectB��Ϊ�������ݸ����ǡ�
            OnCatFoodGenerated?.Invoke(objectB);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(top, 0.1f);
        Gizmos.DrawWireSphere(bottom, 0.1f);
        Gizmos.DrawWireSphere(left, 0.1f);
        Gizmos.DrawWireSphere(right, 0.1f);
        Gizmos.DrawWireSphere(rightTop, 0.1f);
        Gizmos.DrawWireSphere(rightBottom, 0.1f);
        Gizmos.DrawWireSphere(leftTop, 0.1f);
        Gizmos.DrawWireSphere(leftBottom, 0.1f);

        //Gizmos.DrawRay(leftBottom, Vector2.down+rightBottom);
    }
}
