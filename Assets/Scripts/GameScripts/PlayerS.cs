using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerS : MonoBehaviour
{
    float moveHorizontal;
    float moveVertical;
    [SerializeField]
    Vector3 movement;
    Rigidbody2D rb2d;

    public float speed = 5f;
    public GameObject catFood;

    public float moveSpeed = 5.0f;  // 正常移动速度
    public float sprintSpeed = 10.0f;  // 加速移动速度

    [SerializeField]
    private float currentSpeed;  // 当前速度

    public static event Action<GameObject> OnCatFoodGenerated;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 开始时设定当前速度为正常移动速度
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); 
        GenerateCatFood();        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void GenerateCatFood()
    {
        // 生成物体B
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Vector3 pos = transform.position + transform.forward * 2f;
            GameObject objectB = Instantiate(catFood, pos, Quaternion.identity);

            // 触发事件，通知其他订阅了这个事件的脚本
            OnCatFoodGenerated?.Invoke(objectB);
        }        
    }

    void GetInput() 
    {
        // 获取方向键的输入
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        // 创建移动向量
        movement = new Vector3(moveHorizontal, moveVertical, 0.0f).normalized;

        // 判断是否按下加速键 (例如，Shift键)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }

    void MovePlayer()
    {
        // 如果使用Rigidbody2D移动，优先使用velocity属性进行移动
        
        if (rb2d != null)
        {
            rb2d.velocity = movement * currentSpeed;
        }
        else
        {
            // 否则还是使用Transform来移动
            transform.position += movement * currentSpeed * Time.deltaTime;
        }
    }
}
