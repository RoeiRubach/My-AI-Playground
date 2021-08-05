using System.Collections;
using UnityEngine;

namespace Lesson7_TrafficSystem
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private int _numOfSpawns;

        private GameObject _prefabToSpawn;

        void Start() => StartCoroutine(Spawn());

        private IEnumerator Spawn()
        {
            int spawnCount = 0;

            while (spawnCount < _numOfSpawns)
            {
                _prefabToSpawn = _prefabs[Random.Range(0, _prefabs.Length)];
                GameObject newObject = Instantiate(_prefabToSpawn);
                Transform objectChild = transform.GetChild(Random.Range(0, transform.childCount - 1));
                newObject.GetComponent<WaypointNavigator>().CurrentWaypoint = objectChild.GetComponent<Waypoint>();
                newObject.transform.position = objectChild.position;

                yield return new WaitForEndOfFrame();

                spawnCount++;
            }
        }
    }
}