using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSaver : MonoBehaviour
{

    public GameObject textToAppear;
    public GameObject buttonToAppear;

    public GameObject inputField;

    public void StoreName()
    {
        if (inputField.GetComponent<Text>().text == "")
        {
            textToAppear.SetActive(true);
        }
        else
        {
            NameSaving.playerName = inputField.GetComponent<Text>().text;
            GameObject.Find("Button1").SetActive(false);
            buttonToAppear.SetActive(true);
        }
    }
    
}
