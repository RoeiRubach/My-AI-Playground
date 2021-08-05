using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson8_Astar
{
    public class PathPlanningManager : MonoBehaviour
    {
        #region SINGLETON
        private static PathPlanningManager _instance = null;

        public static PathPlanningManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PathPlanningManager>();

                    if (_instance == null)
                    {
                        GameObject _instance = new GameObject("PathPlanner", typeof(PathPlanningManager));
                    }
                }

                return _instance;
            }
        }
        #endregion

        public AStar[] aStarPathPlanners;
    }
}