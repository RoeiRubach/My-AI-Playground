using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson8_Astar
{
    public class Waypoint : MonoBehaviour
    {
        public int ID = -1;
        public Waypoint PreviousWaypoint { get; set; } = null;
        public bool IsConfiguredToPath { get; set; }
        public float CostToEndGoal { get; set; } = float.MaxValue;

        [HideInInspector] public List<Waypoint> Neighbors = new List<Waypoint>();

        public bool IsNeighbor(Waypoint neighborPossibility) => Neighbors.Contains(neighborPossibility);

        public void ResetWaypoint()
        {
            PreviousWaypoint = null;
            IsConfiguredToPath = false;
            CostToEndGoal = float.MaxValue;
        }
    }
}