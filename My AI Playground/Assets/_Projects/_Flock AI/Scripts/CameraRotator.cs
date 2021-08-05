using UnityEngine;

namespace Lesson6_Flock
{
    public class CameraRotator : MonoBehaviour
    {
        [Range(-100, 100)]
        [SerializeField] private float _rotationSpeed = 5;
        private Transform _cachedTransform;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        void Update()
        {
            _cachedTransform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        }
    }
}
