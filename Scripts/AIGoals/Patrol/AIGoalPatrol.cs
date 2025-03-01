using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    [CreateAssetMenu(fileName = "AIGoalPatrol", menuName = "ScriptableObjects /GOAP/AIGoal/AIGoalPatrol", order = 1)]
    public class AIGoalPatrol : AIGoal
    {

        public override void SetUp()
        {
            EndGoal.Add("secureArea", true);
        }

        public override void CalculatePriority()
        {
            throw new System.NotImplementedException();
        }
    }
}
