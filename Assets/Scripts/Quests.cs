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
    {{"","Construct a Research and Development center (RnD).","Construct an EXHIBIT.","Place a FLAG from DECORATIONS-MENU of your choice.","","","","","",""},
    {"","1","2","3","4","5","","","",""},
    {"","heey","hello","","","","","","",""}};
    int activeQuests = 0;
    public int questOpen = -1;

    int[] myInformation = new int[10];

    public bool tutorialBack = false;
    
    #region questRequirements
    int[,,] questRequirements = new int[3, 10, 10] // money, researchPoints, buildings [ 1 - 8 ]
    { { {0,0,0,0,0,0,0,0,0,0}, // {money, RP, building1, buildings2, buildings3, buildings4, buildings5, buildings6, buildings7, buildings8}
        {0,0,1,0,0,0,0,0,0,0},
        {0,0,0,1,0,0,0,0,0,0},
        {0,0,0,0,1,0,0,0,0,0}, // flag
        {0,0,0,0,0,0,0,0,0,0},  // none
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,1,0,0,0,0,0,0},
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
        {100,0,100,0,0,0,0,0,0,0},
        {0,0,100,0,0,0,0,0,0,0},
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
            if (questLineProgress[i] != 0)
            {
                if (CheckIfRequirementsAreSet(i, questLineProgress[i]))
                {
                    ShowInformation(i, allText[i, questLineProgress[i]], false, questLineProgress[i]);
                }
            }
        }

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
            ShowInformation(questOpen, allText[questOpen, questLineProgress[questOpen]], true);
        }
    }

    void ShowInformation(int toProgress, string text, bool showButton, int questDone = 0)
    {
        GUI.TextArea(new Rect(Screen.width / 3f, Screen.height / 4, 300, 100), text, textStyle);
        if (showButton)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 1.7f, 150, 100), "Back"))
            {
                questOpen = -1;
                if (tutorialBack)
                {
                    tutorialBack = false;
                }
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 1.7f, 150, 100), "Collect") && CheckIfRequirementsAreSet(toProgress, questDone, true))
            {
                questLineProgress[toProgress]++;
            }
        }
    }

    public bool CheckIfRequirementsAreSet(int questline, int quest, bool clickedCollect = false)
    {
        myInformation[0] = account.money;
        myInformation[1] = account.researchPoints;
        for(int i = 0; i < 8; i++)
        {
            myInformation[i + 2] = account.amountOfEachBuilding[i];
        }
        bool ifEnough = true;
        for (int i = 0; i < 10; i++)
        {
            if(myInformation[i] < questRequirements[questline, quest, i])
            {
                ifEnough = false;
            }
        }
        if (clickedCollect)
        {
            if (ifEnough)
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
