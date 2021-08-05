using System.Collections.Generic;
using UnityEngine;

namespace Lesson8_Astar
{
    public class AStar : MonoBehaviour
    {
        [SerializeField] private Waypoint[] _waypointsArray;

        private float[,] _distances;
        private Queue<Waypoint> _queuedConfiguredWaypoints = new Queue<Waypoint>();
        private SortedList<float, Waypoint> _sortedWaypointsByCost = new SortedList<float, Waypoint>();

        #region PUBLIC_METHODS
        public void ProcessPath()
        {
            if (_waypointsArray == null) return;

            _distances = new float[_waypointsArray.Length, _waypointsArray.Length];
            ClearExistingNeighbors();
            StoreDistancesBetweenAllWaypoints();
        }

        public Waypoint GetClosestWaypoint(Vector3 position)
        {
            Waypoint closestWaypoint = null;
            float minDistance = float.MaxValue;

            if (_waypointsArray != null)
            {
                for (int waypoint1Counter = 0; waypoint1Counter < _waypointsArray.Length; waypoint1Counter++)
                {
                    float distance = Vector3.Distance(_waypointsArray[waypoint1Counter].transform.position, position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestWaypoint = _waypointsArray[waypoint1Counter];
                    }
                }
            }

            return closestWaypoint;
        }

        public Stack<Waypoint> GetCalculatedPath(Waypoint startingWaypoint, Waypoint goalWaypoint)
        {
            Stack<Waypoint> path = new Stack<Waypoint>();

            if (startingWaypoint.IsNeighbor(goalWaypoint))
            {
                path.Push(goalWaypoint);
                path.Push(startingWaypoint);
            }
            else
            {
                FirstWaypointInitialize(startingWaypoint);
                Waypoint currentWaypoint = null;

                CalculateAndSortBestPath(ref currentWaypoint, goalWaypoint);

                if (HasNoPath(currentWaypoint, goalWaypoint)) return null;

                StackBestPathFromEndToStart(goalWaypoint, ref path);

                ResetConfiguredWaypoints();
            }

            return path;
        }
        #endregion
        
        #region PRIVATE_METHODS

        private void ClearExistingNeighbors()
        {
            if (_waypointsArray.Length == 0) return;

            for (int waypoint1Counter = 0; waypoint1Counter < _waypointsArray.Length; waypoint1Counter++)
            {
                Waypoint waypoint1 = _waypointsArray[waypoint1Counter];
                waypoint1.Neighbors.Clear();
                waypoint1.ID = waypoint1Counter;
            }
        }

        private void StoreDistancesBetweenAllWaypoints()
        {
            for (int waypointsRowsLoop = 0; waypointsRowsLoop < _waypointsArray.Length; waypointsRowsLoop++)
            {
                Waypoint currentWaypointInRow = _waypointsArray[waypointsRowsLoop];

                for (int waypointsColumnsLoop = waypointsRowsLoop + 1; waypointsColumnsLoop < _waypointsArray.Length; waypointsColumnsLoop++)
                {
                    if (waypointsRowsLoop == waypointsColumnsLoop) continue;

                    Waypoint currentWaypointInColumn = _waypointsArray[waypointsColumnsLoop];
                    Vector3 direction = currentWaypointInColumn.transform.position - currentWaypointInRow.transform.position;
                    float distance = direction.magnitude;

                    if (HasNoObstacleBetweenWaypoints(currentWaypointInRow, direction, distance))
                        CreateNeighboringBetweenThem(currentWaypointInRow, currentWaypointInColumn);

                    _distances[waypointsRowsLoop, waypointsColumnsLoop] = distance;
                    _distances[waypointsColumnsLoop, waypointsRowsLoop] = distance;
                }
            }
        }

        private bool HasNoObstacleBetweenWaypoints(Waypoint startingPoint, Vector3 direction, float distance) =>
            !Physics.Raycast(startingPoint.transform.position, direction.normalized, distance);

        private void CreateNeighboringBetweenThem(Waypoint first, Waypoint second)
        {
            first.Neighbors.Add(second);
            second.Neighbors.Add(first);
        }

