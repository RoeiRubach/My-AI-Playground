using UnityEngine;

namespace Lesson5_RandomizeFSM
{
    public class Player : MonoBehaviour
    {
        private float _movementSpeed = 10f;
        private Movement _movement;
        private IUnityInputService _unityInputService;
        
        private void Start()
        {
            _movement = new Movement(_movementSpeed);
            if (_unityInputService == null)
                _unityInputService = new UnityInputService();
        }

        private void Update()
        {
            transform.position += _movement.Calculate(
                _unityInputService.GetAxisRaw("Horizontal"),
                _unityInputService.GetAxisRaw("Vertical"),
                _unityInputService.GetDeltaTime());
        }
    }
}