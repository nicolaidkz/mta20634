using UnityEngine;
using UnityEngine.UI;

public class KeyPressIndicator : MonoBehaviour
{
    public GameObject[] keyPrefabs;
    public float alpha = 0.001f;
    Color darkGreen = new Color(0, 0.7f, 0, 1);
    string keyOne = KeyCode.T.ToString();
    string keyTwo = KeyCode.Y.ToString();
    string keyThree = KeyCode.U.ToString();
    string keyFour = KeyCode.I.ToString();
    private int numKeys = 0;
    private int maxKeys;

    private void Start()
    {
        maxKeys = keyPrefabs.Length;

        for (int i = 0; i < keyPrefabs.Length; i++)
        {
            GameObject newPrefab = Instantiate(keyPrefabs[i], new Vector3(transform.position.x + i, transform.position.y, transform.position.z), Quaternion.identity, transform.parent);
            newPrefab.transform.parent = gameObject.transform.GetChild(0).transform;
            Color tmpColor = newPrefab.GetComponent<SpriteRenderer>().color; //temporarily store color
            tmpColor.a = alpha; //change alpha
            newPrefab.GetComponent<SpriteRenderer>().color = tmpColor; //assign alpha
        }
    }

    //Used in KeySequenceInput Script, changes color of keySequencer
    public void UIKey(string keyPressed, string action) 
    {
        if (action == "instantiate" && numKeys <= 3) //when button is first clicked make a new key on screen
        {            
            if (keyPressed == keyOne) //T
            {
                GameObject newPrefab = Instantiate(keyPrefabs[0], new Vector3(transform.position.x + numKeys, transform.position.y, transform.position.z), Quaternion.identity, transform.parent);
                newPrefab.transform.parent = gameObject.transform.GetChild(1).transform;
                if (numKeys < keyPrefabs.Length)
                {
                    newPrefab.transform.GetChild(0).GetComponent<Text>().text = "";
                }
                numKeys++;
            }
            else if (keyPressed == keyTwo) //Y
            {
                GameObject newPrefab = Instantiate(keyPrefabs[1], new Vector3(transform.position.x + numKeys, transform.position.y, transform.position.z), Quaternion.identity, transform.parent);
                newPrefab.transform.parent = gameObject.transform.GetChild(1).transform;
                if (numKeys < keyPrefabs.Length)
                {
                    newPrefab.transform.GetChild(0).GetComponent<Text>().text = "";
                }
                numKeys++;
            }
            else if (keyPressed == keyThree) //U
            {
                GameObject newPrefab = Instantiate(keyPrefabs[2], new Vector3(transform.position.x + numKeys, transform.position.y, transform.position.z), Quaternion.identity, transform.parent);
                newPrefab.transform.parent = gameObject.transform.GetChild(1).transform;
                if (numKeys < keyPrefabs.Length)
                {
                    newPrefab.transform.GetChild(0).GetComponent<Text>().text = "";
                }
                numKeys++;
            }
            else if (keyPressed == keyFour) //I
            {
                GameObject newPrefab = Instantiate(keyPrefabs[3], new Vector3(transform.position.x + numKeys, transform.position.y, transform.position.z), Quaternion.identity, transform.parent);
                newPrefab.transform.parent = gameObject.transform.GetChild(1).transform;
                if (numKeys < keyPrefabs.Length)
                {
                    newPrefab.transform.GetChild(0).GetComponent<Text>().text = "";
                }
                numKeys++;
            }

        }

        if (transform.GetChild(1).childCount > 0)
        {

            if (action == "reset")
            {

                while (transform.GetChild(1).childCount > 0)
                {
                    Transform child = transform.GetChild(1).GetChild(0);
                    child.parent = null;
                    Destroy(child.gameObject);
                    numKeys--;
                }
            }
        }


        

        //Changes the color of keys depending on what key is pressed
        //if (keyPressed == keyOne && action == "keyPressed") //Q
         //   keys[0].GetComponent<SpriteRenderer>().color = Color.gray;
        //else if (keyPressed == keyOne && action == "accepted")
       //    keys[0].GetComponent<SpriteRenderer>().color = Color.green;
        
        /*
        if (action == "reset")
        {
            //resets the color of sprite and text of all keys
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].GetComponent<SpriteRenderer>().color = originColor[i];
                keysText[i].GetComponent<Text>().color = originColorText[i];
            }
        }
        else if (action == "success")
        {
            //Changes the color of sprite and text of all keys
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].GetComponent<SpriteRenderer>().color = darkGreen;
                keysText[i].GetComponent<Text>().color = Color.white;
            }
        }
        */
    }
}
