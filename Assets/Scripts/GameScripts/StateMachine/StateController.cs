using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class StateController<T>
{
    //����һ��ֻ����State״̬
    //�������������State��������ʼ����ת������פ

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

    //��Stateһֱ���ֵ�ǰ��״̬
    public void Update() 
    {
        currentState.Update();
    }
}

//����״̬���̳�State���ҷ���ΪPlayerS,����ʵ��State���Virtual���������ҿ���ʹ��PlayerS��������
/// <summary>
/// ���Ǵ���״̬
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
        //Debug.Log("���ڳ���Idle");
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
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
        context.SetAnimationBool("isGround", true);
        context.SetAnimationBool("isJump", false);
    }

    public override void Update() 
    {
        context.SetAniamtionValue("Speed", Mathf.Abs(context.movement.x));
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
        context.SetAnimationBool("isJump", true);
        context.SetAnimationBool("isGround", false);
    }

    public override void Update()
    {
        context.SetAniamtionValue("yVelocity", context.yVelocity);
        //context.SetAnimationBool("isGround", false);
    }
}
