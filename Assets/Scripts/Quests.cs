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
    string[,] allText = new string[4, 10]
    {{"","Construct a Research and Development Centre (R&D)","Construct an Exhibition Centre","Place a Flag (from the Decorations Menu)","","","","","",""},
    {"","Construct 3 Mission Buildings","Upgrade the Headquarters to Level 2","Upgrade all the Resource Buildings to Level 2","Plant 2 Trees","","","","",""},
    {"","Play all mini-games (easy mode)","Upgrade the Exhibition Centre to Level 2","Construct a new Exhibition Centre","Upgrade Mission Buildings to Level 3","","","","",""},
    {"","Set up the Science College","Upgrade all the Mission Buildings to Level 2","Construct a new Research & Development Centre","Upgrade the Headquarters to Level 3","","","","",""}};
    public int questOpen = -1;

    int[] myInformation = new int[20];
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
    int[,,] questRequirements = new int[4, 10, 20] // money, researchPoints, buildings [ 1 - 8 ]
    { { {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // {money, RP, ex, sc, hq, rnd, lp, mc, flag, tree, thing, minigame1, 2, 3} AMOUNT after: level {ex, sc, hq, rnd, lp, mc}
        {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0}, // flag
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},  // none
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // empty / off
        {0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0}, 
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0}, 
        {0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // empty / off
        {0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0}, 
        {0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},

      { {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // empty / off
        {0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2},
        {0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}} };
    #endregion
    #region questRewards
    int[,,] questRewards = new int[4, 10, 10] // money, researchPoints, exp, empty, dialogTrigger
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
        {50,0,0,0,17,0,0,0,0,0},
        {100,0,0,0,18,0,0,0,0,0},
        {200,0,0,0,19,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {0,0,0,0,0,0,0,0,0,0},
        {0,200,0,0,20,0,0,0,0,0},
        {200,0,0,0,21,0,0,0,0,0},
        {200,0,0,0,22,0,0,0,0,0},
        {0,200,0,0,23,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0}},

        { {100,0,0,0,0,0,0,0,0,0},
        {0,200,0,0,24,0,0,0,0,0},
        {100,0,0,0,25,0,0,0,0,0},
        {200,0,0,0,26,0,0,0,0,0},
        {0,0,0,0,27,0,0,0,0,0},
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
                    MakeCanvas(allText[i, questLineProgress[i]], questRewards[i, questLineProgress[i], 0], questRewards[i, questLineProgress[i], 1], i, questRewards[i, questLineProgress[i], 5]);
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

        myInformation[11] = PlayerPrefs.GetInt("Minigame1");
        myInformation[12] = PlayerPrefs.GetInt("Minigame2");
        myInformation[13] = PlayerPrefs.GetInt("Minigame3");

        for (int i = 14; i < myInformation.Length; i++)
        {
            myInformation[i] = 0;
        }
        for (int i = 0; i < myInformation.Length; i++)
        {
            if (questRequirements[questline, quest, i] != 0)
            {
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                for (int h = 0; h < buildings.Length; h++)
                {
                    if (buildings[h].GetComponent<BuildingMain>() != null)
                    {
                        switch (buildings[h].GetComponent<BuildingMain>().buildingName) // level {ex, sc, hq, rnd, lp, mc}
                        {
                            case "Exhibition Centre":
                                {
                                    if (buildings[h].GetComponent<BuildingMain>().level + 1 > myInformation[14])
                                    {
                                        myInformation[14] = buildings[h].GetComponent<BuildingMain>().level + 1;
                                    }
                                    break;
                                }
                            case "Science College":
                                {
                                    if (buildings[h].GetComponent<BuildingMain>().level + 1 > myInformation[15])
                                    {
                                        myInformation[15] = buildings[h].GetComponent<BuildingMain>().level + 1;
                                    }
                                    break;
                                }
                            case "Headquarters":
                                {
                                    if (buildings[h].GetComponent<BuildingMain>().level + 1 > myInformation[16])
                                    {
                                        myInformation[16] = buildings[h].GetComponent<BuildingMain>().level + 1;
                                    }
                                    break;
                                }
                            case "R&D Centre":
                                {
                                    if (buildings[h].GetComponent<BuildingMain>().level + 1 > myInformation[17])
                                    {
                                        myInformation[17] = buildings[h].GetComponent<BuildingMain>().level + 1;
                                    }
                                    break;
                                }
                            case "Launchpad":
                                {
                                    if (buildings[h].GetComponent<BuildingMain>().level + 1 > myInformation[18])
                                    {
                                        myInformation[18] = buildings[h].GetComponent<BuildingMain>().level + 1;
                                    }
                                    break;
                                }
                            case "Mission Control":
                                {
                                    if (buildings[h].GetComponent<BuildingMain>().level + 1 > myInformation[19])
                                    {
                                        myInformation[19] = buildings[h].GetComponent<BuildingMain>().level + 1;
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }

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
        MainGameController.ChangeColliders(true);
        MainGameController.CanMove(true);
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
            MainGameController.ChangeColliders(false);
            MainGameController.CanMove(false);
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
