using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerS : MonoBehaviour
{
    //角色参数
    [Header("角色参数")]
    public float moveSpeed = 5.0f;  // 正常移动速度
    public float sprintSpeed = 10.0f;  // 加速移动速度    
    public float currentSpeed;  // 当前速度
    public Vector2 jumpForce = new Vector2(0, 10f);
    public float yVelocity;
    

    //输入变量
    float moveHorizontal;
    float moveVertical;
    public Vector3 movement;

    //角色组件
    Rigidbody2D rb2d;
    public GameObject catFood;
    Animator anim;
    SpriteRenderer sr;
    BoxCollider2D col;

    //角色状态
    public bool isWalkMode;
    public bool canMoveVelocity;

    //状态机
    public StateController<PlayerS> playerStateMachine;
    IdleState idleState;
    RunState runState;
    JumpState jumpState;

    //事件
    public static event Action<GameObject> OnCatFoodGenerated;

    //角色自定义检测
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
        //生成不同状态？
        idleState = new IdleState(this);
        runState = new RunState(this);
        jumpState = new JumpState(this);

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
        playerStateMachine.Update();
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
        //判断角色的速度
        if (Mathf.Abs(movement.x) > 0.1&&playerStateMachine.currentState!=jumpState)
        {
            playerStateMachine.TransitionState(runState);
        }

        if (movement.x == 0)
        {
            playerStateMachine.TransitionState(idleState);
        }

        //判断并控制角色的朝向
        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;
        }

        //判断角色的跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.AddForce(jumpForce, ForceMode2D.Impulse);
            
            playerStateMachine.TransitionState(jumpState);
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
    /// 获取角色的控制输入
    /// </summary>
    void GetInput()
    {
        if (isWalkMode)
        {
            // 获取方向键的输入
            moveHorizontal = Input.GetAxisRaw("Horizontal");
            //moveVertical = Input.GetAxisRaw("Vertical");

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
    }

    void MovePlayer()
    {
        // 如果使用Rigidbody2D移动，优先使用velocity属性进行移动
        if (isWalkMode&&canMoveVelocity)
        {
            rb2d.velocity = new Vector2(movement.x * currentSpeed, rb2d.velocity.y);            
        }
        else
        {
            //跳跃模式加一个力之类的
            //rb2d.velocity = new Vector2(movement.x * currentSpeed, rb2d.velocity.y + movement.y * currentSpeed);
        }

        // 否则还是使用Transform来移动
        //transform.position += movement * currentSpeed * Time.deltaTime;       
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
        RaycastHit2D groundHitL,groundHitR,groundHitCenter;
        groundHitL = Physics2D.Raycast(leftBottom, Vector2.down, 0.1f, groundLayer);
        groundHitR = Physics2D.Raycast(rightBottom, Vector2.down, 0.1f, groundLayer);
        groundHitCenter = Physics2D.Raycast(bottom, Vector2.down, 0.1f, groundLayer);
        if (groundHitL||groundHitR||groundHitCenter)
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
        RaycastHit2D wallHitLeftTop, wallHitLeftBottom, wallHitRightTop, wallHitRightBottom;
        wallHitLeftTop = Physics2D.Raycast(leftTop, Vector2.left, 0.1f,wallLayer);
        wallHitLeftBottom = Physics2D.Raycast(leftBottom,Vector2.left,0.1f,wallLayer);
        wallHitRightBottom = Physics2D.Raycast(rightBottom,Vector2.right,0.1f,wallLayer);
        wallHitRightTop = Physics2D.Raycast(rightTop,Vector2.right,0.1f,wallLayer);
        if (wallHitLeftTop||wallHitLeftBottom||wallHitRightBottom||wallHitRightTop) 
        {            
            canMoveVelocity = false;
        }

        if ((wallHitLeftTop||wallHitLeftBottom)&&movement.x>0||(wallHitRightTop||wallHitRightBottom)&&movement.x<0||(!wallHitRightTop&&!wallHitLeftTop&&!wallHitLeftTop&&!wallHitRightBottom)) 
        {
            canMoveVelocity = true;            
        }
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
        Gizmos.DrawWireSphere(top, 0.1f);
        Gizmos.DrawWireSphere(bottom, 0.1f);
        Gizmos.DrawWireSphere(left, 0.1f);
        Gizmos.DrawWireSphere(right, 0.1f);
        Gizmos.DrawWireSphere(rightTop, 0.1f);
        Gizmos.DrawWireSphere(rightBottom, 0.1f);
        Gizmos.DrawWireSphere(leftTop, 0.1f);
        Gizmos.DrawWireSphere(leftBottom, 0.1f);

        Gizmos.DrawRay(leftTop, Vector2.left);
    }
}
