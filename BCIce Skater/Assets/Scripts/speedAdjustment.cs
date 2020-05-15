using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAdjustment : MonoBehaviour
{
    public enum AdjustmentType { Constant, Staggered, None }
    public float decreaseT = 1.8f, increaseT = 0.5f;
    public float unit = 0.5f;
    public AdjustmentType Scenario;
    List<float> trialHistory = new List<float>();
    //public void OnGameDecision(GameDecisionData decisionData) 
    //{
    //    // this is where we get an update on the latest result of a trial.
    //    Debug.Log("this is the decision from speedAdjustment: " + decisionData.decision);
    //    float precision = 0f;
    //    if (decisionData.decision == InputTypes.AcceptAllInput) precision = 1f; // precision here needs to be based on the time since window start...
    //    AddTrialEntry(precision);
    //}


    public void InputAccepted(float timeSinceWindowStart)
    {
        float precision = timeSinceWindowStart;
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
                float lastTrialPrecision = trialHistory[trialHistory.Count - 1]; // last trial precision for comparison
                if (lastTrialPrecision != 0)
                {
                    if (lastTrialPrecision < increaseT)
                    {
                        Debug.Log("increase!");
                        float ItIs = GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds;
                        if (ItIs - unit >= 5)
                        {
                            GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds -= unit;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs - unit));
                        }
                    }
                    else if (lastTrialPrecision > decreaseT)
                    {
                        Debug.Log("within parameters, no changes made");
                    }

                    break;
                }
                else 
                {
                    Debug.Log("Window Expired");
                    Debug.Log("decrease!");
                    float ItIs = GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds;
                    if (ItIs + unit <= 10)
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds += unit;
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs + unit));
                    }
                    break;
                }
                

            case AdjustmentType.Staggered:
                float[] lastTrialPrecisions = new float[3]; // last three precisions for comparison
                if (trialHistory.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        lastTrialPrecisions[i] = trialHistory[trialHistory.Count - (1 + i)];
                    }
                    Debug.Log("Adjusting Speed Staggered based on: " + lastTrialPrecisions[0] + lastTrialPrecisions[1] + lastTrialPrecisions[2]);
                    // if the average of three trials are below a certain threshold, increase the speed by UNIT*3
                    if (AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) < increaseT) { }
                    // if the average of three  trials are above a certain threshold, lower the speed by UNIT*3
                    else if (AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) > decreaseT) { }
                    // else (if between thresholds) leave the poor speed alone
                    else { Debug.Log("average within parameters, no changes made"); }
                }
                else Debug.Log("staggering speed adjustment..");
                // we are still logging three trials before making a decision.
                break;
            case AdjustmentType.None:
                break;
            default:
                Debug.Log("Error with adjustment type: " + Scenario.ToString());
                // this part of the code is unreachable, if you see this error message, get to a phone!
                break;
        }


    }

    float AverageBetween(float one, float two, float three)
    {
        return one + two + three / 3; // BASS!
    }
}


/* TODO:     FINALLY, CHANGE THE INTERTRIALINTERVALSECONDS IN GAMEMANAGER BASED ON THE ADJUSTMENT TYPE CHOSEN*/
