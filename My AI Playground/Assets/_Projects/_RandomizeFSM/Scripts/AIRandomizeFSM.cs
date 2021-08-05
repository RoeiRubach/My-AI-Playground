using UnityEngine;

namespace Lesson5_RandomizeFSM
{
    public class AIRandomizeFSM : RandomizeFSMBase
    {
        public enum RandomizeFSMState
        {
            None,
            Patrol,
            Chase
        }
        private RandomizeFSMState currentState;

        protected override void Initialize()
        {
            currentState = RandomizeFSMState.Patrol;
            FindNextPoint();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected override void RandomizeFSMUpdate()
        {
            switch (currentState)
            {
                case RandomizeFSMState.Patrol:
                    UpdatePatrolState();
                    break;
                case RandomizeFSMState.Chase:
                    UpdateChaseState();
                    break;
            }

            if (5 >= timeOut)
                timeOut -= Time.deltaTime;
        }

        protected void UpdatePatrolState()
        {
            if (Mathf.Abs(transform.position.x - wayPoints[indexOfWayPoints].position.x) < 1 &&
                Mathf.Abs(transform.position.z - wayPoints[indexOfWayPoints].position.z) < 1)
            {
                FindNextPoint();
            }
            else if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 2.0f &&
                Mathf.Abs(transform.position.z - playerTransform.position.z) <= 2.0f)
            {
                if (timeOut <= 0)
                {
                    int randomNum = GetRandomNumber();
                    print(randomNum);
                    if (randomNum < rateOfSuccess)
                        currentState = RandomizeFSMState.Chase;
                    else
                        timeOut = 5f;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, wayPoints[indexOfWayPoints].position, moveSpeed * Time.deltaTime);
        }

        protected void UpdateChaseState()
        {
            if (Mathf.Abs(transform.position.x - playerTransform.position.x) >= 6.0f &&
                Mathf.Abs(transform.position.z - playerTransform.position.z) >= 6.0f)
            {
                currentState = RandomizeFSMState.Patrol;
            }

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }
}
