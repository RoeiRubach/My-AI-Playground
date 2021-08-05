using UnityEngine;

namespace Lesson6_Flock
{
    public static class BoidFieldOfSight
    {
        private const int NUM_VIEW_DIRECTIONS = 300;
        public static readonly Vector3[] Directions;

        static BoidFieldOfSight()
        {
            Directions = new Vector3[NUM_VIEW_DIRECTIONS];

            float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
            float angleIncrement = Mathf.PI * 2 * goldenRatio;

            for (int i = 0; i < NUM_VIEW_DIRECTIONS; i++)
            {
                float t = (float)i / NUM_VIEW_DIRECTIONS;
                float inclination = Mathf.Acos(1 - 2 * t);
                float azimuth = angleIncrement * i;

                float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
                float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
                float z = Mathf.Cos(inclination);
                Directions[i] = new Vector3(x, y, z);
            }
        }
    }
}