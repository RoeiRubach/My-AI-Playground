using UnityEngine;
using System.Collections.Generic;

public abstract class BasePath : MonoBehaviour
{
	public bool bDebug = true;
	public float radius = 2.0f;	

	[SerializeField]
	protected List<Vector3> waypoints = new List<Vector3>();

	public float Length { get { return waypoints.Count; } }

	public Vector3 GetPoint(int index)
	{
		return waypoints[index];
	}
}
