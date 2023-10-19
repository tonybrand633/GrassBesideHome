using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // ����״̬�������ȼ�
    public enum MyStates
    {
        Emergency,    // ������ȼ�
        Hurt,
        Attack,
        Jump,
        Run,
        WallSlideRight,
        WallSlideLeft,
        Idle,         // ������ȼ�        
    }

    private MyStates currentState = MyStates.Idle;

    // ���·��������ݵ�ǰ״ִ̬����Ӧ�߼�
    void Update()
    {
        switch (currentState)
        {
            case MyStates.Emergency:
                HandleEmergencyState();
                break;

            case MyStates.Hurt:
                HandleHurtState();
                break;

            case MyStates.Attack:
                HandleAttackState();
                break;

            case MyStates.Jump:
                HandleJumpState();
                break;

            case MyStates.Run:
                HandleRunState();
                break;

            case MyStates.WallSlideRight:
                HandleWallSliderRightState();
                break;

            case MyStates.WallSlideLeft:
                HandleWallSliderLeftState();
                break;

            case MyStates.Idle:
                HandleIdleState();
                break;
                // ... ����״̬����
        }
    }

    // ״̬������
    void HandleEmergencyState()
    {
        // �߼�����...
    }

    void HandleHurtState()
    {
        // �߼�����...
    }
    void HandleAttackState()
    {
        // �߼�����...
    }
    void HandleRunState()
    {
        // �߼�����...
    }
    void HandleJumpState()
    {
        // �߼�����...
    }
    void HandleWallSliderRightState()
    {
        // �߼�����...
    }
    void HandleWallSliderLeftState()
    {
        // �߼�����...
    }
    void HandleIdleState()
    {
        // �߼�����...
    }

    // ����״̬�����������ȼ����
    public void ChangeState(MyStates newState)
    {
        if (newState <= currentState)
        {
            currentState = newState;
            // ������Ҫ��������������߼������粥���ض�������������������ͷż��ܻ��ߵ��ߺ󣬹�����������
        }
    }

    // ǿ�Ƹ���״̬
    public void ForceChangeState(MyStates newState)
    {
        currentState = newState;
        // ������Ҫ��������������߼���
    }
}
