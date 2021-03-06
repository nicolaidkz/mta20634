﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedAdjustment : MonoBehaviour
{
    public enum AdjustmentType { Constant, Staggered, None }
    public float decreaseT = 1.8f, increaseT = 1.28f, IncreaseTs = 1.28f;
    public float unit = 0.5f, inputUnit = 0.3f;
    private float intervalMin = 2f, intervalMax= 10f, inputMin = 0.5f, inputMax = 3f;
    public AdjustmentType Scenario;
    public List<float> trialHistory = new List<float>();
    GameObject GM;
    public float lastPrecision = 0f;
    public float[] lastThreePrecision = new float[] { };

    void Start()
    {
        GM = GameObject.Find("GameManager");
    }

    void Update() 
    {
        increaseT = GM.GetComponent<GameManager>().inputWindowSeconds / 2;
        IncreaseTs = GM.GetComponent<GameManager>().inputWindowSeconds;
        //decreaseT = (GM.GetComponent<GameManager>().inputWindowSeconds / 3) * 2;
    }
    public void InputAccepted(float timeSinceWindowStart)
    {
        float precision = timeSinceWindowStart;
        AddTrialEntry(precision);
        lastPrecision = precision;
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
                    if (lastTrialPrecision < IncreaseTs)
                    {
                        Debug.Log("increase!");


                        float ItIs = GM.GetComponent<GameManager>().interTrialIntervalSeconds;
                        if (ItIs - unit/2 >= intervalMin)
                        {
                            //GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit;
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit/2);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs - unit/2));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(intervalMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InS - inputUnit/2 >= inputMin/2)
                        {
                            //GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit;
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit/2);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS - inputUnit/2));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(inputMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMin);
                        }
                    }
                                                                                                                                                                              
                    if (lastTrialPrecision < increaseT)
                    {
                        Debug.Log("increase!");


                        float ItIs = GM.GetComponent<GameManager>().interTrialIntervalSeconds;
                        if (ItIs - unit >= intervalMin)
                        {
                            //GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit;
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit);
                           GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs - unit));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(intervalMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InS - inputUnit >= inputMin)
                        {
                            //GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit;
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS - inputUnit));
                        }
                        else 
                        {
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(inputMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMin);
                        }
                    }
                    else if (lastTrialPrecision > increaseT)
                    {
                        Debug.Log("within parameters, no changes made");
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().ForceReDraw();
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
                        //GM.GetComponent<GameManager>().interTrialIntervalSeconds += unit;
                        GM.GetComponent<GameManager>().SetInterTrialSeconds(GM.GetComponent<GameManager>().interTrialIntervalSeconds += unit);
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs + unit));
                    }
                    else
                    {
                        GM.GetComponent<GameManager>().SetInterTrialSeconds(intervalMax);
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMax);
                    }
                    float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                    if (InS + inputUnit <= inputMax) 
                    {
                        //GM.GetComponent<GameManager>().inputWindowSeconds += inputUnit;
                        GM.GetComponent<GameManager>().SetInputWindowSeconds(GM.GetComponent<GameManager>().inputWindowSeconds += inputUnit);
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS + inputUnit));
                    }
                    else
                    {
                        GM.GetComponent<GameManager>().SetInputWindowSeconds(inputMax);
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
                    lastThreePrecision = lastTrialPrecisions;
                    Debug.Log("Adjusting Speed Staggered based on: " + AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]));
                    // if the average of three trials are below a certain threshold, increase the speed by UNIT*3
                    if (AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) < increaseT
                        && AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) != 0)
                    {
                        Debug.Log("increase!");


                        float InterTrialSeconds = GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds;
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InterTrialSeconds - unit * 2 >= intervalMin)
                        {

                            //GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds -= unit*3;
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit * 2);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (InterTrialSeconds - unit * 2));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(intervalMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        if (InS - inputUnit * 2 >= inputMin)
                        {
                            //GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit*3;
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit * 2);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS - inputUnit * 2));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(inputMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMin);
                        }
                        break;
                    }
                    // if the average of three  trials are above a certain threshold, leave the speed alone
                    else if (AverageBetween(lastTrialPrecisions[0], lastTrialPrecisions[1], lastTrialPrecisions[2]) > increaseT)
                    {
                        Debug.Log("within parameters, small changes made");

                        float InterTrialSeconds = GM.GetComponent<GameManager>().interTrialIntervalSeconds;
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (InterTrialSeconds - unit * 1 >= intervalMin)
                        {

                            //GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit * 1.5f;
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(GM.GetComponent<GameManager>().interTrialIntervalSeconds -= unit * 1);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (InterTrialSeconds - unit * 1));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(intervalMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMin);
                        }
                        if (InS - inputUnit * 1 >= inputMin)
                        {
                            //GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit * 1.5f;
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(GM.GetComponent<GameManager>().inputWindowSeconds -= inputUnit * 1);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS - inputUnit * 1));
                        }
                        else
                        {
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(inputMin);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMin);
                        }
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().ForceReDraw();
                        break;
                    }

                    else
                    {
                        Debug.Log("Window Expired");
                        Debug.Log("decrease!");
                        float ItIs = GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds;
                        float InS = GM.GetComponent<GameManager>().inputWindowSeconds;
                        if (ItIs + unit * 2 <= intervalMax)
                        {
                            //GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds += unit * 3;
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(GM.GetComponent<GameManager>().interTrialIntervalSeconds += unit * 2);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", (ItIs + unit * 2));
                        }
                        else
                        {
                            //GameObject.Find("GameManager").GetComponent<GameManager>().interTrialIntervalSeconds = 10;
                            GM.GetComponent<GameManager>().SetInterTrialSeconds(intervalMax);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateIntertrialWindow", intervalMax);
                        }
                        if (InS + inputUnit * 2 <= inputMax)
                        {
                            //GM.GetComponent<GameManager>().inputWindowSeconds += inputUnit * 3;
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(GM.GetComponent<GameManager>().inputWindowSeconds += inputUnit * 2);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", (InS + inputUnit * 2));
                        }
                        else
                        {
                            //GameObject.Find("GameManager").GetComponent<GameManager>().inputWindowSeconds = inputMax;
                            GM.GetComponent<GameManager>().SetInputWindowSeconds(inputMax);
                            GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().SendMessage("UpdateInputWindow", inputMax);
                        }
                        GameObject.Find("ProgressIndication").GetComponent<ProgressIndication>().ForceReDraw();
                        break;
                    }
                }
                else 
                {
                    Debug.Log("staggering speed adjustment..");
                    lastThreePrecision = new float[]{0f,0f,0f};
                }
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
