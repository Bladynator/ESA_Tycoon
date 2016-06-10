using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Dialogs : MonoBehaviour
{
    string[,] dialogText = new string[16, 10]
        {{"a","Hi there! I was expecting you. Welcome to " + DateTime.Today.Year + " I will be your assistant and help you fulfill your mission.","In order to start your mission you need to develop a Space Centre. Start by giving it a name.","","","","","","",""},
    {"a","Excellent! Cool name!","","","","","","","",""},
    {"a","Here you have some coins to get started. Coins are the primary currency. Use it wisely.","","","","","","","",""},
    {"a","These are the Headquarters. The base of all operations.","Quests are the tasks that you need to perform to accomplish your mission. Go ahead and have a look.","","","","","","",""},
    {"a","","","","","","","","",""},
    {"a","Good job! You have completed your first task.","","","","","","","",""},
    {"a","Research points are the secondary currency. The more you learn, the more points you get.","","","","","","","",""},
    {"a","Well done! A Space Exhibition Centre is a nice way of generating revenue.","","","","","","","",""},
    {"a","Now, let’s assign a task to the Exhibition Centre to start generating some revenue. Tap on the building to continue.","","","","","","","",""},
    {"a","With three buildings in place, it’s time to mark your Space Centre as your own! Place a flag of your choice to show who is in charge here.","","","","","","","",""},
    {"a","What a beautiful flag you have right there! Your Space Centre looks really professional. Great job!","","","","","","","",""},
        {"a","Let me tell you one last thing before we finish...","You can play mini-games in the R&D, Mission Control and Launchpad sections.","They are pretty exciting and help you understand the mission better. Maybe you should start playing one right now.","Well, that’s it for now. Good luck on your mission! And if you need any help, I’ll be close-by. See you soon!","","","","",""},
    {"a","Well done! Not bad for a first time.","","","","","","","",""},
    {"a","Nice score! You’re getting better at this.","","","","","","","",""},
    {"a","Wow, this is going pretty well! Keep up the good work.","","","","","","","",""},
    {"a","Awesome! You should be proud of your score!","","","","","","","",""}};


    public bool talk = false, waitForInput = false;
    public string msg01;
    string output = "HELLO WORLD";
    private int pos = 0;
    public float currentTime;
    int numberToSay = 0, dialogNumberMain;
    public bool tutorial = false;

    [SerializeField]
    GameObject canvas;
    [SerializeField]
    Sprite[] peopleToTalkTo;
    [SerializeField]
    Image character;

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
                    GameObject.Find("Main Camera").GetComponent<CameraChanger>().activeDialog = false;
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
        GameObject.Find("Main Camera").GetComponent<CameraChanger>().activeDialog = true;
        canvas.GetComponent<RectTransform>().SetAsLastSibling();
        if (number == 0)
        {
            Image[] tempImages = canvas.GetComponentsInChildren<Image>();

            if(dialogText[dialogNumber, number] == "f")
            {
                character.sprite = peopleToTalkTo[1];
            }
            else
            {
                character.sprite = peopleToTalkTo[0];
            }
            ActivateTalking(dialogNumber, 1);
        }
        else
        {
            msg01 = "";
            pos = 0;
            talk = true;
            numberToSay = number;
            dialogNumberMain = dialogNumber;
            output = dialogText[dialogNumber, numberToSay];
            GameObject.Find("Account").GetComponent<Account>().ChangeColliders(false);
        }
    }
}
