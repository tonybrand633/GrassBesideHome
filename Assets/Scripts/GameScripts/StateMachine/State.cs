using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public abstract class State<T>
{
    //����������
    protected T context;

    public State(T context)
    {
        this.context = context;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
