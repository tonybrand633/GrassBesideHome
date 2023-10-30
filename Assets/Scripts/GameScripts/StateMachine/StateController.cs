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


