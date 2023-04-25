using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSingletonHelper<T> where T : class, new()
{
    private static T instance;

    protected XSingletonHelper() { }

    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance; 
        
        }
    }
}
