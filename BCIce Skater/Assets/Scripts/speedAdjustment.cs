using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAdjustment : MonoBehaviour
{
    public enum AdjustmentType { Constant, Staggered, None }
    public float decreaseT = 1.8f, increaseT = 1.28f;
    public float unit = 0.5f, inputUnit = 0.3f;
    private float intervalMin = 2f, intervalMax= 10f, inputMin = 0.5f, inputMax = 3f;
    public AdjustmentType Scenario;
    List<float> trialHistory = new List<float>();
    GameObject GM;

    void Start()
    {
        GM = GameObject.Find("GameManager");
    }

    void Update() 
    {
        increaseT = GameObject.Find("GameManager").GetComponent<GameManager>().inputWindowSeconds / 2;
    }
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


                        float ItIs = GM.GetComponent<GameManager>().interTrialIntervalSeconds;
                        if (ItIs - unit >= intervalMin)
                        {
                           GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit;
                           GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs - unit));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().interTrialIntervalSeconds = intervalMin;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InS - inputUnit >= inputMin)
                        {
                            GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS - inputUnit));
                        }
                        else 
                        {
                            GM.GetComponent<GameManager>().inputWindowSeconds = inputMin;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMin);
                        }
                    }
                    else if (lastTrialPrecision > increaseT)
                    {
                        Debug.Log("within parameters, no changes made");
                    }

                    break;
                }
                else 
                {
                    Debug.Log("Window Expired");
                    Debug.Log("decrease!");
                    float ItIs = GM.GetComponent<GameManager>().interTrialIntervalSeconds;
                    if (ItIs + unit <= intervalMax)
                    {
                        GM.GetComponent<GameManager>().interTrialIntervalSeconds += unit;
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs + unit));
                    }
                    else
                    {
                        GM.GetComponent<GameManager>().interTrialIntervalSeconds = intervalMax;
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMax);
                    }
                    float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                    if (InS + inputUnit <= inputMax) 
                    {
                        GM.GetComponent<GameManager>().inputWindowSeconds += inputUnit;
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS + inputUnit));
                    }
                    else
                    {
                        GM.GetComponent<GameManager>().inputWindowSeconds = inputMax;
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMax);
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
                    Debug.Log("Adjusting Speed Staggered based on: " + AverageBetween(lastTrialPrecisions[0],lastTrialPrecisions[1],lastTrialPrecisions[2]));
                    // if the average of three trials are below a certain threshold, increase the speed by UNIT*3
                    if (AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) < increaseT) 
                    {
                        Debug.Log("increase!");


                        float InterTrialSeconds = GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds;
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InterTrialSeconds - unit*3 >= intervalMin)
                        {

                            GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds -= unit*3;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (InterTrialSeconds - unit*3));
                        }
                        else 
                        {
                            GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds = intervalMin;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        if (InS - inputUnit * 3 >= inputMin) 
                        {
                        
                        }
                        break;
                    }
                    // if the average of three  trials are above a certain threshold, leave the speed alone
                    else if (AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) > increaseT) 
                    {
                        Debug.Log("within parameters, small changes made");

                        float InterTrialSeconds = GM.GetComponent<GameManager>().interTrialIntervalSeconds;
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InterTrialSeconds - unit * 1.5 >= intervalMin)
                        {

                            GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit * 1.5f;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (InterTrialSeconds - unit * 1.5));
                        }
                        else
                        {
                            GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds = intervalMin;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        break;
                    }
                  
                    else 
                    {
                        Debug.Log("Window Expired");
                        Debug.Log("decrease!");
                        float ItIs = GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds;
                        if (ItIs + unit * 3 <= intervalMax)
                        {
                            GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds += unit * 3;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs + unit * 3));
                        }
                        else 
                        {
                            GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds = 10;
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", 10);
                        }
                        break;
                    }
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
