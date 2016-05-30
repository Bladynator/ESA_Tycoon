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
    string townName = "ESA ESTEC";
    [SerializeField]
    GameObject hq;
    public bool tutorialDoing = false;
    BuildingButtons builderMenu;
    [SerializeField]
    GameObject arrow, canvas, nameInputCanvas, skymap;
    [SerializeField]
    GameObject[] arrowLocations;
    GameObject tempArrow;
    bool arrowSpawned = false, once = false, onceName = false, onceCanvas = false;

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
        //Debug.Log(activeQuest + " / " + questPart);
        switch (activeQuest)
        {
            case 1:
                {
                    switch (questPart)
                    {
                        case 1:
                            {
                                tutorialDoing = true;
                                account.ChangeColliders(false);
                                account.autoSave = false;
                                hud.EnableButton(false);
                                hud.CanvasEnabled(false);
                                questLine.ShowQuests(false);
                                dialogs.tutorial = true;
                                skymap.SetActive(false);
                                ShowDialog(0);
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
                                ShowDialog(1);
                                break;
                            }
                        case 4:
                            {
                                MakeCanvas("You got: 1000 Gold!", 0, 0, 0);
                                break;
                            }
                        case 5:
                            {
                                ShowDialog(2);
                                break;
                            }
                        case 6:
                            {
                                PlaceHQ(hq);
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
                                ShowDialog(4);
                                break;
                            }
                        case 11:
                            {
                                ShowArrow(4);
                                account.ChangeColliders(true);
                                questPart++;
                                break;
                            }
                        case 12:
                            {
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
                                ShowArrow(4);
                                break;
                            }
                        case 4:
                            {
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
                                ShowArrow(4);
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
                                    temp.GetComponent<BuildingMain>().ableToSave = false;
                                }
                                break;
                            }
                        case 4:
                            {
                                hud.buildButton.gameObject.SetActive(false);
                                if (account.money >= 1130)
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
                                hud.buildButton.gameObject.SetActive(false);
                                ShowArrow(0);
                                questLine.ShowQuests(true);
                                if (!account.waitForInput)
                                {
                                    questLine.tutorialBack = true;
                                    questPart++;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 7:
                            {
                                ShowArrow(1);
                                if (questLine.tutorialBack == false)
                                {
                                    questPart = -6;
                                    DestroyArrow();
                                    hud.buildButton.gameObject.SetActive(true);
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
                                tutorialDoing = false;
                                questPart++;
                                DestroyArrow();
                                GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
                                foreach (GameObject temp in allBuildings)
                                {
                                    temp.GetComponent<BuildingMain>().ableToSave = true;
                                }
                                account.exp += 20;
                                Destroy(gameObject);
                                questLine.questLineProgress[1] = 1;
                                questLine.ResetQuests();
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
            nameInputCanvas.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate { nameInputCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedDoneButton(nameInputCanvas.GetComponentInChildren<InputField>().text); }); });
        }
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
            canvas.GetComponentInChildren<Text>().text = text;
        }
    }

    void ShowArrow(int location)
    {
        if (!arrowSpawned)
        {
            arrowSpawned = true;
            tempArrow = (GameObject)Instantiate(arrow, arrowLocations[location].transform.position, arrowLocations[location].transform.rotation);
        }
        tempArrow.transform.position = arrowLocations[location].transform.position;
    }

    public void DestroyArrow()
    {
        Destroy(tempArrow);
        arrowSpawned = false;
    }

    void ShowDialog(int part, bool tutorial = true)
    {
        dialogs.tutorial = tutorial;
        dialogs.ActivateTalking(part);
        oldQuestPart = questPart;
        questPart = -1;
    }

    void PlaceHQ(GameObject hq)
    {
        Vector2 activePlaceOnGrid = new Vector2(2, 5);
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
