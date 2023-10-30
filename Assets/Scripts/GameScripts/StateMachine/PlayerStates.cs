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
        context.isJumping = false;
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
        Debug.Log("Idle");
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
        context.isJumping = false;
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
        Debug.Log("Runing");
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
        context.isGround = false;
        context.isJumping = true;
        context.SetAnimationBool("isJump", true);
        context.SetAnimationBool("isGround", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("yVelocity", context.yVelocity);
        Debug.Log("Jumping");
    }
}

/// <summary>
/// 主角攻击状态
/// </summary>

public class AttackState : State<PlayerS>
{
    float exitTime;
    public AttackState(PlayerS context) : base(context)
    {

    }

    public override void Enter()
    {
        context.SetAniamtionTrigger("Attack");
        context.attackStartTime = Time.time;
    }

    public override void Update()
    {
        exitTime = context.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Debug.Log("Attacking");
        if (exitTime == 1) 
        {
            this.Exit();
        }
    }

    public override void Exit()
    {
        
    }
}


