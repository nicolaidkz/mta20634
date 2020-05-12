using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FlipAnim()
    {
        if(spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
        }
        if(spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
        }

        yield return new WaitForSeconds(1);
    }

    public void OnGameStateChanged(GameData gameData)
    {
        while(gameData.gameState == GameState.Running)
        {
            StartCoroutine(FlipAnim());
        }
    }
}
