using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

//这是一个抽象的状态类，太抽象了，以至于我不知道它是干嘛的
public abstract class State<T>
{
    //上下文物体
    protected T context;

    public State(T context)
    {
        this.context = context;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
