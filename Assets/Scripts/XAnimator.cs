using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAnimator : MonoBehaviour
{
    private string path;
    public Animator animator;
    public AnimatorOverrideController aoc;
    public AnimationClip[] animationClips;


    // Start is called before the first frame update
    void Start()
    {
        path = this.gameObject.name;
        Debug.Log(path);
        animator = GetComponent<Animator>();
        aoc = new AnimatorOverrideController(GetComponent<Animator>().runtimeAnimatorController);
        animationClips = XLoadHelper.LoadAnimation(path);
        

        for (int i = 0; i < aoc.overridesCount; i++)
        {
            aoc[aoc.animationClips[i].name] = animationClips[i];
        }
        animator.runtimeAnimatorController = aoc;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
