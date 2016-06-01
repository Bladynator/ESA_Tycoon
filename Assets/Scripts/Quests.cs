using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Quests : MonoBehaviour 
{
    Account account;
    [Header("Quest Setting")]
    [Tooltip("Requirements")]
    public int[] questLineProgress; // 0 = off
    [SerializeField]
    string[,] allText = new string[3, 10]
    {{"","Construct a Research and Development center (RnD).","Construct an EXHIBIT.","Place a FLAG from DECORATIONS-MENU of your choice.","","","","","",""},
    {"","Play the second minigame","2","3","4","5","","","",""},
    {"","heey","hello","","","","","","",""}};
    public int questOpen = -1;

    int[] myInformation = new int[10];
    int[] questsActive = new int[3];

    public bool tutorialBack = false, wait = false;
    [SerializeField]
    GameObject questInfoCanvas, questScreen;
    [SerializeField]
    Button[] buttons;
    Button questButton;

    List<string> texts = new List<string>();
    
    #region questRequirements
    int[,,] questRequirements = new int[3, 10, 10] // money, researchPoints, buildings [ 1 - 8 ]
    { { {0,0,0,0,0,0,0,0,0,1}, // {money, RP, building1, buildings2, Flag, minigame1Score, buildings5, buildings6, buildings7, buildings8}
        {0,0,0,0,0,1,0,0,0,0},
        {0,0,1,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,1,0}, // flag
        {0,0,0,0,0,0,0,0,0,0},  // none
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1},
        {0,0,0,0,0,10,0,0,0,0}, // minigame1 score
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1},
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
        {0,0,100,0,0,0,0,0,0,0},
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
        questButton = GameObject.Find("QuestsScreen").GetComponentInChildren<Button>();
        questButton.gameObject.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if (questLineProgress[i] != 0)
            {
                if (CheckIfRequirementsAreSet(i, questLineProgress[i]) && !wait)
                {
                    if(allText[i, questLineProgress[i]] == "")
                    {
                        questLineProgress[i] = 0;
                        ResetQuests();
                        break;
                    }
                    texts.Add(allText[i, questLineProgress[i]]);
                    ShowInformation(i, texts, false, questLineProgress[i]);
                    texts = new List<string>();
                    wait = true;
                }
            }
        }
    }

    public void ShowQuests(bool show)
    {
        questScreen.GetComponentInChildren<Button>().interactable = show;
    }

    public void ResetQuests()
    {
        texts = new List<string>();
        questButton.gameObject.SetActive(false);
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if (questLineProgress[i] != 0)
            {
                questButton.gameObject.SetActive(true);
                questButton.onClick.AddListener(delegate { OpenQuest(); });
                texts.Add(allText[i, questLineProgress[i]]);
            }
        }
    }

    void OpenQuest()
    {
        ResetQuests();
        ShowInformation(questOpen, texts, true);
        texts = new List<string>();
    }

    void PressedBack()
    {
        questOpen = -1;
        if (tutorialBack)
        {
            tutorialBack = false;
        }
        texts = new List<string>();
        questInfoCanvas.SetActive(false);
    }

    void PressedCollect(int toProgress)
    {
        questLineProgress[toProgress]++;
        wait = false;
        questInfoCanvas.SetActive(false);
        texts = new List<string>();
    }

    void ShowInformation(int toProgress, List<string> text, bool showButton, int questDone = 0)
    {
        questInfoCanvas.SetActive(true);
        buttons[0].onClick.AddListener(delegate { PressedBack(); });
        
        Text[] allTexts = questInfoCanvas.GetComponentsInChildren<Text>();
        allTexts = allTexts.Take(allTexts.Length - 1).ToArray();
        for (int i = 0; i < allTexts.Length; i++)
        {
            allTexts[i].text = "";
        }
        for (int i = 0; i < text.Count; i++)
        {
            allTexts[i].text = text[i];
        }
        buttons[0].gameObject.SetActive(true);
        buttons[1].gameObject.SetActive(true);
        if (showButton)
        {
            buttons[1].gameObject.SetActive(false);
        }
        else
        {
            buttons[1].onClick.RemoveAllListeners();
            buttons[1].onClick.AddListener(delegate { PressedCollect(toProgress); });
            buttons[0].gameObject.SetActive(false);
        }
        texts = new List<string>();
    }

    public bool CheckIfRequirementsAreSet(int questline, int quest, bool clickedCollect = false)
    {
        myInformation[0] = account.money;
        myInformation[1] = account.researchPoints;
        for(int i = 0; i < 8; i++)
        {
            myInformation[i + 2] = account.amountOfEachBuilding[i];
        }
        /*
        for(int i = 0; i < 1; i++)
        {
            myInformation[i + 9] = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[i];
        }
        */
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
                account.money += questRewards[questline, quest, 0];
                account.researchPoints += questRewards[questline, quest, 1];
                account.exp += questRewards[questline, quest, 2];
            }
        }
        return ifEnough;
    }
}
