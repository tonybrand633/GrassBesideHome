using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class Utils
{
    public static Vector2 GetTopPoint(Bounds bounds) 
    {
        Vector2 point;
        point = new Vector2(bounds.center.x, bounds.max.y);
        return point;
    }
    public static Vector2 GetLeftPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.min.x, bounds.center.y);
        return point;
    }
    public static Vector2 GetLeftTopPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.min.x, bounds.max.y);
        return point;
    }
    public static Vector2 GetLeftBottomPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.min.x, bounds.min.y);
        return point;
    }
    public static Vector2 GetRightTopPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.max.x, bounds.max.y);
        return point;
    }
    public static Vector2 GetRightBottomPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.max.x, bounds.min.y);
        return point;
    }
    public static Vector2 GetRightPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.max.x, bounds.center.y);
        return point;
    }
    public static Vector2 GetBottomPoint(Bounds bounds)
    {
        Vector2 point;
        point = new Vector2(bounds.center.x, bounds.min.y);
        return point;
    }
}
