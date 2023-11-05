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
        context.ClearAttackSymbol();
        context.attackCount = 0;
        Debug.Log("Enter Idle");
        context.jumpCount = 1;
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
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
        context.ClearAttackSymbol();
        Debug.Log("Enter Run");
        context.jumpCount = 1;
        context.attackCount = 0;
        context.SetAnimationBool("isGround", context.isGround);
        context.SetAnimationBool("isJump", context.isJumping);
    }

    public override void Update()
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
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
        context.ClearAttackSymbol();
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

/// <summary>
/// 主角掉落状态-分离了Jump
/// </summary>
public class FallState : State<PlayerS>
{
    public FallState(PlayerS context) : base(context)
    {

    }

    public override void Enter() 
    {
        context.ClearAttackSymbol();
        Debug.Log("Enter Fall");
    }

    public override void Update()
    {
        context.isFall = true;
        context.SetAniamtionValue("yVelocity", context.yVelocity);
        context.SetAnimationBool("isGround", context.isGround);
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

public class AttackState_01 : State<PlayerS>
{
    float exitTime;
    AnimatorStateInfo attackStateInfo;
    


    public AttackState_01(PlayerS context) : base(context)
    {

    }

    public override void Enter()
    {
        context.SetAniamtionTrigger("Attack_01");
        context.isAttacking = true;
        context.canMoveByAttack = false;
        context.attackCount = 1;
        Debug.Log("Enter Attack_01");        
    }

    public override void Update()
    {
        //Debug.Log("Attacking_01");
        if (context.eventTrigger1)
        {
            context.isAttacking = false;
            
        }
        else 
        {
            context.isAttacking = true;
        }        
    }

    public override void Exit()
    {
        context.eventTrigger1 = false;
        context.eventTrigger2 = false;
        context.eventTrigger3 = false;
        context.canMoveByAttack = true;
        context.lastAttckTime = -Mathf.Infinity;
        Debug.Log("Exit Attack_01");
    }

    public class AttackState_02 : State<PlayerS>
    {
        public AttackState_02(PlayerS context) : base(context)
        {

        }

        public override void Enter() 
        {
            context.attackCount = 2; 
            Debug.Log("Enter Attack2");
            context.canMoveByAttack = false;
            context.SetAniamtionTrigger("Attack_02");

        }

        public override void Update() 
        {
            //Debug.Log("Attacking02"); 
            if (context.eventTrigger2)
            {
                context.isAttacking = false;

            }
            else
            {
                context.isAttacking = true;
            }
        }

        public override void Exit()
        {
            context.canMoveByAttack = true;
            context.eventTrigger1 = false;
            context.eventTrigger2 = false;
            context.eventTrigger3 = false;
            context.lastAttckTime = -Mathf.Infinity;
            Debug.Log("Exit Attack_02");
        }
    }

    public class AttackState_03 : State<PlayerS>
    {
        public AttackState_03(PlayerS context) : base(context)
        {
            
        }

        public override void Enter()
        {
            Debug.Log("Enter Attack3");
            context.attackCount = 3;
            context.canMoveByAttack = false;
            context.SetAniamtionTrigger("Attack_03");
        }

        public override void Update()
        {
            //Debug.Log("Attacking03");
            if (context.eventTrigger3)
            {
                context.isAttacking = false;

            }
            else
            {
                context.isAttacking = true;
            }
        }

        public override void Exit()
        {
            context.canMoveByAttack = true;
            context.eventTrigger1 = false;
            context.eventTrigger2 = false;
            context.eventTrigger3 = false;
            context.lastAttckTime = -Mathf.Infinity;
            Debug.Log("Exit Attack_03");
        }
    }
}


