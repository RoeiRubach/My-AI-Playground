//using System.Collections.Generic;
using UnityEngine;

namespace Lesson6_Flock
{
    public class FlockManager : MonoBehaviour
    {
        private Boid[] _boids;
        private float _awarenessToOtherBoidsRadius = 4f;

        private void Start()
        {
            _boids = FindObjectsOfType<Boid>();
        }

        private void Update()
        {
            if (_boids == null) return;

            foreach (var boid in _boids)
            {
                boid.NumOfOtherBoidsAround = GetNumOfBoidsNearIt(boid);
                boid.UpdateBoid();
            }
        }

        private int GetNumOfBoidsNearIt(Boid currentBoid)
        {
            int numOfBoidsAround = 0;

            foreach (var boid in _boids)
            {
                if (boid != currentBoid)
                {
                    float distance = Vector3.Distance(currentBoid.transform.position, boid.transform.position);

                    if (distance < _awarenessToOtherBoidsRadius)
                    {
                        currentBoid.AverageFlockHeadingDirection += boid.TransformVelocity;
                        currentBoid.LocalFlockmatesCenter += boid.transform.position;
                        currentBoid.AverageAvoidanceHeading += currentBoid.transform.position - boid.transform.position;
                        numOfBoidsAround++;
                    }
                }
            }
            return numOfBoidsAround;
        }
    }
}