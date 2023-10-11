using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSceneScripts : MonoBehaviour
{
    bool singleClickCheckUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&singleClickCheckUp)
        {
            singleClickCheckUp = false;
            Debug.Log("LeftMouseDown");
            GameManager.instance.restoreCount++;
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            singleClickCheckUp = true;            
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            Debug.Log("Count is:" + GameManager.instance.restoreCount);
        }
    }
}
