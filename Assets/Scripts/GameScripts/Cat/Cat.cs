using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public CatCharacterEnum character;
    public CatStageEnum stage;

    [SerializeField]
    bool isDetectedFood;
    SpriteRenderer sr;


    public int foodCount;
    public Vector2 targetPosition;
    public GameObject targetGameObject;
    public bool isMealing = false;

    [Header("������ֵ")]
    public float speed;
    public float detect_Radius;
    public float size;

    [Header("״̬�����ֵ")]
    public float a_speed;
    public float a_detectRadius;
    public float a_size;

    void Awake() 
    {
        sr = GetComponent<SpriteRenderer>(); 
        CatCSVReader.LoadData();
        InitializeData();
    }

    void Update()
    {
        DetectFood();
        if (targetGameObject != null && !isMealing)
        {
            MoveToFood();
        }
    }

    void DetectFood()
    {
        Collider2D detectedFood = Physics2D.OverlapCircle(transform.position, detect_Radius, LayerMask.GetMask("CatFood"));

        if (detectedFood != null&&!isDetectedFood)
        {
            targetGameObject = detectedFood.gameObject;
            isDetectedFood = true;
            detectedFood.GetComponent<Cat_Food>().currentTargetCAT.Add(this);
        }        
    }

    void MoveToFood()
    {
        // �ڵ�һ���ƶ���ʳ��ʱ����Ŀ��λ��
        if (targetPosition == Vector2.zero)
        {
            // ��ȡһ������Ƕ�
            float angle = Random.Range(0, 360);

            // ʹ�����Ǻ�������Ŀ����λ��
            targetPosition.x = targetGameObject.transform.position.x + targetGameObject.GetComponent<Cat_Food>().detectionRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            targetPosition.y = targetGameObject.transform.position.y + targetGameObject.GetComponent<Cat_Food>().detectionRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        }

        // ȷ�����򲢷�תSprite
        Vector2 direction = targetPosition - (Vector2)transform.position;
        FlipSprite(direction.x < 0);

        // �ƶ���Ŀ��λ��
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // �ж��Ƿ񵽴�Ŀ��λ��
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= 0.1f)
        {
            //Debug.Log("On Position");
            Vector2 dir = targetGameObject.transform.position - transform.position;
            FlipSprite(dir.x < 0);
            StartMealing();
        }
    }

    void FlipSprite(bool flip)
    {
        if (flip)
        {
            sr.flipX = true;
        }
        else 
        {
            sr.flipX = false;
        }
        //Vector3 scale = transform.localScale;
        //scale.x = flip ? -scale.x : scale.x;
        //transform.localScale = scale;
    }

    void StartMealing()
    {
        isMealing = true;
        // ���ܵĽ�ʳ�����߼�
        foodCount++;
    }

    public void StopMealingOrLoseTarget()
    {
        isMealing = false;
        isDetectedFood = false;
        targetGameObject = null;
        targetPosition = Vector3.zero;
        // ���ܵ�ֹͣ��ʳ�����߼�
    }

    void InitializeData()
    {
        speed = CatCSVReader.GetBaseSpeed(character);
        detect_Radius = CatCSVReader.GetBaseDetectSize(character);
        size = CatCSVReader.GetBaseSize(character);
        Vector3 temp = transform.localScale;
        temp.x *= size;
        temp.y *= size;
        transform.localScale = temp;
    }

    void ReadState() 
    {
        
    }

    public void ApplyState() 
    {
    
    }    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detect_Radius);
    }
}
