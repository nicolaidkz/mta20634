using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPressIndicator : MonoBehaviour
{
    public GameObject[] keys;
    private Color[] Origincolor;

    private void Start()
    {
        Origincolor = new Color[keys.Length];
        int i = 0;
        foreach (GameObject key in keys)
        {
            Origincolor[i] = key.GetComponent<SpriteRenderer>().color;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Q"))
            keys[0].GetComponent<SpriteRenderer>().color = Color.gray;
        else
            keys[0].GetComponent<SpriteRenderer>().color = Origincolor[0];

        if (Input.GetButton("W"))
            keys[1].GetComponent<SpriteRenderer>().color = Color.gray;
        else
            keys[1].GetComponent<SpriteRenderer>().color = Origincolor[1];

        if (Input.GetButton("E"))
            keys[2].GetComponent<SpriteRenderer>().color = Color.gray;
        else
            keys[2].GetComponent<SpriteRenderer>().color = Origincolor[2];

        if (Input.GetButton("R"))
            keys[3].GetComponent<SpriteRenderer>().color = Color.gray;
        else
            keys[3].GetComponent<SpriteRenderer>().color = Origincolor[3];


    }
}
