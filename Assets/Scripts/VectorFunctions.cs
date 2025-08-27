using System;
using System.Collections.Generic;
using UnityEngine;

public class VectorFunctions
{
    public static Vector2 MakeVector(int degrees)
    {
        return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad));
    }

    public static float GetVectorAngle(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        return (angle < 0 ? angle + 360 : angle);
    }

    public static void DebugVector2List(List<Vector2> list)
    {
        string test = "";
        for (int i = Mathf.Max(list.Count - 25, 0); i < list.Count; i++)
        {
            test += list[i].ToString() + " ";
        }

        Debug.Log(test);
    }
}