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

    public float moveSpeed = 5.0f;  // �����ƶ��ٶ�
    public float sprintSpeed = 10.0f;  // �����ƶ��ٶ�

    [SerializeField]
    private float currentSpeed;  // ��ǰ�ٶ�

    public static event Action<GameObject> OnCatFoodGenerated;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // ��ʼʱ�趨��ǰ�ٶ�Ϊ�����ƶ��ٶ�
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
        // ��������B
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Vector3 pos = transform.position + transform.forward * 2f;
            GameObject objectB = Instantiate(catFood, pos, Quaternion.identity);

            // �����¼���֪ͨ��������������¼��Ľű�
            OnCatFoodGenerated?.Invoke(objectB);
        }        
    }

    void GetInput() 
    {
        // ��ȡ�����������
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        // �����ƶ�����
        movement = new Vector3(moveHorizontal, moveVertical, 0.0f).normalized;

        // �ж��Ƿ��¼��ټ� (���磬Shift��)
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
        // ���ʹ��Rigidbody2D�ƶ�������ʹ��velocity���Խ����ƶ�
        
        if (rb2d != null)
        {
            rb2d.velocity = movement * currentSpeed;
        }
        else
        {
            // ������ʹ��Transform���ƶ�
            transform.position += movement * currentSpeed * Time.deltaTime;
        }
    }
}
