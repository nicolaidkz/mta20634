using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skaterMovement : MonoBehaviour
{
    private GameObject targetPos;
    GameObject incorrectPosMarker;
    public float speed = 1f;
    private int pointNr = 1;
    private bool correctInput = false; // To keep track of correct BCI input
    private Vector3 incorrectPos;
    private bool collided = false;
    private bool posSet = false;
    public float turnDistance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = GameObject.Find("Point" + pointNr); // Target Position Ice Skater will travel toward
        incorrectPos = targetPos.transform.position + ((targetPos.transform.position - transform.position).normalized * 2);
        incorrectPosMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        incorrectPosMarker.transform.position = incorrectPos;
    }
    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(correctPos);
        // if (Input.GetKeyDown("space"))
        //  {
        //      bciActivated(); // Run method for succesful BCI activation
        // }

        if (!collided)
        {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos.transform.position, speed * Time.deltaTime); // Move Ice Skater toward target position
        }
        else
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), incorrectPos, speed * Time.deltaTime); // Move Ice Skater toward correct position
        }

        if (Vector2.Distance(transform.position, targetPos.transform.position) < turnDistance && correctInput == true) // If distance between Ice Skater and Target Pos is less than x
        {
            OnTargetPosChanged();
            correctInput = false;
            posSet = false;
            speed = 1f;
        }

        if (Vector2.Distance(transform.position, targetPos.transform.position) < turnDistance && correctInput == false) // If distance between Ice Skater and Target Pos is less than x
        {
            transform.position = Vector3.MoveTowards(transform.position,incorrectPos, speed * Time.deltaTime);
            speed = 1f;
        }
        if (Vector2.Distance(transform.position, incorrectPos) < turnDistance) 
        {
            Vector3.Lerp(transform.position, targetPos.transform.position, 0.5f);
            //correctInput = true;
        }

        //if (Vector2.Distance(transform.position, incorrectPos) < 0.1f && collided == true) // If distance between Ice Skater and incorrect Pos is less than x
        //{
        //    pointNr++; targetPos = GameObject.Find("Point" + pointNr); // Set next point as target pos
        //    correctInput = false;
        //    collided = false;
        //    posSet = false;
        //    speed = 1f;
        //}


    }

    void bciActivated()
    {
        Debug.Log("Direction Changed");
        correctInput = true;
    }

    void OnTargetPosChanged() 
    {
        pointNr++; targetPos = GameObject.Find("Point" + pointNr); // Set next point as target pos
        incorrectPos = targetPos.transform.position + ((targetPos.transform.position - transform.position).normalized * 2); // arrange incorrectPos
        incorrectPosMarker.transform.position = incorrectPos;
    }

    void OnTriggerEnter()
    {
        Debug.Log("test");
        collided = true;
        speed = 0.5f;
    }
}
