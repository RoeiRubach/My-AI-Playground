using System.Collections;
using UnityEngine;

public class ObstaclesAvoiding : MonoBehaviour
{
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float speed = 20.0f;
    [SerializeField]
    private float rotSpeed = 5.0f;
    [SerializeField]
    private float mass = 5.0f;
    [SerializeField]
    private float force = 10.0f;
    [SerializeField]
    private float minimumDistToAvoid = 10.0f;

    private float curSpeed;
    private Vector3 targetPoint;

    private void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100.0f))
        {
            targetPoint = hit.point;
        }

        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();
        AvoidObstacles(ref dir);

        if (Vector3.Distance(targetPoint, transform.position) < 3.0f)
        {
            return;
        }

        curSpeed = speed * Time.deltaTime;
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
        transform.position += transform.forward * curSpeed;
    }

    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, mask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            dir = transform.forward + hitNormal * force;
        }
    }
}
