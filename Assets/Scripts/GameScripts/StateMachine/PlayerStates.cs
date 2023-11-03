using UnityEngine;


//设置状态，继承State并且泛型为PlayerS,可以实现State里的Virtual方法，并且可以使用PlayerS的上下文
/// <summary>
/// 主角待机状态
/// </summary>
public class IdleState : State<PlayerS>
{
    public IdleState(PlayerS context) : base(context)
    {
        Debug.Log(context.gameObject.name + " !!!!!!!!Enter Idle State!!!!!!!!");
    }

    public override void Enter()
    {
        context.isGround = true;
        Debug.Log("Enter Idle");
        context.jumpCount = 1;
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
        //Debug.Log("Idling");
    }
}

/// <summary>
/// 主角跑动状态
/// </summary>
public class RunState : State<PlayerS>
{
    public RunState(PlayerS context) : base(context)
    {

    }

    public override void Enter()
    {
        context.isGround = true;
        Debug.Log("Enter Run");
        context.jumpCount = 1;
        context.SetAnimationBool("isGround", context.isGround);
        context.SetAnimationBool("isJump", context.isJumping);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
        //Debug.Log("Runing");
        //if (context.groundCount < 2)
        //{
        //    context.col.enabled = false;
        //}
        //else
        //{
        //    context.col.enabled = true;
        //}
    }
}

/// <summary>
/// 主角跳跃状态
/// </summary>
public class JumpState : State<PlayerS>
{
    public JumpState(PlayerS context) : base(context)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter Jump");
        context.jumpCount--;
        context.isGround = false;
        context.detectGround = false;
        context.isJumping = true;
        context.SetAnimationBool("isJump", context.isJumping);
        context.SetAnimationBool("isGround", context.isGround);
    }

    public override void Update()
    {
        context.SetAniamtionValue("yVelocity", context.yVelocity);        
    }

    public override void Exit() 
    {
                
    }
}

public class FallState : State<PlayerS>
{
    public FallState(PlayerS context) : base(context)
    {

    }

    public override void Enter() 
    {
        Debug.Log("Enter Fall");
    }

    public override void Update()
    {
        context.isFall = true;
        context.SetAniamtionValue("yVelocity", context.yVelocity);
        context.SetAnimationBool("isFall", context.isFall);
        context.detectGround = true;
    }

    public override void Exit()
    {
        context.isFall = false;
        context.isJumping = false;
        context.SetAnimationBool("isFall", context.isFall);
    }
}

/// <summary>
/// 主角攻击状态
/// </summary>

public class AttackState : State<PlayerS>
{
    float exitTime;
    AnimatorStateInfo attackStateInfo;
    public AttackState(PlayerS context) : base(context)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter Attack");
        context.canMoveByAttack = false;
        context.SetAniamtionTrigger("Attack");
    }

    public override void Update()
    {
        Debug.Log("Attacking");          
    }

    public override void Exit()
    {
        context.canMoveByAttack = true;
    }
}


