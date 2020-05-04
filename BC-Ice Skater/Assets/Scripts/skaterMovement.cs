using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skaterMovement : MonoBehaviour
{
    private GameObject targetPos;
    public float speed = 1f;
    private int pointNr = 1;
    private bool correctInput = false; // To keep track of correct BCI input
    private Vector2 correctPos;
    private bool collided = false;
    private bool posSet = false;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = GameObject.Find("Point"+pointNr); // Target Position Ice Skater will travel toward

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(correctPos);
        if (Input.GetKeyDown("space"))
        {
            bciActivated(); // Run method for succesful BCI activation
        }

        if (!collided)
        {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos.transform.position, speed * Time.deltaTime); // Move Ice Skater toward target position
        }
        else
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), correctPos, speed * Time.deltaTime); // Move Ice Skater toward correct position
        }

        if (Vector2.Distance(transform.position, targetPos.transform.position) < 2 && correctInput == true) // If distance between Ice Skater and Target Pos is less than x
        {
            pointNr++; targetPos = GameObject.Find("Point"+pointNr); // Set next point as target pos
            correctInput = false;
            posSet = false;
            speed = 1f;
        }

        if (Vector2.Distance(transform.position, correctPos) < 0.1f && collided == true) // If distance between Ice Skater and Target Pos is less than x
        {
            pointNr++; targetPos = GameObject.Find("Point" + pointNr); // Set next point as target pos
            correctInput = false;
            collided = false;
            posSet = false;
            speed = 1f;
        }

        if (Vector2.Distance(transform.position, targetPos.transform.position) < 2 && !correctInput && !posSet)
        {
            correctPos = transform.position; // Save correct turn position
            posSet = true;
        }
    }

    void bciActivated()
    {
        Debug.Log("Space Pressed");
        correctInput = true;
    }

    void OnTriggerEnter()
    {
        collided = true;
        speed = 0.5f;
    }
}
