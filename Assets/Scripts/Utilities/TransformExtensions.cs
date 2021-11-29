using UnityEngine;

public static class TransformExtensions
{
    public static Transform[] GetChildren(this Transform trans)
    {
        var children = new Transform[trans.childCount];
        for (var i = 0; i < trans.childCount; i++)
        {
            children[i] = trans.GetChild(i);
        }
        return children;
    }
    
    public static Vector3[] GetChildrenPosition(this Transform trans)
    {
        var children = new Vector3[trans.childCount];
        for (var i = 0; i < trans.childCount; i++)
        {
            children[i] = trans.GetChild(i).position;
        }
        return children;
    }
    
    public static float GetWaypointsDistance(this Transform trans)
    {
        var wayPoints = trans.GetChildrenPosition();
        var distance = 0f;
        for (var i = 1; i < wayPoints.Length; i++)
        {
            distance += Vector3.Distance(wayPoints[i], wayPoints[i - 1]);
        }
        return distance;
    }
}
