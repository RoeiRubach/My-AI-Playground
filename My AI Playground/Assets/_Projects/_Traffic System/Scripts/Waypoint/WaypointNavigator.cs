using UnityEngine;

namespace Lesson7_TrafficSystem
{
    public class WaypointNavigator : MonoBehaviour
    {
        public Waypoint CurrentWaypoint;
        [SerializeField] private float _movementSpeed = 5f;

        private Transform _cachedTransform;
        private Vector3 _waypointPosition;
        private Vector3 _velocity;
        private int _direction;

        private void Start()
        {
            _cachedTransform = transform;
            _direction = Mathf.RoundToInt(Random.Range(0f, 1f));
            _waypointPosition = CurrentWaypoint.GetPosition();
        }

        private void Update()
        {
            if (Vector3.Distance(_cachedTransform.position, _waypointPosition) < 1)
            {
                bool shouldBranch = false;

                if (CurrentWaypoint.Branches != null && CurrentWaypoint.Branches.Count > 0)
                    shouldBranch = Random.Range(0f, 1f) <= CurrentWaypoint.BranchRatio;

                if (shouldBranch)
                    CurrentWaypoint = CurrentWaypoint.Branches[Random.Range(0, CurrentWaypoint.Branches.Count)];
                else
                    KeepOnTrack();

                _waypointPosition = CurrentWaypoint.GetPosition();
            }

            _velocity += Steer(_waypointPosition);
            _cachedTransform.position += _velocity * Time.deltaTime;
            _cachedTransform.rotation = Quaternion.LookRotation(_velocity);
        }

        private Vector3 Steer(Vector3 target)
        {
            Vector3 desiredVelocity = (target - _cachedTransform.position);
            desiredVelocity.Normalize();
            desiredVelocity *= _movementSpeed;

            Vector3 steeringForce = desiredVelocity - _velocity;
            return steeringForce;
        }

        private void KeepOnTrack()
        {
            if (_direction == 0)
            {
                if (CurrentWaypoint.NextWaypoint != null)
                    CurrentWaypoint = CurrentWaypoint.NextWaypoint;
                else
                {
                    CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;
                    _direction = 1;
                }
            }
            else if (_direction == 1)
            {
                if (CurrentWaypoint.PreviousWaypoint != null)
                    CurrentWaypoint = CurrentWaypoint.PreviousWaypoint;
                else
                {
                    CurrentWaypoint = CurrentWaypoint.NextWaypoint;
                    _direction = 0;
                }
            }
        }
    }
}