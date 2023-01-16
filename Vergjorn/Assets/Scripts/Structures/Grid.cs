using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    
    public static float gridSize = 1;


    public Vector3 GetNearestAllowedPoint(Vector3 point)
    {
        point -= transform.position;

        int xCount = Mathf.RoundToInt(point.x / gridSize);
        int yCount = Mathf.RoundToInt(point.y / gridSize);
        int zCount = Mathf.RoundToInt(point.z / gridSize);

        Vector3 result = new Vector3((float)xCount * gridSize, (float)yCount * gridSize, (float)zCount * gridSize);

        result += transform.position;

        return result;
    }
}
