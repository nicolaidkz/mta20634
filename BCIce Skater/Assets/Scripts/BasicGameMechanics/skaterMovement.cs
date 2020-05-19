using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class skaterMovement : MonoBehaviour
{
    private GameObject targetPos;
    GameObject incorrectPosMarker;
    public float speed = 10f;
    private int pointNr = 1;
    private bool correctInput = false; // To keep track of correct BCI input
    private Vector3 incorrectPos;
    private bool collided = false;
    private float turnDistance = 0.001f;
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

    private GameObject directionSwitch;

    public GameObject Fail;
    public GameObject Win2;
    public GameObject Alert;

    private bool star;

    private float windowTimer;

    public GameObject endText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("StartText").SetActive(false);
        directionSwitch = GameObject.Find("Player");
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

        if (pointNr < gameManager.GetComponent<GameManager>().trials)
        {
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
                if (star == true) StartCoroutine(Signal(Win2));
                star = false;
                //GameObject.Find("KeySequencer").SendMessage("TurnGreen"); for this to work, it needs to be a coRoutine that shines green for some amount of time...
                gameManager.GetComponent<GameManager>().interTrialTimer = 0;
                GameObject.Find("DifficultyAdjuster").SendMessage("InputAccepted", windowTimer);
                windowTimer = 0;
                gameManager.SendMessage("ResumeTrial");

            }
            else if (!correctInput && !check2 && Vector2.Distance(transform.position, targetPos.transform.position) < turnDistance)
            {
                startPos = transform.position;
                fail = true;
                SetDestination(incorrectPos, 2);
                check2 = true;
                gameManager.SendMessage("PauseTrial");
                //GameObject.Find("KeySequencer").SendMessage("TurnRed"); for this to work, it needs to be a coRoutine that shines red for some amount of time...
                GameObject.Find("DifficultyAdjuster").SendMessage("InputRejected");
                StartCoroutine(Signal(Fail));
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
    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPos = transform.position;
        timeToReachTarget = time;
        target = destination;
    }

    void bciActivated(float inputWindowTimer)
    {
        Debug.Log("Direction Changed");
        correctInput = true;
        star = true;
        windowTimer = inputWindowTimer;
    }

    void OnTargetPosChanged() 
    {
        collided = false;
        fail = false;
        correctInput = false;
        if (!Animations.rotate)
        {
            Animations.rotate = true;
        }
        else if (Animations.rotate)
        {
            Animations.rotate = false;
        }
        directionSwitch.GetComponent<Animations>().ToggleRotate();
        pointNr++;
        //if (pointNr > gameManager.GetComponent<GameManager>().trials)
        //{
          //  Debug.Log("test");
        //}

        targetPos = GameObject.Find("Point" + pointNr); // Set next point as target pos
        incorrectPos = targetPos.transform.position + ((targetPos.transform.position - transform.position).normalized * distancetoIncorrect); // arrange incorrectPos
    }

    //public void OnGameStateChanged(GameData gameData)
    //{
    //    // Set InputWindow, InputTime indication.
    //    timeofWindow = gameData.interTrialIntervalSeconds + gameData.inputWindowSeconds;
    //}

    public void UpdateTimeofWindow(float newDuration) 
    {
        timeofWindow = newDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EndGame")
        {
            // Log what needs to be logged
            GameObject.Find("LoggingManager").GetComponent<LoggingManager>().SendLogs();
            GetComponent<Animations>().ToggleIsRunning();
            if (SceneManager.GetActiveScene().name == "Constant")
            {
                SceneManager.LoadScene("Staggered");
            }
            if (SceneManager.GetActiveScene().name == "Staggered")
            {
                SceneManager.LoadScene("None");
            }
            if (SceneManager.GetActiveScene().name == "None")
            {
                endText.SetActive(true);
            }
        }
    }

    public void AlertSignal()
    {
        StartCoroutine(Signal(Alert));
    }

    IEnumerator Signal(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
