using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Food : MonoBehaviour
{
    public float existTimeAfterDetection = 5.0f; // CatFood 在被 Cat 检测到后存在的时间
    public float detectionRadius = 5.0f; // Cat 的检测半径
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
        // 检测周围是否有 Cat
        Collider2D[] detectedCats = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Cat"));

        // 遍历检测到的所有 Cat
        foreach (Collider2D catCollider in detectedCats)
        {
            //Debug.Log(catCollider.name);
            Cat catBehavior = catCollider.GetComponent<Cat>();
            // 如果 Cat 正在进食
            if (catBehavior != null && catBehavior.isMealing && catBehavior.targetGameObject == this.gameObject)
            {
                StartCoroutine(ExistCountdown());
                return;  // 如果找到一个正在进食的 Cat，开始倒计时并退出循环
            }
        }
    }

    //void DetectCat()
    //{
    //    Collider2D detectedCat = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Cat"));
    //    if (detectedCat != null && !isCatDetected)
    //    {
    //        isCatDetected = true; // 避免多次启动协程
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
