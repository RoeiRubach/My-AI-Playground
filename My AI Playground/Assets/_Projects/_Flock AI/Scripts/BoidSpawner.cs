using UnityEngine;

namespace Lesson6_Flock
{
    public class BoidSpawner : MonoBehaviour
    {
        [SerializeField] private Boid _boidPrefab;
        [Range(1, 100)]
        [SerializeField] private int _numBoidsToSpawn = 50;
        private float _spawnRadius = 5f;

        private void Awake()
        {
            for (int i = 0; i < _numBoidsToSpawn; i++)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * _spawnRadius;
                Boid boid = Instantiate(_boidPrefab);
                boid.transform.position = spawnPosition;
                boid.transform.forward = Random.insideUnitSphere;
            }
        }
    }
}