        private void FirstWaypointInitialize(Waypoint first)
        {
            _sortedWaypointsByCost.Clear();

            first.CostToEndGoal = 0;
            _sortedWaypointsByCost.Add(0, first);
            _queuedConfiguredWaypoints.Enqueue(first);
        }

        private void CalculateAndSortBestPath(ref Waypoint current, Waypoint goal)
        {
            do
            {
                current = _sortedWaypointsByCost[_sortedWaypointsByCost.Keys[0]];
                ConfigureWaypointAndRemove(current);

                if (current.Neighbors.Count == 0) continue;

                WhetherCostMovingThroughNeighborsBeneficial(current, goal);

            } while (_sortedWaypointsByCost.Count > 0 && (current != goal));
        }

        private void ConfigureWaypointAndRemove(Waypoint current)
        {
            current.IsConfiguredToPath = true;
            _sortedWaypointsByCost.RemoveAt(0);
        }

        private void WhetherCostMovingThroughNeighborsBeneficial(Waypoint current, Waypoint goal)
        {
            for (int neighborsLoop = 0; neighborsLoop < current.Neighbors.Count; neighborsLoop++)
            {
                Waypoint neighborWaypoint = current.Neighbors[neighborsLoop];

                if (neighborWaypoint.IsConfiguredToPath) continue;

                float combinedCost = GetCombinedCost_CurrentPlusCurrentAndNeighbor(current, neighborWaypoint);

                if (IsBeneficialToMoveFromCurrentToNeighbor(combinedCost, neighborWaypoint.CostToEndGoal))
                {
                    IndeedBeneficial(neighborWaypoint, combinedCost, current);
                    AddSortedWithCostToGoal(neighborWaypoint, goal);
                }
            }
        }

        private float GetCombinedCost_CurrentPlusCurrentAndNeighbor(Waypoint current, Waypoint neighbor) => current.CostToEndGoal + _distances[current.ID, neighbor.ID];

        private bool IsBeneficialToMoveFromCurrentToNeighbor(float combinedCost, float neighborCost) => combinedCost < neighborCost;

        private void IndeedBeneficial(Waypoint neighbor, float combinedCost, Waypoint current)
        {
            neighbor.CostToEndGoal = combinedCost;
            neighbor.PreviousWaypoint = current;
        }

        private void AddSortedWithCostToGoal(Waypoint neighbor, Waypoint goal)
        {
            _sortedWaypointsByCost.Add(neighbor.CostToEndGoal + _distances[neighbor.ID, goal.ID], neighbor);
            _queuedConfiguredWaypoints.Enqueue(neighbor);
        }

        private bool HasNoPath(Waypoint current, Waypoint end) => current != end;

        private void StackBestPathFromEndToStart(Waypoint endWaypoint, ref Stack<Waypoint> path)
        {
            Waypoint waypoint = endWaypoint;
            while (waypoint != null)
            {
                path.Push(waypoint);
                waypoint = waypoint.PreviousWaypoint;
            }
        }

        private void ResetConfiguredWaypoints()
        {
            while (_queuedConfiguredWaypoints.Count > 0)
            {
                _queuedConfiguredWaypoints.Dequeue().ResetWaypoint();
            }
        }
        #endregion

        #region UNITY_GIZMO
        private void OnDrawGizmos()
        {
            if (_waypointsArray == null) return;

            for (int waypointCounter = 0; waypointCounter < _waypointsArray.Length; waypointCounter++)
            {
                Waypoint waypoint = _waypointsArray[waypointCounter];
                DrawSpheresAroundWaypoints(waypoint);

                if (waypoint.Neighbors.Count > 0)
                    ConnectNeighborsWithLines(waypoint);
            }
        }

        private void DrawSpheresAroundWaypoints(Waypoint current) => Gizmos.DrawWireSphere(current.transform.position, 0.1f);

        private void ConnectNeighborsWithLines(Waypoint current)
        {
            for (int neighborId = 0; neighborId < current.Neighbors.Count; ++neighborId)
            {
                Waypoint neighbor = current.Neighbors[neighborId];
                Gizmos.DrawLine(current.transform.position, neighbor.transform.position);
            }
        }
        #endregion
    }
}