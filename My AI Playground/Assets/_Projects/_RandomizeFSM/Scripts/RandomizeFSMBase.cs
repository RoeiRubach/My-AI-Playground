using UnityEngine;

namespace Lesson5_RandomizeFSM
{
    public class RandomizeFSMBase : MonoBehaviour
    {
        [Range(0, 100)]
        [SerializeField] protected int rateOfSuccess;
        [SerializeField] protected Transform[] wayPoints;
        protected Transform playerTransform;
        protected int indexOfWayPoints;
        protected float moveSpeed = 9f;
        protected float timeOut = 0f;

        protected virtual void Initialize() { }
        protected virtual void RandomizeFSMUpdate() { }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            RandomizeFSMUpdate();
        }

        protected void FindNextPoint()
        {
            if (indexOfWayPoints == wayPoints.Length - 1)
            {
                indexOfWayPoints--;
            }
            else
            {
                indexOfWayPoints++;
            }
        }

        protected int GetRandomNumber()
        {
            return Random.Range(0, 101);
        }
    }
}
