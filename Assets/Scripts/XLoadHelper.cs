using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XLoadHelper
{
    public static AnimationClip[] LoadAnimation(string path) 
    {
        string animPath;
        List<AnimationClip> clips = new List<AnimationClip>();
        animPath = "Animation/"+ path;
        Object []objs = Resources.LoadAll(animPath, typeof(AnimationClip));

        foreach (Object obj in objs) 
        {
            AnimationClip clip = obj as AnimationClip;
            if (clip != null) 
            {
                clips.Add(clip);
            }            
        }
        return clips.ToArray();
    }
}
