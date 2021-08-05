using UnityEngine;

namespace Lesson7_TrafficSystem
{
    public class RandomMaterial : MonoBehaviour
    {
        private void Start() => GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}