using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AttackState_01;

public class PlayerS : MonoBehaviour
{
    //��ɫ����
    [Header("��ɫ����")]
    public float moveSpeed = 5.0f;  // �����ƶ��ٶ�
    public float sprintSpeed = 10.0f;  // �����ƶ��ٶ�    
    public float currentSpeed;  // ��ǰ�ٶ�
    public Vector2 jumpForce = new Vector2(0, 10f);
    public float yVelocity;
    public float groundDetectDis = 0.05f;
    public float wallDetectDis = 0.1f;
    public int jumpCount = 1;

    //�������
    [Header("�������")]
    public float attackStartTime;
    public float lastAttckTime = -Mathf.Infinity;
    public float attackCoolDown = 0.25f;
    public float timeSinceLastAttack;
    public int attackCount;
    
    
    
    float moveHorizontal;
    float moveVertical;
    [Header("�������")]
    public Vector3 movement;

    //��ɫ���
    Rigidbody2D rb2d;
    public GameObject catFood;
    public Animator anim;
    SpriteRenderer sr;
    BoxCollider2D col;

    //��ɫ״̬
    [Header("��ɫ״̬")]
    public bool isWalkMode;
    public bool canMoveByHitWall;
    public bool isAttacking;
    public bool canMoveByAttack;
    public bool canJump;
    public bool isJumping;
    public bool isGround;
    public bool isAttack;
    public bool isFall;
    public bool eventTrigger1 = false;
    public bool eventTrigger2 = false;
    public bool eventTrigger3 = false;
    public string currentState;

    //״̬��
    public StateController<PlayerS> playerStateMachine;
    IdleState idleState;
    RunState runState;
    JumpState jumpState;
    AttackState_01 attackState_01;
    AttackState_02 attackState_02;
    AttackState_03 attackState_03;
    FallState fallState;

    //�¼�
    public static event Action<GameObject> OnCatFoodGenerated;

    [Header("��ɫ���")]
    //��ɫ�Զ�����
    public float groundCheckOffset;
    public float groundCount;
    public bool detectGround;
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
    RaycastHit2D groundHitL, groundHitR, groundHitCenter, groundHitCenterNextL, groundHitCenterNextR;
    RaycastHit2D wallHitLeftTop, wallHitLeftBottom, wallHitRightTop, wallHitRightBottom, wallHitRight, wallHitLeft;
    bool[] groundCheck;

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
        attackState_01 = new AttackState_01(this);
        attackState_02 = new AttackState_02(this);
        attackState_03 = new AttackState_03(this);
        fallState = new FallState(this);
        
        detectGround = true;
        canMoveByAttack = true;

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
        //AttackCondition();
        playerStateMachine.Update();
        currentState = playerStateMachine.currentState.ToString();
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

        //�жϽ�ɫ����Ծ����
        if (jumpCount > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (yVelocity<0&&playerStateMachine.currentState!=fallState) 
        {
            //------����׹��״̬---------//
            playerStateMachine.TransitionState(fallState);
        }

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

        //�жϽ�ɫ����Ծ,�ж��Ƿ��¿ո�--ʹ��isJumping�жϻᵼ��Ҫһֱ����״̬,����������ˣ��Ϳ����ж�Ϊ��Ծ
        if (Input.GetKeyDown(KeyCode.Space)&&canJump)
        {            
            rb2d.AddForce(jumpForce, ForceMode2D.Impulse);
            //------������Ծ״̬---------//
            playerStateMachine.TransitionState(jumpState);
        }



        //�жϽ�ɫ����������������        
        if (Input.GetMouseButtonDown(0)) 
        {            
            timeSinceLastAttack = Time.time - lastAttckTime;
            //------���빥��״̬---------//
            if (lastAttckTime == -Mathf.Infinity && attackCount == 0)
            {
                if (timeSinceLastAttack != Mathf.Infinity && timeSinceLastAttack >= attackCoolDown)
                {
                    playerStateMachine.TransitionState(attackState_01);
                    lastAttckTime = Time.time;
                }
                else 
                {
                    playerStateMachine.TransitionState(attackState_01);
                    lastAttckTime = Time.time;
                }          
            }
            else if (attackCount == 1 && timeSinceLastAttack >= attackCoolDown)
            {
                playerStateMachine.TransitionState(attackState_02);
                lastAttckTime = Time.time;
            } else if (attackCount == 2 && timeSinceLastAttack>= attackCoolDown) 
            {
                playerStateMachine.TransitionState(attackState_03);
                lastAttckTime = Time.time;
            } 
        }        
    }

    void MovePlayer()
    {
        if (isWalkMode&&canMoveByHitWall&&canMoveByAttack)
        {
            rb2d.velocity = new Vector2(movement.x * currentSpeed, rb2d.velocity.y);            
        }            
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
        groundHitL = Physics2D.Raycast(leftBottom, Vector2.down, groundDetectDis, groundLayer);
        groundHitR = Physics2D.Raycast(rightBottom, Vector2.down, groundDetectDis, groundLayer);
        groundHitCenter = Physics2D.Raycast(bottom, Vector2.down, groundDetectDis, groundLayer);
        groundHitCenterNextR = Physics2D.Raycast(new Vector2(bottom.x+groundCheckOffset,bottom.y), Vector2.down, groundDetectDis, groundLayer);
        groundHitCenterNextL = Physics2D.Raycast(new Vector2(bottom.x-groundCheckOffset, bottom.y), Vector2.down, groundDetectDis, groundLayer);

        groundCheck = new bool[5] { groundHitL, groundHitR, groundHitCenter ,groundHitCenterNextL,groundHitCenterNextR};

        groundCount = 0;
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (groundCheck[i]) groundCount++;
        }


