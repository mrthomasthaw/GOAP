using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    [CreateAssetMenu(fileName = "AIActionScanOrQueryPatrolPoint", menuName = "ScriptableObjects /GOAP/AIAction/AIActionScanOrQueryPatrolPoint", order = 3)]
    public class AIActionScanOrQueryPatrolPoint : AIAction
    {

        public override void SetUp()
        {
            Effects.Add("hasPatrolPointToSecure", true);
        }

        public override void Process()
        {
            throw new System.NotImplementedException();
        } 
    }
}
