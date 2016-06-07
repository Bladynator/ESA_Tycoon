using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class Tutorial : MonoBehaviour 
{
    Quests questLine;
    Dialogs dialogs;
    HUD hud;
    Account account;
    int activeQuest;
    int questPart = 1, oldQuestPart;
    int activeArrow = 0;
    string townName = "ESA ESTEC";
    [SerializeField]
    GameObject hq, timemachine;
    public bool tutorialDoing = false;
    BuildingButtons builderMenu;
    [SerializeField]
    GameObject canvas, nameInputCanvas, skymap;
    [SerializeField]
    Toggle menu;
    [SerializeField]
    GameObject[] arrowLocations;
    bool onceName = false, onceCanvas = false;

    void Start () 
	{
        questLine = GameObject.Find("Quests").GetComponent<Quests>();
        dialogs = GameObject.Find("Quests").GetComponent<Dialogs>();
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        account = GameObject.Find("Account").GetComponent<Account>();
        builderMenu = hud.buildMenu;
    }
    
    void FixedUpdate () 
	{
        activeQuest = questLine.questLineProgress[0];
    }

    void Update()
    {
        switch (activeQuest)
        {
            case 1:
                {
                    switch (questPart)
                    {
                        case 1:
                            {
                                GameObject.Find("HUD").GetComponent<HUD>().ChangeBadge();
                                GameObject.Find("HUD").GetComponent<HUD>().UpdateNotification(account.newbuildings[account.level]);
                                tutorialDoing = true;
                                menu.interactable = false;
                                account.ChangeColliders(false);
                                account.autoSave = false;
                                hud.EnableButton(false);
                                hud.CanvasEnabled(false);
                                questLine.ShowQuests(false);
                                dialogs.tutorial = true;
                                skymap.SetActive(false);
                                ShowDialog(0);
                                //PlaceHQ(timemachine, new Vector2(10, 5));
                                break;
                            }
                        case 2:
                            {
                                MakeCanvasName(townName);
                                onceName = true;
                                break;
                            }
                        case 3:
                            {
                                MakeCanvas("You got: 1000 Gold!", 1000, 0, 0);
                                break;
                            }
                        case 4:
                            {
                                ShowDialog(1);
                                break;
                            }
                        case 5:
                            {
                                ShowDialog(2);
                                break;
                            }
                        case 6:
                            {
                                PlaceHQ(hq, new Vector2(2,5));
                                questPart++;
                                break;
                            }
                        case 7:
                            {
                                ShowDialog(3);
                                account.waitForInput = true;
                                break;
                            }
                        case 8:
                            {
                                ShowArrow(0);
                                questLine.ShowQuests(true);
                                if (!account.waitForInput)
                                {
                                    account.waitForInput = true;
                                    questLine.tutorialBack = true;
                                    questPart++;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 9:
                            {
                                ShowArrow(1);
                                if (!account.waitForInput)
                                {
                                    account.waitForInput = true;
                                    questPart++;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 10:
                            {
                                ShowArrow(4);
                                account.ChangeColliders(true);
                                if(GameObject.Find("BuildMenu") != null)
                                {
                                    GameObject.Find("BuildMenu").GetComponentInChildren<BuildingButtons>().tutorialBack = true;
                                    questPart++;
                                    DestroyArrow();
                                    ShowArrow(3);
                                }
                                break;
                            }
                        case 11:
                            {
                                if(GameObject.Find("BuildMenu").GetComponentInChildren<BuildingButtons>().tutorialBack == false)
                                {
                                    questPart++;
                                }
                                break;
                            }
                        case 12:
                            {
                                DestroyArrow();
                                ShowArrow(2);
                                questPart = -2;
                                break;
                            }
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch(questPart)
                    {
                        case 1:
                            {
                                ShowDialog(5, false);
                                break;
                            }
                        case 2:
                            {
                                MakeCanvas("You got 500 Research Points!", 0, 500, 1);
                                break;
                            }
                        case 3:
                            {
                                ShowDialog(6, false);
                                account.waitForInput = true;
                                break;
                            }
                        case 4:
                            {
                                ShowArrow(4);
                                if (!account.waitForInput)
                                {
                                    questLine.tutorialBack = true;
                                    questPart = 20;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 5:
                            {
                                questPart = -3;
                                if (!builderMenu.tutorialBack)
                                {
                                    DestroyArrow();
                                    questPart++;
                                    builderMenu.tutorialBack = true;
                                }
                                break;
                            }
                        case 6:
                            {
                                ShowArrow(3);
                                if (!builderMenu.tutorialBack)
                                {
                                    DestroyArrow();
                                    questPart++;
                                    builderMenu.tutorialBack = true;
                                }
                                break;
                            }
                        case 7:
                            {
                                questPart = -3;
                                break;
                            }
                        case 20:
                            {
                                ShowArrow(1);
                                if (account.waitForInput == false)
                                {
                                    account.waitForInput = true;
                                    DestroyArrow();
                                    questPart = 5;
                                    builderMenu.tutorialBack = true;
                                }
                                break;
                            }
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                        case -2:
                            {
                                questPart = 1;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch(questPart)
                    {
                        case 1:
                            {
                                ShowDialog(7, false);
                                break;
                            }
                        case 2:
                            {
                                MakeCanvas("You got: 1000 Gold!", 1000, 0, 2);
                                break;
                            }
                        case 3:
                            {
                                ShowDialog(8, false);
                                questLine.ShowQuests(false);
                                GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
                                foreach(GameObject temp in allBuildings)
                                {
                                    if (temp.GetComponent<BuildingMain>() != null)
                                    {
                                        temp.GetComponent<BuildingMain>().ableToSave = false;
                                    }
                                }
                                break;
                            }
                        case 4:
                            {
                                hud.buildButton.interactable = false;
                                if (account.money >= 1230)
                                {
                                    questPart++;
                                }
                                break;
                            }
                        case 5:
                            {
                                ShowDialog(9, false);
                                break;
                            }
                        case 6:
                            {
                                questPart = -6;
                                break;
                            }
                        case 7:
                            {
                                ShowArrow(1);
                                if (questLine.tutorialBack == false)
                                {
                                    questPart = -6;
                                    DestroyArrow();
                                    hud.buildButton.interactable = true;
                                }
                                break;
                            }
                        
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                        case -3:
                            {
                                questPart = 1;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
            case 0:
                {
                    switch (questPart)
                    {
                        case -6:
                            {
                                if (questLine.questLineProgress[0] == 0)
                                {
                                    questPart = 7;
                                }
                                break;
                            }
                        case 7:
                            {
                                hud.EnableButton(true);
                                questLine.ShowQuests(true);
                                account.exp += 20;
                                ShowDialog(10, false);
                                break;
                            }
                        case 8:
                            {
                                MakeCanvas("You got: 1000 Gold!", 1000, 0, 3);
                                break;
                            }
                        case 9:
                            {
                                ShowDialog(11, false);
                                break;
                            }
                        case 10:
                            {
                                tutorialDoing = false;
                                questPart++;
                                DestroyArrow();
                                GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
                                foreach (GameObject temp in allBuildings)
                                {
                                    temp.GetComponent<BuildingMain>().ableToSave = true;
                                }
                                Destroy(gameObject);
                                questLine.questLineProgress[1] = 1;
                                questLine.ResetQuests();
                                menu.interactable = true;
                                skymap.SetActive(true);
                                account.autoSave = true;
                                account.PushSave();
                                break;
                            }
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void PressedDoneButton(string name)
    {
        name = name.Replace("<DB>", "");
        hud.SetName(name);
        questPart++;
        nameInputCanvas.SetActive(false);
    }

    void MakeCanvasName(string name)
    {
        if (!onceName)
        {
            nameInputCanvas.SetActive(true);
            nameInputCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedDoneButton(name); });
            nameInputCanvas.GetComponentInChildren<InputField>().text = name;
            nameInputCanvas.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate { MakeDoneButton(); });
        }
    }

    void MakeDoneButton()
    {
        nameInputCanvas.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        nameInputCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedDoneButton(nameInputCanvas.GetComponentInChildren<InputField>().text); });
    }

    void PressedCollectButton(int addedMoney, int addedRP, int time)
    {
        account.money += addedMoney;
        account.researchPoints += addedRP;

        questPart++;
        if (time != 2)
        {
            hud.canvas[time].SetActive(true);
        }
        onceCanvas = false;
        canvas.SetActive(false);
    }

    void MakeCanvas(string text, int addedMoney, int addedRP, int time)
    {
        if(!onceCanvas)
        {
            onceCanvas = true;
            canvas.SetActive(true);
            canvas.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            canvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedCollectButton(addedMoney, addedRP, time); });
            Text[] allText = canvas.GetComponentsInChildren<Text>();
            allText[0].text = text;
            allText[2].text = addedMoney.ToString();
            allText[3].text = addedRP.ToString();
        }
    }

    void ShowArrow(int location)
    {
        arrowLocations[location].SetActive(true);
        arrowLocations[location].GetComponent<RectTransform>().SetAsLastSibling();
        activeArrow = location;
    }

    public void DestroyArrow()
    {
        arrowLocations[activeArrow].SetActive(false);
    }

    void ShowDialog(int part, bool tutorial = true)
    {
        dialogs.tutorial = tutorial;
        dialogs.ActivateTalking(part);
        oldQuestPart = questPart;
        questPart = -1;
    }

    void PlaceHQ(GameObject hq, Vector2 positionNew)
    {
        Vector2 activePlaceOnGrid = positionNew;
        EmptyField[,] grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        for (int i = 0; i < hq.GetComponent<BuildingMain>().size.x; i++)
        {
            for (int y = 0; y < hq.GetComponent<BuildingMain>().size.y; y++)
            {
                grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble = false;
            }
        }

        GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].building = hq.GetComponent<BuildingMain>().buildingName;
        Vector2 size = hq.GetComponent<BuildingMain>().size;
        Vector3 newPosition = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].transform.position;
        GameObject tempBuilding = (GameObject)Instantiate(hq, newPosition, transform.rotation);

        tempBuilding.transform.localScale = size;
        tempBuilding.GetComponent<BuildingMain>().ID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].ID;
        tempBuilding.GetComponent<BuildingMain>().gridPosition = activePlaceOnGrid;
    }
}
