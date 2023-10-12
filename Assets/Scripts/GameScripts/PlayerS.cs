using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerS : MonoBehaviour
{
    public float speed = 5f;
    public GameObject catFood;

    public static event Action<GameObject> OnCatFoodGenerated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime;
        transform.Translate(movement);


        // 如果按下空格键，则生成物体B
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCatFood(transform.position + transform.forward * 2f);  // 在物体C的前方稍远的位置生成物体B,这里可以改成玩家面朝的方向
        }
    }

    void GenerateCatFood(Vector3 position)
    {
        // 生成物体B
        GameObject objectB = Instantiate(catFood, position, Quaternion.identity);
        // 触发事件，通知其他订阅了这个事件的脚本
        OnCatFoodGenerated?.Invoke(objectB);
    }
}
