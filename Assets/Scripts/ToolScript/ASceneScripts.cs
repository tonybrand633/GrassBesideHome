using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASceneScripts : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("SceneA:LeftMouseDown");
            GameManager.instance.restoreCount++;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Count is:" + GameManager.instance.restoreCount);
        }
    }
}
