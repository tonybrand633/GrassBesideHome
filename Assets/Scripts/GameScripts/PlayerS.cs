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


        // ������¿ո��������������B
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCatFood(transform.position + transform.forward * 2f);  // ������C��ǰ����Զ��λ����������B,������Ըĳ�����泯�ķ���
        }
    }

    void GenerateCatFood(Vector3 position)
    {
        // ��������B
        GameObject objectB = Instantiate(catFood, position, Quaternion.identity);
        // �����¼���֪ͨ��������������¼��Ľű�
        OnCatFoodGenerated?.Invoke(objectB);
    }
}
