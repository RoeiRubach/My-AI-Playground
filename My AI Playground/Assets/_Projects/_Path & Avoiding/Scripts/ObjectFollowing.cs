using UnityEngine;

public class ObjectFollowing : MonoBehaviour
{
    public BasePath path;
    public float speed = 5.0f;
    public bool isLooping = true;
    public bool reversed;

    private bool reachedFinalPoint = false;
    private float curSpeed;
    private int curPathIndex;
    private float pathLength;
    private Vector3 targetPoint;

    private void Start()
    {
        pathLength = path.Length;
        curPathIndex = 0;
    }

    private void Update()
    {
        curSpeed = speed * Time.deltaTime;
        targetPoint = path.GetPoint(curPathIndex);

        if (Vector3.Distance(transform.position, targetPoint) < path.radius)
        {
            if (reversed)
                GetReversedPoint();
            else
                GetNextPoint();
        }

        RotateTowardsPoint();

        if (!reachedFinalPoint)
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    private void GetReversedPoint()
    {
        if (curPathIndex > 0)
            curPathIndex--;

        else if (isLooping)
        {
            curPathIndex = (int)pathLength - 1;
            reachedFinalPoint = false;
        }

        else
            reachedFinalPoint = true;
    }

    private void GetNextPoint()
    {
        if (curPathIndex < pathLength - 1)
            curPathIndex++;

        else if (isLooping)
        {
            curPathIndex = 0;
            reachedFinalPoint = false;
        }

        else
            reachedFinalPoint = true;
    }

    private void RotateTowardsPoint()
    {
        Vector3 direction = targetPoint - transform.position;
        Quaternion tarRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, speed * Time.deltaTime);
    }
}
