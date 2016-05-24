﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogs : MonoBehaviour 
{
    string[,] dialogText = new string[12,10]
        {{"Hi Chief! Welcome to 2016. My name is PAOLO and I’m an Engineer, just like YOU.","I will help you fulfill your mission.","In order to start your mission you need to develop a SPACE CENTER.","Start by giving it a name.","","","","","",""},
    {"Good! Cool name!","","","","","","","","",""},
    {"Here YOU have some MONEY to get started.","MONEY is the primary currency. Use it wisely.","","","","","","","",""},
    {"This is the HEADQUARTERS. The base of all operations.","QUESTS are the TASKS YOU need to perform to execute your mission properly.","Go ahead and have a look.","","","","","","",""},
    {"Here you can find FACILITIES which you will need for your mission and upgrading the SPACE CENTER.","","","","","","","","",""},
    {"Good job! You completed your first TASK.","","","","","","","","",""},
    {"RESEARCH POINTS are the secondary currency. The better you learn, the more you get.","","","","","","","","",""},
    {"Well done Chief! A SPACE EXHIBIT is a nice way of generating revenue.","","","","","","","","",""},
    {"Now, let’s assign a TASK to the EXHIBIT to generate revenue.","Click on exhibit to assign new task.","","","","","","","",""},
    {"With three buildings set, now it’s time to mark your SPACE CENTER as YOURS!","Place a FLAG of your choice to show who’s in charge.","","","","","","","",""},
    {"What a beautiful flag you have right there! Your SPACE CENTER looks so professional. Great!","","","","","","","","",""},
        {"What a beautiful flag you have right there! Your SPACE CENTER looks so professional. Great!","","","","","","","","",""}};

    public bool talk = false, waitForInput = false;
    public string msg01;
    string output = "HELLO WORLD";
    private int pos = 0;
    public float currentTime;
    int numberToSay = 0, dialogNumberMain;
    public bool tutorial = false;

    [SerializeField]
    GameObject canvas;

    void Update()
    {
        if (talk)
        {
            if (Time.time >= currentTime + .05f && msg01 != output)
            {
                pos++;
                currentTime = Time.time;
                msg01 = output.Substring(0, pos);
                canvas.GetComponentInChildren<Text>().text = msg01;
            }
            
            if(msg01 == output && !waitForInput && Input.GetMouseButtonUp(0))
            {
                numberToSay++;
                if(dialogText[dialogNumberMain, numberToSay] == "")
                {
                    talk = false;
                    if (!tutorial)
                    {
                        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(true);
                    }
                    canvas.SetActive(false);
                }
                else
                {
                    waitForInput = true;
                }
            }
            
            if (msg01 != output && Input.GetMouseButtonUp(0))
            {
                msg01 = output;
                canvas.GetComponentInChildren<Text>().text = msg01;
            }
        }

        if (waitForInput)
        {
            if (Input.GetMouseButtonUp(0))
            {
                waitForInput = false;
                pos = 0;
                canvas.SetActive(true);
                ActivateTalking(dialogNumberMain, numberToSay);
            }
        }
    }

    public void ActivateTalking(int dialogNumber, int number = 0)
    {
        canvas.SetActive(true);
        msg01 = "";
        pos = 0;
        talk = true;
        numberToSay = number;
        dialogNumberMain = dialogNumber;
        output = dialogText[dialogNumber, numberToSay];
        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(false);
    }
}
