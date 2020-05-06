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

    // Start is called before the first frame update
    void Start()
    {
        targetPos = GameObject.Find("Point" + pointNr); // Target Position Ice Skater will travel toward
        incorrectPos = targetPos.transform.position + ((targetPos.transform.position - transform.position).normalized * distancetoIncorrect);
        incorrectPosMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        incorrectPosMarker.transform.position = incorrectPos;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            bciActivated(); // Run method for succesful BCI activation
        }
        if (Vector2.Distance(transform.position, targetPos.transform.position) > turnDistance && !fail)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos.transform.position, speed * Time.deltaTime); // Move Ice Skater toward target position
        }
        else if (Vector2.Distance(transform.position, targetPos.transform.position) < turnDistance && correctInput)
        {
            OnTargetPosChanged();
        }
        else if (!correctInput && !collided)
        {
            fail = true;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), incorrectPos, speed * Time.deltaTime);
        }
        if ((Vector2.Distance(transform.position, incorrectPos) < turnDistance && fail) || collided)
        {
            collided = true;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos.transform.position, speed * Time.deltaTime); // Move Ice Skater toward target position
            correctInput = true;
        }
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
        incorrectPosMarker.transform.position = incorrectPos;
    }
}
