using UnityEngine;
using System.Collections;

public class Dialogs : MonoBehaviour 
{
    string[,] dialogText = new string[10,10]
        {{"Hi Chief! Welcome to 2016. My name is PAOLO and I’m an Engineer, just like YOU.\nI will help you fulfill your mission.","In order to start your mission you need to develop a SPACE CENTER.\nStart by giving it a name.","","","","","","","",""},
    {"Good! Cool name!","","","","","","","","",""},
    {"Here YOU have some MONEY to get started.\nMONEY is the primary currency.\nUse it wisely.","","","","","","","","",""},
    {"This is the HEADQUARTERS. The base of all operations.","QUESTS are the TASKS YOU need to perform to execute your mission properly.\nGo ahead and have a look.","","","","","","","",""},
    {"Here you can find FACILITIES which you will need for your mission and upgrading the SPACE CENTER.","","","","","","","","",""},
    {"Good job! You completed your first TASK.","","","","","","","","",""},
    {"RESEARCH POINTS are the secondary currency.\nThe better you learn, the more you get.","","","","","","","","",""},
    {"Well done Chief! A SPACE EXHIBIT is a nice way of generating revenue.","","","","","","","","",""},
    {"With three buildings set, now it’s time to mark your SPACE CENTER as YOURS!\nPlace a FLAG of your choice to show who’s in charge.","","","","","","","","",""},
        {"What a beautiful flag you have right there! Your SPACE CENTER looks so professional. Great!","","","","","","","","",""}};

    public bool talk = false, waitForInput = false;
    public string msg01;
    string output = "HELLO WORLD";
    private int pos = 0;
    public float currentTime;
    int numberToSay = 0, dialogNumberMain;
    public bool tutorial = false;

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
            
            if(msg01 == output && !waitForInput && Input.GetMouseButtonDown(0))
            {
                numberToSay++;
                if(dialogText[dialogNumberMain, numberToSay] == "")
                {
                    talk = false;
                    if (!tutorial)
                    {
                        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(true);
                    }
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
    }
    
    void OnGUI()
    {
        if (talk)
        {
            GUI.DrawTexture(new Rect(0, Screen.height - Screen.height / 4, Screen.width, Screen.height / 4), backgroundDialog);
            GUI.TextArea(new Rect(10, Screen.height - Screen.height / 5, Screen.width - 10, 150), msg01, dialogStyle);
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