        if ((groundHitL || groundHitR || groundHitCenter || groundHitCenterNextL || groundHitCenterNextR)&&detectGround)
        {
            if (Mathf.Abs(movement.x) > 0 && playerStateMachine.currentState != runState &&!isAttacking)
            {
                //------�����ܶ�״̬---------//
                playerStateMachine.TransitionState(runState);

            }
            else if (movement.x == 0 && playerStateMachine.currentState != idleState && !isAttacking)
            {                                
                //------�������״̬---------//
                playerStateMachine.TransitionState(idleState);
            }
        }
        //һ���������ʵ�ֵĹ��ܣ�������漴��Ϊ����Ծ
        else
        {
            //if (playerStateMachine.currentState != jumpState&&canMoveByAttack) 
            //{
            //    playerStateMachine.TransitionState(jumpState);
            //}                       
        }         
    }

    void CheckForWall() 
    {
        wallHitLeftTop = Physics2D.Raycast(leftTop, Vector2.left, wallDetectDis,wallLayer);
        wallHitLeftBottom = Physics2D.Raycast(leftBottom,Vector2.left,wallDetectDis,wallLayer);
        wallHitRightBottom = Physics2D.Raycast(rightBottom,Vector2.right,wallDetectDis,wallLayer);
        wallHitRightTop = Physics2D.Raycast(rightTop,Vector2.right, wallDetectDis, wallLayer);
        wallHitLeft = Physics2D.Raycast(left,Vector2.left, wallDetectDis, wallLayer);
        wallHitRight = Physics2D.Raycast(right, Vector2.right, wallDetectDis, wallLayer);
        if (wallHitLeftTop||wallHitLeftBottom||wallHitRightBottom||wallHitRightTop) 
        {            
            canMoveByHitWall = false;
        }
        

        if (((wallHitLeftTop||wallHitLeftBottom||wallHitLeft)&&movement.x>0)||((wallHitRightTop||wallHitRightBottom||wallHitRight)&&movement.x<0)||(!wallHitRightTop&&!wallHitLeftTop&&!wallHitLeftBottom&&!wallHitRightBottom&&!wallHitLeft&&!wallHitRight)) 
        {
            canMoveByHitWall = true;

            bool leftbool = ((wallHitLeftTop || wallHitLeftBottom||wallHitLeft) && movement.x > 0);
            bool rightbool = ((wallHitRightTop || wallHitRightBottom||wallHitRight) && movement.x < 0);
            bool allbool = (!wallHitRightTop && !wallHitLeftTop && !wallHitLeftTop && !wallHitRightBottom&&!wallHitLeft&&!wallHitRight);
            if (wallHitLeftTop.collider!=null) 
            {
                //Debug.Log(wallHitLeftTop.collider.gameObject.name);
                //Debug.Log("leftbool: " + leftbool);
                //Debug.Log("rightbool: " + rightbool);
                //Debug.Log("allbool: " + allbool);
            }
            if (wallHitLeftBottom.collider != null)
            {
                //Debug.Log(wallHitLeftBottom.collider.gameObject.name);
                //Debug.Log("leftbool: " + leftbool);
                //Debug.Log("rightbool: " + rightbool);
                //Debug.Log("allbool: " + allbool);
            }
            if (wallHitRightTop.collider != null)
            {
                //Debug.Log(wallHitRightTop.collider.gameObject.name);
                //Debug.Log("leftbool: " + leftbool);
                //Debug.Log("rightbool: " + rightbool);
                //Debug.Log("allbool: " + allbool);

            }
            if (wallHitRightBottom.collider != null)
            {
                //Debug.Log(wallHitRightBottom.collider.gameObject.name);
                //Debug.Log("leftbool: " + leftbool);
                //Debug.Log("rightbool: " + rightbool);
                //Debug.Log("allbool: " + allbool);
            }         
        }
    }

    public void Attack1_Done(int count)
    {       
        if (playerStateMachine.currentState == attackState_01|| playerStateMachine.currentState == attackState_02|| playerStateMachine.currentState == attackState_03) 
        {
            eventTrigger1 = true;
        }        
    }

    public void Attack2_Done(int count)
    {
        if (playerStateMachine.currentState == attackState_01 || playerStateMachine.currentState == attackState_02 || playerStateMachine.currentState == attackState_03)
        {
            eventTrigger2 = true;
        }
    }
    public void Attack3_Done(int count)
    {
        if (playerStateMachine.currentState == attackState_01 || playerStateMachine.currentState == attackState_02 || playerStateMachine.currentState == attackState_03)
        {
            eventTrigger3 = true;
        }
    }

    public void ClearAttackSymbol() 
    {
        isAttacking = false;
        eventTrigger1 = false;
        eventTrigger2 = false;
        eventTrigger3 = false;
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
        Gizmos.DrawWireSphere(top, groundDetectDis);
        Gizmos.DrawWireSphere(bottom, groundDetectDis);
        Gizmos.DrawWireSphere(left, groundDetectDis);
        Gizmos.DrawWireSphere(right, groundDetectDis);
        Gizmos.DrawWireSphere(rightTop, groundDetectDis);
        Gizmos.DrawWireSphere(rightBottom, groundDetectDis);
        Gizmos.DrawWireSphere(leftTop, groundDetectDis);
        Gizmos.DrawWireSphere(leftBottom, groundDetectDis);
        Gizmos.DrawWireSphere(new Vector2(bottom.x + groundCheckOffset,bottom.y), groundDetectDis);
        Gizmos.DrawWireSphere(new Vector2(bottom.x - groundCheckOffset, bottom.y), groundDetectDis);
    }
}
