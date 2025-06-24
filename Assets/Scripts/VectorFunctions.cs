using System;
using System.Collections.Generic;
using UnityEngine;

public class VectorFunctions
{
    public static Vector2 MakeVector(int degrees)
    {
        return new Vector2((float)Math.Cos(degrees * Mathf.Deg2Rad), (float)Math.Sin(degrees * Mathf.Deg2Rad));
    }

    public static float GetVectorAngle(Vector2 vector)
    {
        float angle = (float)Math.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        return (angle < 0 ? angle + 360 : angle);
    }

    public static void DebugVector2List(List<Vector2> list)
    {
        string test = "";
        for (int i = Math.Max(list.Count - 25, 0); i < list.Count; i++)
        {
            test += list[i].ToString() + " ";
        }

        Debug.Log(test);
    }
}