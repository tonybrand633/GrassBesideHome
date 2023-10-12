using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField]
    bool isDetectedFood;

    public Vector2 targetPosition;
    public GameObject targetGameObject;
    public bool isMealing = false;
    public float desRadius = 3.0f;

    //��ⷶΧ��ʳ�������
    public float detectRadius = 10.0f;
    public float moveToFoodSpeed = 2.0f;

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
        Collider2D detectedFood = Physics2D.OverlapCircle(transform.position, detectRadius, LayerMask.GetMask("CatFood"));

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
            targetPosition.x = targetGameObject.transform.position.x + desRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            targetPosition.y = targetGameObject.transform.position.y + desRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        }

        // ȷ�����򲢷�תSprite
        Vector2 direction = targetPosition - (Vector2)transform.position;
        FlipSprite(direction.x < 0);

        // �ƶ���Ŀ��λ��
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveToFoodSpeed * Time.deltaTime);

        // �ж��Ƿ񵽴�Ŀ��λ��
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= 0.1f)
        {
            Debug.Log("On Position");
            Vector2 dir = targetGameObject.transform.position - transform.position;
            FlipSprite(dir.x < 0);
            StartMealing();
        }
    }

    void FlipSprite(bool flip)
    {
        Vector3 scale = transform.localScale;
        scale.x = flip ? -1 : 1;
        transform.localScale = scale;
    }

    void StartMealing()
    {
        isMealing = true;
        // ���ܵĽ�ʳ�����߼�
    }

    public void StopMealingOrLoseTarget()
    {
        isMealing = false;
        isDetectedFood = false;
        targetGameObject = null;
        targetPosition = Vector3.zero;
        // ���ܵ�ֹͣ��ʳ�����߼�
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
