using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson7_TrafficSystem
{
    public class Waypoint : MonoBehaviour
    {
        public Waypoint PreviousWaypoint;
        public Waypoint NextWaypoint;
        public List<Waypoint> Branches;

        [Range(0, 1)]
        public float BranchRatio = .25f;
        [Range(0, 5)]
        public float WidthLength = 1f;

        public Vector3 GetPosition()
        {
            Vector3 minBound = transform.position + transform.right * WidthLength / 2;
            Vector3 maxBound = transform.position - transform.right * WidthLength / 2;

            return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
        }
    }
}