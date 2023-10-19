using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // 定义状态及其优先级
    public enum MyStates
    {
        Emergency,    // 最高优先级
        Hurt,
        Attack,
        Jump,
        Run,
        WallSlideRight,
        WallSlideLeft,
        Idle,         // 最低优先级        
    }

    private MyStates currentState = MyStates.Idle;

    // 更新方法，根据当前状态执行相应逻辑
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
                // ... 其他状态处理
        }
    }

    // 状态处理方法
    void HandleEmergencyState()
    {
        // 逻辑处理...
    }

    void HandleHurtState()
    {
        // 逻辑处理...
    }
    void HandleAttackState()
    {
        // 逻辑处理...
    }
    void HandleRunState()
    {
        // 逻辑处理...
    }
    void HandleJumpState()
    {
        // 逻辑处理...
    }
    void HandleWallSliderRightState()
    {
        // 逻辑处理...
    }
    void HandleWallSliderLeftState()
    {
        // 逻辑处理...
    }
    void HandleIdleState()
    {
        // 逻辑处理...
    }

    // 更改状态方法，带优先级检查
    public void ChangeState(MyStates newState)
    {
        if (newState <= currentState)
        {
            currentState = newState;
            // 如有需要，可以添加其他逻辑，例如播放特定动画。这里可以用做释放技能或者道具后，攻击大于受伤
        }
    }

    // 强制更改状态
    public void ForceChangeState(MyStates newState)
    {
        currentState = newState;
        // 如有需要，可以添加其他逻辑。
    }
}
