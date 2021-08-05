using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Lesson7_TrafficSystem
{
    [InitializeOnLoad]
    public class WaypointGizmoEditor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.yellow * .5f;

            Gizmos.DrawSphere(waypoint.transform.position, .1f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.WidthLength / 2),
                waypoint.transform.position - (waypoint.transform.right * waypoint.WidthLength / 2));

            if (waypoint.PreviousWaypoint != null)
                ConnectPreviousWayPointWithRedLine(waypoint);

            if (waypoint.NextWaypoint != null)
                ConnectNextWayPointWithGreenLine(waypoint);

            if (waypoint.Branches != null)
                ConnectBranchWayPointWithBlueLine(waypoint);
        }

        private static void ConnectPreviousWayPointWithRedLine(Waypoint waypoint)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.WidthLength / 2;
            Vector3 offsetTo = waypoint.PreviousWaypoint.transform.right * waypoint.WidthLength / 2;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.PreviousWaypoint.transform.position + offsetTo);
        }

        private static void ConnectNextWayPointWithGreenLine(Waypoint waypoint)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.WidthLength / 2;
            Vector3 offsetTo = waypoint.NextWaypoint.transform.right * -waypoint.WidthLength / 2;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.NextWaypoint.transform.position + offsetTo);
        }

        private static void ConnectBranchWayPointWithBlueLine(Waypoint waypoint)
        {
            foreach (Waypoint currentBranch in waypoint.Branches)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(waypoint.transform.position, currentBranch.transform.position);
            }
        }
    }
}