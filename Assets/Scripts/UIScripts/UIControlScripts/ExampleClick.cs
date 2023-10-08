using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using System;

public class ExampleClick : PointerManipulator
{
    Vector3 m_Start;
    protected bool m_Active;
    int m_PointerID;
    Vector2 m_StartSize;

    public ExampleClick() 
    {
        m_PointerID = -1;
        activators.Add(new ManipulatorActivationFilter (){ button = MouseButton.LeftMouse });
        m_Active = false;
    }
    public ExampleClick(Action handle):this()
    {
        
    }

    //≥ÈœÛ¿‡
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointDown);
        target.RegisterCallback<PointerUpEvent>(OnPointUp);
        //target.RegisterCallback<PointerMoveEvent>(OnPointMove);
    }

    

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointDown);
        target.UnregisterCallback<PointerUpEvent>(OnPointUp);
        //target.UnregisterCallback<PointerMoveEvent>(OnPointMove);
    }

    protected void OnPointDown(PointerDownEvent evt)
    {
        if (m_Active) 
        {
            evt.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(evt))
        {
            m_Start = evt.localPosition;
            m_PointerID = evt.pointerId;

            m_Active = true;
            target.CapturePointer(m_PointerID);
            Debug.Log("Clicked");
            evt.StopPropagation();
        }
    }

    protected void OnPointUp(PointerUpEvent evt)
    {
        if (!m_Active||!target.HasPointerCapture(m_PointerID)||!CanStartManipulation(evt)) 
        {
            return;
        }

        m_Active = false;
        target.ReleaseMouse();
        evt.StopPropagation();
    }

    //protected void OnPointMove(PointerMoveEvent evt)
    //{
    //    throw new NotImplementedException();
    //}
}
