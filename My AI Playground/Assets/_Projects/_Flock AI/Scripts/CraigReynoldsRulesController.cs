using UnityEngine;

namespace Lesson6_Flock
{
    public class CraigReynoldsRulesController : MonoBehaviour
    {
        [Range(0, 5)]
        public float AlignmentWeight = 2.75f;
        [Range(0, 3)]
        public float CohesionWeight = 1.25f;
        [Range(0, 4)]
        public float SeperationWeight = 2.75f;
    }
}