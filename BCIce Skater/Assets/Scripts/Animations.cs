using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool flip;
    public static bool isRunning = false;
    public static bool rotate = false;
    private float elapsedTime = 0.0f;
    //private Vector3 startPosition;
    //private Quaternion startRotation = transform.rotation;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        flip = true;
        targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f));
    }

    IEnumerator FlipAnim()
    {
        while (isRunning)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
            else if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }

            yield return new WaitForSeconds(0.7f);
        } 
    }

    IEnumerator RotateAnim(float time)
    {
        if (!rotate)
        {
            Quaternion startRotation = transform.rotation;
            float originalTime = time;
            while (time > 0.0f)
            {
                time -= Time.deltaTime;
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0, 0, -45), 1 - (time / originalTime));
            }
        }
        if (rotate)
        {
            Quaternion startRotation = transform.rotation;
            float originalTime = time;
            while (time > 0.0f)
            {
                time -= Time.deltaTime;
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(0, 0, 45), 1 - (time / originalTime));
                
            }
        }

        yield return new WaitForSeconds(0.7f);
    }



    public void ToggleIsRunning()
    {
        StopCoroutine(FlipAnim());
        isRunning = !isRunning;
        StartCoroutine(FlipAnim());
    }

    public void ToggleRotate()
    {
        StopCoroutine(RotateAnim(7.0f));
        StartCoroutine(RotateAnim(7.0f));
    }
}
