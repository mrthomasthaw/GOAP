using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{
    public class AIPlanner
    {
       public List<AIGoal> goalList = new List<AIGoal>();

        public List<AIAction> currentGoalActionList = new List<AIAction>();

        public List<AIAction> minCostActionList = new List<AIAction>();

        public Dictionary<string, object> currentWorldStates = new Dictionary<string, object>();

        public AIGoal currentGoal;

        public void SetUp(List<AIGoal> _goalList)
        {
            GenerateAgentCurrentWorldStates();

            goalList = _goalList;
            goalList.ForEach(g => g.SetUp());

            currentGoal = goalList[0];
            currentGoalActionList = currentGoal.actionList;

            currentGoalActionList.ForEach(a => a.SetUp());
        }

        private void GenerateAgentCurrentWorldStates()
        {
            currentWorldStates = new Dictionary<string, object>();
            currentWorldStates.Add("hasPatrolPointToSecure", false);
            currentWorldStates.Add("reachedPatrolPoint", false);
            currentWorldStates.Add("secureArea", false);
        }
             
        public void CalculateGoalPriority()
        {
            List<AIGoal> aIGoals = goalList;
            foreach(AIGoal goal in aIGoals)
            {
                goal.CalculatePriority();
            }
        }

        public Queue<AIAction> CalculateActionPlan()
        {
            List<AIAction> actionList = currentGoalActionList;
            List<AIActionNode> collectedActionNodeList = new List<AIActionNode>();

            float minCostPlanSoFar = Mathf.Infinity;


            var tGoals = goalList.OrderBy(x => x.Priority);

            AIGoal activeGoal = null;

            foreach (AIGoal goal in tGoals)
            {
                //if (!goal.IsApplicapable(ai) || AIWorldState.ConditionsMatch(goal.goalStates, currentWorldState) || !AIWorldState.ConditionsMatch(goal.sensorPreCons, currentWorldState))
                //{
                //    goal.Applicapable = false;
                //    continue;
                //}
                //goal.Applicapable = true;

                minCostActionList.Clear();
            

                // Creates paths including first lowest cost action path
                float maxCSoFar = Mathf.Infinity;

                //List<AIAction> applicapableNRelatedActions = new List<AIAction>();
                //foreach (var action in applicapableActions)
                //{
                //    if (goalsToRelatedActions[goal].Contains(action))
                //        applicapableNRelatedActions.Add(action);
                //}

                BuildActionTree(actionList, collectedActionNodeList, new AIActionNode(), currentWorldStates, currentGoal.EndGoal, ref minCostPlanSoFar);

                DebugActionNode(collectedActionNodeList);


                if (minCostActionList.Count > 0)
                {
                    Queue<AIAction> actionQ = new Queue<AIAction>();
                    foreach (AIAction action in minCostActionList)
                    {
                        Debug.Log(action.GetType().Name);
                        actionQ.Enqueue(action);
                    }
                    activeGoal = goal;

                    return actionQ;
                }
            }

            //if (collectedActionNodeList.Count > 0)
            //{
            //    AIActionNode lowCostActionNode = null;
            //    int minCost = int.MaxValue;
            //    for (int x = 0; x < collectedActionNodeList.Count; x++)
            //    {
            //        AIActionNode actionNode = collectedActionNodeList[x];
            //        if(actionNode.Cost < minCost)
            //        {
            //            minCost = actionNode.Cost;
            //            lowCostActionNode = actionNode;
            //        }
            //    }


            //    Queue<AIAction> actionQueue = new Queue<AIAction>();
            //    int count = 0;
            //    if (lowCostActionNode != null)
            //    {
            //        AIActionNode tempNode = lowCostActionNode;
            //        while (tempNode != null)
            //        {
            //            Debug.Log(++count + " low Cost Action : " + tempNode.Action.GetType());
            //            actionQueue.Enqueue(tempNode.Action);
            //            tempNode = tempNode.ParentNode;
            //        }
            //    }

            //    Debug.Log("Action Queue : " + string.Join(",", actionQueue));

            //    return actionQueue;

            //}



            return new Queue<AIAction>();
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

        //private void BuildActionTree(List<AIAction> actionList, List<AIActionNode> collectedActionNodeList, Dictionary<string, object> goal, AIActionNode rootNode)
        //{
        //    int i = 0;
        //    bool foundSomething = false;
        //    foreach (AIAction action in actionList)
        //    {
        //        if (action.CheckEffects(goal))
        //        {
        //            foundSomething = true;
        //            Debug.Log(++i + " Action found : " + action.GetType().Name);

        //            List<AIAction> updatedActionList = new List<AIAction>(actionList);
        //            updatedActionList.Remove(action);

        //            Dictionary<string, object> tempGoal = new Dictionary<string, object>(goal);                
        //            CopyWorldStates(action.Preconditions, tempGoal);

        //            int parentNodeCost = rootNode == null ? 0 : rootNode.Cost;
        //            AIActionNode selectedNode = new AIActionNode(action.cost + parentNodeCost, rootNode, action);

        //            BuildActionTree(updatedActionList, collectedActionNodeList, tempGoal, selectedNode);
        //        }
        //    }

        //    if(! foundSomething && rootNode != null) // Found something in above but not in this level
        //        collectedActionNodeList.Add(rootNode);
        //}

        public void BuildActionTree(List<AIAction> actionList, List<AIActionNode> collectedActionNodeList, AIActionNode rootNode, Dictionary<string, object> cWorldStates, Dictionary<string, object> currentGoalStates, ref float minCostPlanSoFar)
        {
            foreach(AIAction action in actionList)
            {
                if (rootNode.Cost + action.cost < minCostPlanSoFar && MatchWorldStates(action.Preconditions, cWorldStates))
                {

                    AIActionNode newNode = new AIActionNode(rootNode.Cost + action.cost, rootNode, action);
                    Dictionary<string, object> newWorldStates = new Dictionary<string, object>(cWorldStates);
                    CopyWorldStates(action.Effects, newWorldStates); // // Alters the copy of agent's internal state to action effects

                    if (MatchWorldStates(currentGoalStates, newWorldStates))
                    {
                        collectedActionNodeList.Add(newNode);
                        minCostPlanSoFar = newNode.Cost;
                        minCostActionList.Clear();
                       
                        AIActionNode tempNode = newNode;
                        while (tempNode.ParentNode != null)
                        {
                            minCostActionList.Insert(0, tempNode.Action);
                            tempNode = tempNode.ParentNode;
                        }
                    }
                    else
                    {
                        List<AIAction> newActionList = new List<AIAction>(actionList);
                        newActionList.Remove(action);
                        BuildActionTree(newActionList, collectedActionNodeList, newNode, newWorldStates, currentGoalStates, ref minCostPlanSoFar);
                    }

                }
            }
        }

  


        public bool MatchWorldStates(Dictionary<string, object> conditions, Dictionary<string, object> worldStateToCheck)
        {
            foreach (KeyValuePair<string, object> kv in conditions)
            {
                if (! worldStateToCheck.ContainsKey(kv.Key))
                {
                    return false;
                }

                if (! worldStateToCheck[kv.Key].Equals(kv.Value))
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
