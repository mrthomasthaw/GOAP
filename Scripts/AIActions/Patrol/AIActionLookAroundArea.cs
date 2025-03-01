using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    [CreateAssetMenu(fileName = "AIActionLookAroundArea", menuName = "ScriptableObjects /GOAP/AIAction/AIActionLookAroundArea", order = 5)]
    public class AIActionLookAroundArea : AIAction
    {
        public override void SetUp()
        {
            Preconditions.Add("reachedPatrolPoint", true);

            Effects.Add("secureArea", true);
        }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }
}
