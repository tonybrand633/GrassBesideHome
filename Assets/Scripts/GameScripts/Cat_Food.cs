using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Food : MonoBehaviour
{
    //CircleCollider2D circleCollider;

    bool isStartDestory;

    public bool hasSavedFirstCat;
    float offsetTime= 0.1f;
    GameObject firstArriveCat;

    public float existTimeAfterDetection = 5.0f; // CatFood �ڱ� Cat ��⵽����ڵ�ʱ��
                                                 
    public float detectionRadius = 5.0f; // Cat �ļ��뾶
    public List<Cat> currentTargetCAT;
    //private bool isCatDetected = false;

    private void Awake()
    {

    }

    private void Update()
    {
        if (firstArriveCat!=null&&!isStartDestory) 
        {
            Debug.Log("First Cat Arrive: " + firstArriveCat.name);
            CheckForFirstMealCat();
        }
        
        //DetectCat();
        //CheckForMealCat();
    }

    public void SaveFirstCat(GameObject sender) 
    {
        firstArriveCat = sender;
        hasSavedFirstCat = true;
    }

    void CheckForFirstMealCat()
    {
        isStartDestory = true;
        StartCoroutine(ExistCountdown(firstArriveCat.GetComponent<Cat>().mealFinishTime));        
        //Collider2D collider = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Cat"));

        //if (collider != null && collider.gameObject.GetComponent<Cat>().targetGameObject == this.gameObject) 
        //{
        //    Cat cat = collider.gameObject.GetComponent<Cat>();
        //    Debug.Log("First Cat Arrive: " + collider.gameObject.name);
        //    hasDetectFirstCat = true;
        //    StartCoroutine(ExistCountdown(cat.mealFinishTime));
        //    return;
        //}
    }

    //void CheckForMealCat() 
    //{
    //    // �����Χ�Ƿ��� Cat
    //    Collider2D[] detectedCats = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Cat"));

    //    // ������⵽������ Cat
    //    foreach (Collider2D catCollider in detectedCats)
    //    {
    //        //Debug.Log(catCollider.name);
    //        Cat catBehavior = catCollider.GetComponent<Cat>();
    //        // ��� Cat ���ڽ�ʳ
    //        if (catBehavior != null && catBehavior.isMealing && catBehavior.targetGameObject == this.gameObject)
    //        {
    //            StartCoroutine(ExistCountdown());
    //            return;  // ����ҵ�һ�����ڽ�ʳ�� Cat����ʼ����ʱ���˳�ѭ��
    //        }
    //    }
    //}

    //void DetectCat()
    //{
    //    Collider2D detectedCat = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Cat"));
    //    if (detectedCat != null && !isCatDetected)
    //    {
    //        isCatDetected = true; // ����������Э��
    //        StartCoroutine(ExistCountdown());
    //    }
    //}

    //IEnumerator ExistCountdown()
    //{
    //    yield return new WaitForSeconds(existTimeAfterDetection);
    //    NotifyCatAndDestroy();
    //}

    IEnumerator ExistCountdown(float existTime)
    {
        yield return new WaitForSeconds(existTime+offsetTime);
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

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Cat cat = collision.gameObject.GetComponent<Cat>();
    //    if (cat!=null&&cat.targetGameObject == this.gameObject) 
    //    {
    //        StartCoroutine(ExistCountdown(cat.mealFinishTime));
    //        Debug.Log("First Cat Arrive: "+cat.name);

    //    }
    //}
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);        
    }
}
