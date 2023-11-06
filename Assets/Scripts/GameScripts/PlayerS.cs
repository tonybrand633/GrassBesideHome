using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AttackState_01;

public class PlayerS : MonoBehaviour
{
    //角色参数
    [Header("角色参数")]
    public float moveSpeed = 5.0f;  // 正常移动速度
    public float sprintSpeed = 10.0f;  // 加速移动速度    
    public float currentSpeed;  // 当前速度
    public Vector2 jumpForce = new Vector2(0, 10f);
    public float yVelocity;
    public float groundDetectDis = 0.05f;
    public float wallDetectDis = 0.1f;
    public int jumpCount = 1;

    //攻击相关
    [Header("攻击相关")]
    public float attackStartTime;
    public float lastAttckTime = -Mathf.Infinity;
    public float attackCoolDown = 0.25f;
    public float timeSinceLastAttack;
    public int attackCount;
    
    
    
    float moveHorizontal;
    float moveVertical;
    [Header("输入变量")]
    public Vector3 movement;

    //角色组件
    Rigidbody2D rb2d;
    public GameObject catFood;
    public Animator anim;
    SpriteRenderer sr;
    BoxCollider2D col;

    //角色状态
    [Header("角色状态")]
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

    //状态机
    public StateController<PlayerS> playerStateMachine;
    IdleState idleState;
    RunState runState;
    JumpState jumpState;
    AttackState_01 attackState_01;
    AttackState_02 attackState_02;
    AttackState_03 attackState_03;
    FallState fallState;

    //事件
    public static event Action<GameObject> OnCatFoodGenerated;

    [Header("角色检测")]
    //角色自定义检测
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
        //生成不同状态？
        idleState = new IdleState(this);
        runState = new RunState(this);
        jumpState = new JumpState(this);
        attackState_01 = new AttackState_01(this);
        attackState_02 = new AttackState_02(this);
        attackState_03 = new AttackState_03(this);
        fallState = new FallState(this);
        
        detectGround = true;
        canMoveByAttack = true;

        // 开始时设定当前速度为正常移动速度
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
    /// 状态机器和设置动画动作
    /// </summary>
    void StateMachineCondition()
    {
        //判断并控制角色的朝向
        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;
        }
        yVelocity = rb2d.velocity.y;

        //判断角色的跳跃次数
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
            //------进入坠落状态---------//
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
    /// 获取角色的控制输入
    /// </summary>
    void GetInput()
    {
        if (isWalkMode)
        {
            // 获取方向键的输入
            moveHorizontal = Input.GetAxisRaw("Horizontal");

            // 创建移动向量
            movement = new Vector3(moveHorizontal, 0.0f, 0.0f).normalized;

            // 判断是否按下加速键 (例如，Shift键)
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        //飞行模式
        else
        {
            // 获取方向键的输入
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            moveVertical = Input.GetAxisRaw("Vertical");

            // 创建移动向量
            movement = new Vector3(moveHorizontal, moveVertical, 0.0f).normalized;

            // 判断是否按下加速键 (例如，Shift键)
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }

        //判断角色的跳跃,判断是否按下空格--使用isJumping判断会导致要一直增加状态,比如空中受伤，就可以判断为跳跃
        if (Input.GetKeyDown(KeyCode.Space)&&canJump)
        {            
            rb2d.AddForce(jumpForce, ForceMode2D.Impulse);
            //------进入跳跃状态---------//
            playerStateMachine.TransitionState(jumpState);
        }



        //判断角色攻击，按下鼠标左键        
        if (Input.GetMouseButtonDown(0)) 
        {            
            timeSinceLastAttack = Time.time - lastAttckTime;
            //------进入攻击状态---------//
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
    /// 角色的检测行为
    /// 一些情况的检测，先进行实现，看看角色是否能检测到物体，以及用什么方法进行检测
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
                //------进入跑动状态---------//
                playerStateMachine.TransitionState(runState);

            }
            else if (movement.x == 0 && playerStateMachine.currentState != idleState && !isAttacking)
            {                                
                //------进入待机状态---------//
                playerStateMachine.TransitionState(idleState);
            }
        }
        //一个不经意间实现的功能，脱离地面即认为是跳跃
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
    /// 以下是主角的喂猫行为-生成猫粮
    /// </summary>
    void GenerateCatFood()
    {
        // 生成物体B
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 pos = transform.position;
            GameObject objectB = Instantiate(catFood, pos, Quaternion.identity);

            //如果OnCatFoodGenerated不为null（即有方法订阅了这个事件或委托），那么调用这些订阅的方法，并将objectB作为参数传递给它们。
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
