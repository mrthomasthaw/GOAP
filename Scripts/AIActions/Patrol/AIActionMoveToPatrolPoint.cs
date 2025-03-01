using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    [CreateAssetMenu(fileName = "AIActionMoveToPatrolPoint", menuName = "ScriptableObjects /GOAP/AIAction/AIActionMoveToPatrolPoint", order = 4)]
    public class AIActionMoveToPatrolPoint : AIAction
    {
        public override void SetUp()
        {
            Preconditions.Add("hasPatrolPointToSecure", true);

            Effects.Add("reachedPatrolPoint", true);
        }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }
}
