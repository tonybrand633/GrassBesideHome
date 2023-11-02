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
        context.isGround = true;
        Debug.Log("Enter Idle");
        context.jumpCount = 1;
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
        Debug.Log("Idling");
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
        context.isGround = true;
        Debug.Log("Enter Run");
        context.jumpCount = 1;
        context.SetAnimationBool("isGround", context.isGround);
        context.SetAnimationBool("isJump", context.isJumping);
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
        if (context.yVelocity<0) 
        {
            context.detectGround = true;
        }
        Debug.Log("Jumping");
    }

    public override void Exit() 
    {
        context.isJumping = false;
        context.detectGround = true;
    }
}

/// <summary>
/// ���ǹ���״̬
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
        context.SetAniamtionTrigger("Attack");
        attackStateInfo = context.anim.GetCurrentAnimatorStateInfo(0);
        
        context.attackStartTime = Time.time;
    }

    public override void Update()
    {
        exitTime = attackStateInfo.normalizedTime;
        Debug.Log(exitTime);
        if (attackStateInfo.normalizedTime>=1)
        {
            Debug.Log("Attack Done");
        }
        else 
        {
            Debug.Log(attackStateInfo.fullPathHash);
        }
               
        context.canMoveVelocity = false;
        Debug.Log("Attacking");        
    }

    public override void Exit()
    {
        Debug.Log("Attack State Exit");
        context.canMoveVelocity = true;
    }
}


