using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class Utils
{
    public const float playerOffset = 0.05f;
    public static Vector2 GetTopPoint(Bounds bounds) 
    {
        Vector2 point;
        point = new Vector2(bounds.center.x, bounds.max.y+playerOffset);
        return point;
    }
    public static Vector2 GetLeftPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.min.x-playerOffset, bounds.center.y);
        return point;
    }
    public static Vector2 GetLeftTopPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.min.x-playerOffset, bounds.max.y+playerOffset);
        return point;
    }
    public static Vector2 GetLeftBottomPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.min.x - playerOffset, bounds.min.y-playerOffset);
        return point;
    }
    public static Vector2 GetRightTopPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.max.x+playerOffset, bounds.max.y + playerOffset);
        return point;
    }
    public static Vector2 GetRightBottomPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.max.x + playerOffset, bounds.min.y - playerOffset);
        return point;
    }
    public static Vector2 GetRightPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.max.x + playerOffset, bounds.center.y);
        return point;
    }
    public static Vector2 GetBottomPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.center.x, bounds.min.y - playerOffset);
        return point;
    }
}
