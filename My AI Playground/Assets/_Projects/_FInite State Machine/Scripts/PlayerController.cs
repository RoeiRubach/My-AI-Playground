using UnityEngine;

public class PlayerController : MonoBehaviour
{
	protected float shootRate = 0.5f;
	protected float elapsedTime;

	[SerializeField]
	private GameObject _bullet;
	[SerializeField]
	private Transform _turret;
	[SerializeField]
	private Transform _bulletSpawn;

    private Rigidbody _rigidbody;
    private float _curSpeed, _targetSpeed, _rotSpeed;
	private float _turretRotSpeed = 10.0f;
	private float _maxSpeed;
	private float _acceleration;

    [HideInInspector] public bool IsEnemyDead;

	private void Start()
	{
		_rotSpeed = 150.0f;
		_maxSpeed = 5.0f;
		_acceleration = 0.7f;
        _rigidbody = GetComponent<Rigidbody>();
    }

	private void Update()
	{
        if (!IsEnemyDead)
        {
            UpdateWeapon();
            UpdateControl();
            Movement();

            _curSpeed = Mathf.Lerp(_curSpeed, _targetSpeed, _acceleration * Time.deltaTime);
            transform.Translate(Vector3.forward * Time.deltaTime * _curSpeed);
        }
	}

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
            _targetSpeed = _maxSpeed;

        else if (Input.GetKey(KeyCode.S))
            _targetSpeed = -_maxSpeed;

        else
            _targetSpeed = 0;

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -_rotSpeed * Time.deltaTime, 0.0f);

        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, _rotSpeed * Time.deltaTime, 0.0f);
    }

	private void UpdateControl()
	{
		Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0, 0, 0));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hitDist = 0;

		if (playerPlane.Raycast(ray, out hitDist))
		{
			Vector3 rayHitPoint = ray.GetPoint(hitDist);

			Quaternion targetRotation = Quaternion.LookRotation(rayHitPoint - transform.position);
			_turret.transform.rotation = Quaternion.Slerp(_turret.transform.rotation, targetRotation, Time.deltaTime * _turretRotSpeed);
		}
	}

	private void UpdateWeapon()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Instantiate(_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= shootRate)
			{
				elapsedTime = 0.0f;
				Instantiate(_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
			}
		}
	}
}
