using UnityEngine;


//����״̬���̳�State���ҷ���ΪPlayerS,����ʵ��State���Virtual���������ҿ���ʹ��PlayerS��������
/// <summary>
/// ���Ǵ���״̬
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
/// �����ܶ�״̬
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
/// ������Ծ״̬
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
/// ���ǹ���״̬
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


