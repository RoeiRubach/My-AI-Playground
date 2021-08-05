using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Lesson7_TrafficSystem
{
    public class WaypointManagerWindow : EditorWindow
    {
        [MenuItem("Tools/Waypoint Editor")]
        public static void OpenWindow() => GetWindow<WaypointManagerWindow>();

        public Transform WaypointRoot;

        private void OnGUI()
        {
            SerializedObject newObject = new SerializedObject(this);

            EditorGUILayout.PropertyField(newObject.FindProperty("WaypointRoot"));

            if (WaypointRoot == null)
                EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform", MessageType.Warning);
            else
            {
                EditorGUILayout.BeginVertical("Box");
                DrawButtons();
                EditorGUILayout.EndVertical();
            }

            newObject.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Create Waypoint"))
                CreateWaypoint();

            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Add Branch Waypoint"))
                    CreateBranch();

                if (GUILayout.Button("Create Waypoint Before"))
                    CreateWaypointBefore();

                if (GUILayout.Button("Create Waypoint After"))
                    CreateWaypointAfter();

                if (GUILayout.Button("Remove Waypoint"))
                    RemoveWaypoint();
            }
        }

        private void CreateBranch()
        {
            Waypoint currentWaypoint = CreateNewWaypoint();
            Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();
            branchedFrom.Branches.Add(currentWaypoint);
            branchedFrom.transform.name = branchedFrom.transform.name + " (has branch)";
            currentWaypoint.transform.position = branchedFrom.transform.position;
            currentWaypoint.transform.forward = branchedFrom.transform.forward;

            Selection.activeGameObject = currentWaypoint.gameObject;
        }

        private void CreateWaypoint()
        {
            Waypoint currentWaypoint = CreateNewWaypoint();

            if (WaypointRoot.childCount > 1)
            {
                currentWaypoint.PreviousWaypoint = WaypointRoot.GetChild(WaypointRoot.childCount - 2).GetComponent<Waypoint>();
                currentWaypoint.PreviousWaypoint.NextWaypoint = currentWaypoint;
                currentWaypoint.transform.position = currentWaypoint.PreviousWaypoint.transform.position;
                currentWaypoint.transform.forward = currentWaypoint.PreviousWaypoint.transform.forward;

                Selection.activeGameObject = currentWaypoint.gameObject;
            }
        }

        private Waypoint CreateNewWaypoint()
        {
            GameObject waypointObject = new GameObject("Waypoint " + WaypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(WaypointRoot, false);
            return waypointObject.GetComponent<Waypoint>();
        }

        private void CreateWaypointBefore()
        {
            GameObject waypointObject = new GameObject("Waypoint " + WaypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(WaypointRoot, false);
            Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            waypointObject.transform.position = selectedWaypoint.transform.position;
            waypointObject.transform.forward = selectedWaypoint.PreviousWaypoint.transform.forward;

            if (selectedWaypoint.PreviousWaypoint != null)
            {
                newWaypoint.PreviousWaypoint = selectedWaypoint.PreviousWaypoint;
                selectedWaypoint.PreviousWaypoint.NextWaypoint = newWaypoint;
            }

            newWaypoint.NextWaypoint = selectedWaypoint;
            selectedWaypoint.PreviousWaypoint = newWaypoint;

            newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
            Selection.activeGameObject = newWaypoint.gameObject;
        }

        private void CreateWaypointAfter()
        {
            GameObject waypointObject = new GameObject("Waypoint " + WaypointRoot.childCount, typeof(Waypoint));
            waypointObject.transform.SetParent(WaypointRoot, false);
            Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            waypointObject.transform.position = selectedWaypoint.transform.position;
            waypointObject.transform.forward = selectedWaypoint.PreviousWaypoint.transform.forward;

            newWaypoint.PreviousWaypoint = selectedWaypoint;

            if (selectedWaypoint.NextWaypoint != null)
            {
                selectedWaypoint.NextWaypoint.PreviousWaypoint = newWaypoint;
                newWaypoint.NextWaypoint = selectedWaypoint.NextWaypoint;
            }

            selectedWaypoint.NextWaypoint = newWaypoint;
            newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex() + 1);
            Selection.activeGameObject = newWaypoint.gameObject;
        }

        private void RemoveWaypoint()
        {
            Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

            if (selectedWaypoint.NextWaypoint != null)
                selectedWaypoint.NextWaypoint.PreviousWaypoint = selectedWaypoint.PreviousWaypoint;

            if (selectedWaypoint.PreviousWaypoint != null)
            {
                selectedWaypoint.PreviousWaypoint.NextWaypoint = selectedWaypoint.NextWaypoint;
                Selection.activeGameObject = selectedWaypoint.PreviousWaypoint.gameObject;
            }

            DestroyImmediate(selectedWaypoint.gameObject);
        }
    }
}