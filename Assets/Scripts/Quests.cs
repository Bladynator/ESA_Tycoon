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
    {"","Construct 3 Mission Buildings","Play all mini-games (easy mode)","3","4","5","","","",""},
    {"","heey","hello","","","","","","",""}};
    public int questOpen = -1;

    int[] myInformation = new int[14];
    int[] questsActive = new int[3];

    public bool tutorialBack = false, wait = false;
    bool onceCanvas = false;
    [SerializeField]
    GameObject questInfoCanvas, questScreen, canvas;
    [SerializeField]
    Button[] buttons;
    [SerializeField]
    Toggle questButton;

    List<string> texts = new List<string>();
    
    #region questRequirements
    int[,,] questRequirements = new int[3, 10, 14] // money, researchPoints, buildings [ 1 - 8 ]
    { { {0,0,0,0,0,0,0,0,0,1,0,0,0,0}, // {money, RP, ex, sc, hq, rnd, lp, mc, flag, tree, thing, minigame1, 2, 3}
        {0,0,0,0,0,1,0,0,0,0,0,0,0,0},
        {0,0,1,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,1,0,0,0,0,0}, // flag
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  // none
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1,0,0,0,0}, // empty / off
        {0,0,0,0,0,1,1,1,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,10,10,10}, // minigames
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1,0,0,0,0}, // empty / off
        {0,0,0,0,0,1,1,1,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0}} };
    #endregion
    #region questRewards
    int[,,] questRewards = new int[3, 10, 10] // money, researchPoints, exp, empty, dialogTrigger
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
        {50,0,0,0,16,0,0,0,0,0},
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
        //questButton = GameObject.Find("QuestsScreen").GetComponentInChildren<Toggle>();
        //questButton.gameObject.SetActive(false);
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
                    //texts.Add(allText[i, questLineProgress[i]]);
                    MakeCanvas(allText[i, questLineProgress[i]], questRewards[i, questLineProgress[i], 0], questRewards[i, questLineProgress[i], 1], i, questRewards[i, questLineProgress[i], 5]);
                    //texts = new List<string>();
                    wait = true;
                }
            }
        }
    }

    public void ShowQuests(bool show)
    {
        questScreen.GetComponentInChildren<Toggle>().interactable = show;
    }

    public void ResetQuests()
    {
        texts = new List<string>();
        questButton.interactable = false;
        for (int i = 0; i < questLineProgress.Length; i++)
        {
            if (questLineProgress[i] != 0)
            {
                questButton.interactable = true;
                //questButton.onValueChanged.AddListener(delegate { OpenQuest(); });
                texts.Add(allText[i, questLineProgress[i]]);
            }
        }
    }

    public void OpenQuest()
    {
        ResetQuests();
        ShowInformation(questOpen, texts);
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

    void ShowInformation(int toProgress, List<string> text)
    {
        questInfoCanvas.SetActive(true);
        buttons[0].onClick.AddListener(delegate { PressedBack(); });
        
        Text[] allTexts = questInfoCanvas.GetComponentsInChildren<Text>();
        allTexts = allTexts.Take(allTexts.Length - 2).ToArray();
        for (int i = 0; i < allTexts.Length; i++)
        {
            allTexts[i].text = "";
        }
        for (int i = 0; i < text.Count; i++)
        {
            allTexts[i].text = text[i];
        }
        buttons[0].gameObject.SetActive(true);
        //buttons[1].gameObject.SetActive(true);
        //buttons[1].gameObject.SetActive(false);
        texts = new List<string>();
    }

    bool CheckIfRequirementsAreSet(int questline, int quest)
    {
        myInformation[0] = account.money;
        myInformation[1] = account.researchPoints;
        for(int i = 0; i < 8; i++)
        {
            myInformation[i + 2] = account.amountOfEachBuilding[i];
        }
        
        myInformation[11] = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[0];
        myInformation[12] = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[1];
        myInformation[13] = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[2];

        bool ifEnough = true;
        for (int i = 0; i < myInformation.Length; i++)
        {
            if(myInformation[i] < questRequirements[questline, quest, i])
            {
                ifEnough = false;
            }
        }
        return ifEnough;
    }
    
    void PressedCollectButton(int addedMoney, int addedRP, int quest, int newDialog)
    {
        account.money += addedMoney;
        account.researchPoints += addedRP;
        onceCanvas = false;
        wait = false;
        questLineProgress[quest]++;
        canvas.SetActive(false);
        if (newDialog != 0)
        {
            GetComponent<Dialogs>().ActivateTalking(newDialog);
        }
    }

    void MakeCanvas(string text, int addedMoney, int addedRP, int quest, int newDialog)
    {
        if (!onceCanvas)
        {
            onceCanvas = true;
            canvas.SetActive(true);
            canvas.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            canvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedCollectButton(addedMoney, addedRP, quest, newDialog); });
            Text[] allText = canvas.GetComponentsInChildren<Text>();
            allText[0].text = text;
            allText[2].text = addedMoney.ToString();
            allText[3].text = addedRP.ToString();
        }
    }
}
