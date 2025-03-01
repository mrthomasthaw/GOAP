using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MrThaw {
    public class GoapPlannerTest : MonoBehaviour
    {
        [SerializeField] private List<AIGoal> goalList;

        private AIPlanner aiPlanner;
        // Start is called before the first frame update
        void Start()
        {
            aiPlanner = new AIPlanner();
            aiPlanner.SetUp(goalList);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Key press F");
                aiPlanner.CalculateActionPlan();
            }

        }
    }
}
