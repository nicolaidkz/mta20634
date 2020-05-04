using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public GameObject player;
    public int offset;
    private Vector3 position;
    private Vector3 playerposition;

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        playerposition = player.transform.position;
        position.y = playerposition.y + offset; // Camera follows the player with specified offset position
        transform.position = position;
    }
}
