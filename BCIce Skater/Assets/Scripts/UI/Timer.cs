using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    bool timerStarted = false;
    float secondsCount;
    float minuteCount;
    Text timer;

    void Start()
    {
        timer = GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space") && !timerStarted)
        {
            timerStarted = true;
            StartCoroutine(StartTimer());  
        }
    }
    public IEnumerator StartTimer()
    {
        while (timerStarted)
        {
            secondsCount += Time.deltaTime;
            timer.text = minuteCount.ToString("00") + ":" + ((int)secondsCount).ToString("00");
            if (secondsCount >= 60)
            {
                minuteCount++;
                secondsCount = 0;
            }

            yield return null;
        }
    }
}
