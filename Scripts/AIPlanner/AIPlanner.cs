using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    public class AIPlanner
    {
        public List<AIGoal> goalList = new List<AIGoal>();

        public List<AIAction> currentGoalActionList = new List<AIAction>();

        public Dictionary<string, object> worldStates = new Dictionary<string, object>();

        public AIGoal currentGoal;

        public void SetUp(List<AIGoal> _goalList)
        {
            goalList = _goalList;
            goalList.ForEach(g => g.SetUp());

            currentGoal = goalList[0];
            currentGoalActionList = currentGoal.actionList;

            currentGoalActionList.ForEach(a => a.SetUp());
        }

        public void CalculateGoalPriority()
        {
            List<AIGoal> aIGoals = goalList;
            foreach(AIGoal goal in aIGoals)
            {
                goal.CalculatePriority();
            }
        }

        public void CalculateActionPlan()
        {
            List<AIAction> actionList = currentGoalActionList;
            List<AIActionNode> collectedActionNodeList = new List<AIActionNode>();
            BuildActionTree(actionList, collectedActionNodeList, currentGoal.EndGoal, null);

            DebugActionNode(collectedActionNodeList);
        }

        private void DebugActionNode(List<AIActionNode> collectedActionNodeList)
        {
            int i = 0;
            foreach (AIActionNode node in collectedActionNodeList)
            {
                List<string> actionNameList = new List<string>();
                AIActionNode nextNode = node;
                do
                {
                    if (nextNode != null && nextNode.Action != null)
                    {

                        actionNameList.Add(nextNode.Action.GetType().Name);
                        //Debug.Log(nextNode.Action.GetType().Name);
                    }


                    nextNode = nextNode.ParentNode;
                } while (nextNode != null);

                i++;
                Debug.Log(i + " " + string.Join("  , ", actionNameList));
            }
        }

        private void BuildActionTree(List<AIAction> actionList, List<AIActionNode> collectedActionNodeList, Dictionary<string, object> goal, AIActionNode rootNode)
        {
            int i = 0;
            bool foundSomething = false;
            foreach (AIAction action in actionList)
            {
                if (action.CheckEffects(goal))
                {
                    foundSomething = true;
                    Debug.Log(++i + " Action found : " + action.GetType().Name);

                    List<AIAction> updatedActionList = new List<AIAction>(actionList);
                    updatedActionList.Remove(action);

                    Dictionary<string, object> tempGoal = new Dictionary<string, object>(goal);                
                    CopyWorldStates(action.Preconditions, tempGoal);

                    BuildActionTree(updatedActionList, collectedActionNodeList, tempGoal, new AIActionNode(1, rootNode, action));
                }
            }

            if(! foundSomething)
                collectedActionNodeList.Add(rootNode);
        }

        public bool MatchWorldStates(Dictionary<string, object> conditions, Dictionary<string, object> worldStateToCheck)
        {
            foreach (KeyValuePair<string, object> kv in conditions)
            {
                if (worldStateToCheck.ContainsKey(kv.Key))
                {
                    if (!worldStateToCheck[kv.Key].Equals(kv.Value))
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

        public void CopyWorldStates(Dictionary<string, object> fromData, Dictionary<string, object> toData)
        {
            foreach(KeyValuePair<string, object> kv in fromData)
            {
                if(! toData.ContainsKey(kv.Key))
                {
                    toData.Add(kv.Key, kv.Value);
                }
                else
                {
                    toData[kv.Key] = kv.Value;
                }
            }
        }
    }
}
