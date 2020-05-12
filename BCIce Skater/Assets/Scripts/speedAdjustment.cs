using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAdjustment : MonoBehaviour
{
    public enum AdjustmentType { Constant, Staggered }
    public AdjustmentType Scenario;

    List <float> trialHistory = new List<float>();
    //public void OnGameDecision(GameDecisionData decisionData) 
    //{
    //    // this is where we get an update on the latest result of a trial.
    //    Debug.Log("this is the decision from speedAdjustment: " + decisionData.decision);
    //    float precision = 0f;
    //    if (decisionData.decision == InputTypes.AcceptAllInput) precision = 1f; // precision here needs to be based on the time since window start...
    //    AddTrialEntry(precision);
    //}


    public void InputAccepted() 
    {
        float precision = 1f;
        AddTrialEntry(precision);
    }
    public void InputRejected()
    {
        float precision = 0f;
        AddTrialEntry(precision);
    }
    void AddTrialEntry(float input)
    {
        trialHistory.Add(input);
        MakeSpeedAdjustment();
    }

    void MakeSpeedAdjustment()
    {
        switch (Scenario)
        {
            case AdjustmentType.Constant:
                float lastTrialPrecision = trialHistory[trialHistory.Count-1]; // last trial precision for comparison

                Debug.Log("Adjusting speed Constant based on: " + lastTrialPrecision);
                    break;

            case AdjustmentType.Staggered:
                float[] lastTrialPrecisions = new float[3]; // last three precisions for comparison
                if (trialHistory.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        lastTrialPrecisions[i] = trialHistory[trialHistory.Count - (1 + i)];
                    }
                    Debug.Log("Adjusting Speed Staggered based on: " + lastTrialPrecisions[0] + lastTrialPrecisions[1] + lastTrialPrecisions[2]);
                }
                else Debug.Log("staggering speed adjustment..");
                    break;
            default:
                Debug.Log("Error with adjustment type: " + Scenario.ToString());
                    break;
        }


    }
}


/* TODO:    FIND OUT WHY WE ARE NOT GETTING THE GREEN LIGHT AS EXPECTED, IT SEEMS THE INPUT IS RESET BEFORE WE RECEIVE IT HERE :(
            ALSO, MAKE THE GAMEMANAGER SERVE US THE TIME SINCE WINDOW STARTED WHEN THE PLAYER HAS COMPLETED A TRIAL, SO WE CAN USE THAT AS PRECISION
            FINALLY, CHANGE THE INTERTRIALINTERVALSECONDS IN GAMEMANAGER BASED ON THE ADJUSTMENT TYPE CHOSEN*/