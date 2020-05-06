using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skaterMovement : MonoBehaviour
{
    private GameObject targetPos;
    GameObject incorrectPosMarker;
    public float speed = 10f;
    private int pointNr = 1;
    private bool correctInput = false; // To keep track of correct BCI input
    private Vector3 incorrectPos;
    private bool collided = false;
    private float turnDistance = 0.1f;
    private bool fail = false;

    public float distancetoIncorrect = 3;
    float t;
    Vector3 startPos;
    Vector3 target;
    public float timeToReachTarget;
    public float timeofWindow;

    private bool check1 = false;
    private bool check2 = false;
    private bool check3 = false;

    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        startPos = target = transform.position;
        targetPos = GameObject.Find("Point" + pointNr); // Target Position Ice Skater will travel toward
        incorrectPos = targetPos.transform.position + ((targetPos.transform.position - transform.position).normalized * distancetoIncorrect);
    }
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPos, target, t);

        if (Input.GetKeyDown("space"))
        {
            bciActivated(); // Run method for succesful BCI activation
        }
        if (Vector2.Distance(transform.position, targetPos.transform.position) > turnDistance && !fail && !check1)
        {
            SetDestination(targetPos.transform.position, timeofWindow);
            check1 = true;
        }
        else if (Vector2.Distance(transform.position, targetPos.transform.position) < turnDistance && correctInput)
        {
            startPos = transform.position;
            OnTargetPosChanged();
            SetDestination(targetPos.transform.position, timeofWindow);
            check1 = false; check2 = false; check3 = false;
            gameManager.SendMessage("ResumeTrial");
        }
        else if (!correctInput && !collided && !check2 && Vector2.Distance(transform.position, targetPos.transform.position) < turnDistance)
        {
            startPos = transform.position;
            fail = true;
            SetDestination(incorrectPos, 2);
            check2 = true;
            gameManager.SendMessage("ResetTrial");
            gameManager.SendMessage("PauseTrial");
        }
        if ((Vector2.Distance(transform.position, incorrectPos) < turnDistance && fail && !check3) || (collided && !check3))
        {
            startPos = transform.position;
            collided = true;
            SetDestination(targetPos.transform.position, 2);
            correctInput = true;
            check3 = true;
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPos = transform.position;
        timeToReachTarget = time;
        target = destination;
    }

    void bciActivated()
    {
        Debug.Log("Direction Changed");
        correctInput = true;
    }

    void OnTargetPosChanged() 
    {
        collided = false;
        fail = false;
        correctInput = false;
        pointNr++; targetPos = GameObject.Find("Point" + pointNr); // Set next point as target pos
        incorrectPos = targetPos.transform.position + ((targetPos.transform.position - transform.position).normalized * distancetoIncorrect); // arrange incorrectPos
    }

    public void OnGameStateChanged(GameData gameData)
    {
        // Set InputWindow, InputTime indication.
        timeofWindow = gameData.interTrialIntervalSeconds + gameData.inputWindowSeconds;
    }
}
