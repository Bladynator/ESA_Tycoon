using UnityEngine;
using System.Collections;

public class Quests : MonoBehaviour 
{
    public int[] questLineProgress; // 0 = off
    [SerializeField]
    string[,] allText = new string[3, 10]
    {{"","Hello, build a HQ","yesyes","yesyesyes","yesyesyesyes","","","","",""},
    {"","1","2","3","4","5","","","",""},
    {"","heey","hello","","","","","","",""}};
    int activeQuests = 0;
    int questOpen = -1;

    int[] myInformation = new int[10];

    #region questRequirements
    int[,,] questRequirements = new int[3, 10, 10] // money, researchPoints, buildings [ 1 - 8 ]
    { { {0,0,0,0,0,0,0,0,0,0}, // {money, RP, building1, buildings2, buildings3, buildings4, buildings5, buildings6, buildings7, buildings8}
        {2000,1000,2,3,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}}, };
    #endregion

    #region questRewards
    int[,,] questRewards = new int[3, 10, 10] // money, researchPoints, exp
    { { {0,0,0,0,0,0,0,0,0,0},
        {0,0,100,0,0,0,0,0,0,0},
        {0,50,100,0,0,0,0,0,0,0},
        {50,50,100,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}}, };
    #endregion

    void OnGUI()
    {
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if(questLineProgress[i] != 0)
            {
                if(GUI.Button(new Rect(0, Screen.height / 3 + (activeQuests * 50), 50, 50), i.ToString()))
                {
                    if (questOpen == i)
                    {
                        questOpen = -1;
                    }
                    else
                    {
                        questOpen = i;
                    }
                }
                activeQuests++;
            }
        }
        activeQuests = 0;
        ShowQuest();
    }

    void ShowQuest()
    {
        if (questOpen != -1)
        {
            ShowInformation(questOpen, allText[questOpen, questLineProgress[questOpen]]);
        }
    }

    void ShowInformation(int toProgress, string text)
    {
        GUI.TextArea(new Rect(Screen.width / 2.5f, Screen.height / 3, 200, 100), text);
        if (GUI.Button(new Rect(Screen.width / 2 + 25, Screen.height / 1.7f, 100, 50), "Finish") && CheckIfRequirementsAreSet(questOpen, questLineProgress[toProgress]))
        {
            questLineProgress[toProgress]++;
            questOpen = -1;
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 1.7f, 100, 50), "Back"))
        {
            questOpen = -1;
        }
    }

    bool CheckIfRequirementsAreSet(int questline, int quest)
    {
        myInformation[0] = GameObject.Find("Account").GetComponent<Account>().money;
        myInformation[1] = GameObject.Find("Account").GetComponent<Account>().researchPoints;
        bool ifEnough = true;
        for (int i = 0; i < 10; i++)
        {
            if(myInformation[i] < questRequirements[questline, quest, i])
            {
                ifEnough = false;
            }
        }
        if (ifEnough)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject.Find("Account").GetComponent<Account>().money -= questRequirements[questline, quest, 0];
                GameObject.Find("Account").GetComponent<Account>().researchPoints -= questRequirements[questline, quest, 1];

                GameObject.Find("Account").GetComponent<Account>().money += questRewards[questline, quest, 0];
                GameObject.Find("Account").GetComponent<Account>().researchPoints += questRewards[questline, quest, 1];
                GameObject.Find("Account").GetComponent<Account>().exp += questRewards[questline, quest, 2];
            }
        }
        return ifEnough;
    }
}
