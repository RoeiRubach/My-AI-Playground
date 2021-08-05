using UnityEngine;

public class ObstaclesMovement : MonoBehaviour
{
    [SerializeField]
    private float _farthestToMove = 3f;

    [SerializeField]
    private float _movementSpeed = 1f;
    private Vector3 startPosition;
    private Vector3 directionTo;

    bool directionTrigger = false;

    private void Start()
    {
        startPosition = transform.localPosition;
        directionTo = LeftDirection;
    }

    private void Update()
    {
        transform.Translate(directionTo);
        if (DistanceFromStartingPosition > _farthestToMove)
        {
            if (directionTrigger)
            {
                directionTo = RightDirection;
                directionTrigger = false;
            }
            else
            {
                directionTo = LeftDirection;
                directionTrigger = true;
            }
        }
    }

    private float DistanceFromStartingPosition => Vector3.Distance(startPosition, transform.localPosition);

    private Vector3 RightDirection => Vector3.right * (_movementSpeed * Time.deltaTime);

    private Vector3 LeftDirection => Vector3.left * (_movementSpeed * Time.deltaTime);
}
