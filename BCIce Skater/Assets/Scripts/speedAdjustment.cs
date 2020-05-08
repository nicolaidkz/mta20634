using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAdjustment : MonoBehaviour
{
    public void OnGameDecision(GameDecisionData decisionData) 
    {
        // this is where we get an update on the latest result of a trial.
        Debug.Log("this is the decision from speedAdjustment: " + decisionData.decision);
    }
}
