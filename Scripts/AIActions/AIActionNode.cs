using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrThaw
{

    public class AIActionNode
    {
        public int Cost { get; set; }
        public AIActionNode ParentNode { get; set; }
        public AIAction Action { get; set; }

        public AIActionNode() { }

        public AIActionNode(int cost, AIActionNode parentNode, AIAction action)
        {
            this.Cost = cost;
            this.ParentNode = parentNode;
            this.Action = action;
        }
    }
}