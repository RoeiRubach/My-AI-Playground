using UnityEngine;

namespace Sensors
{
    public class Smell : Sense
    {
        [Range(10f, 50f)]
        [SerializeField] private int _smellRadius = 15;
        private Transform _playerTransform;

        protected override void Initialize()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected override void UpdateSense()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= detectionRate)
            {
                DetectAspect();
            }
        }

        private void DetectAspect()
        {
            float distance = Vector3.Distance(_playerTransform.position, transform.position);

            if (distance <= _smellRadius)
            {
                Debug.Log("You're near! I can smell you!!!!");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, _smellRadius); 
        }
    }
}
