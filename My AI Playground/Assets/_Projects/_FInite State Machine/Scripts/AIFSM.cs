using UnityEngine;

public class AIFSM : FSM
{
	public enum FSMState
	{
		None,
		Patrol,
		Hunt,
		Attack,
        Teleport,
        FourthDimension,
		Dead,
	}

	public FSMState curState;

	[SerializeField]
	private GameObject _bullet;

	private float _curSpeed;
	private float _curRotSpeed;
    private float _teleportTimer = 2f;
    private bool _dead, _isDialogStart, _torwardsOnce;
	private int _health, _flySpeed = 5;
    private Rigidbody _rigidbody;

    protected override void Initialize()
	{
		//Personal variables
		curState = FSMState.Patrol;
		_curSpeed = 15.0f;
		_curRotSpeed = 2.0f;
		_dead = false;
		_health = 100;
		//Variables inherited
		elapsedTime = 0.0f;
		shootRate = 3.0f;
		wayPoints = GameObject.FindGameObjectsWithTag("Way Point");
		FindNextPoint();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_rigidbody = GetComponent<Rigidbody>();
	}

	protected override void FSMUpdate()
	{
		switch (curState)
		{
			case FSMState.Patrol:
				UpdatePatrolState();
				break;
			case FSMState.Hunt:
				UpdateHuntState();
				break;
			case FSMState.Attack:
				UpdateAttackState();
				break;
            case FSMState.Teleport:
                UpdateTeleportState();
                break;
            case FSMState.FourthDimension:
                UpdateFourthDimensionState();
                break;
            case FSMState.Dead:
				UpdateDeadState();
				break;
			default:
				break;
		}

		elapsedTime += Time.deltaTime;

		if (_health <= 0)
		{
            curState = FSMState.FourthDimension;
            //curState = FSMState.Dead;
        }
    }

	protected void UpdatePatrolState()
	{
		if (Mathf.Abs(transform.position.x - wayPoints[indexOfWayPoints].transform.position.x) < 1 && Mathf.Abs(transform.position.z - wayPoints[indexOfWayPoints].transform.position.z) < 1)
		{
			FindNextPoint();
		}
		else if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 5.0f && Mathf.Abs(transform.position.z - playerTransform.position.z) <= 5.0f)
		{
			curState = FSMState.Hunt;
		}

		Quaternion targetRot = Quaternion.LookRotation(wayPoints[indexOfWayPoints].transform.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, wayPoints[indexOfWayPoints].transform.position, _curSpeed * Time.deltaTime);
	}

	protected void FindNextPoint()
	{
		int randomIndex = GetRandomIndex();

		if (randomIndex == indexOfWayPoints)
		{
			if (indexOfWayPoints == wayPoints.Length - 1)
			{
				indexOfWayPoints--;
			}
			else
			{
				indexOfWayPoints++;
			}
		}
		else
		{
			indexOfWayPoints = randomIndex;
		}
	}

	private int GetRandomIndex()
	{
		return Random.Range(0, wayPoints.Length - 1);
	}

	protected void UpdateHuntState()
	{
		if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 3.0f && Mathf.Abs(transform.position.z - playerTransform.position.z) <= 3.0f)
		{
			curState = FSMState.Attack;
		}
		else if (Mathf.Abs(transform.position.x - playerTransform.position.x) >= 5.0f || Mathf.Abs(transform.position.z - playerTransform.position.z) >= 5.0f)
		{
			curState = FSMState.Patrol;
		}
		Quaternion targetRot = Quaternion.LookRotation(playerTransform.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, _curSpeed * Time.deltaTime);
	}

	protected void UpdateAttackState()
	{
		if ((Mathf.Abs(transform.position.x - playerTransform.position.x) >= 3.0f && Mathf.Abs(transform.position.z - playerTransform.position.z) >= 3.0f) && (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 5.0f && Mathf.Abs(transform.position.z - playerTransform.position.z) <= 5.0f))
		{
			Quaternion targetRot = Quaternion.LookRotation(playerTransform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, _curSpeed * Time.deltaTime);
			curState = FSMState.Attack;
		}
		else if (Mathf.Abs(transform.position.x - playerTransform.position.x) >= 5.0f || Mathf.Abs(transform.position.z - playerTransform.position.z) >= 5.0f)
		{
			curState = FSMState.Patrol;
		}
		Quaternion turretRotation = Quaternion.LookRotation(playerTransform.position - turret.position);
		turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, _curRotSpeed * Time.deltaTime);
		ShootBullet();
    }

    protected void UpdateTeleportState()
    {
        _teleportTimer += Time.deltaTime;

        if (_teleportTimer >= 2)
        {
            _teleportTimer = 0;
            FindNextPoint();
            transform.rotation = Quaternion.identity;
            transform.position = wayPoints[indexOfWayPoints].transform.position;
            transform.LookAt(playerTransform.transform);
        }
    }

    protected void UpdateFourthDimensionState()
    {
        if (!_isDialogStart)
        {
            _isDialogStart = true;
            transform.position = playerTransform.position - Vector3.forward * 2;
            transform.LookAt(playerTransform.transform);

            playerTransform.GetComponent<PlayerController>().IsEnemyDead = true;
            UIManager.Instance.ActivatePanel();
        }
        else if (UIManager.Instance.IsStartingToBelieve)
        {
            playerTransform.LookAt(positionToLookAt);
            transform.LookAt(positionToLookAt);

            if (!_torwardsOnce)
            {
                Invoke("FadeBlackScreen", 11f);
                _torwardsOnce = true;
                playerTransform.GetChild(0).localRotation = Quaternion.identity;
                transform.GetChild(0).localRotation = Quaternion.identity;
                playerTransform.GetComponent<Rigidbody>().useGravity = false;
                _rigidbody.useGravity = false;
            }

            float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            if (distance >= 3)
            {
                transform.position += transform.forward * (Time.deltaTime * _flySpeed);
                playerTransform.position += transform.forward * (Time.deltaTime * _flySpeed);
            }
            else
                UIManager.Instance.ActivatePanel();
        }
    }

    private void FadeBlackScreen()
    {
        SceneController.LoadScene(_faderDuration: 4);
    }

    private void ShootBullet()
	{
		if (elapsedTime >= shootRate)
		{
			Instantiate(_bullet, bulletSpawn.position, bulletSpawn.rotation);
			elapsedTime = 0.0f;
		}
    }

    protected void UpdateDeadState()
	{
		if (!_dead)
		{
			_dead = true;
			Destroy(gameObject, 1.5f);
		}
	}

    private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Bullet"))
		{
			_health -= collision.gameObject.GetComponent<Bullet>().damage;
            
            if (_health == 25)
                curState = FSMState.Teleport;
        }
	}
}
