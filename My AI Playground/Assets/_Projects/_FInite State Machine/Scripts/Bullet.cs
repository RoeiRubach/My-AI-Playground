using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage = 25;

	[SerializeField]
	private float _speed = 50.0f;
	[SerializeField]
	private float _lifeTime = 3.0f;

	private void Start()
	{
		Destroy(gameObject, _lifeTime);
	}

	private void Update()
	{
		transform.Translate(Vector3.up * Time.deltaTime * _speed);
	}

	private void OnCollisionEnter(Collision collision)
	{
		Destroy(gameObject);
	}
}
