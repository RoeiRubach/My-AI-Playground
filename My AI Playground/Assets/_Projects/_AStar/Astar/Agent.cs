using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson8_Astar
{
    [RequireComponent(typeof(Rigidbody))]
    public class Agent : MonoBehaviour
    {
        private const float CLOSE_DISTANCE = .5f;

        public int aStarID { get; private set; } = 0;
        [SerializeField] private Waypoint _firstWaypointGoalToGoTO;
        [SerializeField] private Waypoint _secondWaypointGoalToGoTo;


        private Waypoint _currentWaypoint;
        private Waypoint _goalWaypoint;
        private float _movementSpeed = 3;
        private bool _reachedGoal = false;
        private bool _hasPath = false;
        private AStar _aStarRef;
        private Rigidbody _rigidbodyRef;
        private Stack<Waypoint> _pathToGoal;

        private void Start()
        {
            _rigidbodyRef = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ProcessAgentPath();

            if (_pathToGoal == null || _currentWaypoint == null) return;

            CheckWaypoint();
            Move();
        }

        private void ProcessAgentPath()
        {
            if (_hasPath) return;

             _aStarRef = PathPlanningManager.Instance.aStarPathPlanners[aStarID];
            if (_aStarRef == null) return;
            
            _aStarRef.ProcessPath();
            _currentWaypoint = _aStarRef.GetClosestWaypoint(transform.position);
            _goalWaypoint = GetCurrentGoalWaypoint();

            _pathToGoal = _aStarRef.GetCalculatedPath(_currentWaypoint, _goalWaypoint);
            _hasPath = true;
        }

        private Waypoint GetCurrentGoalWaypoint()
        {

            if(_goalWaypoint == null)
                return _firstWaypointGoalToGoTO;
            else
            {
                _reachedGoal = false;
                return _secondWaypointGoalToGoTo;
            }
        }

        private void CheckWaypoint()
        {
            if (_reachedGoal) return;

            if (IsCloseToDesiredWaypoint)
            {
                if (_currentWaypoint == _goalWaypoint || _pathToGoal == null || _pathToGoal.Count == 0)
                {
                    _reachedGoal = true;
                    _hasPath = false;
                    aStarID++;
                }
                else
                    _currentWaypoint = _pathToGoal.Pop();
            }
        }

        private bool IsCloseToDesiredWaypoint => Vector3.Distance(transform.position, _currentWaypoint.transform.position) < CLOSE_DISTANCE;

        private void Move()
        {
            _rigidbodyRef.MovePosition(_rigidbodyRef.position + (_currentWaypoint.transform.position - _rigidbodyRef.position).normalized * _movementSpeed * Time.deltaTime);
        }

        // Draws next waypoint location.
        private void OnDrawGizmos()
        {
            if (_currentWaypoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_currentWaypoint.transform.position, 0.2f);
                Gizmos.color = Color.white;
            }
        }
    }
}