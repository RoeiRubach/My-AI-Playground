using UnityEngine;

namespace Lesson6_Flock
{
    public class Boid : MonoBehaviour
    {
        private const float MIN_MOVEMENT_SPEED = 3f;
        private const float MAX_MOVEMENT_SPEED = 8f;
        private const float MAX_STEER_FORCE = 4f;

        public Vector3 TransformVelocity { get; private set; }
        public Vector3 AverageFlockHeadingDirection { get; set; }
        public Vector3 AverageAvoidanceHeading { get; set; }
        public Vector3 LocalFlockmatesCenter { get; set; }
        public int NumOfOtherBoidsAround { get; set; }

        private CraigReynoldsRulesController _craigReynoldsRulesController;
        [SerializeField] private LayerMask _obstaclesToAvoid;
        private Transform _cachedTransform;
        private Vector3 _acceleration;
        private float _sphereCastBoundsRadius = 0.3f;
        private float _sphereCastCollisionAvoidanceDistance = 5f;
        private float _collisionAvoidanceWeight = 30f;

        private void Start()
        {
            _craigReynoldsRulesController = FindObjectOfType<CraigReynoldsRulesController>();
            _cachedTransform = transform;
            float startingSpeed = (MIN_MOVEMENT_SPEED + MAX_MOVEMENT_SPEED) / 2;
            TransformVelocity = _cachedTransform.forward * startingSpeed;
        }

        public void UpdateBoid()
        {
            _acceleration = Vector3.zero;

            if (NumOfOtherBoidsAround > 0)
            {
                CraigReynoldsSeperation();
                CraigReynoldsAlignment();
                CraigReynoldsCohesion();
            }

            if (IsHeadingForCollision())
                ChangeDirectionToClearPath();

            UpdateTransformVelocity();
            UpdateTransformPosition();

            AverageFlockHeadingDirection = Vector3.zero;
        }

        private void CraigReynoldsAlignment()
        {
            AverageFlockHeadingDirection /= NumOfOtherBoidsAround;
            var alignmentForce = SteerTowards(AverageFlockHeadingDirection) * _craigReynoldsRulesController.AlignmentWeight;
            _acceleration += alignmentForce;
        }

        private void CraigReynoldsCohesion()
        {
            LocalFlockmatesCenter /= NumOfOtherBoidsAround;
            Vector3 offsetToCenterOfGroupedBoids = (LocalFlockmatesCenter - _cachedTransform.position);
            var cohesionForce = SteerTowards(offsetToCenterOfGroupedBoids) * _craigReynoldsRulesController.CohesionWeight;
            _acceleration += cohesionForce;
        }

        private void CraigReynoldsSeperation()
        {
            AverageAvoidanceHeading /= NumOfOtherBoidsAround;
            var seperationForce = SteerTowards(AverageAvoidanceHeading) * _craigReynoldsRulesController.SeperationWeight;
            _acceleration += seperationForce;
        }

        private bool IsHeadingForCollision()
        {
            if (Physics.SphereCast(_cachedTransform.position, _sphereCastBoundsRadius, _cachedTransform.forward, out RaycastHit hit, _sphereCastCollisionAvoidanceDistance, _obstaclesToAvoid))
                return true;

            return false;
        }

        private void ChangeDirectionToClearPath()
        {
            Vector3 clearPath = ObstacleRays();
            Vector3 collisionAvoidanceForce = SteerTowards(clearPath) * _collisionAvoidanceWeight;
            _acceleration += collisionAvoidanceForce;
        }

        private Vector3 ObstacleRays()
        {
            Vector3[] rayDirections = BoidFieldOfSight.Directions;

            for (int i = 0; i < rayDirections.Length; i++)
            {
                Vector3 directionForRay = _cachedTransform.TransformDirection(rayDirections[i]);
                Ray ray = new Ray(_cachedTransform.position, directionForRay);
                if (!Physics.SphereCast(ray, _sphereCastBoundsRadius, _sphereCastCollisionAvoidanceDistance, _obstaclesToAvoid))
                    return directionForRay;
            }

            return _cachedTransform.forward;
        }

        private Vector3 SteerTowards(Vector3 externalVector)
        {
            Vector3 internalVector = externalVector.normalized * MAX_MOVEMENT_SPEED - TransformVelocity;
            return Vector3.ClampMagnitude(internalVector, MAX_STEER_FORCE);
        }

        private void UpdateTransformVelocity()
        {
            TransformVelocity += _acceleration * Time.deltaTime;
            float speed = TransformVelocity.magnitude;
            Vector3 headingDirection = TransformVelocity / speed;
            speed = Mathf.Clamp(speed, MIN_MOVEMENT_SPEED, MAX_MOVEMENT_SPEED);
            TransformVelocity = headingDirection * speed;

            _cachedTransform.forward = headingDirection;
        }

        private void UpdateTransformPosition()
        {
            _cachedTransform.position += TransformVelocity * Time.deltaTime;
        }
    }
}