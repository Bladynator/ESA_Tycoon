using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Quests : MonoBehaviour 
{
    Account account;
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
    int[] questsActive = new int[3];

    public bool tutorialBack = false, wait = false;
    [SerializeField]
    GameObject questCanvas, questInfoCanvas;
    GameObject tempQuestCanvas, tempQuestInfoCanvas;
    Button[] questButtons = new Button[3];
    
    #region questRequirements
    int[,,] questRequirements = new int[3, 10, 10] // money, researchPoints, buildings [ 1 - 8 ]
    { { {0,0,0,0,0,0,0,0,0,1}, // {money, RP, building1, buildings2, buildings3, buildings4, buildings5, buildings6, buildings7, buildings8}
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
        {100,0,50,0,0,0,0,0,0,0},
        {0,0,40,0,0,0,0,0,0,0},
        {50,50,10,0,0,0,0,0,0,0},
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
        tempQuestCanvas = Instantiate(questCanvas);
        questButtons = tempQuestCanvas.GetComponentsInChildren<Button>();
        for(int i = 0; i < 3; i++)
        {
            questButtons[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if (questLineProgress[i] != 0)
            {
                if (CheckIfRequirementsAreSet(i, questLineProgress[i]) && !wait)
                {
                    ShowInformation(i, allText[i, questLineProgress[i]], false, questLineProgress[i]);
                    wait = true;
                }
            }
        }
    }

    public void ResetQuests()
    {
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if (questLineProgress[i] != 0)
            {
                questsActive[activeQuests] = i;
                questButtons[activeQuests].gameObject.SetActive(true);
                questButtons[activeQuests].onClick.AddListener(delegate { OpenQuest(activeQuests); });
                activeQuests++;
            }
        }
        activeQuests = 0;
    }

    void OpenQuest(int button)
    {
        int i = questsActive[button];
        if (questOpen == i)
        {
            questOpen = -1;
        }
        else
        {
            questOpen = i;
        }
        ShowInformation(questOpen, allText[questOpen, questLineProgress[questOpen]], true);
    }

    void PressedBack()
    {
        questOpen = -1;
        if (tutorialBack)
        {
            tutorialBack = false;
        }
        Destroy(tempQuestInfoCanvas);
    }

    void PressedCollect(int toProgress)
    {
        questLineProgress[toProgress]++;
        wait = false;
        Destroy(tempQuestInfoCanvas);
    }

    void ShowInformation(int toProgress, string text, bool showButton, int questDone = 0)
    {
        tempQuestInfoCanvas = Instantiate(questInfoCanvas);
        Button[] buttons = tempQuestInfoCanvas.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(delegate { PressedBack(); });
        buttons[1].onClick.AddListener(delegate { PressedCollect(toProgress); });
        tempQuestInfoCanvas.GetComponentInChildren<Text>().text = text;
        
        if (showButton)
        {
            buttons[1].gameObject.SetActive(false);
        }
        else
        {
            buttons[0].gameObject.SetActive(false);
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
