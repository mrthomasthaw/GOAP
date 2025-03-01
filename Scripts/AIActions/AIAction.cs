using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    //[CreateAssetMenu(fileName = "AIAction", menuName = "ScriptableObjects /GOAP/AIAction", order = 3)]
    public abstract class AIAction : ScriptableTool
    {
        private Dictionary<string, object> preconditions = new Dictionary<string, object>();

        private Dictionary<string, object> effects = new Dictionary<string, object>();

        public Dictionary<string, object> Preconditions { get => preconditions; protected set => preconditions = value; }
        public Dictionary<string, object> Effects { get => effects; protected set => effects = value; }

        public abstract void SetUp();

        public abstract void Process();

        public bool CheckPreconditions(Dictionary<string, object> worldStates)
        {
            if (preconditions.Count == 0 || worldStates.Count == 0)
                return false;

            foreach (KeyValuePair<string, object> kv in preconditions)
            {
                if(worldStates.ContainsKey(kv.Key))
                {
                    if (! worldStates[kv.Key].Equals(kv.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

            return true;
        }

        public bool CheckEffects(Dictionary<string, object> worldStates)
        {
            if (effects.Count == 0 || worldStates.Count == 0)
                return false;

            foreach (KeyValuePair<string, object> kv in Effects)
            {
                if (worldStates.ContainsKey(kv.Key))
                {
                    if (! worldStates[kv.Key].Equals(kv.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

            return true;
        }


    }
}
