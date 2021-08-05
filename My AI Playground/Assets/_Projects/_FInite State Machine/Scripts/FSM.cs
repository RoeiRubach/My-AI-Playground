using UnityEngine;

public class FSM : MonoBehaviour
{
	[SerializeField]
	protected Transform turret;
	[SerializeField]
	protected Transform bulletSpawn, positionToLookAt;

	protected Transform playerTransform;
	[SerializeField]
	protected GameObject[] wayPoints;
	[SerializeField]
	protected int indexOfWayPoints;
	protected float shootRate;
	protected float elapsedTime;

	protected virtual void Initialize() { }
	protected virtual void FSMUpdate() { }
	protected virtual void FSMFixedUpdate() { }

	private void Start()
	{
		Initialize();
	}

	private void Update()
	{
		FSMUpdate();
	}

	private void FixedUpdate()
	{
		FSMFixedUpdate();
	}
}
