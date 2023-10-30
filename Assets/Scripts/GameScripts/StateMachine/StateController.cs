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


