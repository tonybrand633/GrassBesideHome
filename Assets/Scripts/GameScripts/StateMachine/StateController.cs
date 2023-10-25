using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class StateController<T>
{
    //包含一个只读的State状态
    //这个类用来控制State，包含初始化，转换，常驻

    public State<T> currentState 
    {
        get; private set;
    }

    public void InitializeState(State<T>initializeState) 
    {
        currentState = initializeState;
        initializeState.Enter();
    }

    public void TransitionState(State<T>transState) 
    {
        currentState.Exit();
        currentState = transState;
        transState.Enter();
    }

    //让State一直保持当前的状态
    public void Update() 
    {
        currentState.Update();
    }
}

//设置状态，继承State并且泛型为PlayerS,可以实现State里的Virtual方法，并且可以使用PlayerS的上下文
/// <summary>
/// 主角待机状态
/// </summary>
public class IdleState : State<PlayerS>
{
    public IdleState(PlayerS context) : base(context)
    {
        Debug.Log(context.gameObject.name+" Enter Idle State!!!!!!!!");
    }

    public override void Enter() 
    {
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update()
    {
        //Debug.Log("正在持续Idle");
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
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
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
        context.SetAnimationBool("isJump", true);
        context.SetAnimationBool("isGround", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("yVelocity", context.yVelocity);
        //context.SetAnimationBool("isGround", false);
    }
}
