using UnityEngine;
using System.Collections;

public class Dialogs : MonoBehaviour 
{
    string[,] dialogText = new string[10,10]
        {{"First Text","Hello, build a HQ","yesyes Hello, build a HQ Hello, build a HQ","yesyesyes","yesyesyesyes","","","","",""},
    {"","1","2","3","4","5","","","",""},
    {"","heey","hello","","","","","","",""},
    {"","heey","hello","","","","","","",""},
    {"","heey","hello","","","","","","",""},
    {"","heey","hello","","","","","","",""},
    {"","heey","hello","","","","","","",""},
    {"","heey","hello","","","","","","",""},
    {"","heey","hello","","","","","","",""},
        {"","heey","hello","","","","","","",""}};

    bool talk = false, waitForInput = false;
    public string msg01;
    string output = "HELLO WORLD";
    private int pos = 0;
    public float currentTime;
    int numberToSay = 0, dialogNumberMain;

    public GUIStyle dialogStyle;
    public Texture2D backgroundDialog;

    void Update()
    {
        if (talk)
        {
            if (Time.time >= currentTime + .05f && msg01 != output)
            {
                pos++;
                currentTime = Time.time;
                msg01 = output.Substring(0, pos);
            }
            
            if(msg01 == output && !waitForInput)
            {
                numberToSay++;
                if(dialogText[dialogNumberMain, numberToSay] == "")
                {
                    talk = false;
                    GameObject.Find("Account").GetComponent<Account>().ChangeColliders(true);
                }
                else
                {
                    waitForInput = true;
                }
            }

            
            if (msg01 != output && Input.GetMouseButtonDown(0))
            {
                msg01 = output;
            }
        }

        if (waitForInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                waitForInput = false;
                pos = 0;
                ActivateTalking(dialogNumberMain, numberToSay);
            }
        }

        // testing ONLY!   !!!!!!!!!!!!!!!!!!!!!!! KEYCODES NEEDS TO BE REPLACED WITH CLICK !!!!!!!!!!!!!!!!!!!!!!!!!!
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateTalking(0);
        }
    }
    
    void OnGUI()
    {
        if (talk)
        {
            GUI.DrawTexture(new Rect(0, Screen.height - Screen.height / 4, Screen.width, Screen.height / 4), backgroundDialog);
            GUI.Label(new Rect(10, Screen.height - Screen.height / 5, Screen.width, 150), msg01, dialogStyle);
        }
    }

    public void ActivateTalking(int dialogNumber, int number = 0)
    {
        talk = true;
        numberToSay = number;
        dialogNumberMain = dialogNumber;
        output = dialogText[dialogNumber, numberToSay];
        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(false);
    }
}
