using UnityEngine;
using System.Collections;

public class Quests : MonoBehaviour 
{
    Account account;
    public GUIStyle textStyle;
    [Header("Quest Setting")]
    [Tooltip("Requirements")]
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

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
    }

    void OnGUI()
    {
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if(questLineProgress[i] != 0)
            {
                if(GUI.Button(new Rect(0, Screen.height / 4 + (activeQuests * 75), 75, 75), i.ToString()))
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
        GUI.TextArea(new Rect(Screen.width / 3f, Screen.height / 4, 300, 100), text, textStyle);
        if (GUI.Button(new Rect(Screen.width / 2 + 25, Screen.height / 1.7f, 150, 100), "Finish") && CheckIfRequirementsAreSet(questOpen, questLineProgress[toProgress]))
        {
            questLineProgress[toProgress]++;
            questOpen = -1;
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 1.7f, 150, 100), "Back"))
        {
            questOpen = -1;
        }
    }

    bool CheckIfRequirementsAreSet(int questline, int quest)
    {
        myInformation[0] = account.money;
        myInformation[1] = account.researchPoints;
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
                account.money -= questRequirements[questline, quest, 0];
                account.researchPoints -= questRequirements[questline, quest, 1];

                account.money += questRewards[questline, quest, 0];
                account.researchPoints += questRewards[questline, quest, 1];
                account.exp += questRewards[questline, quest, 2];
            }
        }
        return ifEnough;
    }
}
