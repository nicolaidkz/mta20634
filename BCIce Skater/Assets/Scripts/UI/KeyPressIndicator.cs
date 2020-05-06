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

    public void ColorKey(KeyCode keypressed, string state)
    {
        if (keypressed == KeyCode.T && state == "keypressed")
            keys[0].GetComponent<SpriteRenderer>().color = Color.gray;
        else if (keypressed == KeyCode.T && state == "rejected")
            keys[0].GetComponent<SpriteRenderer>().color = Color.red;
        else if (keypressed == KeyCode.T && state == "accepted")
            keys[0].GetComponent<SpriteRenderer>().color = Color.green;
        else if (state == "reset")
            keys[0].GetComponent<SpriteRenderer>().color = Origincolor[0];

        if (keypressed == KeyCode.Y && state == "keypressed")
            keys[1].GetComponent<SpriteRenderer>().color = Color.gray;
        else if (keypressed == KeyCode.Y && state == "rejected")
            keys[1].GetComponent<SpriteRenderer>().color = Color.red;
        else if (keypressed == KeyCode.Y && state == "accepted")
            keys[1].GetComponent<SpriteRenderer>().color = Color.green;
        else if (state == "reset")
            keys[1].GetComponent<SpriteRenderer>().color = Origincolor[1];

        if (keypressed == KeyCode.U && state == "keypressed")
            keys[2].GetComponent<SpriteRenderer>().color = Color.gray;
        else if (keypressed == KeyCode.U && state == "rejected")
            keys[2].GetComponent<SpriteRenderer>().color = Color.red;
        else if (keypressed == KeyCode.U && state == "accepted")
            keys[2].GetComponent<SpriteRenderer>().color = Color.green;
        else if (state == "reset")
            keys[2].GetComponent<SpriteRenderer>().color = Origincolor[2];

        if (keypressed == KeyCode.I && state == "keypressed")
            keys[3].GetComponent<SpriteRenderer>().color = Color.gray;
        else if (keypressed == KeyCode.I && state == "rejected")
            keys[3].GetComponent<SpriteRenderer>().color = Color.red;
        else if (keypressed == KeyCode.I && state == "accepted")
            keys[3].GetComponent<SpriteRenderer>().color = Color.green;
        else if (state == "reset")
            keys[3].GetComponent<SpriteRenderer>().color = Origincolor[3];
    }
}
