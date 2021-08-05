using System.Collections;
using UnityEngine;

public class Path : BasePath
{
    void OnDrawGizmos()
    {
        if (!bDebug)
        {
            return;
        }

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (i + 1 < waypoints.Count)
            {
                Debug.DrawLine(waypoints[i], waypoints[i + 1], Color.red);
            }
        }
    }
}
