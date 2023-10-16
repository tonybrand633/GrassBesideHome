using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Food : MonoBehaviour
{
    public float existTimeAfterDetection = 5.0f; // CatFood �ڱ� Cat ��⵽����ڵ�ʱ��
    public float detectionRadius = 5.0f; // Cat �ļ��뾶
    public List<Cat> currentTargetCAT;
    //private bool isCatDetected = false;

    private void Awake()
    {
        //detectionRadius = GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        //DetectCat();
        CheckForMealCat();
    }

    void CheckForMealCat() 
    {
        // �����Χ�Ƿ��� Cat
        Collider2D[] detectedCats = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Cat"));

        // ������⵽������ Cat
        foreach (Collider2D catCollider in detectedCats)
        {
            //Debug.Log(catCollider.name);
            Cat catBehavior = catCollider.GetComponent<Cat>();
            // ��� Cat ���ڽ�ʳ
            if (catBehavior != null && catBehavior.isMealing && catBehavior.targetGameObject == this.gameObject)
            {
                StartCoroutine(ExistCountdown());
                return;  // ����ҵ�һ�����ڽ�ʳ�� Cat����ʼ����ʱ���˳�ѭ��
            }
        }
    }

    //void DetectCat()
    //{
    //    Collider2D detectedCat = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Cat"));
    //    if (detectedCat != null && !isCatDetected)
    //    {
    //        isCatDetected = true; // ����������Э��
    //        StartCoroutine(ExistCountdown());
    //    }
    //}

    IEnumerator ExistCountdown()
    {
        yield return new WaitForSeconds(existTimeAfterDetection);
        NotifyCatAndDestroy();
    }

    void NotifyCatAndDestroy()
    {
        Collider2D []detectedCat = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Cat"));
        foreach (Collider2D c in detectedCat) 
        {
            if (c != null)
            {
                c.GetComponent<Cat>().StopMealingOrLoseTarget();
            }
        }
        foreach (Cat cat in currentTargetCAT) 
        {
            if (cat!=null) 
            {
                cat.GetComponent<Cat>().StopMealingOrLoseTarget();
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);        
    }
}
