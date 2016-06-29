using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Dialogs : MonoBehaviour
{
    string[,] dialogText = new string[39, 10]
        {{"a","Hi there! I was expecting you. Welcome to " + DateTime.Today.Year + ". I will be your assistant and will help you to fulfill your mission.","In order to start your mission you need to develop a Space Centre. Start by giving it a name.","","","","","","",""},
    {"a","Excellent! Cool name!","","","","","","","",""},
    {"a","Here you have some coins to get started. Coins are the primary currency. Use it wisely.","","","","","","","",""},
    {"a","These are the Headquarters. The base of all operations.","Quests are the tasks that you need to perform to accomplish your mission. Go ahead and have a look.","","","","","","",""},
    {"a","","","","","","","","",""},
    {"a","Good job! You have completed your first task.","","","","","","","",""},
    {"a","Research points are the secondary currency. The more you learn, the more points you get.","","","","","","","",""},
    {"a","Well done! A Space Exhibition Centre is a nice way of generating revenue.","","","","","","","",""},
    {"a","Now, let’s assign a task to the Exhibition Centre to start generating some revenue. Tap on the building to continue.","","","","","","","",""},
    {"a","With three buildings in place, it’s time to mark your Space Centre as your own! Place a flag to show who is in charge here.","","","","","","","",""},
    {"a","What a beautiful flag you have right there! Your Space Centre looks really professional. Great job!","","","","","","","",""},
        {"a","Let me tell you one last thing before we finish...","You can play mini-games in the R&D, Mission Control and Launchpad sections.","They are pretty exciting and help you understand the mission better. Maybe you should start playing one right now.","Well, that’s it for now. Good luck on your mission! And if you need any help, I’ll be close-by. See you soon!","","","","",""},
    {"a","Well done! Not bad for a first time.","","","","","","","",""},
    {"a","Nice score! You’re getting better at this.","","","","","","","",""},
    {"a","Wow, this is going pretty well! Keep up the good work.","","","","","","","",""},
    {"a","Awesome! You should be proud of your score!","","","","","","","",""},
    {"a","You have constructed not one, not two, but three Mission Buildings! Awesome! Keep up the good work.","","","","","","","",""},// first quest dialog
        {"a","Wow, your building has an amazing new look. Cool!","","","","","","","",""},
        {"a","You are pretty good in generating revenue because you just reached level 2. Nice!","","","","","","","",""},
        {"a","Those trees look lovely! Good to see you are styling your Space Centre. It looks amazing!","","","","","","","",""},
        {"a","Good job! You played all the mini-games on the easy level. It’s time to take it to the next level.","","","","","","","",""},
        {"a","Hey! Listen: yesterday, over 250 people visited the Exhibition Centre! This means you earned a lot of money. Check your balance.","","","","","","","",""},
        {"a","Hey! Now you don’t have one, but two Exhibition Centres! This means more money coming you way. Well done!","","","","","","","",""},
        {"a","Good job! All your Mission buildings have been upgraded.","","","","","","","",""},
        {"a","Hi there! Quick update: the Science College funded a few new projects for your Space Centre. You received some money from them. Go check it out.","","","","","","","",""},
        {"a","Seems like the testing phase went pretty well because you have just reached level 2. Nice!","","","","","","","",""},
        {"a","Hey! Now you don’t have one, but two Research & Development Centres. This means you can double your research and earn double the amount of research points. Well done!","","","","","","","",""},
        {"a","Hello there. How are you? You have unlocked a new task, which is organizing an open day. Go to the Headquarters to check it out.","","","","","","","",""},
        {"a","Wow, that installation is really cool! Good to see you are styling your Space Centre. It looks amazing!","","","","","","","",""}, // new
        {"a","Those trees look lovely! Good to see you are styling your Space Centre. It looks amazing!","","","","","","","",""},
        {"a","Hey! Now you have 6 new Mission Buildings. This means you can generate more Research Points. Well done!","","","","","","","",""},
        {"a","Hey! You upgraded your Mission Control. Nice! The building looks really good!","","","","","","","",""},
        {"a","Hey! You upgraded your Research & Development Centres. Nice! The buildings look lovely.","","","","","","","",""}, // end
        {"a","Congratulations! You have achieved your objective and launched the AIM mission!","The science collected will help us to learn more about potentially dangerous asteroids, and how to deflect them to save the world in the future!","The world thanks you, and we encourage you to keep playing to collect even more science!","","","","","",""},
        {"f","Hey! How are you doing?","You launched the satellite? That’s amazing! Good job.","Everyone is so proud of you. We hope you know that.","Okay, talk to you soon. Bye!","","","","",""},
        {"f","Hi there! How is it going?","Good to know. Oh, your research is coming in. Thank you!","We all think it is so cool what you’re doing. Anyway, good luck with the rest of your mission.","Talk to you soon. Bye bye!","","","","",""},
        {"f","Hello there! What’s going on?","The research you sent last time was really valuable. Did you discover anything new?","Ah, you’re sending it right now. Thanks!","Talk to you later. See you!","","","","",""},
        {"f","Hello there! Thank you for the files you sent us.","And for the ones you’re sending right now. Awesome!","Talk to you in a bit. Bye!","","","","","",""},
        {"f","Hi! How is your operation going?","You have over 13.000 people who are following you on our livestream channel. That’s crazy, right?","You have fans here and they look up to you and… Okay, I’ll just let you get on with your mission. Bye!","","","","","",""}}; // last quest dialog


    public bool talk = false, waitForInput = false;
    public string msg01;
    string output = "HELLO WORLD";
    private int pos = 0;
    public float currentTime;
    int numberToSay = 0, dialogNumberMain;
    public bool tutorial = false;

    [SerializeField]
    GameObject canvas, continueButton;
    [SerializeField]
    Sprite[] peopleToTalkTo;
    [SerializeField]
    Image character;
    [SerializeField]
    AudioClip talkSound;

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
                if (msg01.Substring(msg01.Length - 1) != " ")
                {
                    GameObject.Find("SFXController").GetComponent<AudioSource>().PlayOneShot(talkSound);
                }
            }

            if (msg01 == output)
            {
                continueButton.SetActive(true);
            }

            if (msg01 == output && !waitForInput && Input.GetMouseButtonUp(0))
            {
                numberToSay++;
                if (dialogText[dialogNumberMain, numberToSay] == "")
                {
                    talk = false;
                    if (!tutorial)
                    {
                        MainGameController.ChangeColliders(true);
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
    }

    public void ActivateTalking(int dialogNumber, int number = 0)
    {
        
        continueButton.SetActive(false);
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
            MainGameController.ChangeColliders(false);
        }
    }
}
