using UnityEngine;

namespace Lesson5_RandomizeFSM
{
    public class Movement
    {
        private float _speed;

        public Movement(float speed)
        {
            _speed = speed;
        }

        public Vector3 Calculate(float horizontal, float vertical, float deltaTime)
        {
            var x = horizontal * _speed * deltaTime;
            var z = vertical * _speed * deltaTime;

            return new Vector3(x, 0, z);
        }
    }
}
